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

        DepartmentSelector ds;
        ConfigurationSpecific cs;
        GeneralProperties gp;
        MachineProperties mp;
        Ops op;

        private bool AssmEventsAssigned = false;
        private bool PartEventsAssigned = false;
        //private bool DrawEventsAssigned = false;

        private bool PartSetup = false;
        private bool DrawSetup = false;

        public SWTaskPaneHost()
        {
            InitializeComponent();
        }

        public void Start()
        {
            this.SwApp = this.RequestSW();

            this.SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
            this.SwApp.DestroyNotify += SwApp_DestroyNotify;
            this.Controls.Add(new ModelRedbrick(this._swApp));
            //this.ConnectOpenDoc();
        }

        int SwApp_DestroyNotify()
        {
            // Solidworks closed
            return 0;
        }

        int SwApp_ActiveDocChangeNotify()
        {
            this.ClearControls();
            this.ConnectOpenDoc();
            return 0;
        }

        private void ConnectOpenDoc()
        {
            if (this.SwApp != null)
            {
                this.Document = (ModelDoc2)this.SwApp.ActiveDoc;
                this.prop = new SwProperties(this._swApp);
                this.prop.GetPropertyData(this.Document);
                if (this.Document != null)
                {
                    this.swSelMgr = Document.SelectionManager;
                    swDocumentTypes_e docT = (swDocumentTypes_e)this.Document.GetType();
                    switch (docT)
                    {
                        case swDocumentTypes_e.swDocASSEMBLY:
                            this.SetupAssy();
                            if (!this.PartSetup)
                            {
                                this.SetupPart();
                                this.LinkControls(cs, gp, mp, op);
                            }
                            else
                            {
                                this.LinkControls(cs, gp, mp, op);
                            }
                            break;
                        case swDocumentTypes_e.swDocDRAWING:
                            this.SetupDrawing();   
                            break;
                        case swDocumentTypes_e.swDocNONE:
                            this.SetupOther();
                            break;
                        case swDocumentTypes_e.swDocPART:
                            if (!this.PartSetup)
                            {
                                this.SetupPart();
                                this.LinkControls(cs, gp, mp, op);
                            }
                            else
                            {
                                this.LinkControls(cs, gp, mp, op);
                            }
                            break;
                        case swDocumentTypes_e.swDocSDM:
                            this.SetupOther();
                            break;
                        default:
                            this.SetupOther();
                            break;
                    }   
                }
                else
                {
                    throw new NullReferenceException("You need to have a document opened.");
                }
            }
        }

        private void ConnectSelection()
        {
            this.prop = new SwProperties(this._swApp);
            this.prop.GetPropertyData(this.Document);
            swDocumentTypes_e docT = (swDocumentTypes_e)this.Document.GetType();
            switch (docT)
            {
                case swDocumentTypes_e.swDocASSEMBLY:
                    if (!this.PartSetup)
                    {
                        this.SetupPart();
                        this.LinkControls(cs, gp, mp, op);
                    }
                    else
                    {
                        this.LinkControls(cs, gp, mp, op);
                    }
                    this.SetupAssy();
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    this.SetupDrawing();
                    break;
                case swDocumentTypes_e.swDocNONE:
                    this.SetupOther();
                    break;
                case swDocumentTypes_e.swDocPART:
                    if (!this.PartSetup)
                    {
                        this.SetupPart();
                        this.LinkControls(cs, gp, mp, op);
                    }
                    else
                    {
                        this.LinkControls(cs, gp, mp, op);
                    }
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
            this.SetupPart();
        }

        private void SetupDrawing()
        {
            this.Controls.Add(new DrawingRedbrick(this._swApp));
            foreach (Control item in this.Controls)
            {
                item.Dock = DockStyle.Fill;
            }
            this.DrawSetup = true;
        }

        private void SetupPart()
        {
            DockStyle d = DockStyle.Fill;
            this.ClearControls();
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.AutoScroll = true;
            tlp.AutoSize = true;
            //tlp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlp.Dock = d;
            tlp.ColumnCount = 1;
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            //tlp.RowCount = 5;
            //tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); /* Selector */
            //tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); /* Specific */
            //tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); /* General */
            //tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); /* Machine */
            //tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); /* Ops */
            tlp.Size = new System.Drawing.Size();
            tlp.TabIndex = 1;
            tlp.Tag = string.Empty;

            this.ds = new DepartmentSelector(ref prop);
            this.cs = new ConfigurationSpecific(ref prop);
            this.gp = new GeneralProperties(ref prop);
            this.mp = new MachineProperties(ref prop);
            this.op = new Ops(ref prop);

            if (ds != null && !this.PartEventsAssigned)
            {
                this.PartEventsAssigned = true;
                this.ds.CheckedChanged += ds_CheckedChanged;
            }

            tlp.Controls.Add(ds, 0, 0);
            tlp.Controls.Add(cs, 0, 1);
            tlp.Controls.Add(gp, 0, 2);
            tlp.Controls.Add(mp, 0, 3);
            tlp.Controls.Add(op, 0, 4);

            foreach (Control item in tlp.Controls)
            {
                item.Dock = d;
            }
            this.LinkControls(cs, gp, mp, op);
            this.Controls.Add(tlp);

            tlp.ResumeLayout(true);
            this.ResumeLayout(true);

            this.PartSetup = true;
        }

        void ds_CheckedChanged(object sender, EventArgs e)
        {
            op.OpType = ds.OpType;
            op.RefreshOps(ds.OpType);
            cs.ToggleFields(ds.OpType);
            gp.ToggleFields(ds.OpType);
        }

        private void ClearControls()
        {
            this.PartSetup = false;
            this.DrawSetup = false;
            foreach (Control item in this.Controls)
            {
                this.Controls.Remove(item);
            }
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
                ad = (AssemblyDoc)this.Document;
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

        public void Write()
        {
            this.prop.ReadControls();
            System.Windows.Forms.MessageBox.Show(this.prop.ToString());
            this.prop.Write(this.Document);
        }

        private void LinkControls(ConfigurationSpecific cs, GeneralProperties gp, MachineProperties mp, Ops op)
        {

            this.LinkControlToProperty("CUTLIST MATERIAL", cs.GetCutlistMatBox());
            this.LinkControlToProperty("EDGE FRONT (L)", cs.GetEdgeFrontBox());
            this.LinkControlToProperty("EDGE BACK (L)", cs.GetEdgeBackBox());
            this.LinkControlToProperty("EDGE LEFT (W)", cs.GetEdgeLeftBox());
            this.LinkControlToProperty("EDGE RIGHT (W)", cs.GetEdgeRightBox());

            this.LinkControlToProperty("Description", gp.GetDescriptionBox());
            this.LinkControlToProperty("LENGTH", gp.GetLengthBox());
            gp.UpdateLengthRes(this.prop.GetProperty("LENGTH"));
            this.LinkControlToProperty("WIDTH", gp.GetWidthBox());
            gp.UpdateWidthRes(this.prop.GetProperty("WIDTH"));
            this.LinkControlToProperty("THICKNESS", gp.GetThicknessBox());
            gp.UpdateThickRes(this.prop.GetProperty("THICKNESS"));
            this.LinkControlToProperty("WALL THICKNESS", gp.GetWallThicknessBox());
            gp.UpdateWallThickRes(this.prop.GetProperty("WALL THICKNESS"));
            this.LinkControlToProperty("COMMENT", gp.GetCommentBox());
            this.LinkControlToProperty("BLANK QTY", mp.GetPartsPerBlankBox());
            this.LinkControlToProperty("CNC1", mp.GetCNC1Box());
            this.LinkControlToProperty("CNC2", mp.GetCNC2Box());
            this.LinkControlToProperty("OVERL", mp.GetOverLBox());
            this.LinkControlToProperty("OVERW", mp.GetOverWBox());
            this.LinkControlToProperty("OP1", op.GetOp1Box());
            this.LinkControlToProperty("OP2", op.GetOp2Box());
            this.LinkControlToProperty("OP3", op.GetOp3Box());
            this.LinkControlToProperty("OP4", op.GetOp4Box());
            this.LinkControlToProperty("OP5", op.GetOp5Box());

            TextBox tbBlankL = mp.GetBlankLBox();
            TextBox tbBlankW = mp.GetBlankWBox();
            double l = gp.PartLength - cs.EdgeDiffL;
            double w = gp.PartWidth - cs.EdgeDiffW;
            tbBlankL.Text = l.ToString();
            tbBlankW.Text = w.ToString();
        }

        private void LinkControlToProperty(string property, Control c)
        {
            SwProperty p = this.prop.GetProperty(property);
            if (this.prop.Contains(p))
            {
#if DEBUG
                System.Diagnostics.Debug.Print(p.ToString());
#endif
                p.SwApp = this._swApp;
                c.Text = p.Value;
                p.Ctl = c;
            }
            else
            {
                SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                x.SwApp = this._swApp;
                x.Ctl = c;
            }
        }

        protected SldWorks RequestSW()
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
        public Redbrick MyParent { get; set; }
    }
}
