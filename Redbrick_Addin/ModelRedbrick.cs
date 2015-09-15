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

namespace Redbrick_Addin
{
    public partial class ModelRedbrick : UserControl
    {
        private DepartmentSelector ds;
        private ConfigurationSpecific cs;
        private GeneralProperties gp;
        private MachineProperties mp;
        private Ops op;
        protected SwProperties props;

        public ModelRedbrick(SldWorks sw)
        {
            InitializeComponent();
            DockStyle d = DockStyle.Fill;
            this._swApp = sw;
            props = new SwProperties(this._swApp);
            props.GetPropertyData((ModelDoc2)this._swApp.ActiveDoc);

            ds = new DepartmentSelector(ref this.props);
            ds.Dock = d;
            cs = new ConfigurationSpecific(ref this.props);
            cs.Dock = d;
            gp = new GeneralProperties(ref this.props);
            gp.Dock = d;
            mp = new MachineProperties(ref this.props);
            mp.Dock = d;
            op = new Ops(ref this.props);
            op.Dock = d;

            this.tlpMain.Controls.Add(ds);
            this.tlpMain.Controls.Add(cs);
            this.tlpMain.Controls.Add(gp);
            this.tlpMain.Controls.Add(mp);
            this.tlpMain.Controls.Add(op);

            foreach (Control item in this.tlpMain.Controls)
            {
                item.Dock = d;
            }

            this.tlpMain.Controls.Add(ds, 0, 0);
            this.gbSpecProp.Controls.Add(cs);
            this.gbGlobProp.Controls.Add(gp);
            this.gbMachProp.Controls.Add(mp);
            this.gbOp.Controls.Add(op);
            this.tlpMain.ResumeLayout(true);
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
    }
}
