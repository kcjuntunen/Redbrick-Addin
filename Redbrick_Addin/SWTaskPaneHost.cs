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
        public ModelDoc2 Document;

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
            this.ConnectOpenDoc();
        }

        private void ConnectOpenDoc()
        {
            Document = (ModelDoc2)this.RequestSW().ActiveDoc;
            swDocumentTypes_e docT = (swDocumentTypes_e)Document.GetType();
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
            this.button1.Text = "Assembly";
        }

        private void SetupDrawing()
        {
            this.button1.Text = "Drawing";
        }

        private void SetupPart()
        {
            this.button1.Text = "Part";
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
            if (this.Document.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
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
            throw new NotImplementedException();
        }

        int ad_UserSelectionPostNotify()
        {
            throw new NotImplementedException();
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
