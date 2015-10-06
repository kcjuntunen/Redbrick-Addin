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
using SolidWorksTools;

using System.Runtime.InteropServices;

namespace Redbrick_Addin
{
    [ComVisible(true)]
    [ProgId(SWTASKPANE_PROGID)]
    public partial class SWTaskPaneHost : UserControl
    {
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

        public SWTaskPaneHost()
        {
            InitializeComponent();
        }

        public void Start()
        {
            this.SwApp = this.RequestSW();
            this.prop = new SwProperties(this._swApp);
            this.SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
            this.SwApp.DestroyNotify += SwApp_DestroyNotify;
            this.SwApp.FileCloseNotify += SwApp_FileCloseNotify;
            this.Document = this.SwApp.ActiveDoc;
            if (this.Document != null)
            {
                this.ConnectSelection();
            }
        }

        int SwApp_FileCloseNotify(string FileName, int reason)
        {
            if (this.PartSetup)
                if (this.mrb.IsDirty)
                    if (MaybeSave())
                        this.Write();

            if (this.DrawSetup)
                if (this.drb.IsDirty)
                    if (MaybeSave())
                        this.Write();

            return 0;
        }

        int SwApp_DestroyNotify()
        {
            if (this.PartSetup)
                if (this.mrb.IsDirty)
                    if (MaybeSave())
                        this.Write();

            if (this.DrawSetup)
                if (this.drb.IsDirty)
                    if (MaybeSave())
                        this.Write();

            // Solidworks closed
            return 0;
        }

        int SwApp_ActiveDocChangeNotify()
        {
            if (this.PartSetup)
            {
                if (this.mrb.IsDirty)
                    if (MaybeSave())
                        this.Write();
            }

            if (this.DrawSetup)
                if (this.drb.IsDirty)
                    if (MaybeSave())
                        this.Write();

            this.ClearControls(this);                   // SW level doc switching

            if (this.SwApp == null)
                this.SwApp = this.RequestSW();

            this.Document = this.SwApp.ActiveDoc;
            this.ConnectSelection();
            return 0;
        }

         private void ConnectSelection()
        {
            this.prop.Clear();                                                      // Blow out the propertyset so we can get new ones.
            System.IO.FileInfo fi = 
                new System.IO.FileInfo(this.Document.GetPathName());
            this.prop.PartName = fi.Name.Split(' ', '.')[0];
            this.prop.GetPropertyData(this.Document);                               // get new ones.
            swDocumentTypes_e docT = (swDocumentTypes_e)this.Document.GetType();    // what sort of doc is open?
            switch (docT)
            {
                case swDocumentTypes_e.swDocASSEMBLY:
                    if (!this.PartSetup)
                    {
                        this.SetupPart();                                           // Part/assembly props are about the same
                        this.mrb.Update(ref this.prop);                             // link whatever's in the prop to the controls
                    }
                    else
                    {
                        this.mrb.Update(ref this.prop);                             // already set up. We can re-link with out re-setting-up.
                    }
                    if (!AssySetup)
                        this.SetupAssy();                                           // this pretty much only assigns the selection manager and sets up
                    break;                                                          // events
                case swDocumentTypes_e.swDocDRAWING:
                    this.SetupDrawing();                                            // set up and link all controls, and any events.
                    break;
                case swDocumentTypes_e.swDocNONE:
                    this.SetupOther();                                              // What's this? 
                    break;
                case swDocumentTypes_e.swDocPART:
                    if (!this.PartSetup)
                    {
                        this.SetupPart();                                           // setup
                        this.mrb.Update(ref this.prop);                             // link
                    }
                    else
                    {
                        this.mrb.Update(ref this.prop);                             // or just link
                    }
                    break;
                case swDocumentTypes_e.swDocSDM:
                    this.SetupOther();                                              // What's this?
                    break;
                default:
                    this.SetupOther();                                              // OK, whatever.
                    break;
            }
        }

        private void SetupAssy()
        {
            this.swSelMgr = Document.SelectionManager;      // Get a selection manager.
            this.ConnectAssemblyEvents();                   // connect events
            this.AssySetup = true;                          // Zap!
        }

        private void SetupDrawing()
        {
            drb = new DrawingRedbrick(this._swApp);         // New drawing handler. It can use the whole swapp since it doesn't have to figure out the config.
            this.Controls.Add(drb);                         // Add it to the taskpane
            foreach (Control item in this.Controls)         // whatever's in here, make it dock.
            {
                item.Dock = DockStyle.Fill;
            }
            this.ConnectDrawingEvents();                    // drawing related events (spoiler: there aren't any)
            this.DrawSetup = true;                          // Pow-bang! We're set up.
        }

