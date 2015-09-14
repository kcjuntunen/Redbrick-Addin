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

        private bool AssmEventsAssigned = false;
        private bool PartEventsAssigned = false;
        private bool DrawEventsAssigned = false;

        public SWTaskPaneHost()
        {
            InitializeComponent();
        }

        public SWTaskPaneHost(SldWorks sw)
        {
            this.SwApp = sw;
            InitializeComponent();
        }

        public void Start()
        {
            this.SwApp = this.RequestSW();

            this.SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
            this.SwApp.DestroyNotify += SwApp_DestroyNotify;
            this.ConnectOpenDoc();
        }

        int SwApp_DestroyNotify()
        {
            // Solidworks closed
            return 0;
        }

        int SwApp_ActiveDocChangeNotify()
        {
            this.ConnectOpenDoc();
            return 0;
        }

        private void ConnectOpenDoc()
        {
            this.Document = (ModelDoc2)this.SwApp.ActiveDoc;
            this.swSelMgr = Document.SelectionManager;
            swDocumentTypes_e docT = (swDocumentTypes_e)this.Document.GetType();
            switch (docT)
            {
                case swDocumentTypes_e.swDocASSEMBLY:
                    this.SetupAssy();
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    this.SetupDrawing();
                    break;
                case swDocumentTypes_e.swDocNONE:
                    this.SetupOther();
                    break;
                case swDocumentTypes_e.swDocPART:
                    this.SetupPart();
                    break;
                case swDocumentTypes_e.swDocSDM:
                    this.SetupOther();
                    break;
                default:
                    this.SetupOther();
                    break;
            }
        }

        private void ConnectSelection()
        {
            swDocumentTypes_e docT = (swDocumentTypes_e)this.Document.GetType();
            switch (docT)
            {
                case swDocumentTypes_e.swDocASSEMBLY:
                    this.SetupAssy();
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    this.SetupDrawing();
                    break;
                case swDocumentTypes_e.swDocNONE:
                    this.SetupOther();
                    break;
                case swDocumentTypes_e.swDocPART:
                    this.SetupPart();
                    break;
                case swDocumentTypes_e.swDocSDM:
                    this.SetupOther();
                    break;
                default:
                    this.SetupOther();
                    break;
            }

        }

        private void SetupAssy()
        {
            this.ConnectAssemblyEvents();
        }

        private void SetupDrawing()
        {
        }

        private void SetupPart()
        {
        }

        private void SetupOther()
        {            
            this.SwApp.SendMsgToUser2(
                Properties.Resources.MustOpenDocument, 
                (int)swMessageBoxIcon_e.swMbInformation, 
                (int)swMessageBoxBtn_e.swMbOk);
        }

        private void ConnectAssemblyEvents()
        {
            if ((this.Document.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && !this.AssmEventsAssigned)
            {
                AssemblyDoc ad = (AssemblyDoc)this.Document;
                ad.UserSelectionPostNotify += ad_UserSelectionPostNotify;
                ad.DestroyNotify2 += ad_DestroyNotify2;
                ad.ActiveDisplayStateChangePostNotify += ad_ActiveDisplayStateChangePostNotify;
                ad.ActiveViewChangeNotify += ad_ActiveViewChangeNotify;
            }
            else
            {

            }
        }

        private void DisconnectAssemblyEvents()
        {
            
        }

        int ad_ActiveViewChangeNotify()
        {
            this.ConnectOpenDoc();
            return 0;
        }

        int ad_ActiveDisplayStateChangePostNotify(string DisplayStateName)
        {
            throw new NotImplementedException();
        }

        int ad_DestroyNotify2(int DestroyType)
        {
            this.Document = null;
            this.SwApp = null;
            return 0;
        }

        int ad_UserSelectionPostNotify()
        {
            this.swSelComp = swSelMgr.GetSelectedObjectsComponent2(1);
            if (swSelComp != null)
            {
                this.Document = this.swSelComp.GetModelDoc2();
                this.ConnectSelection();
            }
            else
            {
                this.Document = this.SwApp.ActiveDoc;
                this.ConnectOpenDoc();
            }

            //ConfigurationManager cm = default(ConfigurationManager);
            //Configuration specConf = default(Configuration);
            //CustomPropertyManager gcpm = default(CustomPropertyManager);
            //CustomPropertyManager scpm = default(CustomPropertyManager);

            //if (swSelMgr != null)
            //{
            //    //object swSelObj = (ModelDoc2)swSelMgr.GetSelectedObject5(1);
            //    if (swSelComp != null)
            //    {
            //        this.Document = swSelComp.GetModelDoc2();
            //        cm = this.Document.ConfigurationManager;
            //        specConf = cm.ActiveConfiguration;
            //        gcpm = this.Document.Extension.get_CustomPropertyManager(string.Empty);
            //        scpm = this.Document.Extension.get_CustomPropertyManager(specConf.Name);
            //        this.textBox1.Text += string.Format("{0}: {1}\n", specConf.Name, this.swSelComp.GetPathName());
            //        this.ConnectSelection();
            //    }
            //    else
            //    {
            //        this.Document = this.SwApp.ActiveDoc;
            //        specConf = Document.GetActiveConfiguration();
            //        gcpm = this.Document.Extension.get_CustomPropertyManager(string.Empty);
            //        scpm = this.Document.Extension.get_CustomPropertyManager(specConf.Name);
            //        this.textBox1.Text += string.Format("{0}: {1}\n", specConf.Name, this.Document.GetPathName());
            //        this.ConnectOpenDoc();
            //    }
            //}

            return 0;
        }

        private void ConnectDrawingEvents()
        {
            if (this.Document.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
            {
                DrawingDoc dd = (DrawingDoc)this.Document;
                

            }
        }

        private void ConnectPartEvents()
        {
            if (this.Document.GetType() == (int)swDocumentTypes_e.swDocPART)
            {
                PartDoc pd = (PartDoc)this.Document;
            }
        }

        protected SldWorks RequestSW()
        {
            if (OnRequestSW == null)
                throw new Exception("No SW!");

            return OnRequestSW();
        }

        public Func<SldWorks> OnRequestSW;

        public SldWorks SwApp { get; set; }
        public Redbrick MyParent { get; set; }
    }
}
