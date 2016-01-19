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

          cutlistData.OpType = ParseDept(md);
          configName = c.Name;
          ParsePropertyData2(g, docType);
          ParsePropertyData2(s, docType);
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
            configName = comp.ReferencedConfiguration;
            CustomPropertyManager s = modeldoc.Extension.get_CustomPropertyManager(configName);

            cutlistData.OpType = ParseDept(modeldoc);
            ParsePropertyData2(g, docType);
            ParsePropertyData2(s, docType);
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
              if (!Contains("MATID")) {
                pNew.Rename("MATID");
                pNew.ID = cutlistData.GetMaterialID(pOld.Value).ToString();
                pOld.ID = pNew.ID;
                pNew.Value = pNew.ID;
                pOld.Descr = pOld.ResValue;
                pNew.Descr = pNew.ResValue;
              }
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "MATID":
              if (!Contains("CUTLIST MATERIAL")) {
                pOld.Rename("CUTLIST MATERIAL");
                int o = 0;
                if (int.TryParse(pNew.Value, out o)) {
                  pNew.ID = pNew.Value;
                  pOld.ID = pNew.ID;
                  pNew.Descr = cutlistData.GetMaterialByID(pNew.ID);
                  pOld.Descr = pNew.Descr;
                } else {
                  pNew.ID = cutlistData.GetMaterialID(pNew.Descr).ToString();
                  pNew.Value = pNew.ID;
                  pOld.ID = pNew.ID;
                }
              }
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EDGE FRONT (L)":
              if (!Contains("EFID")) {
                pNew.Rename("EFID");
                pNew.ID = cutlistData.GetEdgeID(pNew.Value).ToString();
                pNew.Value = pNew.ID;
                pOld.ID = pNew.ID;
                pOld.Descr = pOld.ResValue;
                pNew.Descr = pNew.ResValue;
              }
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EDGE BACK (L)":
              if (!Contains("EBID")) {
                pNew.Rename("EBID");
                pNew.ID = cutlistData.GetEdgeID(pNew.Value).ToString();
                pNew.Value = pNew.ID;
                pOld.ID = pNew.ID;
                pOld.Descr = pOld.ResValue;
                pNew.Descr = pNew.ResValue;
              }
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EDGE LEFT (W)":
              if (!Contains("ELID")) {
                pNew.Rename("ELID");
                pNew.ID = cutlistData.GetEdgeID(pNew.Value).ToString();
                pNew.Value = pNew.ID;
                pOld.ID = pNew.ID;
                pOld.Descr = pOld.ResValue;
                pNew.Descr = pNew.ResValue;
              }
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EDGE RIGHT (W)":
              if (!Contains("ERID")) {
                pNew.Rename("ERID");
                pNew.ID = cutlistData.GetEdgeID(pNew.Value).ToString();
                pNew.Value = pNew.ID;
                pOld.ID = pNew.ID;
                pOld.Descr = pOld.ResValue;
                pNew.Descr = pNew.ResValue;
              }
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EFID":
              if (!Contains("EDGE FRONT (L)")) {
                pOld.Rename("EDGE FRONT (L)");
                pOld.ID = pOld.Value;
                pNew.ID = pNew.Value;
                pOld.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pNew.Descr = pOld.Value;
              }
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "EBID":
              if (!Contains("EDGE BACK (L)")) {
                pOld.Rename("EDGE BACK (L)");
                pOld.ID = pOld.Value;
                pNew.ID = pNew.Value;
                pOld.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pNew.Descr = pOld.Value;
              }
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "ELID":
              if (!Contains("EDGE LEFT (W)")) {
                pOld.Rename("EDGE LEFT (W)");
                pOld.ID = pOld.Value;
                pNew.ID = pNew.Value;
                pOld.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pNew.Descr = pOld.Value;
              }
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "ERID":
              if (!Contains("EDGE RIGHT (W)")) {
                pOld.Rename("EDGE RIGHT (W)");
                pOld.ID = pOld.Value;
                pNew.ID = pNew.Value;
                pOld.Descr = cutlistData.GetEdgeByID(pNew.ID);
                pNew.Descr = pOld.Value;
              }
              pOld.Type = swCustomInfoType_e.swCustomInfoText;
              pNew.Type = swCustomInfoType_e.swCustomInfoNumber;
              pOld.Global = false;
              pNew.Global = false;
              break;
            case "UPDATE CNC":
              pOld.Global = true;
              pNew.Global = true;
              pOld.Type = swCustomInfoType_e.swCustomInfoYesOrNo;
              pNew.Type = swCustomInfoType_e.swCustomInfoYesOrNo;
              pNew.ID = pOld.ID = valOut.ToUpper().Contains("YES") ? "-1" : "0";
              pOld.Value = pNew.Value = valOut;
              pOld.ResValue = pNew.ResValue = resValOut;
              break;
            case "DEPARTMENT": // let's ignore these as already parsed.
              break;
            case "DEPTID":
              break;
            default:
              pOld.Global = true;
              pNew.Global = true;
              pOld.Value = valOut;
              pNew.Value = valOut;
              pOld.ResValue = resValOut;
              pNew.ResValue = resValOut;

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

          if (!Contains(pNew))
            Add(pNew);

          if (Properties.Settings.Default.Testing && !Contains(pOld))
            Add(pOld);
        }
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
            //if (p.ID == null || p.ID == string.Empty) p.ID = "0";
            int tp = -1;
            if (int.TryParse(p.ID, out tp)) {
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
      this._innerArray.Add(q);
      // Recurse!
      return this.GetProperty(name);
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
            p.ID = (p.Ctl as System.Windows.Forms.CheckBox).Checked ? "-1" : "0";
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
          this._innerArray.RemoveAt(i);
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