        private void SetupPart()
        {
            this.ClearControls(this);                       // Blow out any existing controls, and dump events.
            DockStyle d = DockStyle.Fill;                   // Fill everything so it stretches.

            this.mrb = new ModelRedbrick(ref this.prop);    // New model handler with current property (aquired in this.Connect...())
            this.mrb.Dock = d;                              // If it's not docked, dock it.
            this.Controls.Add(mrb);                         // put the redbrick in this control
            this.Dock = d;                                  // Dock this control in the taskpane.

            foreach (Control item in mrb.Controls)
            {
                item.Dock = d;
                item.ResumeLayout(true);
            }
            this.ResumeLayout(true);

            this.ds = mrb.aDepartmentSelector;              // Gonna use access to all these controls.
            this.cs = mrb.aConfigurationSpecific;
            this.gp = mrb.aGeneralProperties;
            this.mp = mrb.aMachineProperties;
            this.op = mrb.aOps;

            this.ConnectPartEvents();                       // Part-related events.
            this.PartSetup = true;                          // Boom, we're set up.
        }

        private void ClearControls(Control c)
        {
            foreach (Control item in c.Controls)            // any controls, no matter how deep, g'bye.
            {
                if (item.HasChildren) /* Recurse */
                {
                    this.ClearControls(item);
                }
                c.Controls.Remove(item);
            }

            this.DisconnectAssemblyEvents();
            this.DisconnectPartEvents();
            this.DisconnectDrawingEvents();
            this.PartSetup = false;                         // everything's undone.
            this.DrawSetup = false;
            this.AssySetup = false;
        }

        private void SetupOther()                           // if this ever comes up, I suppose it's because the wrong kind of
        {                                                   // doc is open.
            this.SwApp.SendMsgToUser2(
                Properties.Resources.MustOpenDocument, 
                (int)swMessageBoxIcon_e.swMbInformation, 
                (int)swMessageBoxBtn_e.swMbOk);
        }

