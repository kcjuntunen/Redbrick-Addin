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

        public ModelRedbrick(ref SwProperties p)
        {
            this.props = p;
            this._swApp = p.SwApp;
            InitializeComponent();                              // MS init
            Init();                                             // my additional init
        }

        private void Init()                                     // If this didn't start out as a VSTA plugin, I probably wouldn't have
        {                                                       // to do this.
            DockStyle d = DockStyle.Fill;

            ds = new DepartmentSelector(ref this.props);        // Since I'm doing this, I can feed arguments to the controls. Might as well.
            ds.Dock = d;
            cs = new ConfigurationSpecific(ref this.props);
            cs.Dock = d;
            gp = new GeneralProperties(ref this.props);
            gp.Dock = d;
            mp = new MachineProperties(ref this.props);
            mp.Dock = d;
            op = new Ops(ref this.props);
            op.Dock = d;

            this.tlpMain.Controls.Add(cs, 0, 0);                // Add them, in order to the main table.
            this.tlpMain.Controls.Add(gp, 0, 1);                // But I think something's not right here.
            this.tlpMain.Controls.Add(mp, 0, 2);                // TODO: have a close look at what's going on here.
            this.tlpMain.Controls.Add(ds, 0, 3);
            this.tlpMain.Controls.Add(op, 0, 4);

            foreach (Control item in this.tlpMain.Controls)     // If anything's not docked, dock it.
            {
                item.Dock = d;
            }

            this.gbSpecProp.Controls.Add(cs);
            this.gbGlobProp.Controls.Add(gp);
            this.gbMachProp.Controls.Add(mp);
            this.gbOp.Controls.Add(op);
            this.tlpMain.ResumeLayout(true);
        }

        public void Update(ref SwProperties p)
        {
            this.props = p;                                                     // Order matters here. The first thing SwProperties does informs what ds
            this.ds.Update(ref p);                                              // does. Ds informs these other guys.
            this.cs.Update(ref p);
            this.gbSpecProp.Text = "Configuration Specific - " + p.configName;
            this.gp.Update(ref p);
            this.op.Update(ref p);
            this.mp.Update(ref p);
        }

        public void Write(ModelDoc2 doc)
        {
            this.props.ReadControls();                                          // OK, so the controls were linked on update. This reads whatever was
            this.props.Write(doc);                                              // entered into the controls, then writes to SW.
            doc.ForceRebuild3(false);                                           // Show changes.
            int err = 0;
            int wrn = 0;
            swSaveAsOptions_e op = swSaveAsOptions_e.swSaveAsOptions_Silent;
            doc.Save3((int)op, ref err, ref wrn);                               // This isn't too slow. Might be too slow in DrawingRedbrick though.
        }

        protected SldWorks RequestSW()
        {
            if (OnRequestSW == null)
                throw new Exception("No SW!");

            return OnRequestSW();
        }

        public Func<SldWorks> OnRequestSW;

        protected SldWorks _swApp;

        protected ModelDoc2 RequestDoc()
        {
            if (OnRequestDoc == null)
                throw new Exception("No SW!");

            return OnRequestDoc();
        }

        public Func<ModelDoc2> OnRequestDoc;

        public SldWorks SwApp
        {
            get { return this._swApp; }
            set { this._swApp = value; }
        }

        public DepartmentSelector aDepartmentSelector
        {
            get { return this.ds; }
        }
        public ConfigurationSpecific aConfigurationSpecific
        {
            get { return this.cs;  }
        }

        public GeneralProperties aGeneralProperties
        {
            get { return this.gp; }
        }

        public MachineProperties aMachineProperties
        {
            get { return this.mp; }
        }

        public Ops aOps
        {
            get { return this.op; }
        }
    }
}
