using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
//using SolidWorksTools;

using System.Runtime.InteropServices;

namespace Redbrick_Addin {
  [ComVisible(true)]
  [ProgId(SWTASKPANE_PROGID)]
  public partial class SWTaskPaneHost : UserControl {
    public const string SWTASKPANE_PROGID = "Redbrick.SWTaskPane_Addin";
    protected ModelDoc2 Document;
    protected SelectionMgr swSelMgr;
    protected Component2 swSelComp;
    protected SwProperties prop;

    protected AssemblyDoc ad;
    protected PartDoc pd;
    protected DrawingDoc dd;

    DepartmentSelector ds;
    ConfigurationSpecific cs;
    GeneralProperties gp;
    MachineProperties mp;
    Ops op;

    ModelRedbrick mrb;
    DrawingRedbrick drb;

    private bool AssmEventsAssigned = false;
    private bool PartEventsAssigned = false;
    private bool DrawEventsAssigned = false;

    private bool PartSetup = false;
    private bool DrawSetup = false;
    private bool AssySetup = false;

    private bool regen_ok = false;

    private ModelDoc2 md_last;
    public int cookie;
    public SWTaskPaneHost() {
      InitializeComponent();
    }

    public void Start() {
      SwApp = RequestSW();
      prop = new SwProperties(_swApp);
      SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
      SwApp.DestroyNotify += SwApp_DestroyNotify;
      SwApp.FileCloseNotify += SwApp_FileCloseNotify;
      SwApp.CommandCloseNotify += SwApp_CommandCloseNotify;
      Document = SwApp.ActiveDoc;
      ConnectSelection();
    }

    public void ReStart() {
      ClearControls(this);

      md_last = null;
      if (SwApp == null)
        SwApp = RequestSW();

      ConnectSelection();
    }

    int SwApp_CommandCloseNotify(int Command, int reason) {
      if ((swCommands_e)Command == swCommands_e.swCommands_Close || (swCommands_e)Command == swCommands_e.swCommands_Close_) {
        ClearControls(this);
      }

      if ((swCommands_e)Command == swCommands_e.swCommands_Make_Lightweight ||
        (swCommands_e)Command == swCommands_e.swCommands_Lightweight_Toggle ||
        (swCommands_e)Command == swCommands_e.swCommands_Lightweight_All) {
        ReStart();
      }

      return 0;
    }

    int SwApp_FileCloseNotify(string FileName, int reason) {
      ClearControls(this);
      return 0;
    }

    int SwApp_DestroyNotify() {
      ClearControls(this);
      // Solidworks closed
      return 0;
    }

