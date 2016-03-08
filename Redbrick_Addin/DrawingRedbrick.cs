using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Linq;
using System.Data.Linq;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class DrawingRedbrick : UserControl //Form
  {
    private DirtTracker dirtTracker;
    private bool custo_clicked = false;
    private System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Settings.Default.ClipboardSound);

    public DrawingRedbrick(SldWorks sw) {
      this._swApp = sw;
      InitializeComponent();

      this.PropertySet = new DrawingProperties(this._swApp);
      this.RevSet = new DrawingRevs(this._swApp);

      this.fillMat();
      this.fillAuthor();
      this.fillCustomer();
      fillStatus();
      fillRev();

      this.GetData();
      t();

      if (Properties.Settings.Default.MakeSounds) {
        try {
          sp.LoadAsync();
        } catch (Exception ex) {
          PropertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
        }
      }

      this.dirtTracker = new DirtTracker(this);
    }

    public void t() {
      tvRevs t = new tvRevs(ref this._propSet, ref this._revSet);
      t.TabIndex = 16;
      this.tableLayoutPanel1.Controls.Add(t, 0, 2);
      t.Dock = DockStyle.Fill;
    }

    private void SetLocation() {
      this.Top = Properties.Settings.Default.Top;
      this.Left = Properties.Settings.Default.Left;
    }

    private void GetData() {
      this.PropertySet.Read();
      this.RevSet.Read();

      this.FillBoxes();

      if (PropertySet.GetProperty("CUSTOMER").Value == string.Empty) {
        cbCustomer.SelectedIndex = Properties.Settings.Default.LastCustomerSelection;
      }
    }

    private void linkControls() {
      this.PropertySet.GetProperty("").Ctl = this.tbFinish1;
    }

    private void FillBoxes() {
      SwProperty partNo = this.PropertySet.GetProperty("PartNo");
      SwProperty custo = this.PropertySet.GetProperty("CUSTOMER");
      SwProperty by = this.PropertySet.GetProperty("DrawnBy");
      SwProperty d = this.PropertySet.GetProperty("DATE");
      SwProperty rl = PropertySet.GetProperty("REVISION LEVEL");

      if (partNo != null) {
        label4.Text = partNo.ResValue;
        partNo.Ctl = this.tbItemNo;
      } else {
        partNo = new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoText, "$PRP:\"SW-File Name\"", true);
        partNo.SwApp = this.SwApp;
        partNo.Ctl = this.tbItemNo;
        this.PropertySet.Add(partNo);
      }

      if (custo != null) {
        custo.Ctl = this.cbCustomer;
      } else {
        custo = new SwProperty("CUSTOMER", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
        custo.SwApp = this.SwApp;
        custo.Ctl = this.cbCustomer;
        this.PropertySet.Add(custo);
      }

      if (by != null) {
        if (by.Value == string.Empty) {
          by.ID = PropertySet.CutlistData.GetCurrentAuthor().ToString();
          by.Value = PropertySet.CutlistData.GetCurrentAuthorInitial();
          by.ResValue = by.Value;
        }
        by.Ctl = this.cbAuthor;
      } else {
        by = new SwProperty("DrawnBy", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
        by.SwApp = this.SwApp;
        by.Ctl = this.cbAuthor;
        this.PropertySet.Add(by);
      }

      if (d != null) {
        d.Ctl = this.dpDate;
      } else {
        d = new SwProperty("DATE", swCustomInfoType_e.swCustomInfoDate, string.Empty, true);
        d.SwApp = this.SwApp;
        d.Ctl = this.dpDate;
        this.PropertySet.Add(d);
      }

      if (rl != null) {
        rl.Ctl = cbRevision;
      } else {
        rl = new SwProperty("REVISION LEVEL", swCustomInfoType_e.swCustomInfoText, "100", true);
        rl.SwApp = SwApp;
        rl.Ctl = cbRevision;
        PropertySet.Add(rl);
      }

      for (int i = 1; i < 6; i++) {
        if (this.PropertySet.Contains("M" + i.ToString())) {
          foreach (Control c in this.tableLayoutPanel3.Controls) {
            if (c.Name.ToUpper().Contains("M" + i.ToString()))
              this.PropertySet.GetProperty("M" + i.ToString()).Ctl = c;


            if (c.Name.ToUpper().Contains("FINISH" + i.ToString()))
              this.PropertySet.GetProperty("FINISH " + i.ToString()).Ctl = c;
          }
        } else {
          foreach (Control c in this.tableLayoutPanel3.Controls) {
            if (c.Name.ToUpper().Contains("M" + i.ToString())) {
              SwProperty mx = new SwProperty("M" + i.ToString(), swCustomInfoType_e.swCustomInfoText, string.Empty, true);
              mx.SwApp = this.SwApp;
              mx.Ctl = c;
              //System.Diagnostics.Debug.Print("M: " + mx.Value);
              this.PropertySet.Add(mx);
            }

            if (c.Name.ToUpper().Contains("FINISH" + i.ToString())) {
              SwProperty fx = new SwProperty("FINISH " + i.ToString(), swCustomInfoType_e.swCustomInfoText, string.Empty, true);
              fx.SwApp = this.SwApp;
              fx.Ctl = c;
              //System.Diagnostics.Debug.Print("F: " + fx.Value);
              this.PropertySet.Add(fx);
            }
          }
        }
      }

      DataSet ds = PropertySet.CutlistData.GetCutlistData(PropertySet.GetProperty("PartNo").ResValue, PropertySet.GetProperty("REVISION LEVEL").Value);
      int stat = 0;
      if (ds.Tables[0].Rows.Count > 0 && int.TryParse(ds.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.STATEID].ToString(), out stat)) {
        cbStatus.Enabled = true;
        cbStatus.SelectedValue = stat;
      } else {
        cbStatus.Enabled = false;
      }

      this.PropertySet.UpdateFields();
      //this.RevSet.UpdateListBox();
      this.tbItemNoRes.Text = this.PropertySet.GetProperty("PartNo").ResValue;
    }

    private void fillMat() {
      System.Collections.Specialized.StringCollection sc = Properties.Settings.Default.Materials;
      foreach (string s in sc) {
        this.cbM1.Items.Add(s);
        this.cbM2.Items.Add(s);
        this.cbM3.Items.Add(s);
        this.cbM4.Items.Add(s);
        this.cbM5.Items.Add(s);
      }
    }

    private void fillAuthor() {
      cbAuthor.ValueMember = "INITIAL";
      cbAuthor.DisplayMember = "NAME";
      cbAuthor.DataSource = PropertySet.CutlistData.GetAuthors().Tables[0];
    }

    private void fillCustomer() {
      List<string> sc = PropertySet.CutlistData.GetCustomersForDrawing();
      foreach (string s in sc) {
        this.cbCustomer.Items.Add(s);
      }
    }

    private void fillRev() {
      for (int i = 100; i < 110; i++) {
        cbRevision.Items.Add(i);
      }
    }

    private void fillStatus() {
      cbStatus.DataSource = PropertySet.CutlistData.States.Tables[0];
      cbStatus.DisplayMember = "STATE";
      cbStatus.ValueMember = "ID";
    }

    public delegate void selectLayer();

    public void CorrectLayers(selectLayer f) {
      DrawingDoc d = (DrawingDoc)PropertySet.SwApp.ActiveDoc;
      Sheet curSht = (Sheet)d.GetCurrentSheet();
      string[] shts = (string[])d.GetSheetNames();
      foreach (string s in shts) {
        f();
      }
      d.ActivateSheet(curSht.GetName());
    }

    public void chooseLayer() {
      LayerMgr lm = (PropertySet.SwApp.ActiveDoc as ModelDoc2).GetLayerManager();
      string head = getLayerNameHeader(lm);
      int revcount = RevSet.Count - 1;

      for (int i = 0; i < Properties.Settings.Default.LayerTails.Count; i++) {
        string currentTail = Properties.Settings.Default.LayerTails[i];
        try {
          Layer l = (Layer)lm.GetLayer(string.Format("{0}{1}", head, currentTail));

          l.Visible = false;
          if (Math.Floor((double)(revcount / 5)) == i) {
            l.Visible = true;
          }
        } catch (Exception) {
          // Sometimes the layer doesn't exist.
        }
      }
    }

    private string getLayerNameHeader(LayerMgr lm) {
      foreach (string h in Properties.Settings.Default.LayerHeads) {
        foreach (string t in Properties.Settings.Default.LayerTails) {
          Layer l = (Layer)lm.GetLayer(string.Format("{0}{1}", h, t));
          if (l != null && l.Visible) {
            return h;
          }
        }
      }
      return "AMS";
    }

    public bool IsDirty {
      get {
        if (this.dirtTracker != null)                       // TODO: Why is this null sometimes?
          return this.dirtTracker.IsDirty;
        else
          return false;
      }
      set { this.dirtTracker.IsDirty = value; }
    }

    private DrawingProperties _propSet;

    public DrawingProperties PropertySet {
      get { return _propSet; }
      set { _propSet = value; }
    }

    private DrawingRevs _revSet;

    public DrawingRevs RevSet {
      get { return _revSet; }
      set { _revSet = value; }
    }


    private SldWorks _swApp;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }

    private void bCancel_Click(object sender, EventArgs e) {
      //this.Close();
    }

    private void bOK_Click(object sender, EventArgs e) {
      this.PropertySet.ReadControls();
      this.PropertySet.Write(this.SwApp);

      //this.RevSet.ReadControls();
      this.RevSet.Write(this.SwApp);
      (this.SwApp.ActiveDoc as ModelDoc2).ForceRebuild3(true);
      //this.Close();
    }

    private void DrawingRedbrick_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.Left = this.Left;
      Properties.Settings.Default.Top = this.Top;
      Properties.Settings.Default.Save();
    }

    private void lbRevs_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {

    }

    public void Write(DrawingDoc doc) {
      this.PropertySet.ReadControls();
      this.PropertySet.Write(this.SwApp);
      this.RevSet.Write(this.SwApp);
      (doc).ForceRebuild();
      this.dirtTracker = null;
    }

    public void Write(ModelDoc2 md) {
      this.PropertySet.ReadControls();
      this.PropertySet.Write(md);
      this.RevSet.Write(md);
      FillBoxes();
      (md as DrawingDoc).ForceRebuild();
      this.dirtTracker = null;
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      EventHandler eh = Closing;
      if (eh != null)
        eh(this, e);
    }

    private void btnOK_Click(object sender, EventArgs e) {
      this.PropertySet.ReadControls();
#if DEBUG
      string x = this.PropertySet.ToString() + "\n" + this.RevSet.ToString();
      System.Windows.Forms.MessageBox.Show(x);
#endif
      this.PropertySet.Write(this.SwApp);
      this.RevSet.Write(this.SwApp);
      (this.SwApp.ActiveDoc as DrawingDoc).ForceRebuild();
      //EventHandler eh = Closing;
      //if (eh != null)
      //    eh(this, e);
    }

    protected virtual void OnCheckedChanged(EventArgs e) {
      EventHandler eh = Closing;
      if (eh != null)
        eh(this, e);
    }

    public event EventHandler Closing;

    private void button1_Click(object sender, EventArgs e) {
      CutlistHeaderInfo chi = new CutlistHeaderInfo(PropertySet);
      chi.ShowDialog();

      FillBoxes();
    }

    private void dpDate_ValueChanged(object sender, EventArgs e) {

    }

    private void btnLookup_Click(object sender, EventArgs e) {
      DataDisplay dd = new DataDisplay();
      ModelDoc2 doc = (ModelDoc2)PropertySet.SwApp.ActiveDoc;
      CutlistData cd = PropertySet.CutlistData;
      string name = string.Empty;
      swTableType.swTableType st;

      try {
        st = new swTableType.swTableType(doc, Properties.Settings.Default.MasterTableHash);
        DataTable stp = (DataTable)DictToPartList(st.GetParts(), cd);

        if (doc != null) {
          name = doc.GetPathName();
          dd.Text = System.IO.Path.GetFileNameWithoutExtension(name) + " cutlist BOM...";
        }

        dd.Grid.DataSource = stp;
        dd.ShowDialog();
      } catch (Exception ex) {
        PropertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
      }
    }

    private void btnMatList_Click(object sender, EventArgs e) {
      DataDisplay dd = new DataDisplay();
      ModelDoc2 doc = (ModelDoc2)PropertySet.SwApp.ActiveDoc;
      CutlistData cd = PropertySet.CutlistData;
      string name = string.Empty;
      swTableType.swTableType st;

      try {
        st = new swTableType.swTableType(doc, Properties.Settings.Default.MasterTableHash);
        DataTable stp = (DataTable)DictToPartList(st.GetParts(), cd);

        if (doc != null) {
          name = doc.GetPathName();
          dd.Text = "Materials/Edges used in " + System.IO.Path.GetFileNameWithoutExtension(name);
        }

        var q = (from a in stp.AsEnumerable()
                 group a by a.Field<string>("Material") into x
                 orderby x.Key
                 select new { Material = x.Key, Count = x.Count() });

        var r = (from a in stp.AsEnumerable()
                 group a by a.Field<string>("EdgeFront") into x
                 where x.Key != string.Empty
                 orderby x.Key
                 select new { Edging = x.Key, Count = x.Count() });

        var s = (from a in stp.AsEnumerable()
                 group a by a.Field<string>("EdgeBack") into x
                 where x.Key != string.Empty
                 orderby x.Key
                 select new { Edging = x.Key, Count = x.Count() });

        var t = (from a in stp.AsEnumerable()
                 group a by a.Field<string>("EdgeLeft") into x
                 where x.Key != string.Empty
                 orderby x.Key
                 select new { Edging = x.Key, Count = x.Count() });

        var u = (from a in stp.AsEnumerable()
                 group a by a.Field<string>("EdgeRight") into x
                 where x.Key != string.Empty
                 orderby x.Key
                 select new { Edging = x.Key, Count = x.Count() });

        var v = (from a in q select a.Material)
          .Union
          (from b in r select b.Edging)
          .Union
          (from c in s select c.Edging)
          .Union
          (from d in t select d.Edging)
          .Union
          (from f in u select f.Edging);

        dd.Grid.DataSource = ListToDataTable(v.ToList());
        dd.ShowDialog();
      } catch (Exception ex) {
        PropertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
      }
    }

    private static DataTable DictToPartList(Dictionary<string, Part> d, CutlistData cd) {
      List<object> lp = new List<object>();
      DataTable dt = new DataTable();
      foreach (KeyValuePair<string, Part> p in d) {
        Part i = p.Value;
        var o = new {
          Part = i.PartNumber,
          Description = i.Description,
          Qty = i.Qty,
          Material = cd.GetMaterialByID(i.MaterialID.ToString()),
          L = i.Length,
          W = i.Width,
          T = i.Thickness,
          BlankQty = i.BlankQty,
          OverL = i.OverL,
          OverW = i.OverW,
          CNC1 = i.CNC1,
          CNC2 = i.CNC2,
          Op1 = cd.GetOpAbbreviationByID(i.get_OpID(0).ToString()),
          Op2 = cd.GetOpAbbreviationByID(i.get_OpID(1).ToString()),
          Op3 = cd.GetOpAbbreviationByID(i.get_OpID(2).ToString()),
          Op4 = cd.GetOpAbbreviationByID(i.get_OpID(3).ToString()),
          Op5 = cd.GetOpAbbreviationByID(i.get_OpID(4).ToString()),
          EdgeFront = cd.GetEdgeByID(i.EdgeFrontID.ToString()),
          EdgeBack = cd.GetEdgeByID(i.EdgeBackID.ToString()),
          EdgeLeft = cd.GetEdgeByID(i.EdgeLeftID.ToString()),
          EdgeRight = cd.GetEdgeByID(i.EdgeRightID.ToString()),
          Comment = i.Comment,
          Deptartment = cd.GetDeptByID((int)i.DepartmentID),
          Update = i.UpdateCNC ? "Yes" : "No"
        };

        lp.Add(o);
      }

      System.Reflection.PropertyInfo[] props = lp[0].GetType().GetProperties();
      foreach (var prop in props) {
        dt.Columns.Add(prop.Name);
      }

      foreach (var item in lp) {
        var values = new object[props.Length];
        for (var i = 0; i < props.Length; i++) {
          values[i] = props[i].GetValue(item, null);
        }
        dt.Rows.Add(values);
      }

      return dt;
    }


    private static DataTable ListToDataTable(List<string> l) {
      List<object> lp = new List<object>();
      DataTable dt = new DataTable();
      foreach (string p in l) {
        var o = new {
          Material = p
        };

        lp.Add(o);
      }

      System.Reflection.PropertyInfo[] props = lp[0].GetType().GetProperties();
      foreach (var prop in props) {
        dt.Columns.Add(prop.Name);
      }

      foreach (var item in lp) {
        var values = new object[props.Length];
        for (var i = 0; i < props.Length; i++) {
          values[i] = props[i].GetValue(item, null);
        }
        dt.Rows.Add(values);
      }

      return dt;
    }

    private void btnDelete_Click(object sender, EventArgs e) {
      string prtno = tbItemNoRes.Text;
      string revno = cbRevision.Text;

      string question = string.Format("Really delete {0} REV {1}",
        prtno,
        revno);

      swMessageBoxResult_e mebore = (swMessageBoxResult_e)SwApp.SendMsgToUser2(question,
        (int)swMessageBoxIcon_e.swMbQuestion,
        (int)swMessageBoxBtn_e.swMbYesNo);

      if (mebore == swMessageBoxResult_e.swMbHitYes) {
        PropertySet.CutlistData.DeleteCutlist(prtno, revno);

        if (Properties.Settings.Default.MakeSounds)
          System.Media.SystemSounds.Beep.Play();
      }
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e) {
      if (tbItemNoRes.Text != string.Empty && cbRevision.Text != string.Empty) {
        int clid = PropertySet.CutlistData.GetCutlistID(tbItemNoRes.Text, cbRevision.Text);
        if (cbStatus.SelectedValue != null && clid != 0) {
          PropertySet.CutlistData.SetState(clid, (int)cbStatus.SelectedValue);
        }
      }
    }

    private void cbRevision_SelectedIndexChanged(object sender, EventArgs e) {
      FillBoxes();
    }

    private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e) {
      if (custo_clicked) {
        Properties.Settings.Default.LastCustomerSelection = (sender as ComboBox).SelectedIndex;
        Properties.Settings.Default.Save();
        custo_clicked = false;
      }
    }

    private void cbCustomer_MouseDown(object sender, MouseEventArgs e) {
      custo_clicked = true;
    }

    private void label4_Click(object sender, EventArgs e) {
      Clip(label4.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0]);
    }

    private void Clip(string to_clip) {
      if (to_clip != null && to_clip != string.Empty) {
        System.Windows.Forms.Clipboard.SetText(to_clip);

        if (Properties.Settings.Default.MakeSounds) {
          try {
            sp.PlaySync();
          } catch (Exception ex) {
            PropertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
          }
        }

      } else {
        //
      }
    }

  }
}