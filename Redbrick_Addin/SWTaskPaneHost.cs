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

    /// <summary>
    /// After this object is created and some variables defined, we fire things up.
    /// </summary>
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

    /// <summary>
    /// I don't know how often this is used, but, the Redbrick (used to?) occasionally stop updating.
    /// So I created this function to re-fire things up. I hardly use it manually anymore.
    /// </summary>
    public void ReStart() {
      ClearControls(this);

      md_last = null;
      if (SwApp == null)
        SwApp = RequestSW();
      ConnectSelection();
    }

    /// <summary>
    /// This function uses this.ReStart(). Closing docs and switching to lightweight models screws stuff up,
    /// so I handle them specially.
    /// HINT: Lightweight models have no properties.
    /// </summary>
    /// <param name="Command">What command did SW receive from the user?</param>
    /// <param name="reason">I don't use this.</param>
    /// <returns>Nothing useful, unless you want a 0.</returns>
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

    /// <summary>
    /// Let's not leave the Redbrick looking useable.
    /// </summary>
    /// <param name="FileName">Unused</param>
    /// <param name="reason">Unused</param>
    /// <returns>A 0.</returns>
    int SwApp_FileCloseNotify(string FileName, int reason) {
      ClearControls(this);
      return 0;
    }

    /// <summary>
    /// Solidworks is closed.
    /// </summary>
    /// <returns>0!</returns>
    int SwApp_DestroyNotify() {
      ClearControls(this);
      return 0;
    }

    /// <summary>
    /// Toss out old events and reconnect the Redbrick to the new doc.
    /// </summary>
    /// <returns>0. Every time.</returns>
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

    /// <summary>
    /// Check for an existing hash. We basically want it to be fresh every time.
    /// This would be a good place to abuse the Redbrick, if you had a mind to.
    /// </summary>
    private void AddHash() {
      System.IO.FileInfo fi;
      try {
        if (!string.IsNullOrWhiteSpace(Document.GetPathName())) {
          fi = new System.IO.FileInfo(Document.GetPathName());
          prop.PartFileInfo = fi;

          if (!prop.Contains("CRC32")) {
            prop.Hash = Redbrick.GetHash(string.Format("{0}\\{1}", prop.PartFileInfo.Directory.FullName, prop.PartFileInfo.Name));
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

    /// <summary>
    /// Run this function after this.Document is populated. It fills two ref vars with swDocumentTypes_e.
    /// </summary>
    /// <param name="d">The document type.</param>
    /// <param name="od">The top-level document type.</param>
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

    /// <summary>
    /// As yet unused warning.
    /// </summary>
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

    /// <summary>
    /// Overgrown monster function that does everything. We need some refactoring here.
    /// </summary>
    public void ConnectSelection() {
      if (Properties.Settings.Default.IdiotLight) {
        if (PartSetup && mrb.IsDirty) {
          if (System.Windows.Forms.MessageBox.Show(@"Save property changes?", @"Properties changed", MessageBoxButtons.YesNo) == DialogResult.Yes)
            Write();
        }
        if (DrawSetup && drb.IsDirty) {
          if (System.Windows.Forms.MessageBox.Show(@"Save property changes?", @"Properties changed", MessageBoxButtons.YesNo) == DialogResult.Yes)
            Write();
        }
      }

      // Since I create objects and just toss them out liberally, I want to force garbage collection.
      // I think this leaves fewer GDI objects around. (This seems to be a good thing)
      System.GC.Collect(2, GCCollectionMode.Forced);
      prop.CutlistID = 0;
      if (Document != null) {
        // ignore clicks on the same object
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
              // a switch in a switch!
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

    /// <summary>
    /// Use this if we're looking at an assembly document.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    private void SetupAssy(ModelDoc2 md) {
      // Get a selection manager.
      swSelMgr = md.SelectionManager;

      // connect events
      ConnectAssemblyEvents(md);

      // Zap!
      AssySetup = true;
    }

    /// <summary>
    /// Use this if we're looking at a drawing document.
    /// </summary>
    private void SetupDrawing() {
      // New drawing handler. It can use the whole swapp since it doesn't have to figure out the config.
      drb = new DrawingRedbrick(_swApp);

      // Add it to the taskpane
      Controls.Add(drb);

      // whatever's in here, make it dock.
      // TODO: This is kinda stupid. It should be fixed 9-19-2016.
      foreach (Control item in Controls)
        item.Dock = DockStyle.Fill;

      // drawing related events (spoiler: there aren't any)
      ConnectDrawingEvents();
      // Pow-bang! We're set up.
      DrawSetup = true;
      AssySetup = false;
      PartSetup = false;
      drb.unselect();
    }

    /// <summary>
    /// Use this if we're looking at a part document. But it's also used were we're looking
    /// at an assembly because we use the same properties.
    /// </summary>
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

    /// <summary>
    /// Use this if we're looking at a part document. But it's also used were we're looking
    /// at an assembly because we use the same properties.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
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

    /// <summary>
    /// Blank out the Redbrick
    /// </summary>
    /// <param name="c">A Control object.</param>
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

    /// <summary>
    /// If this ever comes up, I suppose it's because the wrong kind of doc is open.
    /// </summary>
    private void SetupOther() {
      // Blow out any existing controls, and dump events.
      ClearControls(this);
      SwApp.SendMsgToUser2(
          Properties.Resources.MustOpenDocument,
          (int)swMessageBoxIcon_e.swMbInformation,
          (int)swMessageBoxBtn_e.swMbOk);
    }

    /// <summary>
    /// Hook up events for the assembly context.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
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

    /// <summary>
    /// Hook up events in the drawing context.
    /// </summary>
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

    /// <summary>
    /// This proved to be annoying.
    /// </summary>
    /// <param name="EntityType">Unused</param>
    /// <param name="itemName">Unused</param>
    /// <returns>Every time, a 0!</returns>
    int dd_AddItemNotify(int EntityType, string itemName) {
      //drb.DrbUpdate();
      return 0;
    }

    /// <summary>
    /// If you have a blank drawing, conditions change as soon as you add a view. Alert the Redbrick!
    /// </summary>
    /// <param name="viewBeingAdded">Unused</param>
    /// <returns>0</returns>
    int dd_ViewNewNotify2(object viewBeingAdded) {
      if (viewBeingAdded is ModelDoc2) {
        Document = (ModelDoc2)viewBeingAdded;
      }
      ConnectSelection();
      return 0;
    }

    /// <summary>
    /// Update when the drawing is gone.
    /// </summary>
    /// <returns>0</returns>
    int dd_DestroyNotify() {
      ConnectSelection();
      return 0;
    }

    /// <summary>
    /// Dump controls if there's no longer any reason to have them.
    /// </summary>
    /// <param name="DestroyType">Unused</param>
    /// <returns>0</returns>
    int dd_DestroyNotify2(int DestroyType) {
      ClearControls(this);
      return 0;
    }

    /// <summary>
    /// This never really worked the way I hoped.
    /// </summary>
    /// <param name="propName">Prop name</param>
    /// <param name="Configuration">Configuration</param>
    /// <param name="oldValue">unused</param>
    /// <param name="NewValue">New Value</param>
    /// <param name="valueType">Type of value</param>
    /// <returns>0</returns>
    int dd_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType) {
      SwProperty p = prop.GetProperty(propName);
      p.Value = NewValue;
      p.Type = (swCustomInfoType_e)valueType;
      //drb.DrbUpdate();
      return 0;
    }

    /// <summary>
    /// Hook up events in the part document context.
    /// </summary>
    private void ConnectPartEvents() {
      if (Document.GetType() == (int)swDocumentTypes_e.swDocPART && !PartEventsAssigned) {
        pd = (PartDoc)Document;
        // When the config changes, the app knows.
        pd.ActiveConfigChangePostNotify += pd_ActiveConfigChangePostNotify;
        pd.FileSavePostNotify += pd_FileSavePostNotify;
        //pd.ChangeCustomPropertyNotify += pd_ChangeCustomPropertyNotify;
        pd.DestroyNotify2 += pd_DestroyNotify2;
        DisconnectDrawingEvents();
        PartEventsAssigned = true;
      }
    }

    /// <summary>
    /// Update after save. We get a filename and a hash when we've saved something that was unsaved.
    /// </summary>
    /// <param name="saveType">unused</param>
    /// <param name="FileName">unused</param>
    /// <returns>0</returns>
    int pd_FileSavePostNotify(int saveType, string FileName) {
      md_last = null;
      ConnectSelection();
      return 0;
    }

    /// <summary>
    /// Hook up events in the part document context.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    private void ConnectPartEvents(ModelDoc2 md) {
      if (md.GetType() == (int)swDocumentTypes_e.swDocPART && !PartEventsAssigned) {
        pd = (PartDoc)md;
        // When the config changes, the app knows.
        pd.ActiveConfigChangePostNotify += pd_ActiveConfigChangePostNotify;
        pd.FileSavePostNotify += pd_FileSavePostNotify;
        //pd.ChangeCustomPropertyNotify += pd_ChangeCustomPropertyNotify;
        pd.DestroyNotify2 += pd_DestroyNotify2;
        //pd.RegenNotify += pd_RegenNotify;
        DisconnectDrawingEvents();
        PartEventsAssigned = true;
      }
    }

    /// <summary>
    /// Update on regen.
    /// </summary>
    /// <returns>0</returns>
    int pd_RegenNotify() {
      if (regen_ok) {
        md_last = null;
        ConnectSelection();
      }
      return 0;
    }

    /// <summary>
    /// Update on destruction of object.
    /// </summary>
    /// <param name="DestroyType">unused</param>
    /// <returns>0</returns>
    int pd_DestroyNotify2(int DestroyType) {
      ClearControls(this);
      return 0;
    }

    /// <summary>
    /// This has never worked the way I hoped it would.
    /// </summary>
    /// <param name="propName">Prop name</param>
    /// <param name="Configuration">Configuration</param>
    /// <param name="oldValue">unused</param>
    /// <param name="NewValue">New Value</param>
    /// <param name="valueType">Type of value</param>
    /// <returns>0</returns>
    int pd_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType) {
      md_last = null;
      ConnectSelection();
      return 0;
    }

    /// <summary>
    /// Unhook events in the assembly context.
    /// </summary>
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

    /// <summary>
    /// Unhook events in the part context.
    /// </summary>
    private void DisconnectPartEvents() {
      // unhook 'em all
      if (PartEventsAssigned) {
        pd.ActiveConfigChangePostNotify -= pd_ActiveConfigChangePostNotify;
        pd.DestroyNotify2 -= pd_DestroyNotify2;
        pd.ChangeCustomPropertyNotify -= pd_ChangeCustomPropertyNotify;
        pd.FileSavePostNotify -= pd_FileSavePostNotify;
      }
      PartEventsAssigned = false;
      PartSetup = false;
    }

    /// <summary>
    /// Unhook events in the drawing context.
    /// </summary>
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

    /// <summary>
    /// A different doc is active, let's reconnect.
    /// </summary>
    /// <returns>0</returns>
    int ad_ActiveViewChangeNotify() {
      Document = SwApp.ActiveDoc;
      ConnectSelection();
      return 0;
    }

    int ad_ActiveDisplayStateChangePostNotify(string DisplayStateName) {
      // gotta figure out when this goes off
      //throw new NotImplementedException();
      return 0;
    }

    /// <summary>
    /// In VBA, it would delete files from the network if the references still pointed to their objects. Just being superstitious.
    /// </summary>
    /// <param name="DestroyType">unused</param>
    /// <returns>0</returns>
    int ad_DestroyNotify2(int DestroyType) {
      ClearControls(this);
      Document = null;
      return 0;
    }

    /// <summary>
    /// Nothing! Haha!
    /// </summary>
    /// <param name="SelType">unused</param>
    /// <returns>0</returns>
    int ad_UserSelectionPreNotify(int SelType) {
      return 0;
    }

    /// <summary>
    /// On click, update Redbrick. 
    /// 
    /// This is where we used to wait 10-20 seconds for the built-in property manager. 
    /// 
    /// If you were wondering, people don't get raises for that kind of increase in productivity. 
    /// It pays the same as sitting in front of a brake all day. I'm considering bringing the
    /// wait back. We just need this:
    /// System.Threading.Thread.Sleep(10000);
    /// </summary>
    /// <returns>0</returns>
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

    /// <summary>
    /// I put all the L x W x T x WT validation in here. Refactoring needed.
    /// </summary>
    private void Check() {
      bool exclude = !(Document.GetType() == (int)swDocumentTypes_e.swDocPART) ||
        ((Document.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) &&
        Properties.Settings.Default.WarnExcludeAssy);
      if (!exclude) {
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
    }

    /// <summary>
    /// Write to property set, and update Redbrick.
    /// </summary>
    public void Write() {
      if (PartSetup) {
        md_last = null;
        regen_ok = false;
        // update doc metadata & rebuild & save
        mrb.Write(Document);
        mrb.IsDirty = false;
        Check();
        // rescoop new metadata
        ConnectSelection();
        //this.mrb.Update(ref this.prop);
      }

      if (DrawSetup) {
        // update doc metadata & rebuild & save
        drb.Write(Document);
        drb.IsDirty = false;
        drb.CorrectLayers(drb.chooseLayer);
      }
    }

    /// <summary>
    /// This is how we get the swapp object down here.
    /// </summary>
    /// <returns>A SldWorks object.</returns>
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
