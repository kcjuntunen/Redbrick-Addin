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

namespace Redbrick_Addin {
  public partial class ModelRedbrick : UserControl {
    private DepartmentSelector ds;
    private ConfigurationSpecific cs;
    private GeneralProperties gp;
    private MachineProperties mp;
    private Ops op;
    //private CutlistHandler ch;

    Size s;
    private bool deptEvents = false;

    protected SwProperties props;
    private DirtTracker dirtTracker;

    public ModelRedbrick(ref SwProperties p) {
      props = p;
      _swApp = p.SwApp;
      p.frm = this.ParentForm;
      InitializeComponent();                              // MS init
      Init();                                             // my additional init
      dirtTracker = new DirtTracker(this);
      dirtTracker.Besmirched += dirtTracker_Besmirched;
      IsDirty = false;

      s = gp.Size;
    }

    void dirtTracker_Besmirched(object sender, EventArgs e) {
      if (!gbSpecProp.Text.EndsWith(Properties.Settings.Default.NotSavedMark)) {
        gbSpecProp.Text = gbSpecProp.Text + Properties.Settings.Default.NotSavedMark;
      }
    }

    private void Init()                                     // If this didn't start out as a VSTA plugin, I probably wouldn't have
    {                                                       // to do this.
      //DockStyle d = DockStyle.None;
      gbSpecProp.Click += gbSpecProp_Click;
      gbGlobProp.Click += gbGlobProp_Click;
      ds = new DepartmentSelector(ref this.props);
      ds.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
      //ds.Dock = d;
      cs = new ConfigurationSpecific(ref this.props);
      cs.Location = new System.Drawing.Point(3, 13);
      cs.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
      //cs.Dock = d;
      gp = new GeneralProperties(ref this.props);
      gp.Location = new System.Drawing.Point(3, 13);
      gp.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
      //gp.Dock = d;
      mp = new MachineProperties(ref this.props);
      mp.Location = new System.Drawing.Point(3, 13);
      mp.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
      //mp.Dock = d;
      op = new Ops(ref this.props);
      op.Location = new System.Drawing.Point(3, 13);
      op.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
      op.TabIndex = 2;
      //op.Dock = d;
      //ch = new CutlistHandler(ref this.props);
      //ch.TabIndex = 1;
      //ch.Dock = d;

      // If anything's not docked, dock it.
      //foreach (Control item in this.tlpMain.Controls) {
      //  item.Dock = d;
      //}

      gbSpecProp.Controls.Add(cs);
      gbGlobProp.Controls.Add(gp);
      gbMachProp.Controls.Add(mp);
      tlp1.Controls.Add(ds);
      tlp1.Controls.Add(op);
      //gbCutlist.Controls.Add(ch);
      //tlpMain.ResumeLayout(true);
      ResumeLayout(false);
      PerformLayout();
    }

    void gbGlobProp_Click(object sender, EventArgs e) {
      // Can't really do this yet. It needs to handle resizing well.
      //MachineView();
    }

    void gbSpecProp_Click(object sender, EventArgs e) {
      Redbrick.Clip(gbSpecProp.Text.Split(new char[] {' '})[0]);
    }

    public void MachineView() {
      if (this.gp.Size == s)
        this.gp.Size = new Size(0, 0);
      else
        this.gp.Size = s;
    }


    public void Update(ref SwProperties p) {
      dirtTracker.Besmirched -= dirtTracker_Besmirched;
      //if (ch.CutlistComboBox.SelectedItem != null && ch.RevComboBox.Text != string.Empty) {
      //  Properties.Settings.Default.CurrentCutlist = int.Parse((ch.CutlistComboBox.SelectedItem as DataRowView)[0].ToString());
      //  Properties.Settings.Default.CurrentRev = int.Parse(ch.RevComboBox.Text);
      //  Properties.Settings.Default.Save();
      //}
      TearDownDeptSelectEvent();
      // Order matters here. The first thing SwProperties does informs what ds
      // does. Ds informs these other guys.
      props = p;
      ds.Update(ref p);
      cs.Update(ref p);
      gbSpecProp.Text = p.PartName + " - " + p.configName;
      gp.Update(ref p);
      op.Update(ref p);
      mp.Update(ref p, cs.EdgeDiffL, cs.EdgeDiffW);
      //ch.Update(ref p);
      if (props.PartFileInfo != null) {
        int hash = Redbrick.GetHash(string.Format("{0}\\{1}", props.PartFileInfo.Directory.FullName, props.PartFileInfo.Name));
        if (int.Parse(props.GetProperty("CRC32").Value) != hash) {
          props.GetProperty("CRC32").Value = hash.ToString();
        }
      }
      SetupDeptSelectEvent();
      IsDirty = false;
      dirtTracker.Besmirched += dirtTracker_Besmirched;
    }

    private void SetupDeptSelectEvent() {
      if (!deptEvents) {
        ds.Selected += ds_Selected;
        deptEvents = true;
      }
    }

    private void TearDownDeptSelectEvent() {
      if (deptEvents) {
        ds.Selected -= ds_Selected;
        deptEvents = false;
      }
    }

    void ds_Selected(object d, EventArgs e) {
      cs.Update(ref props);
      gp.Update(ref props);
      mp.Update(ref props);
      op.Update(ref props);
    }

    public void Write(ModelDoc2 doc) {
      // OK, so the controls were linked on update. This reads whatever was
      // entered into the controls, then writes to SW.
      //mp.Update(ref props, cs.EdgeDiffL, cs.EdgeDiffW);
      ReadControls();
      props.Write();
      //ch.Write();
      // Show changes.
      doc.ForceRebuild3(false);

      // This isn't too slow. Might be too slow in DrawingRedbrick though.
      //int err = 0;
      //int wrn = 0;
      //swSaveAsOptions_e op = swSaveAsOptions_e.swSaveAsOptions_Silent;
      //doc.Save3((int)op, ref err, ref wrn);
    }

    private void ReadControls() {
      //props.CutlistID = this.ch.CutlistID;
      //props.CutlistQuantity = this.ch.CutlistQty;
      props.ReadControls();
    }

    public bool IsDirty {
      get {
        if (dirtTracker != null)
          return dirtTracker.IsDirty;
        else
          return false;
      }
      set { dirtTracker.IsDirty = value; }
    }

    protected SldWorks RequestSW() {
      if (OnRequestSW == null)
        throw new Exception("No SW!");

      return OnRequestSW();
    }

    public Func<SldWorks> OnRequestSW;

    protected SldWorks _swApp;

    protected ModelDoc2 RequestDoc() {
      if (OnRequestDoc == null)
        throw new Exception("No SW!");

      return OnRequestDoc();
    }

    public Func<ModelDoc2> OnRequestDoc;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }

    public DepartmentSelector aDepartmentSelector {
      get { return ds; }
    }
    public ConfigurationSpecific aConfigurationSpecific {
      get { return cs; }
    }

    public GeneralProperties aGeneralProperties {
      get { return gp; }
    }

    public MachineProperties aMachineProperties {
      get { return mp; }
    }

    public Ops aOps {
      get { return op; }
    }

    public string OP1 {
      get { return op.GetOp1Box().Text; }
    }

    public string OP2 {
      get { return op.GetOp2Box().Text; }
    }

    public string OP3 {
      get { return op.GetOp3Box().Text; }
    }

    public string OP4 {
      get { return op.GetOp4Box().Text; }
    }

    public string OP5 {
      get { return op.GetOp5Box().Text; }
    }


    private void ModelRedbrick_Load(object sender, EventArgs e) {

    }
  }
}