    int SwApp_ActiveDocChangeNotify() {
      if (SwApp == null)
        SwApp = RequestSW();

      Document = SwApp.ActiveDoc;

      if (!(Document.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && AssySetup)
        DisconnectAssemblyEvents();

      if (!(Document.GetType() == (int)swDocumentTypes_e.swDocPART) && PartSetup)
        DisconnectPartEvents();

      if (!(Document.GetType() == (int)swDocumentTypes_e.swDocDRAWING) && DrawSetup)
        DisconnectDrawingEvents();

      ConnectSelection();
      return 0;
    }

    private void AddHash() {
      System.IO.FileInfo fi;
      try {
        if (!string.IsNullOrWhiteSpace(Document.GetPathName())) {
          fi = new System.IO.FileInfo(Document.GetPathName());
          prop.PartFileInfo = fi;

          if (!prop.Contains("CRC32")) {
            prop.Hash = GetHash(string.Format("{0}\\{1}", prop.PartFileInfo.Directory.FullName, prop.PartFileInfo.Name));
            SwProperty p = new SwProperty("CRC32", swCustomInfoType_e.swCustomInfoNumber, prop.Hash.ToString(), true);
            p.Old = false;
            prop.Add(p);
          } else {

          }
        } else {
          //this.prop.PartName = "New Document"; // <-- stack overflow? weird
        }
      } catch (ArgumentException ae) {
        prop.PartName = ae.HResult.ToString();
      } catch (Exception e) {
        prop.PartName = e.HResult.ToString();
      }
    }

    private void GetTypes(ref swDocumentTypes_e d, ref swDocumentTypes_e od) {
      swDocumentTypes_e docT = (swDocumentTypes_e)Document.GetType();
      ModelDoc2 overDoc = (ModelDoc2)_swApp.ActiveDoc;
      swDocumentTypes_e overDocT = (swDocumentTypes_e)overDoc.GetType();
      if ((docT != swDocumentTypes_e.swDocDRAWING && swSelMgr != null) && swSelMgr.GetSelectedObjectCount2(-1) > 0) {
        Component2 comp = (Component2)swSelMgr.GetSelectedObjectsComponent4(1, -1);
        if (comp != null) {
          ModelDoc2 cmd = (ModelDoc2)comp.GetModelDoc2();
          docT = (swDocumentTypes_e)cmd.GetType();
          prop.GetPropertyData(comp);
          comp = null;
        } else {
          prop.GetPropertyData(Document);
        }
      } else {
        swSelMgr = null;
        prop.GetPropertyData(Document);
      }
      d = docT;
      od = overDocT;
    }

    private void AskWrite() {
      string q = "Write unwritten properties?";
      swMessageBoxResult_e r = (swMessageBoxResult_e)SwApp.SendMsgToUser2(q,
        (int)swMessageBoxIcon_e.swMbQuestion, (int)swMessageBoxBtn_e.swMbYesNo);

      switch (r) {
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
          Write();
          break;
        default:
          break;
      }
    }

    public void ConnectSelection() {
      if (Properties.Settings.Default.IdiotLight) {
        string q = string.Format("I lost property data because you didn't click the green check.");
        if (PartSetup && mrb.IsDirty) {
          SwApp.SendMsgToUser2(q,
        (int)swMessageBoxIcon_e.swMbQuestion, (int)swMessageBoxBtn_e.swMbOk);
        }
        if (DrawSetup && drb.IsDirty) {
          SwApp.SendMsgToUser2(q,
        (int)swMessageBoxIcon_e.swMbQuestion, (int)swMessageBoxBtn_e.swMbOk);
        }
      }

      System.GC.Collect(2, GCCollectionMode.Forced);
      prop.CutlistID = 0;
      if (Document != null) {
        if (Document != md_last) {

          // Blow out the propertyset so we can get new ones.
          prop.Clear();

          Enabled = true;
          AddHash();

          // what sort of doc is open?
          swDocumentTypes_e docT = swDocumentTypes_e.swDocNONE;
          swDocumentTypes_e overDocT = swDocumentTypes_e.swDocNONE;
          GetTypes(ref docT, ref overDocT);
          switch (overDocT) {
            case swDocumentTypes_e.swDocASSEMBLY:
              DisconnectAssemblyEvents();
              SetupAssy((ModelDoc2)_swApp.ActiveDoc);

              switch (docT) {
                case swDocumentTypes_e.swDocASSEMBLY:
                  regen_ok = true;
                  if (!PartSetup) {
                    SetupPart();
                  }
                  if (!PartEventsAssigned) {
                    ConnectPartEvents(prop.modeldoc);
                    PartSetup = true;
                  }
                  mrb.Update(ref prop);
                  break;
                case swDocumentTypes_e.swDocDRAWING:
                  break;
                case swDocumentTypes_e.swDocNONE:
                  break;
                case swDocumentTypes_e.swDocPART:
                  regen_ok = true;
                  if (!PartSetup) {
                    // ClearControls(this); // <-- redundant
                    // setup
                    SetupPart(prop.modeldoc);
                    // link
                    mrb.Update(ref prop);
                  } else {
                    DisconnectPartEvents();
                    ConnectPartEvents(prop.modeldoc);
                    PartSetup = true; // OK, this isn't how I meant to use this.
                    // or just link
                    mrb.Update(ref prop);
                  }
                  break;
                case swDocumentTypes_e.swDocSDM:
                  break;
                default:
                  break;
              }
              break;
            case swDocumentTypes_e.swDocDRAWING:
              regen_ok = false;
              ClearControls(this);
              SetupDrawing();
              break;
            case swDocumentTypes_e.swDocNONE:
              SetupOther();
              break;
            case swDocumentTypes_e.swDocPART:
              regen_ok = true;
              if (!PartSetup) {
                // ClearControls(this); // <-- redundant
                // setup
                SetupPart(prop.modeldoc);
                // link
                mrb.Update(ref prop);
              } else {
                DisconnectPartEvents();
                ConnectPartEvents(prop.modeldoc);
                PartSetup = true; // OK, this isn't how I meant to use this.
                // or just link
                mrb.Update(ref prop);
              }

              break;
            case swDocumentTypes_e.swDocSDM:
              break;
            default:
              SetupOther();
              break;
          }
        }
        md_last = Document;
      } else {
        Enabled = false;
      }
#if EXPERIMENTAL
      if (Document != null) {
        Feature f = (Feature)Document.FirstFeature();
        string typeName = f.GetTypeName2();
        do {
          f.GetNextSubFeature();
          typeName = f.GetTypeName2();
        } while (typeName.ToUpper().Contains("FILE"));
        if (typeName != string.Empty) {
          int res0 = SwApp.AddMenuPopupItem4(
            (int)SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY,
            cookie,
            typeName,
            "Uaaaaaaaaaaaaaaaaa.........",
            "donothing(1)",
            "enablemethod(1)",
            "pow",
            "bip;bop"
            );
          int res1 = SwApp.AddMenuPopupItem4(
             (int)SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY,
             cookie,
             typeName,
             "Ummmmmmmmmmmmmmm.........",
             "donothing(2)",
             "enablemethod(0)",
             "pow",
             "bip;bop"
             );
          int res2 = SwApp.AddMenuPopupItem(
             (int)SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY,
             (int)SolidWorks.Interop.swconst.swSelectType_e.swSelBODYFEATURES,
             typeName,
             "donothing(2)",
             "bip;bop"
             );
          (SwApp.Frame() as Frame).SetStatusBarText(
            string.Format("Runtime Command IDs: {0}, {1}, {2} | Feature: {3}", res0, res1, res2, typeName)
            );
        }
      }
#endif
    }

#if EXPERIMENTAL
    public void donothing(string data) {
      SwApp.SendMsgToUser2(data, 0, 0);
    }

    public int enablemethod(int i) {
      return i;
    }
#endif

    public void ConnectSelection2() { // Pretty sure I can deprecate this.
      System.GC.Collect(2, GCCollectionMode.Forced);
      // Blow out the propertyset so we can get new ones.
      prop.Clear();

      if (Document != null) {
        Enabled = true;
        AddHash();

        // what sort of doc is open?
        swDocumentTypes_e docT = swDocumentTypes_e.swDocNONE;
        swDocumentTypes_e overDocT = swDocumentTypes_e.swDocNONE;
        GetTypes(ref docT, ref overDocT);

        switch (docT) {
          case swDocumentTypes_e.swDocASSEMBLY:
            if (overDocT != swDocumentTypes_e.swDocASSEMBLY) {
              DisconnectAssemblyEvents();
            }
            if (!PartSetup) {
              // ClearControls(this); // <-- redundant
              //prop.GetPropertyData(Document);
              // Part/assembly props are about the same
              SetupPart(prop.modeldoc);
              // link whatever's in the prop to the controls
              mrb.Update(ref prop);
            } else {
              // already set up. We can re-link with out re-setting-up.
              mrb.Update(ref prop);
            }
            // this pretty much only assigns the selection manager and sets up events
            if (!AssySetup)
              SetupAssy();
            break;
          case swDocumentTypes_e.swDocDRAWING:
            ClearControls(this);
            SetupDrawing();
            break;
          // What's this?
          case swDocumentTypes_e.swDocNONE:
            SetupOther();
            break;
          case swDocumentTypes_e.swDocPART:
            if (AssySetup && overDocT == swDocumentTypes_e.swDocPART) {
              DisconnectAssemblyEvents();
            }

            if (!AssySetup && overDocT == swDocumentTypes_e.swDocASSEMBLY) {
              SetupAssy();
            }

            if (!PartSetup) {
              // ClearControls(this); // <-- redundant
              // setup
              SetupPart(prop.modeldoc);
              // link
              mrb.Update(ref prop);
            } else {
              DisconnectPartEvents();
              ConnectPartEvents(prop.modeldoc);
              PartSetup = true; // OK, this isn't how I meant to use this.
              // or just link
              mrb.Update(ref prop);
            }
            break;
          case swDocumentTypes_e.swDocSDM:
            // What's this?
            SetupOther();
            break;
          default:
            // OK, whatever.
            SetupOther();
            break;
        }
      } else {
        Enabled = false;
      }
    }

    static private int GetHash(string fullPath) {

      DamienG.Security.Cryptography.Crc32 crc = new DamienG.Security.Cryptography.Crc32();

      byte[] b = new byte[fullPath.Length];
      string hash = string.Empty;

      for (int i = 0; i < fullPath.Length; i++)
        b[i] = (byte)fullPath[i];

      foreach (byte byt in crc.ComputeHash(b))
        hash += byt.ToString("x2").ToLower();

      try {
        return int.Parse(hash, System.Globalization.NumberStyles.HexNumber);
      } catch (Exception) {
        return 0;
      }
    }

    private void SetupAssy() {
      // Get a selection manager.
      swSelMgr = Document.SelectionManager;

      // connect events
      ConnectAssemblyEvents();

      // Zap!
      AssySetup = true;
    }

    private void SetupAssy(ModelDoc2 md) {
      // Get a selection manager.
      swSelMgr = md.SelectionManager;

      // connect events
      ConnectAssemblyEvents(md);

      // Zap!
      AssySetup = true;
    }

    private void SetupDrawing() {
      // New drawing handler. It can use the whole swapp since it doesn't have to figure out the config.
      drb = new DrawingRedbrick(_swApp);

      // Add it to the taskpane
      Controls.Add(drb);

      // whatever's in here, make it dock.
      foreach (Control item in Controls)
        item.Dock = DockStyle.Fill;

      // drawing related events (spoiler: there aren't any)
      ConnectDrawingEvents();

      // Pow-bang! We're set up.
      DrawSetup = true;
      AssySetup = false;
      PartSetup = false;
    }

    private void SetupPart() {
      // Blow out any existing controls, and dump events.
      ClearControls(this);
      // Fill everything so it stretches.
      DockStyle d = DockStyle.Fill;

      // New model handler with current property (aquired in this.Connect...())
      mrb = new ModelRedbrick(ref prop);
      // If it's not docked, dock it.
      mrb.Dock = d;
      // put the redbrick in this control
      Controls.Add(mrb);
      // Dock this control in the taskpane.
      Dock = d;

      //foreach (Control item in mrb.Controls) {
      //  item.Dock = d;
      //}

      // Gonna use access to all these controls.
      ds = mrb.aDepartmentSelector;
      cs = mrb.aConfigurationSpecific;
      gp = mrb.aGeneralProperties;
      mp = mrb.aMachineProperties;
      op = mrb.aOps;

      // Part-related events.
      ConnectPartEvents();
      //ResumeLayout(true);
      // Boom, we're set up.
      PartSetup = true;
    }

    private void SetupPart(ModelDoc2 md) {
      // Blow out any existing controls, and dump events.
      ClearControls(this);
      // Fill everything so it stretches.
      DockStyle d = DockStyle.Fill;

      // New model handler with current property (aquired in this.Connect...())
      mrb = new ModelRedbrick(ref prop);
      // If it's not docked, dock it.
      mrb.Dock = d;
      // put the redbrick in this control
      Controls.Add(mrb);
      // Dock this control in the taskpane.
      Dock = d;

      foreach (Control item in mrb.Controls) {
        item.Dock = d;
        //item.ResumeLayout(true);
      }

      // Gonna use access to all these controls.
      ds = mrb.aDepartmentSelector;
      cs = mrb.aConfigurationSpecific;
      gp = mrb.aGeneralProperties;
      mp = mrb.aMachineProperties;
      op = mrb.aOps;

      // Part-related events.
      ConnectPartEvents(md);
      //ResumeLayout(true);
      // Boom, we're set up.
      PartSetup = true;
    }

    private void ClearControls2(Control c) {
      // any controls, no matter how deep, g'bye.
      foreach (Control item in c.Controls) {
        if (item.HasChildren) {
          /* Recurse */
          ClearControls(item);
        }
        c.Controls.Remove(item);
        item.Dispose();
      }

      //if (Document != null && Document.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
      DisconnectAssemblyEvents();
      DisconnectPartEvents();
      DisconnectDrawingEvents();
      // everything's undone.
    }

    private void ClearControls(Control c) {
      foreach (Control item in c.Controls) {
        c.Controls.Remove(item);
        item.Dispose();
      }
      Document = (ModelDoc2)_swApp.ActiveDoc;

      if (Document != null && Document.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
        DisconnectAssemblyEvents();

      DisconnectPartEvents();
      DisconnectDrawingEvents();
      // everything's undone.
    }

    // if this ever comes up, I suppose it's because the wrong kind of doc is open.
    private void SetupOther() {
      // Blow out any existing controls, and dump events.
      ClearControls(this);
      SwApp.SendMsgToUser2(
          Properties.Resources.MustOpenDocument,
          (int)swMessageBoxIcon_e.swMbInformation,
          (int)swMessageBoxBtn_e.swMbOk);
    }

    private void ConnectAssemblyEvents() {
      if ((Document.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && !AssmEventsAssigned) {
        ad = (AssemblyDoc)Document;
        ad.UserSelectionPreNotify += ad_UserSelectionPreNotify;

        // user clicks part/subassembly
        ad.UserSelectionPostNotify += ad_UserSelectionPostNotify;

        // doc closing, I think.
        ad.DestroyNotify2 += ad_DestroyNotify2;

        // Not sure, and not implemented yet.
        ad.ActiveDisplayStateChangePostNotify += ad_ActiveDisplayStateChangePostNotify;

        // switching docs
        ad.ActiveViewChangeNotify += ad_ActiveViewChangeNotify;
        DisconnectDrawingEvents();
        AssmEventsAssigned = true;
      } else {
        // We're already set up, I guess.
      }
    }

    private void ConnectAssemblyEvents(ModelDoc2 md) {
      if ((md.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && !AssmEventsAssigned) {
        ad = (AssemblyDoc)md;
        ad.UserSelectionPreNotify += ad_UserSelectionPreNotify;

        // user clicks part/subassembly
        ad.UserSelectionPostNotify += ad_UserSelectionPostNotify;

        // doc closing, I think.
        ad.DestroyNotify2 += ad_DestroyNotify2;

        // Not sure, and not implemented yet.
        ad.ActiveDisplayStateChangePostNotify += ad_ActiveDisplayStateChangePostNotify;

        // switching docs
        ad.ActiveViewChangeNotify += ad_ActiveViewChangeNotify;
        DisconnectDrawingEvents();
        AssmEventsAssigned = true;
      } else {
        // We're already set up, I guess.
      }
    }

    private void ConnectDrawingEvents() {
      if (Document.GetType() == (int)swDocumentTypes_e.swDocDRAWING && !DrawEventsAssigned) {
        dd = (DrawingDoc)Document;
        //dd.ChangeCustomPropertyNotify += dd_ChangeCustomPropertyNotify;
        dd.AddItemNotify += dd_AddItemNotify;
        dd.DestroyNotify2 += dd_DestroyNotify2;
        dd.ViewNewNotify2 += dd_ViewNewNotify2;
        //dd.DestroyNotify += dd_DestroyNotify;
        DisconnectPartEvents();
        DisconnectAssemblyEvents();
        DrawEventsAssigned = true;
      }
    }

    int dd_AddItemNotify(int EntityType, string itemName) {
      drb.Update();
      return 0;
    }
    
    int dd_ViewNewNotify2(object viewBeingAdded) {
      if (viewBeingAdded is ModelDoc2) {
        Document = (ModelDoc2)viewBeingAdded;
      }
      ConnectSelection();
      return 0;
    }

    int dd_DestroyNotify() {
      ConnectSelection();
      return 0;
    }

    int dd_DestroyNotify2(int DestroyType) {
      ClearControls(this);
      return 0;
    }

    int dd_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType) {
      SwProperty p = prop.GetProperty(propName);
      p.Value = NewValue;
      p.Type = (swCustomInfoType_e)valueType;
      drb.Update();
      return 0;
    }

    private void ConnectPartEvents() {
      if (Document.GetType() == (int)swDocumentTypes_e.swDocPART && !PartEventsAssigned) {
        pd = (PartDoc)Document;
        // When the config changes, the app knows.
        pd.ActiveConfigChangePostNotify += pd_ActiveConfigChangePostNotify;
        //pd.ChangeCustomPropertyNotify += pd_ChangeCustomPropertyNotify;
        pd.DestroyNotify2 += pd_DestroyNotify2;
        DisconnectDrawingEvents();
        PartEventsAssigned = true;
      }
    }

    private void ConnectPartEvents(ModelDoc2 md) {
      if (md.GetType() == (int)swDocumentTypes_e.swDocPART && !PartEventsAssigned) {
        pd = (PartDoc)md;
        // When the config changes, the app knows.
        pd.ActiveConfigChangePostNotify += pd_ActiveConfigChangePostNotify;
        //pd.ChangeCustomPropertyNotify += pd_ChangeCustomPropertyNotify;
        pd.DestroyNotify2 += pd_DestroyNotify2;
        //pd.RegenNotify += pd_RegenNotify;
        DisconnectDrawingEvents();
        PartEventsAssigned = true;
      }
    }

    int pd_RegenNotify() {
      if (regen_ok) {
        md_last = null;
        ConnectSelection();
      }
      return 0;
    }
    
    int pd_DestroyNotify2(int DestroyType) {
      ClearControls(this);
      return 0;
    }

    int pd_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType) {
      md_last = null;
      ConnectSelection();
      return 0;
    }

    private void DisconnectAssemblyEvents() {
      // unhook 'em all
      if (AssmEventsAssigned) {
        ad.UserSelectionPreNotify -= ad_UserSelectionPreNotify;
        ad.UserSelectionPostNotify -= ad_UserSelectionPostNotify;
        ad.DestroyNotify2 -= ad_DestroyNotify2;
        ad.ActiveDisplayStateChangePostNotify -= ad_ActiveDisplayStateChangePostNotify;
        ad.ActiveViewChangeNotify -= ad_ActiveViewChangeNotify;
        swSelMgr = null;
      }
      AssmEventsAssigned = false;
      AssySetup = false;
    }

    private void DisconnectPartEvents() {
      // unhook 'em all
      if (PartEventsAssigned) {
        pd.ActiveConfigChangePostNotify -= pd_ActiveConfigChangePostNotify;
        pd.DestroyNotify2 -= pd_DestroyNotify2;
        pd.ChangeCustomPropertyNotify -= pd_ChangeCustomPropertyNotify;
      }
      PartEventsAssigned = false;
      PartSetup = false;
    }

    private void DisconnectDrawingEvents() {
      // unhook 'em all
      if (DrawEventsAssigned) {
        //dd.ChangeCustomPropertyNotify -= dd_ChangeCustomPropertyNotify;
        dd.DestroyNotify2 -= dd_DestroyNotify2;
        dd.ViewNewNotify2 -= dd_ViewNewNotify2;
        //dd.DestroyNotify -= dd_DestroyNotify;
      }
      DrawEventsAssigned = false;
      DrawSetup = false;
    }

    private bool MaybeSave() {
      //swMessageBoxResult_e dr = (swMessageBoxResult_e)SwApp.SendMsgToUser2(Properties.Resources.MaybeSave,
      //    (int)swMessageBoxIcon_e.swMbQuestion,
      //    (int)swMessageBoxBtn_e.swMbYesNo);

      //switch (dr) {
      //  case swMessageBoxResult_e.swMbHitAbort:
      //    break;
      //  case swMessageBoxResult_e.swMbHitCancel:
      //    break;
      //  case swMessageBoxResult_e.swMbHitIgnore:
      //    break;
      //  case swMessageBoxResult_e.swMbHitNo:
      //    return false;
      //  case swMessageBoxResult_e.swMbHitOk:
      //    break;
      //  case swMessageBoxResult_e.swMbHitRetry:
      //    break;
      //  case swMessageBoxResult_e.swMbHitYes:
      //    return true;
      //  default:
      //    return false;
      //}

      return true;
    }

    int ad_ActiveViewChangeNotify() {
      // a different doc is active, let's reconnect.
      Document = SwApp.ActiveDoc;
      ConnectSelection();
      return 0;
    }

    int ad_ActiveDisplayStateChangePostNotify(string DisplayStateName) {
      // gotta figure out when this goes off
      //throw new NotImplementedException();
      return 0;
    }

    int ad_DestroyNotify2(int DestroyType) {
      ClearControls(this);
      // In VBA, it would delete files from the network if the references still pointed to their objects.
      Document = null;
      //SwApp = null;
      return 0;
    }

    int ad_UserSelectionPreNotify(int SelType) {
      return 0;
    }

    int ad_UserSelectionPostNotify() {
      // What do we got?
      swSelComp = swSelMgr.GetSelectedObjectsComponent2(1);
      if (swSelComp != null) {
        // This thing!
        Document = swSelComp.GetModelDoc2();
        // Let's have a look.
        ConnectSelection();
      } else {
        // Nothing's selected?
        Document = SwApp.ActiveDoc;
        // Just look at the root item then.
        ConnectSelection();
      }

      return 0;
    }


    int pd_ActiveConfigChangePostNotify() {
      md_last = null;
      ConnectSelection();
      return 0;
    }

    private void Check() {
      if (Properties.Settings.Default.Warn) {
        string message = string.Empty;
        double o = 0;
        if (double.TryParse(prop.GetProperty("LENGTH").ResValue, out o)) {
          if (o == 0) {
            message += "Length resolves to 0.000\".\n";
          }
        } else {
          message += "Couldn't resolve length.\n";
        }
        if (double.TryParse(prop.GetProperty("WIDTH").ResValue, out o)) {
          if (o == 0) {
            message += "Width resolves to 0.000\".\n";
          }
        } else {
          message += "Couldn't resolve width.\n";
        }

        int m = 0;
        double epsilon = Properties.Settings.Default.CheckEpsilon;
        bool resMat = int.TryParse(prop.GetProperty("MATID").ID, out m);
        double thk = 0.0f;
        double wthk = 0.0f;
        bool thkParsed = double.TryParse(gp.Thickness, out thk);
        bool wthkParsed = double.TryParse(gp.WallThickness, out wthk);
        if ((thkParsed || wthkParsed) && resMat) {
          double thick = 0.0f;
          double.TryParse(prop.cutlistData.GetMaterial(m).Rows[0][(int)CutlistData.MaterialFields.THICKNESS].ToString(), out thick);
          if ((Math.Abs(thick - thk) < epsilon) || (Math.Abs(thick - wthk) < epsilon)) {
            // looks fine
          } else if (thick != 0) {
            message += string.Format("Part thickness ({0}/{1}) doesn't match material thickness ({2}).\n", thk, wthk, thick);
          }
        } else {
          message += string.Format("Couldn't resolve [WALL] THICKNESS or material ID ({0}).\n", m);
        }
        if (message.Length > 0) {
          _swApp.SendMsgToUser2(message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
        }
      }
    }

    public void Write() {
      if (PartSetup) {
        md_last = null;
        regen_ok = false;
        // update doc metadata & rebuild & save
        mrb.Write(Document);
        Check();
        // rescoop new metadata
        ConnectSelection();
        //this.mrb.Update(ref this.prop);
      }

      if (DrawSetup) {
        // update doc metadata & rebuild & save
        drb.Write(Document);
        drb.CorrectLayers(drb.chooseLayer);
      }
    }

    public void Write(SldWorks s) {
      if (PartSetup) {
        md_last = null;
        regen_ok = false;
        // update doc metadata & rebuild & save
        mrb.Write((ModelDoc2)s.ActiveDoc);
        Check();
        // rescoop new metadata
        ConnectSelection();
        //this.mrb.Update(ref this.prop);
      }

      if (DrawSetup) {
        // update doc metadata & rebuild & save
        drb.Write((DrawingDoc)s.ActiveDoc);
      }
    }

    // This is how we get the swapp object down here.
    protected SldWorks RequestSW() {
      if (OnRequestSW == null)
        throw new Exception("No SW!");

      return OnRequestSW();
    }

    public Func<SldWorks> OnRequestSW;

    protected SldWorks _swApp;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }
  }
}
