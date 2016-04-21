using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin {
  public class SwProperties : ICollection<SwProperty> {
    protected ArrayList _innerArray;
    protected SldWorks swApp;

    public SldWorks SwApp {
      get { return this.swApp; }
    }

    public ModelDoc2 modeldoc { get; set; }
    public string configName { get; set; }

    public SwProperties(SldWorks sw) {
      swApp = sw;
      cutlistData = new CutlistData();
      _innerArray = new ArrayList();
    }

    /// <summary>
    /// Empty metadata can create a problem.
    /// </summary>
    public void CreateDefaultDrawingSet() {
      Add(new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoNumber, "$PRP:\"SW-File Name\"", true));
      Add(new SwProperty("CUSTOMER", swCustomInfoType_e.swCustomInfoNumber, string.Empty, true));
      Add(new SwProperty("REVISION LEVEL", swCustomInfoType_e.swCustomInfoNumber, "100", true));
      Add(new SwProperty("DrawnBy", swCustomInfoType_e.swCustomInfoNumber, string.Empty, true));
      Add(new SwProperty("DATE", swCustomInfoType_e.swCustomInfoNumber, DateTime.Now.ToShortDateString(), true));

      Add(new SwProperty("M1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("FINISH 1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("M2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("FINISH 2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("M3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("FINISH 3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("M4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("FINISH 4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("M5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      Add(new SwProperty("FINISH 5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
    }

    public void GetMaterialMetaData() {
        string atHere = PartFileInfo.Name;
        Add(new SwProperty("MATERIAL",
          swCustomInfoType_e.swCustomInfoText,
          string.Format("\"SW-Material@{0}\"", atHere),
          true));

        Add(new SwProperty("WEIGHT",
          swCustomInfoType_e.swCustomInfoText,
          string.Format("\"SW-Mass@{0}\"", atHere),
          true));

        Add(new SwProperty("VOLUME",
          swCustomInfoType_e.swCustomInfoText,
          string.Format("\"SW-Volume@{0}\"", atHere),
          true));
    }

    /// <summary>
    /// This sucks in all the metadata from a SW doc.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    public void GetPropertyData(ModelDoc2 md) {
      modeldoc = md;
      if (md != null) {
        swDocumentTypes_e docType = (swDocumentTypes_e)md.GetType();
        // Drawings only have global props.
        if (docType == swDocumentTypes_e.swDocDRAWING) {
          CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
          ParsePropertyData(g, docType);
        } else {
          // Getting global and local props.
          CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
          Configuration c = (Configuration)md.ConfigurationManager.ActiveConfiguration;
          CustomPropertyManager s = md.Extension.get_CustomPropertyManager(c.Name);

          cutlistData.OpType = ParseDept2(md);
          configName = c.Name;
          //ParsePropertyData2(g, docType);
          //ParsePropertyData2(s, docType);
          ParseGlobalPropertyData(g);
          ParseSpecificPropertyData(s);
        }
      }
    }

    /// <summary>
    /// This sucks in all the metadata from a SW doc.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    public void GetPropertyData(Component2 comp) {
      if (comp != null) {
        modeldoc = comp.GetModelDoc2();
        if (modeldoc != null) {
          swDocumentTypes_e docType = (swDocumentTypes_e)modeldoc.GetType();
          // Drawings only have global props.
          CustomPropertyManager g = modeldoc.Extension.get_CustomPropertyManager(string.Empty);
          if (docType == swDocumentTypes_e.swDocDRAWING) {
            ParsePropertyData(g, docType);
          } else {
            // Getting specific props.
            // string.Empty may mean "in-use"
            // <http://help.solidworks.com/2016/English/api/sldworksapi/Change_Referenced_Configuration_Example_VB.htm>
            //comp.ReferencedConfiguration = string.Empty;
            configName = comp.ReferencedConfiguration;
            CustomPropertyManager s = modeldoc.Extension.get_CustomPropertyManager(configName);

            cutlistData.OpType = ParseDept2(modeldoc);
            //ParsePropertyData2(g, docType);
            //ParsePropertyData2(s, docType);
            ParseGlobalPropertyData(g);
            ParseSpecificPropertyData(s);
          }
        }
      }
    }

    /// <summary>
    /// Gotta know the dept to know how to populate OPS and turn on/off the right fields.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    /// <returns>Returns the ID of an OpType.</returns>
    public int ParseDept(ModelDoc2 md) {
      const string propName = "DEPARTMENT";
      const string newPropName = "DEPTID";

      int opt = 1;
      bool useCached = false;
      // If we can't find the prop we want, let's just presume a wood part.
      string val = "WOOD";
      string resVal = "WOOD";
      bool wRes = false;
      int tp = 0;
      string resName = "WOOD";

      CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
      Configuration c = (Configuration)md.ConfigurationManager.ActiveConfiguration;
      CustomPropertyManager s = md.Extension.get_CustomPropertyManager(c.Name);

      string[] sa = g.GetNames();
      List<string> ss = new List<string>();

      // Noooo! Null stuff is trouble.
      if (sa != null)
        ss.AddRange(sa);

      sa = s.GetNames();

      if (sa != null)
        ss.AddRange(sa);

      SwProperty pOld = new SwProperty();
      SwProperty pNew = new SwProperty();
      pOld.Type = swCustomInfoType_e.swCustomInfoText;
      pOld.Global = true;
      pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
      pNew.Global = true;
      foreach (string n in ss) {
        switch (n) {
          case propName:
            // old dept field
            pOld.Old = true;
            pNew.Old = false;
            if (g.Get5(propName, useCached, out val, out resVal, out wRes) == (int)swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent)
              s.Get5(propName, useCached, out val, out resVal, out wRes);
            pOld.Rename(propName);
            pNew.Rename(newPropName);

            if (int.TryParse(val, out tp)) {
              resName = cutlistData.GetOpTypeNameByID(tp);
              opt = tp;
            } else {
              opt = cutlistData.GetOpTypeIDByName(resVal.ToUpper());
            }

            pOld.ID = opt.ToString();
            pOld.Value = val;
            pOld.ResValue = resVal;

            pNew.ID = pOld.ID;
            pNew.Value = val;
            pNew.ResValue = resVal;
            break;
          case newPropName:
            // new dept field
            pOld.Old = true;
            pNew.Old = false;
            if (g.Get5(newPropName, useCached, out val, out resVal, out wRes) == (int)swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent)
              s.Get5(newPropName, useCached, out val, out resVal, out wRes);
            pOld.Rename(propName);
            pNew.Rename(newPropName);
            tp = 0;
            resName = "WOOD";
            if (int.TryParse(val, out tp))
              resName = cutlistData.GetOpTypeNameByID(tp);
            opt = tp;

            pNew.Value = val;
            pNew.ResValue = resVal;

            pOld.ID = pNew.ID = val;
            pOld.Descr = pNew.Descr = resName;
            pOld.ResValue = pNew.ResValue = resName;
            break;
          default:
            break;
        }
      }

      if (Properties.Settings.Default.Testing && !Contains(propName))
        Add(pOld);
      else
        Remove(pOld);

      if (!Contains(newPropName))
        Add(pNew);

      return opt;
    }

    public int ParseDept2(ModelDoc2 md) {
      int dept = 1;
      SwProperty oldDept = new SwProperty("DEPARTMENT", swCustomInfoType_e.swCustomInfoText, "WOOD", true);
      SwProperty newDept = new SwProperty("DEPTID", swCustomInfoType_e.swCustomInfoNumber, "1", true);
      oldDept.Old = true;

      if (IsAmongTheProperties(oldDept.Name, modeldoc)) {
        oldDept.Get(modeldoc, cutlistData);
        oldDept.ID = cutlistData.GetOpTypeIDByName(oldDept.Value).ToString();
        newDept.ID = oldDept.ID;
        newDept.Value = oldDept.ID;
        newDept.ResValue = oldDept.ResValue;
        int.TryParse(oldDept.ID, out dept);
      } else {
        newDept.Get(modeldoc, cutlistData);
        newDept.ID = newDept.Value;
        if (int.TryParse(newDept.ID, out dept)) {
          newDept.Descr = cutlistData.GetOpTypeNameByID(dept);
        }
        oldDept.ID = newDept.ID;
        oldDept.Value = newDept.Descr;
        oldDept.ResValue = newDept.Descr;
      }

      if (Properties.Settings.Default.Testing) {
        Add(oldDept);
      }

      Add(newDept);

      return dept;
    }

    /// <summary>
    /// This takes the property data and sets it up for the controls to read properly. This gets the descrs from the cutlist,
    ///  or, if we have descrs, it gets IDs instead.
    /// </summary>
    /// <param name="g">A CustomPropertyManager.</param>
    /// <param name="dt">Drawing or Model?</param>
    public void ParsePropertyData(CustomPropertyManager g, swDocumentTypes_e dt) {
      string valOut = string.Empty;
      string resValOut = string.Empty;
      bool wasResolved = false;
      int res = 0;
      string[] ss = g.GetNames();

      if (ss != null) {
        foreach (string s in ss) {
          res = g.Get5(s, false, out valOut, out resValOut, out wasResolved);
          SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, true);
          p.ResValue = resValOut;
          p.Type = (swCustomInfoType_e)g.GetType2(s);

          if (p.Name.ToUpper().StartsWith("OVER")) {
            p.Global = true;
            p.Type = swCustomInfoType_e.swCustomInfoDouble;                         // Make sure OVERs are doubles.
          }

          if (p.Name.ToUpper().StartsWith("CUTLIST"))                                 // Cutlist material.
                    {
            p.Global = false;
            p.Value = Properties.Settings.Default.DefaultMaterial.ToString();
            p.Type = swCustomInfoType_e.swCustomInfoNumber;
            p.Table = "CLPARTID";
            p.Field = "MATID";
            int tp = 0;
            if (int.TryParse(p.Value, out tp))                                      // Is it numerical? Then it's an ID.
                        {
              if (tp > 0) {
                p.ResValue = cutlistData.GetMaterialByID(p.Value);
              } else                                                                // No mat.
                            {
                p.ResValue = "TBD MATERIAL";
              }
            } else {
              p.Value = cutlistData.GetMaterialID(p.ResValue).ToString();    // Not numerical; must have a descr. Let's get an ID.
            }
          }

          if (p.Name.ToUpper().StartsWith("EDGE"))                                    // Cutlist edge.
                    {
            p.Global = false;
            p.Type = swCustomInfoType_e.swCustomInfoNumber;
            p.Table = "CLPARTID";
            p.Field = "EDGEID_{0}";
            int tp = 0;
            if (int.TryParse(p.Value, out tp))                                      // Is it numerical? We have an ID.
                        {
              if (tp > 0) {
                p.ResValue = cutlistData.GetEdgeByID(p.Value);
              } else                                                                // No edge.
                            {
                p.ResValue = string.Empty;                                      // It's OK for edging to not exist.
              }
            } else {
              p.Value = cutlistData.GetEdgeID(p.ResValue).ToString();             // When life gives you descrs, make IDs.
            }
          }

          if (p.Name.ToUpper().Contains("OP"))                                        // Handle ops. Good thing we've got the dept figured out!
                    {
            p.Global = true;
            p.Type = swCustomInfoType_e.swCustomInfoNumber;
            p.Table = "CUT_PARTS";
            p.Field = "OP{0}ID";
            int tp = 0;
            if (int.TryParse(p.Value, out tp))                                      // Are we numerical?
                        {
              if (tp > 0) {
                p.ResValue = cutlistData.GetOpByID(p.Value);
              } else                                                                // Nothing in there.
                            {
                p.ResValue = string.Empty;                                      // It's ok to nop.
              }
            } else {
              if (p.ResValue.Length < 4 && p.ResValue != string.Empty)                        // When it's not numerical, it's probably
                            {                                                                               // abbreviated.
                List<string> dr = cutlistData.GetOpDataByName(p.ResValue.ToString());       // Getting datarows leads to trouble here
                p.Value = dr[0];                                                            // if they're empty.
                p.ResValue = dr[2];
              } else {
                p.Value = "0";
                p.ResValue = string.Empty;
              }
            }
          }

          p.SwApp = swApp;
          if (!Contains(p))                                                                           // Let's add it if we don't already have it.
                    {
            _innerArray.Add(p);
          }
#if DEBUG
          System.Diagnostics.Debug.Print(s);
#endif
        }
      }

      if (dt == swDocumentTypes_e.swDocDRAWING)                                                           // Well, now, if it's a drawing...
            {
        ss = (string[])g.GetNames();
        if (ss != null) {
          foreach (string s in ss) {
            res = g.Get5(s, false, out valOut, out resValOut, out wasResolved);                     // We can just pull data. Nice!
            SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, false);
            p.ResValue = resValOut;
            p.Type = (swCustomInfoType_e)g.GetType2(s);
            p.SwApp = swApp;
#if DEBUG
            System.Diagnostics.Debug.Print(s);
#endif
            _innerArray.Add(p);
          }
        } else {
          CreateDefaultDrawingSet();                                                                  // Nothing in there. Let's make stuff up.
        }
      }
    }

    public void ParsePropertyData2(CustomPropertyManager g, swDocumentTypes_e dt) {
      string valOut = string.Empty;
      string resValOut = string.Empty;
      bool wasResolved = false;
      int res = 0;
      int o = 0;

      GetMaterialMetaData();
      string[] ss = g.GetNames();

      if (ss != null) {
        foreach (string s in ss) {
          res = g.Get5(s, false, out valOut, out resValOut, out wasResolved);
          SwProperty pOld = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, true);
          pOld.Old = true;
          pOld.Value = valOut;
          pOld.ResValue = resValOut;
          pOld.Type = (swCustomInfoType_e)g.GetType2(s);
          pOld.SwApp = swApp;

          SwProperty pNew = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, true);
          pOld.Old = false;
          pNew.Value = valOut;
          pNew.ResValue = resValOut;
          pNew.Type = (swCustomInfoType_e)g.GetType2(s);
          pNew.SwApp = swApp;

          switch (s.ToUpper()) {
            case "CUTLIST MATERIAL":
              pOld.Old = true;
              pNew.Old = true;

              if (Contains("MATID")) {
                pNew = GetProperty("MATID");
                if (Properties.Settings.Default.Testing) {
                  pNew.Old = false;
                }
              } else {
                pNew.Rename("MATID");
              }

              pNew.ID = cutlistData.GetMaterialID(pOld.Value).ToString();
              pOld.ID = pNew.ID;
              pOld.Descr = pOld.ResValue;
              pNew.Descr = pOld.ResValue;

              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "MATID":
              pOld.Old = true;
              pNew.Old = false;
              if (Contains("CUTLIST MATERIAL")) {
                pOld = GetProperty("CUTLIST MATERIAL");
              }

              if (int.TryParse(pNew.Value, out o)) {
                pNew.ID = pNew.Value;
                pOld.ID = pNew.ID;
                pNew.Descr = cutlistData.GetMaterialByID(pNew.ID);
                pOld.Descr = pNew.Descr;
              }

              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EDGE FRONT (L)":
              pOld.Old = true;
              pNew.Old = false;
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;

              if (Contains("EFID")) {
                pNew = GetProperty("EFID");
                if (Properties.Settings.Default.Testing) {
                  pNew.Old = false;
                }
              } else {
                pNew.Rename("EFID");
              }

              pNew.Descr = pOld.Value;

              if (Properties.Settings.Default.Testing)
                FavorOldEdge(ref pOld, ref pNew);
              break;
            case "EDGE BACK (L)":
              pOld.Old = true;
              pNew.Old = false;
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;

              if (Contains("EBID")) {
                pNew = GetProperty("EBID");
                if (Properties.Settings.Default.Testing) {
                  pNew.Old = false;
                }
              } else {
                pNew.Rename("EBID");
              }

              pNew.Descr = pOld.Value;

              if (Properties.Settings.Default.Testing)
                FavorOldEdge(ref pOld, ref pNew);
              break;
            case "EDGE LEFT (W)":
              pOld.Old = true;
              pNew.Old = false;
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;

              if (Contains("ELID")) {
                pNew = GetProperty("ELID");
                if (Properties.Settings.Default.Testing) {
                  pNew.Old = false;
                }
              } else {
                pNew.Rename("ELID");
              }

              pNew.Descr = pOld.Value;

              if (Properties.Settings.Default.Testing)
                FavorOldEdge(ref pOld, ref pNew);
              break;
            case "EDGE RIGHT (W)":
              pOld.Old = true;
              pNew.Old = false;
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;

              if (Contains("ERID")) {
                pNew = GetProperty("ERID");
                if (Properties.Settings.Default.Testing) {
                  pNew.Old = false;
                }
              } else {
                pNew.Rename("ERID");
              }

              pNew.Descr = pOld.Value;

              if (Properties.Settings.Default.Testing)
                FavorOldEdge(ref pOld, ref pNew);
              break;
            case "EFID":
              pOld.Old = true;
              pNew.Old = false;
              if (Contains("EDGE FRONT (L)")) {
                pOld = GetProperty("EDGE FRONT (L)");
              }

              if (int.TryParse(pNew.Value, out o)) {
                pNew.ID = pNew.Value;
                pOld.ID = pNew.ID;
                pNew.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pOld.Descr = pNew.Descr;
              }

              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EBID":
              pOld.Old = true;
              pNew.Old = false;
              if (Contains("EDGE BACK (L)")) {
                pOld = GetProperty("EDGE BACK (L)");
              }

              if (int.TryParse(pNew.Value, out o)) {
                pNew.ID = pNew.Value;
                pOld.ID = pNew.ID;
                pNew.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pOld.Descr = pNew.Descr;
              }

              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "ELID":
              pOld.Old = true;
              pNew.Old = false;
              if (Contains("EDGE LEFT (W)")) {
                pOld = GetProperty("EDGE LEFT (W)");
              }

              if (int.TryParse(pNew.Value, out o)) {
                pNew.ID = pNew.Value;
                pOld.ID = pNew.ID;
                pNew.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pOld.Descr = pNew.Descr;
              }

              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "ERID":
              pOld.Old = true;
              pNew.Old = false;
              if (Contains("EDGE RIGHT (W)")) {
                pOld = GetProperty("EDGE RIGHT (W)");
              }

              if (int.TryParse(pNew.Value, out o)) {
                pNew.ID = pNew.Value;
                pOld.ID = pNew.ID;
                pNew.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pOld.Descr = pNew.Descr;
              }

              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "UPDATE CNC":
              pOld.Old = true;
              pNew.Old = false;
              pOld.Global = true;
              pNew.Global = true;
              pOld.Type = swCustomInfoType_e.swCustomInfoYesOrNo;
              pNew.Type = swCustomInfoType_e.swCustomInfoYesOrNo;
              pNew.ID = pOld.ID = valOut.ToUpper().Contains("YES") ? "True" : "False";
              pOld.Value = pNew.Value = valOut;
              pOld.ResValue = pNew.ResValue = resValOut;
              break;
            case "DEPARTMENT": // let's ignore these as already parsed.
              pOld.Old = true;
              pNew.Old = true;
              break;
            case "DEPTID":
              pOld.Old = true;
              pNew.Old = false;
              break;
            default:
              pOld.Global = true;
              pNew.Global = true;
              pOld.Value = valOut;
              pNew.Value = valOut;
              pOld.ResValue = resValOut;
              pNew.ResValue = resValOut;
              pOld.Old = true;
              pNew.Old = false;

              if (s.Contains("OVER")) {
                pOld.Type = swCustomInfoType_e.swCustomInfoDouble;
                pNew.Type = swCustomInfoType_e.swCustomInfoDouble;
              }

              for (int i = 1; i < 6; i++) {
                if (s.StartsWith(string.Format("OP{0}", i))) {
                  int tp = 0;
                  pOld.Rename(string.Format("OP{0}", i));
                  pNew.Rename(string.Format("OP{0}ID", i));
                  pOld.Type = swCustomInfoType_e.swCustomInfoText;
                  pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
                  pOld.Value = cutlistData.GetOpIDByName(pOld.ResValue).ToString();
                  pNew.Value = pOld.Value;

                  if (s == string.Format("OP{0}ID", i)) {
                    if (int.TryParse(pNew.ResValue, out tp) || int.TryParse(pOld.ResValue, out tp)) {
                      if (tp > 0) {
                        List<string> dr = cutlistData.GetOpDataByID(pNew.ResValue);
                        pOld.ID = dr[(int)CutlistData.OpFields.OPID];
                        pOld.Descr = dr[(int)CutlistData.OpFields.OPDESCR];
                        pNew.ID = dr[(int)CutlistData.OpFields.OPID];
                        pNew.Descr = dr[(int)CutlistData.OpFields.OPDESCR];
                      } else {
                        pOld.ID = tp.ToString();
                        pOld.Value = string.Empty;
                        pOld.ResValue = string.Empty;
                        pOld.Descr = string.Empty;
                        pNew.Descr = string.Empty;
                        pNew.ID = tp.ToString();
                        pNew.Value = tp.ToString();
                        pNew.ResValue = tp.ToString();
                      }
                    } else {
                      pOld.ID = "0";
                      pOld.Value = string.Empty;
                      pOld.ResValue = string.Empty;
                      pNew.ID = "0";
                      pNew.Value = "0";
                      pNew.ResValue = "0";
                    }
                  } else {
                    if (!int.TryParse(pNew.Value, out tp) && pNew.Value != string.Empty) {
                      List<string> dr = cutlistData.GetOpDataByName(pOld.ResValue);
                      pOld.ID = dr[(int)CutlistData.OpFields.OPID];
                      pNew.ID = dr[(int)CutlistData.OpFields.OPID];
                      pOld.Descr = dr[(int)CutlistData.OpFields.OPDESCR];
                      pNew.Descr = dr[(int)CutlistData.OpFields.OPDESCR];
                    } else {
                      List<string> dr = cutlistData.GetOpDataByID(pNew.Value);
                      pOld.ID = dr[(int)CutlistData.OpFields.OPID];
                      pOld.Value = dr[(int)CutlistData.OpFields.OPNAME];
                      pOld.ResValue = dr[(int)CutlistData.OpFields.OPNAME];
                      pOld.Descr = dr[(int)CutlistData.OpFields.OPDESCR];
                      pNew.ID = dr[(int)CutlistData.OpFields.OPID];
                      pNew.Descr = dr[(int)CutlistData.OpFields.OPDESCR];
                    }
                  }
                }
              }
              break;
          }

          if (!pNew.Old) {
            Add(pNew);
          }

          if (Properties.Settings.Default.Testing)
            Add(pOld);

        }
        // I do want to ignore this in the future.
        if (Properties.Settings.Default.Testing) {
          // Add() only fires if the .Name doesn't already exist.
          Add(new SwProperty("INCLUDE IN CUTLIST",
          swCustomInfoType_e.swCustomInfoYesOrNo, "Yes", true));
        }
      }
    }

    public void ParseSpecificPropertyData(CustomPropertyManager g) {
      SwProperty oldMat = new SwProperty(g, "CUTLIST MATERIAL", swCustomInfoType_e.swCustomInfoText, "TBD MATERIAL", false);
      SwProperty mat = new SwProperty("MATID", swCustomInfoType_e.swCustomInfoNumber, Properties.Settings.Default.DefaultMaterial.ToString(), false);
      oldMat.Old = true;

      SwProperty oldef = new SwProperty(g, "EDGE FRONT (L)", swCustomInfoType_e.swCustomInfoText, string.Empty, false);
      SwProperty oldeb = new SwProperty(g, "EDGE BACK (L)", swCustomInfoType_e.swCustomInfoText, string.Empty, false);
      SwProperty oldel = new SwProperty(g, "EDGE LEFT (W)", swCustomInfoType_e.swCustomInfoText, string.Empty, false);
      SwProperty older = new SwProperty(g, "EDGE RIGHT (W)", swCustomInfoType_e.swCustomInfoText, string.Empty, false);
      SwProperty ef = new SwProperty(g, "EFID", swCustomInfoType_e.swCustomInfoNumber, "0", false);
      SwProperty eb = new SwProperty(g, "EBID", swCustomInfoType_e.swCustomInfoNumber, "0", false);
      SwProperty el = new SwProperty(g, "ELID", swCustomInfoType_e.swCustomInfoNumber, "0", false);
      SwProperty er = new SwProperty(g, "ERID", swCustomInfoType_e.swCustomInfoNumber, "0", false);
      oldef.Old = true;
      oldeb.Old = true;
      oldel.Old = true;
      older.Old = true;

      if (IsAmongTheProperties(oldMat.Name, modeldoc) && !IsAmongTheProperties(mat.Name, modeldoc)) {
        oldMat.Get(modeldoc, cutlistData);
        mat.ID = oldMat.ID;
        mat.Value = oldMat.Value;
        mat.ResValue = oldMat.ResValue;
      } else if (IsAmongTheProperties(mat.Name, modeldoc)) {
        mat.Get(modeldoc, cutlistData);
        mat.ID = mat.Value;
        mat.Descr = cutlistData.GetMaterialByID(mat.ID);
        if (Properties.Settings.Default.Testing) {
          oldMat.ID = mat.ID;
          oldMat.Descr = mat.Descr;
          oldMat.Value = mat.ResValue;
          oldMat.ResValue = mat.ResValue;
        }
      } else {
        mat.Get(modeldoc, cutlistData);
        mat.ID = mat.Value;
        mat.Descr = cutlistData.GetMaterialByID(mat.ID);

        oldMat.ID = mat.ID;
        oldMat.Descr = mat.Descr;
        oldMat.Value = mat.ResValue;
        oldMat.ResValue = mat.ResValue;
      }

      if (IsAmongTheProperties(oldef.Name, modeldoc) && !IsAmongTheProperties(ef.Name, modeldoc)) {
        oldef.Get(modeldoc, cutlistData);
        ef.ID = oldef.ID;
        ef.Value = oldef.Value;
        ef.ResValue = oldef.ResValue;
      } else if (IsAmongTheProperties(ef.Name, modeldoc)) {
        ef.Get(modeldoc, cutlistData);
        ef.ID = ef.Value;
        ef.Descr = cutlistData.GetEdgeByID(ef.ID);
        if (Properties.Settings.Default.Testing) {
          oldef.ID = ef.ID;
          oldef.Descr = ef.Descr;
          oldef.Value = ef.ResValue;
          oldef.ResValue = ef.ResValue;
        }
      } else {
        ef.Get(modeldoc, cutlistData);
        ef.ID = ef.Value;
        ef.Descr = cutlistData.GetEdgeByID(ef.ID);
        oldef.ID = ef.ID;
        oldef.Descr = ef.Descr;
        oldef.Value = ef.ResValue;
        oldef.ResValue = ef.ResValue;
      }

      if (IsAmongTheProperties(oldeb.Name, modeldoc) && !IsAmongTheProperties(eb.Name, modeldoc)) {
        oldeb.Get(modeldoc, cutlistData);
        eb.ID = oldeb.ID;
        eb.Value = oldeb.Value;
        eb.ResValue = oldeb.ResValue;
      } else if (IsAmongTheProperties(eb.Name, modeldoc)) {
        eb.Get(modeldoc, cutlistData);
        eb.ID = eb.Value;
        eb.Descr = cutlistData.GetEdgeByID(eb.ID);
        if (Properties.Settings.Default.Testing) {
          oldeb.ID = eb.ID;
          oldeb.Descr = eb.Descr;
          oldeb.Value = eb.ResValue;
          oldeb.ResValue = eb.ResValue;
        }
      } else {
        eb.Get(modeldoc, cutlistData);
        eb.ID = eb.Value;
        eb.Descr = cutlistData.GetEdgeByID(eb.ID);
        oldeb.ID = eb.ID;
        oldeb.Descr = eb.Descr;
        oldeb.Value = eb.ResValue;
        oldeb.ResValue = eb.ResValue;
      } 
      
      if (IsAmongTheProperties(oldel.Name, modeldoc) && !IsAmongTheProperties(el.Name, modeldoc)) {
        oldel.Get(modeldoc, cutlistData);
        el.ID = oldel.ID;
        el.Value = oldel.Value;
        el.ResValue = oldel.ResValue;
      } else if (IsAmongTheProperties(el.Name, modeldoc)) {
        el.Get(modeldoc, cutlistData);
        el.ID = el.Value;
        el.Descr = cutlistData.GetEdgeByID(el.ID);
        if (Properties.Settings.Default.Testing) {
          oldel.ID = el.ID;
          oldel.Descr = el.Descr;
          oldel.Value = el.ResValue;
          oldel.ResValue = el.ResValue;
        }
      } else {
        el.Get(modeldoc, cutlistData);
        el.ID = el.Value;
        el.Descr = cutlistData.GetEdgeByID(el.ID);
        oldel.ID = el.ID;
        oldel.Descr = el.Descr;
        oldel.Value = el.ResValue;
        oldel.ResValue = el.ResValue;
      }

      if (IsAmongTheProperties(older.Name, modeldoc) && !IsAmongTheProperties(er.Name, modeldoc)) {
        older.Get(modeldoc, cutlistData);
        er.ID = older.ID;
        er.Value = older.Value;
        er.ResValue = older.ResValue;
      } else if (IsAmongTheProperties(er.Name, modeldoc)) {
        er.Get(modeldoc, cutlistData);
        er.ID = er.Value;
        er.Descr = cutlistData.GetEdgeByID(er.ID);
        if (Properties.Settings.Default.Testing) {
          older.ID = er.ID;
          older.Descr = er.Descr;
          older.Value = er.ResValue;
          older.ResValue = er.ResValue;
        }
      } else {
        er.Get(modeldoc, cutlistData);
        er.ID = er.Value;
        er.Descr = cutlistData.GetEdgeByID(er.ID);
        older.ID = er.ID;
        older.Descr = er.Descr;
        older.Value = er.ResValue;
        older.ResValue = er.ResValue;
      }

      if (Properties.Settings.Default.Testing) {
        Add(oldMat);
        Add(oldef);
        Add(oldeb);
        Add(oldel);
        Add(older);
      }

      Add(mat);
      Add(ef);
      Add(eb);
      Add(el);
      Add(er);
    }

    public void ParseGlobalPropertyData(CustomPropertyManager g) {
      if (PartFileInfo != null && PartFileInfo.Name != string.Empty) {
        GetMaterialMetaData();
      }
      SwProperty d = new SwProperty("Description", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty iic = new SwProperty("INCLUDE IN CUTLIST", swCustomInfoType_e.swCustomInfoYesOrNo, "Yes", true);
      SwProperty l = new SwProperty("LENGTH", swCustomInfoType_e.swCustomInfoText, "\"D1@Sketch1\"", true);
      SwProperty w = new SwProperty("WIDTH", swCustomInfoType_e.swCustomInfoText, "\"D2@Sketch1\"", true);
      SwProperty t = new SwProperty("THICKNESS", swCustomInfoType_e.swCustomInfoText, "\"D1@Boss-Extrude1\"", true);
      SwProperty wt = new SwProperty("WALL THICKNESS", swCustomInfoType_e.swCustomInfoText, "\"Thickness@Sheet-Metal1\"", true);
      SwProperty ol = new SwProperty("OVERL", swCustomInfoType_e.swCustomInfoDouble, "0.0", true);
      SwProperty ow = new SwProperty("OVERW", swCustomInfoType_e.swCustomInfoDouble, "0.0", true);
      SwProperty cnc1 = new SwProperty("CNC1", swCustomInfoType_e.swCustomInfoText, "NA", true);
      SwProperty cnc2 = new SwProperty("CNC2", swCustomInfoType_e.swCustomInfoText, "NA", true);
      SwProperty bq = new SwProperty("BLANK QTY", swCustomInfoType_e.swCustomInfoNumber, "1", true);
      SwProperty c = new SwProperty("COMMENT", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty uc = new SwProperty("UPDATE CNC", swCustomInfoType_e.swCustomInfoText, "N", true);
      SwProperty op1 = new SwProperty("OP1", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty op2 = new SwProperty("OP2", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty op3 = new SwProperty("OP3", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty op4 = new SwProperty("OP4", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty op5 = new SwProperty("OP5", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      SwProperty op1id = new SwProperty("OP1ID", swCustomInfoType_e.swCustomInfoNumber, "0", true);
      SwProperty op2id = new SwProperty("OP2ID", swCustomInfoType_e.swCustomInfoNumber, "0", true);
      SwProperty op3id = new SwProperty("OP3ID", swCustomInfoType_e.swCustomInfoNumber, "0", true);
      SwProperty op4id = new SwProperty("OP4ID", swCustomInfoType_e.swCustomInfoNumber, "0", true);
      SwProperty op5id = new SwProperty("OP5ID", swCustomInfoType_e.swCustomInfoNumber, "0", true);
      op1.Old = true;
      op2.Old = true;
      op3.Old = true;
      op4.Old = true;
      op5.Old = true;

      d.Get(modeldoc, cutlistData);
      iic.Get(modeldoc, cutlistData);
      l.Get(modeldoc, cutlistData);
      w.Get(modeldoc, cutlistData);
      t.Get(modeldoc, cutlistData);
      wt.Get(modeldoc, cutlistData);
      ol.Get(modeldoc, cutlistData);
      ow.Get(modeldoc, cutlistData);
      cnc1.Get(modeldoc, cutlistData);
      cnc2.Get(modeldoc, cutlistData);
      bq.Get(modeldoc, cutlistData);
      c.Get(modeldoc, cutlistData);
      uc.Get(modeldoc, cutlistData);

      if (IsAmongTheProperties(op1.Name, modeldoc)) {
        op1.Get(modeldoc, cutlistData);
        op1id.ID = cutlistData.GetOpIDByName(op1.ResValue).ToString();
        op1id.Value = op1.ID;
        op1id.ResValue = op1.ResValue;
      } else {
        op1id.Get(modeldoc, cutlistData);
        op1id.ID = op1id.Value;
        op1id.Descr = cutlistData.GetOpAbbreviationByID(op1id.ID);
        op1.ID = op1id.ID;
        op1.Value = op1id.Descr;
        op1.ResValue = op1id.Descr;
      }

      if (IsAmongTheProperties(op2.Name, modeldoc)) {
        op2.Get(modeldoc, cutlistData);
        op2id.ID = cutlistData.GetOpIDByName(op2.ResValue).ToString();
        op2id.Value = op2.ID;
        op2id.ResValue = op2.ResValue;
      } else {
        op2id.Get(modeldoc, cutlistData);
        op2id.ID = op2id.Value;
        op2id.Descr = cutlistData.GetOpAbbreviationByID(op2id.ID);
        op2.ID = op2id.ID;
        op2.Value = op2id.Descr;
        op2.ResValue = op2id.Descr;
      }

      if (IsAmongTheProperties(op3.Name, modeldoc)) {
        op3.Get(modeldoc, cutlistData);
        op3id.ID = cutlistData.GetOpIDByName(op3.ResValue).ToString();
        op3id.Value = op3.ID;
        op3id.ResValue = op3.ResValue;
      } else {
        op3id.Get(modeldoc, cutlistData);
        op3id.ID = op3id.Value;
        op3id.Descr = cutlistData.GetOpAbbreviationByID(op3id.ID);
        op3.ID = op3id.ID;
        op3.Value = op3id.Descr;
        op3.ResValue = op3id.Descr;
      }

      if (IsAmongTheProperties(op4.Name, modeldoc)) {
        op4.Get(modeldoc, cutlistData);
        op4id.ID = cutlistData.GetOpIDByName(op4.ResValue).ToString();
        op4id.Value = op4.ID;
        op4id.ResValue = op4.ResValue;
      } else {
        op4id.Get(modeldoc, cutlistData);
        op4id.ID = op4id.Value;
        op4id.Descr = cutlistData.GetOpAbbreviationByID(op4id.ID);
        op4.ID = op4id.ID;
        op4.Value = op4id.Descr;
        op4.ResValue = op4id.Descr;
      }

      if (IsAmongTheProperties(op5.Name, modeldoc)) {
        op5.Get(modeldoc, cutlistData);
        op5id.ID = cutlistData.GetOpIDByName(op5.ResValue).ToString();
        op5id.Value = op5.ID;
        op5id.ResValue = op5.ResValue;
      } else {
        op5id.Get(modeldoc, cutlistData);
        op5id.ID = op5id.Value;
        op5id.Descr = cutlistData.GetOpAbbreviationByID(op5id.ID);
        op5.ID = op5id.ID;
        op5.Value = op5id.Descr;
        op5.ResValue = op5id.Descr;
      }

      Add(d);
      Add(iic);
      Add(l);
      Add(w);
      Add(t);
      Add(wt);
      Add(ol);
      Add(ow);
      Add(cnc1);
      Add(cnc2);
      Add(bq);
      Add(c);
      Add(uc);
      Add(op1id);
      Add(op2id);
      Add(op3id);
      Add(op4id);
      Add(op5id);

      if (Properties.Settings.Default.Testing) {
        Add(op1);
        Add(op2);
        Add(op3);
        Add(op4);
        Add(op5);
      }
    }

    private static bool IsAmongTheProperties(string propName, ModelDoc2 m) {
      Configuration cf = m.ConfigurationManager.ActiveConfiguration;
      CustomPropertyManager glpm = m.Extension.get_CustomPropertyManager(string.Empty);
      CustomPropertyManager sppm;

      if (cf != null) {
        sppm = m.Extension.get_CustomPropertyManager(cf.Name);
      } else {
        sppm = m.Extension.get_CustomPropertyManager(string.Empty);
      }

      bool res = false;
      string[] testArray = (string[])glpm.GetNames();
      List<string> gl = new List<string>();
      List<string> sp = new List<string>();

      if (testArray != null) {
        gl = new List<string>(testArray);
      }

      testArray = (string[])sppm.GetNames();
      if (testArray != null) {
        sp = new List<string>(testArray);
      }

      if (gl.Contains(propName) || sp.Contains(propName)) {
        res = true;
      }

      return res;
    }

    private void FavorOldEdge(ref SwProperty pFirst, ref SwProperty pSecond) {
      if (pFirst.Old == true && pSecond.Old == false) {
        pSecond.ID = cutlistData.GetEdgeID(pFirst.Value).ToString();
        pSecond.Descr = cutlistData.GetEdgeByID(pSecond.ID);

        pFirst.ID = pSecond.ID;
      } else if (pFirst.Old == false && pSecond.Old == true) {
        pFirst.ID = cutlistData.GetEdgeID(pSecond.Value).ToString();
        pFirst.Descr = cutlistData.GetEdgeByID(pFirst.ID);

        pSecond.ID = pFirst.ID;
      }
    }

    public void LinkControlToProperty(string property, bool global, System.Windows.Forms.Control c) {
      if (Contains(property)) {
        SwProperty p = GetProperty(property);
        p.SwApp = swApp;
        // Only these fields get resolved to something.
        if (p.Name.ToUpper() == "LENGTH" || p.Name.ToUpper() == "WIDTH"
            || p.Name.ToUpper() == "THICKNESS" || p.Name.ToUpper() == "WALL THICKNESS") {
          c.Text = p.Value;
        } else {
          if (c is System.Windows.Forms.ComboBox) {
            int tp = -1;
            if (int.TryParse(p.ID, out tp) && !p.Old) {
              (c as System.Windows.Forms.ComboBox).SelectedValue = tp;
            }
          } else if (c is System.Windows.Forms.CheckBox) {
            (c as System.Windows.Forms.CheckBox).Checked = (p.ID == "-1" ? true : false);
          } else {
            c.Text = string.Empty;
            if (p.Descr != null && p.Descr != string.Empty)
              c.Text = p.Descr;
            else if (p.ResValue != null && p.ResValue != string.Empty)
              c.Text = p.ResValue;
            else if (p.Value != null && p.Value != string.Empty)
              c.Text = p.Value;
          }
        }
        p.Ctl = c;
      } else {
        SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, global);
        x.SwApp = swApp;
        x.Ctl = c;
        Add(x);
      }
    }

    public void ResetOps() {
      foreach (SwProperty p in _innerArray) {
        if (p.Name.StartsWith("OP")) {
          if (p.Name.EndsWith("ID")) {
            p.Global = true;
            p.ID = "0";
            p.Value = "0";
            p.ResValue = "0";
          } else {
            p.Global = true;
            p.ID = "0";
            p.Value = string.Empty;
            p.ResValue = string.Empty;
          }
        }
      }
    }

    private int GetIndex(System.Data.DataTable dt, string val) {
      int count = -1;
      if (dt != null) {
        foreach (System.Data.DataRow dr in dt.Rows) {
          count++;
          if (dr.ItemArray[0].ToString().Trim().ToUpper() == val.Trim().ToUpper())
            return count;
        }
      }
      return count;
    }

    /// <summary>
    /// Clear out everything the doc has on the config specific side.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    public void DelSpecific(ModelDoc2 md) {
      Configuration conf = md.ConfigurationManager.ActiveConfiguration;
      if (conf != null) {
        CustomPropertyManager sp = md.Extension.get_CustomPropertyManager(conf.Name);
        string[] ss = (string[])sp.GetNames();

        if (ss != null) {
          foreach (string s in ss) {
            sp.Delete2(s);
          }
        }
      }
    }

    public void DelGlobal(ModelDoc2 md) {
      CustomPropertyManager cpm = md.Extension.get_CustomPropertyManager(string.Empty);
      string[] ss = (string[])cpm.GetNames();
      if (ss != null) {
        foreach (string s in ss) {
          cpm.Delete2(s);
        }
      }
    }

    public virtual IEnumerator<SwProperty> GetEnumerator() {
      return (new List<SwProperty>(this).GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return (new List<SwProperty>(this).GetEnumerator());
    }

    public void UpdateProperty(SwProperty property) {
      foreach (SwProperty p in this) {
        if (property.Name == p.Name) {
          p.Ctl = property.Ctl;
          p.Field = property.Field;
          p.Global = property.Global;
          p.ResValue = property.ResValue;
          p.Table = property.Table;
          p.Type = property.Type;
          p.Value = property.Value;
        }
      }
    }

    public SwProperty GetProperty(string name) {
      foreach (SwProperty p in _innerArray) {
        if (name.ToUpper() == p.Name.ToUpper()) {
          return p;
        }
      }
      SwProperty q = new SwProperty(name, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
      _innerArray.Add(q);
      // Recurse!
      return GetProperty(name);
    }

    public void ReadProperties() {
      foreach (SwProperty p in _innerArray) {
        if (p.Ctl != null) {
          p.Ctl.Text = p.Value;
        }
      }
    }

    public void ReadControls() {
      foreach (SwProperty p in _innerArray) {
        if (p.Ctl != null) {
          p.Value = p.Ctl.Text;
          if (p.Ctl is System.Windows.Forms.ComboBox) {
            if ((p.Ctl as System.Windows.Forms.ComboBox).SelectedItem != null) {
              p.ID = ((p.Ctl as System.Windows.Forms.ComboBox).SelectedItem as System.Data.DataRowView).Row.ItemArray[0].ToString();

              if (p.Name.ToUpper().StartsWith("OP") && !p.Name.ToUpper().EndsWith("ID"))
                p.Descr = cutlistData.GetOpAbbreviationByID(p.ID);
            }
          }

          if (p.Ctl is System.Windows.Forms.CheckBox) {
            p.ID = (p.Ctl as System.Windows.Forms.CheckBox).Checked ? "True" : "False";
          }
        }
      }
    }

    public void Write() {
      DelSpecific(modeldoc);
      DelGlobal(modeldoc);
      foreach (SwProperty p in _innerArray) {
        if (!p.Name.StartsWith("STUB")) {
          if (!p.Old || (p.Old && Properties.Settings.Default.Testing))
            p.Write2(modeldoc);
        }
      }

      int hash = cutlistData.GetHash(PartName);
      if (hash != Hash) {
        if (hash != 0) {
          string question = string.Format(Properties.Resources.AlreadyInOtherLocation, PartName);

          swMessageBoxResult_e res = (swMessageBoxResult_e)SwApp.SendMsgToUser2(question,
            (int)swMessageBoxIcon_e.swMbQuestion,
            (int)swMessageBoxBtn_e.swMbYesNo);
          switch (res) {
            case swMessageBoxResult_e.swMbHitAbort:
              break;
            case swMessageBoxResult_e.swMbHitCancel:
              break;
            case swMessageBoxResult_e.swMbHitIgnore:
              break;
            case swMessageBoxResult_e.swMbHitNo:
              break;
            case swMessageBoxResult_e.swMbHitOk:
              break;
            case swMessageBoxResult_e.swMbHitRetry:
              break;
            case swMessageBoxResult_e.swMbHitYes:
              cutlistData.MakeOriginal(this);
              UpdateCutlist();
              break;
            default:
              break;
          }
        } else {
          UpdateCutlist();
        }
      } else {
        UpdateCutlist();
      }
    }

    private void UpdateCutlist() {
      Part prt = CutlistData.MakePartFromPropertySet(this);
      prt.SetQuantity(CutlistQuantity);

      KeyValuePair<string, Part> pair = new KeyValuePair<string, Part>(prt.PartNumber, prt);
      int prtNo = cutlistData.GetPartID(pair.Key);
      if (prtNo > 0) {
        cutlistData.UpdatePart(pair);
        if (CutlistID != 0) {
          cutlistData.UpdateCutlistPart(CutlistID, prtNo, pair);
        }
      }
    }

    public void Write(SldWorks sw) {
      foreach (SwProperty p in _innerArray) {
        p.Write(sw);
      }
    }

    public void Write(ModelDoc2 md) {
      DelSpecific(md);
      DelGlobal(md);
      foreach (SwProperty p in _innerArray) {
        p.Del(md);
        p.Write2(md);
      }
    }

    public override string ToString() {
      string ret = string.Empty;
      foreach (SwProperty p in this) {
        ret += p.ToString() + "\n";
      }
      return ret;
    }

    private int _clID;

    public int CutlistID {
      get { return _clID; }
      set { _clID = value; }
    }

    public string CutlistQuantity { get; set; }

    public bool Primary { get; set; }

    #region ICollection<SwProperty> Members

    public void Add(SwProperty item) {
      if (!Contains(item.Name)) {
        _innerArray.Add(item);
      }
    }

    public void Clear() {
      _innerArray.Clear();
    }

    public bool Contains(SwProperty item) {
      if (item == null)
        return false;

      string n = item.Name.ToUpper();
      foreach (SwProperty p in this._innerArray) {
        if (p.Name.ToUpper() == n) {
          return true;
        }
      }
      return false;
    }

    public bool Contains(string name) {
      string n = name.ToUpper();
      foreach (SwProperty p in this._innerArray) {
        if (p.Name.ToUpper() == n) {
          return true;
        }
      }
      return false;
    }

    public void CopyTo(SwProperty[] array, int arrayIndex) {
      _innerArray.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return _innerArray.Count; }
    }

    public bool IsReadOnly { get; set; }

    public bool Remove(SwProperty item) {
      bool res = false;

      for (int i = 0; i < _innerArray.Count; i++) {
        SwProperty obj = (SwProperty)_innerArray[i];
        if (obj.Name == item.Name) {
          _innerArray.RemoveAt(i);
          res = true;
          break;
        }
      }

      return res;
    }

    public bool Remove(string name) {
      bool res = false;

      for (int i = 0; i < _innerArray.Count; i++) {
        SwProperty obj = (SwProperty)this._innerArray[i];
        if (obj.Name == name) {
          this._innerArray.RemoveAt(i);
          res = true;
          break;
        }
      }

      return res;
    }

    public override int GetHashCode() {
      return this.modeldoc.GetHashCode();
    }

    public virtual SwProperty this[int index] {
      get {
        return (SwProperty)_innerArray[index];
      }
      set {
        _innerArray[index] = value;
      }
    }

    #endregion

    public System.IO.FileInfo PartFileInfo { get; set; }

    public string PartName {
      get {
        if (PartFileInfo != null) {
          return PartFileInfo.Name.Split(' ', '.')[0];
        }
        return GetHashCode().ToString();
      }

      set {
        this.PartName = value;
      }
    }

    public int Hash { get; set; }

    private CutlistData _cutlistData;

    public CutlistData cutlistData {
      get { return _cutlistData; }
      set { _cutlistData = value; }
    }

  }
}