        private void ConnectAssemblyEvents()
        {
            if ((this.Document.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && !this.AssmEventsAssigned)
            {
                ad = (AssemblyDoc)this.Document;
                ad.UserSelectionPreNotify += ad_UserSelectionPreNotify;
                ad.UserSelectionPostNotify += ad_UserSelectionPostNotify;                           // user clicks part/subassembly
                ad.DestroyNotify2 += ad_DestroyNotify2;                                             // doc closing, I think.
                ad.ActiveDisplayStateChangePostNotify += ad_ActiveDisplayStateChangePostNotify;     // Not sure, and not implemented yet.
                ad.ActiveViewChangeNotify += ad_ActiveViewChangeNotify;                             // switching docs
                this.AssmEventsAssigned = true;
            }
            else
            {
                // We're already set up, I guess.
            }
        }

        private void ConnectDrawingEvents()
        {
            if (this.Document.GetType() == (int)swDocumentTypes_e.swDocDRAWING && !this.DrawEventsAssigned)
            {
                dd = (DrawingDoc)this.Document;                                                     // Dunno yet.
                this.DrawEventsAssigned = true;
            }
        }

        private void ConnectPartEvents()
        {
            if (this.Document.GetType() == (int)swDocumentTypes_e.swDocPART && !this.PartEventsAssigned)
            {
                pd = (PartDoc)this.Document;
                pd.ActiveConfigChangePostNotify += pd_ActiveConfigChangePostNotify;                 // When the config changes, the app knows.
                this.PartEventsAssigned = true;
            }
        }

        private void DisconnectAssemblyEvents()
        {
            if (this.AssmEventsAssigned)                                                            // unhook 'em all
            {
                ad.UserSelectionPreNotify -= ad_UserSelectionPreNotify;
                ad.UserSelectionPostNotify -= ad_UserSelectionPostNotify;
                ad.DestroyNotify2 -= ad_DestroyNotify2;
                ad.ActiveDisplayStateChangePostNotify -= ad_ActiveDisplayStateChangePostNotify;
                ad.ActiveViewChangeNotify -= ad_ActiveViewChangeNotify;
                this.AssmEventsAssigned = false;
            }
        }

        private void DisconnectPartEvents()
        {
            if (this.PartEventsAssigned)                                                            // unhook 'em all
            {
                pd.ActiveConfigChangePostNotify -= pd_ActiveConfigChangePostNotify;
                this.PartEventsAssigned = false;
            }
        }

        private void DisconnectDrawingEvents()
        {
            if (this.DrawEventsAssigned)                                                            // unhook 'em all
            {
                this.DrawEventsAssigned = false;
            }
        }

        private bool MaybeSave()
        {
            swMessageBoxResult_e dr = (swMessageBoxResult_e)this.SwApp.SendMsgToUser2(Properties.Resources.MaybeSave, 
                (int)swMessageBoxIcon_e.swMbQuestion, 
                (int)swMessageBoxBtn_e.swMbYesNo);

            switch (dr)
            {
                case swMessageBoxResult_e.swMbHitAbort:
                    break;
                case swMessageBoxResult_e.swMbHitCancel:
                    break;
                case swMessageBoxResult_e.swMbHitIgnore:
                    break;
                case swMessageBoxResult_e.swMbHitNo:
                    return false;
                case swMessageBoxResult_e.swMbHitOk:
                    break;
                case swMessageBoxResult_e.swMbHitRetry:
                    break;
                case swMessageBoxResult_e.swMbHitYes:
                    return true;
                default:
                    return false;
            }

            return false;
        }

        int ad_ActiveViewChangeNotify()
        {
            if (this.mrb.IsDirty)
                if (MaybeSave())
                    this.Write();

            this.Document = this.SwApp.ActiveDoc;                                                   // a different doc is active,
            this.ConnectSelection();                                                                // let's reconnect.
            return 0;                                                                               // 
        }

        int ad_ActiveDisplayStateChangePostNotify(string DisplayStateName)
        {
            throw new NotImplementedException();                                                    // gotta figure out when this goes off
        }

        int ad_DestroyNotify2(int DestroyType)
        {
            if (this.mrb.IsDirty)
                if (MaybeSave())
                    this.Write();

            this.Document = null;                                                                   // In VBA, it would delete files from the
            this.SwApp = null;                                                                      // network if the references still pointed to their objects.
            return 0;                                                                               // 
        }

        int ad_UserSelectionPreNotify(int SelType)
        {
            return 0;
        }

        int ad_UserSelectionPostNotify()
        {
            if (this.mrb.IsDirty)
                if (MaybeSave())
                    this.Write();

            this.swSelComp = swSelMgr.GetSelectedObjectsComponent2(1);                              // What do we got?
            if (swSelComp != null)
            {
                this.Document = this.swSelComp.GetModelDoc2();                                      // This thing!
                this.ConnectSelection();                                                            // Let's have a look.
            }
            else
            {
                this.Document = this.SwApp.ActiveDoc;                                               // Nothing's selected? 
                this.ConnectSelection();                                                            // Just look at the root item then.
            }

            return 0;                                                                               // 
        }


        int pd_ActiveConfigChangePostNotify()
        {
            if (this.mrb.IsDirty)
                if (MaybeSave())
                    this.Write();

            this.ConnectSelection();                                                                // Different config! Look again!
            return 0;
        }

        public void Write()
        {
            if (this.PartSetup)
            {
                this.mrb.Write(this.Document);                                                      // update doc metadata & rebuild & save
                this.ConnectSelection();                                                            // rescoop new metadata
                this.mrb.Update(ref this.prop);                                                     // show it
            }

            if (this.DrawSetup)
            {
                this.drb.Write(this.Document);                                                      // update doc metadata & rebuild & save
            }
        }

        public void Write(SldWorks s)
        {
            if (this.PartSetup)
            {
                this.mrb.Write((ModelDoc2)s.ActiveDoc);                                             // update doc metadata & rebuild & save
                this.ConnectSelection();                                                            // rescoop new metadata
                this.mrb.Update(ref this.prop);                                                     // show it
            }

            if (this.DrawSetup)
            {
                this.drb.Write((DrawingDoc)s.ActiveDoc);                                            // update doc metadata & rebuild & save
            }
        }

        protected SldWorks RequestSW()                                                              // This is how we get the swapp object down here.
        {
            if (OnRequestSW == null)
                throw new Exception("No SW!");

            return OnRequestSW();
        }

        public Func<SldWorks> OnRequestSW;

        protected SldWorks _swApp;

        public SldWorks SwApp 
        { 
            get { return this._swApp; } 
            set { this._swApp = value; }
        }
    }
}
