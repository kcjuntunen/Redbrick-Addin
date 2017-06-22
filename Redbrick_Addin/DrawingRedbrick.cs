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
    private string fileTitle = string.Empty;
    private string fileName = string.Empty;
    private string fileRev = string.Empty;
    private bool custo_clicked = false;

    public DrawingRedbrick(SldWorks sw) {
      this._swApp = sw;
      InitializeComponent();

      this.PropertySet = new DrawingProperties(this._swApp);
      this.RevSet = new DrawingRevs(this._swApp);
      DrbUpdate();
      t();
    }

    /// <summary>
    /// Add a mark to indicate that a control has been messed with.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    void dirtTracker_Besmirched(object sender, EventArgs e) {
      if (!label4.Text.EndsWith(Properties.Settings.Default.NotSavedMark)) {
        label4.Text = label4.Text + Properties.Settings.Default.NotSavedMark;
      }
    }

    /// <summary>
    /// Update the Redbrick. Duh.
    /// </summary>
    public void DrbUpdate() {
      try {
        dirtTracker.Besmirched -= dirtTracker_Besmirched;
      } catch (Exception) {
        // I don't care.
      }

      fillMat();
      fillAuthor();
      fillCustomer();
      fillStatus();
      fillRev();
      GetData();

      dirtTracker = new DirtTracker(this);

      if (dirtTracker != null) {
        IsDirty = false;
        dirtTracker.Besmirched += dirtTracker_Besmirched;
      }
    }

    /// <summary>
    /// Populate the TreeView on this screen. Cool name, eh?
    /// </summary>
    public void t() {
      foreach (Control cont in this.tableLayoutPanel1.Controls) {
        if (cont is tvRevs) {
          cont.Dispose();
        }
      }
      tvRevs t = new tvRevs(ref this._propSet, ref this._revSet);
      t.TabIndex = 16;
      this.tableLayoutPanel1.Controls.Add(t, 0, 2);
      t.Dock = DockStyle.Fill;
      t.Added += t_Added;
      //t.AddedLvl += t_AddedLvl;
      //t.DeletedLvl += t_DeletedLvl;
    }

    void t_DeletedLvl(object sender, EventArgs e) {
      throw new NotImplementedException();
    }

    void t_AddedLvl(object sender, EventArgs e) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the TreeView when a LVL is added.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void t_Added(object sender, EventArgs e) {
      DrawingDoc thisdd = (DrawingDoc)SwApp.ActiveDoc;
      Write(thisdd);
      t();
      DrbUpdate();
    }

    /// <summary>
    /// Gotta manually unselect programatically selected ComboBoxes. Why? I dunno.
    /// </summary>
    public void unselect() {
      Redbrick.unselect(tableLayoutPanel2.Controls);
      Redbrick.unselect(tableLayoutPanel3.Controls);
      Redbrick.unselect(tableLayoutPanel5.Controls);
    }

    /// <summary>
    /// Populate controls.
    /// </summary>
    private void GetData() {
      PropertySet.Read();
      RevSet.Read();

      FillBoxes();

      if (Properties.Settings.Default.RememberLastCustomer && (PropertySet.GetProperty("CUSTOMER").Value == string.Empty)) {
        cbCustomer.SelectedIndex = Properties.Settings.Default.LastCustomerSelection;
      }
    }

    /// <summary>
    /// Select known data in fields, and link controls to properties. I think this should be refactored.
    /// </summary>
    private void FillBoxes() {
      SwProperty partNo = this.PropertySet.GetProperty("PartNo");
      SwProperty custo = this.PropertySet.GetProperty("CUSTOMER");
      SwProperty by = this.PropertySet.GetProperty("DrawnBy");
      SwProperty d = this.PropertySet.GetProperty("DATE");
      SwProperty rl = PropertySet.GetProperty("REVISION LEVEL");
      fileTitle = (PropertySet.SwApp.ActiveDoc as ModelDoc2).GetTitle().Replace(@".SLDDRW", string.Empty);
      fileName = fileTitle.Split(' ')[0].Trim();
      if (fileTitle.ToUpper().Contains(@" REV")) {
        fileRev = fileTitle.Split(new string[] { @" REV", @" " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();        
      } else {
        fileRev = rl.ResValue;
      }

      if (partNo != null) {
        label4.Text = fileName;
        partNo.Ctl = tbItemNo;
      } else {
        partNo = new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoText, "$PRP:\"SW-File Name\"", true);
        partNo.SwApp = SwApp;
        partNo.Ctl = tbItemNo;
        this.PropertySet.Add(partNo);
      }

      if (custo != null) {
        custo.Ctl = cbCustomer;
      } else {
        custo = new SwProperty("CUSTOMER", swCustomInfoType_e.swCustomInfoText, string.Empty, true);
        custo.SwApp = SwApp;
        custo.Ctl = cbCustomer;
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
        by.SwApp = SwApp;
        by.Ctl = cbAuthor;
        this.PropertySet.Add(by);
      }

      if (d != null) {
        d.Ctl = this.dpDate;
      } else {
        d = new SwProperty("DATE", swCustomInfoType_e.swCustomInfoDate, string.Empty, true);
        d.SwApp = SwApp;
        d.Ctl = dpDate;
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
        if (PropertySet.Contains("M" + i.ToString())) {
          foreach (Control c in tableLayoutPanel3.Controls) {
            if (c.Name.ToUpper().Contains("M" + i.ToString()))
              PropertySet.GetProperty("M" + i.ToString()).Ctl = c;


            if (c.Name.ToUpper().Contains("FINISH" + i.ToString()))
              this.PropertySet.GetProperty("FINISH " + i.ToString()).Ctl = c;
          }
        } else {
          foreach (Control c in tableLayoutPanel3.Controls) {
            if (c.Name.ToUpper().Contains("M" + i.ToString())) {
              SwProperty mx = new SwProperty("M" + i.ToString(), swCustomInfoType_e.swCustomInfoText, string.Empty, true);
              mx.SwApp = SwApp;
              mx.Ctl = c;
              PropertySet.Add(mx);
            }

            if (c.Name.ToUpper().Contains("FINISH" + i.ToString())) {
              SwProperty fx = new SwProperty("FINISH " + i.ToString(), swCustomInfoType_e.swCustomInfoText, string.Empty, true);
              fx.SwApp = SwApp;
              fx.Ctl = c;
              PropertySet.Add(fx);
            }
          }
        }
      }

      DataSet ds = PropertySet.CutlistData.GetCutlistData(fileName.Trim(),
        PropertySet.GetProperty("REVISION LEVEL").Value.Trim());
      int stat = 0;
      if (ds.Tables[0].Rows.Count > 0 && int.TryParse(ds.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.STATEID].ToString(), out stat)) {
        cbStatus.Enabled = true;
        cbStatus.SelectedValue = stat;
      } else {
        cbStatus.Enabled = false;
      }

      PropertySet.UpdateFields();
      tbItemNoRes.Text = PropertySet.GetProperty("PartNo").ResValue;
    }

    /// <summary>
    /// Get possible materials.
    /// TODO: This would be better coming from a table in the DB.
    /// </summary>
    private void fillMat() {
      System.Collections.Specialized.StringCollection sc = Properties.Settings.Default.Materials;
      foreach (string s in sc) {
        cbM1.Items.Add(s);
        cbM2.Items.Add(s);
        cbM3.Items.Add(s);
        cbM4.Items.Add(s);
        cbM5.Items.Add(s);
      }
    }

    /// <summary>
    /// Get authors from DB.
    /// </summary>
    private void fillAuthor() {
      cbAuthor.ValueMember = "INITIAL";
      cbAuthor.DisplayMember = "NAME";
      cbAuthor.DataSource = PropertySet.CutlistData.GetAuthors().Tables[0];
    }

    /// <summary>
    /// Get customers from DB.
    /// </summary>
    private void fillCustomer() {
      cbCustomer.ValueMember = "CUSTID";
      cbCustomer.DisplayMember = "CUSTSTRING";
      cbCustomer.DataSource = PropertySet.CutlistData.GetCustomersForDrawing2().Tables[0];
    }

    /// <summary>
    /// Fill Rev box.
    /// </summary>
    private void fillRev() {
      for (int i = 100; i < 110; i++) {
        cbRevision.Items.Add(i);
      }
      cbRevision.Items.Add("NS");
    }

    /// <summary>
    /// Get possible statuses.
    /// </summary>
    private void fillStatus() {
      cbStatus.DataSource = PropertySet.CutlistData.States.Tables[0];
      cbStatus.DisplayMember = "STATE";
      cbStatus.ValueMember = "ID";
    }

    /// <summary>
    /// Validate that item number matches customer, if possible.
    /// </summary>
    /// <returns></returns>
    private bool check_itemnumber() {
      if (Properties.Settings.Default.Warn) {
        string pattern = @"([A-Z]{3,4})(\d{4})";
        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
        string itnu = System.IO.Path.GetFileNameWithoutExtension((_swApp.ActiveDoc as ModelDoc2).GetPathName());
        System.Text.RegularExpressions.Match matches = System.Text.RegularExpressions.Regex.Match(itnu, pattern);
        if (r.IsMatch(itnu)) {
          string itnusubst = itnu.Substring(0, 2);
          int cid = 0;
          if (int.TryParse(PropertySet.CutlistData.GetCustomerIDByProject(matches.Groups[1].ToString()).ToString(), out cid)) {
            if (cbCustomer.SelectedValue != null)
              return cid == (int)cbCustomer.SelectedValue;
          }
        }
        return true;
      }
      return true;
    }

    private bool check_rev() {
      if (Properties.Settings.Default.Warn) {
        if (fileTitle.ToUpper().Contains(@" REV")) {
          if (fileRev != cbRevision.Text) {
            return false;
          }
        }
      }
      return true;
    }

    public delegate void selectLayer();

    /// <summary>
    /// Execute a function on each page.
    /// </summary>
    /// <param name="f">Layer choosing function.</param>
    public void CorrectLayers(selectLayer f) {
      DrawingDoc d = (DrawingDoc)PropertySet.SwApp.ActiveDoc;
      Sheet curSht = (Sheet)d.GetCurrentSheet();
      string[] shts = (string[])d.GetSheetNames();
      foreach (string s in shts) {
        f();
      }
      d.ActivateSheet(curSht.GetName());
    }

    /// <summary>
    /// Make sure we have the right layer to show the latest LVL.
    /// </summary>
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

    /// <summary>
    /// Determine whether it's an 'AMS' drawing, or otherwise.
    /// </summary>
    /// <param name="lm">A LayerMgr object.</param>
    /// <returns>The first characters of layers we want on.</returns>
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

    /// <summary>
    /// Are there edited fields?
    /// </summary>
    public bool IsDirty {
      get {
        // TODO: Why is this null sometimes?
        if (this.dirtTracker != null)
          return dirtTracker.IsDirty;
        else
          return false;
      }
      set {
        if (dirtTracker != null) {
          dirtTracker.IsDirty = value;
        }
      }
    }

    private DrawingProperties _propSet;

    /// <summary>
    /// Our set of properties.
    /// </summary>
    public DrawingProperties PropertySet {
      get { return _propSet; }
      set { _propSet = value; }
    }

    private DrawingRevs _revSet;

    /// <summary>
    /// Our set of LVLs.
    /// </summary>
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

    /// <summary>
    /// Time to write properties to drawing document.
    /// </summary>
    /// <param name="sender">Who clicked?</param>
    /// <param name="e">Any args?</param>
    private void bOK_Click(object sender, EventArgs e) {
      PropertySet.ReadControls();
      PropertySet.Write(SwApp);

      RevSet.Write(SwApp);
      // If you're wondering what takes so long to write props on drawings sometimes, it's this:
      (SwApp.ActiveDoc as ModelDoc2).ForceRebuild3(true);
    }

    /// <summary>
    /// Nothing
    /// </summary>
    /// <param name="sender">unused</param>
    /// <param name="e">unused</param>
    private void lbRevs_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {

    }

    /// <summary>
    /// Write properties to drawing document.
    /// </summary>
    /// <param name="doc">The current DrawingDoc object.</param>
    public void Write(DrawingDoc doc) {
      if (!check_itemnumber()) {
        string itnu = label4.Text;
        string cuss = cbCustomer.Text.Split(' ')[0];
        PropertySet.SwApp.SendMsgToUser2(
          string.Format("The item number '{0}' possibly doesn't match the customer '{1}'.", itnu, cuss),
          (int)swMessageBoxIcon_e.swMbWarning,
          (int)swMessageBoxResult_e.swMbHitOk);
      }

      this.PropertySet.ReadControls();
      this.PropertySet.Write(this.SwApp);
      this.RevSet.Write(this.SwApp);
      (doc).ForceRebuild();
      this.dirtTracker = null;
    }

    /// <summary>
    /// Write properties to drawing document.
    /// </summary>
    /// <param name="md">The current ModelDoc object.</param>
    public void Write(ModelDoc2 md) {
      try {
        dirtTracker.Besmirched -= dirtTracker_Besmirched;
      } catch (Exception) {
        // I don't care.
      }

      if (!check_rev()) {
        if (System.Windows.Forms.MessageBox.Show(
          string.Format(@"Make drawing REV ({0}) match filename ({1})?", cbRevision.Text, fileRev), @"REV mismatch.", MessageBoxButtons.YesNo)
          == DialogResult.Yes) {
          cbRevision.Text = fileRev;
          PropertySet.GetProperty("REVISION LEVEL").Value = fileRev;
        }
      }

      if (!check_itemnumber()) {
        string itnu = label4.Text.Trim();
        string cuss = cbCustomer.Text.Split('-')[0].Trim();
        System.Windows.Forms.MessageBox.Show(
          string.Format("The item number '{0}' doesn't match the customer '{1}'.", itnu, cuss),
          "Wrong customer?",
          MessageBoxButtons.OK);
      }
      this.PropertySet.ReadControls();
      this.PropertySet.Write(md);
      this.RevSet.Write(md);
      FillBoxes();
      (md as DrawingDoc).ForceRebuild();
      this.dirtTracker = null;
      dirtTracker = new DirtTracker(this);

      if (dirtTracker != null) {
        IsDirty = false;
        dirtTracker.Besmirched += dirtTracker_Besmirched;
      }
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

    /// <summary>
    /// Add/Update a cutlist.
    /// </summary>
    /// <param name="sender">Who clicked?</param>
    /// <param name="e">Any args?</param>
    private void button1_Click(object sender, EventArgs e) {
      string descr = get_description(Redbrick.GetFirstView(_swApp));
      CutlistHeaderInfo chi = new CutlistHeaderInfo(PropertySet, descr, _swApp);
      try {
        chi.ShowDialog();
      } catch (ObjectDisposedException odex) {
        // Failed to initiate.
        // Usually, it closed itself because no table was found.
      } catch (Exception ex) {
        _swApp.SendMsgToUser2(ex.Message,
          (int)swMessageBoxIcon_e.swMbStop,
          (int)swMessageBoxBtn_e.swMbOk);
      }

      FillBoxes();
    }

    /// <summary>
    /// Nothing
    /// </summary>
    /// <param name="sender">unused</param>
    /// <param name="e">unused</param>
    private void dpDate_ValueChanged(object sender, EventArgs e) {

    }

    private void insert_BOM() {
      ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
      DrawingDoc dd = (DrawingDoc)SwApp.ActiveDoc;
      ModelDocExtension ex = (ModelDocExtension)md.Extension;
      int bom_type = (int)swBomType_e.swBomType_PartsOnly;
      int bom_numbering = (int)swNumberingType_e.swNumberingType_Flat;
      int bom_anchor =(int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopLeft;
      SolidWorks.Interop.sldworks.View v = GetFirstView(SwApp);

      if (dd.ActivateView(v.Name)) {
        v.InsertBomTable4(
          false,
          Properties.Settings.Default.BOMLocationX, Properties.Settings.Default.BOMLocationY,
          bom_anchor,
          bom_type,
          v.ReferencedConfiguration,
          Properties.Settings.Default.BOMTemplatePath,
          false,
          bom_numbering,
          false);
      }

    }

    /// <summary>
    /// Show what will be uploaded to the as a cutlist, only with hardware and other oddments filtered out.
    /// </summary>
    /// <param name="sender">Who clicked?</param>
    /// <param name="e">Any args?</param>
    private void btnLookup_Click(object sender, EventArgs e) {
      DataDisplay dd = new DataDisplay();
      dd.swApp = _swApp;
      ModelDoc2 doc = (ModelDoc2)PropertySet.SwApp.ActiveDoc;
      CutlistData cd = PropertySet.CutlistData;
      string name = string.Empty;
      swTableType.swTableType st;

      try {
        st = new swTableType.swTableType(doc, Redbrick.MasterHashes);
        cd.IncrementOdometer(CutlistData.Functions.ExamineBOM);
        DataTable stp = (DataTable)DictToPartList(st.GetParts(Redbrick.BOMFilter), cd);
        dd.PathIndex = st.PathList;
        if (doc != null) {
          name = doc.GetPathName();
          dd.Text = System.IO.Path.GetFileNameWithoutExtension(name) + " cutlist BOM...";
        }

        dd.Grid.DataSource = stp;

        dd.ColorRows(
          // λ! Wooo!
          (r) => { return r.Cells["Update"].Value.ToString().ToUpper() == "YES"; },
          Color.Red, 
          Color.Yellow
          );

        dd.ShowDialog();
      } catch (NullReferenceException nre) {
        Redbrick.InsertBOM(_swApp);
        btnLookup_Click(this, new EventArgs());
      } catch (ArgumentOutOfRangeException aoore) {
        PropertySet.SwApp.SendMsgToUser2(@"No acceptable parts in BOM.",
          (int)swMessageBoxIcon_e.swMbStop,
          (int)swMessageBoxBtn_e.swMbOk);
      } catch (Exception ex) {
        PropertySet.SwApp.SendMsgToUser2(ex.Message,
          (int)swMessageBoxIcon_e.swMbStop,
          (int)swMessageBoxBtn_e.swMbOk);
      }
    }

    /// <summary>
    /// Show a list of materials used. Doesn't seem overly useful.
    /// TODO: Make this button cool
    /// </summary>
    /// <param name="sender">Who clicked?</param>
    /// <param name="e">Any args?</param>
    private void btnMatList_Click(object sender, EventArgs e) {
      DataDisplay dd = new DataDisplay();
      ModelDoc2 doc = (ModelDoc2)PropertySet.SwApp.ActiveDoc;
      CutlistData cd = PropertySet.CutlistData;
      string name = string.Empty;
      swTableType.swTableType st;

      try {
        st = new swTableType.swTableType(doc, Redbrick.MasterHashes);
        cd.IncrementOdometer(CutlistData.Functions.MaterialList);
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
        // heheheehe!
        dd.ColorRows((rw) => { return rw.Cells["Material"].Value.ToString().Contains("TBD"); }, 
          Color.Yellow, 
          Color.Red);
        dd.ShowDialog();
      } catch (NullReferenceException nre) {
        Redbrick.InsertBOM(_swApp);
        btnLookup_Click(this, new EventArgs());
      } catch (ArgumentOutOfRangeException aoore) {
        PropertySet.SwApp.SendMsgToUser2(@"No acceptable parts in BOM.",
          (int)swMessageBoxIcon_e.swMbStop,
          (int)swMessageBoxBtn_e.swMbOk);
      } catch (Exception ex) {
        PropertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
      }
    }

    /// <summary>
    /// Take a dictionary, inspect the inner types of the resultant anonymous object, and make a DataTable object.
    /// </summary>
    /// <param name="d">A Dictionary<string, Part> of Part values, with their name as keys.</param>
    /// <param name="cd">An active CutlistData object.</param>
    /// <returns>A DataTable object.</returns>
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

      // Wow, reflection! Fancy! C++ can't do this (yet).
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

    /// <summary>
    /// Convert a simple List object into a DataTable object.
    /// </summary>
    /// <param name="l">A list of strings.</param>
    /// <returns>A DataTable object.</returns>
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

    /// <summary>
    /// Returns the first view found in a drawing.
    /// </summary>
    /// <param name="sw">Active SldWorks object.</param>
    /// <returns>A View object.</returns>
    public static SolidWorks.Interop.sldworks.View GetFirstView(SldWorks sw) {
      ModelDoc2 swModel = (ModelDoc2)sw.ActiveDoc;
      SolidWorks.Interop.sldworks.View v;
      DrawingDoc d = (DrawingDoc)swModel;
      string[] shtNames = (String[])d.GetSheetNames();
      string message = string.Empty;

      //This should find the first page with something on it.
      int x = 0;
      do {
        try {
          d.ActivateSheet(shtNames[x]);
        } catch (IndexOutOfRangeException e) {
          throw new IndexOutOfRangeException("Went beyond the number of sheets.", e);
        } catch (Exception e) {
          throw e;
        }
        v = (SolidWorks.Interop.sldworks.View)d.GetFirstView();
        v = (SolidWorks.Interop.sldworks.View)v.GetNextView();
        x++;
      } while ((v == null) && (x < d.GetSheetCount()));

      message = (string)v.GetName2() + ":\n";

      if (v == null) {
        throw new Exception("I couldn't find a model anywhere in this document.");
      }
      return v;
    }

    /// <summary>
    /// Pull the description string from a part/assembly in a View.
    /// </summary>
    /// <param name="v">A View object.</param>
    /// <returns>A description string.</returns>
    public static string get_description(SolidWorks.Interop.sldworks.View v) {
      ModelDoc2 md = (ModelDoc2)v.ReferencedDocument;
      ConfigurationManager cfMgr = md.ConfigurationManager;
      Configuration cf = cfMgr.ActiveConfiguration;

      CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
      CustomPropertyManager scpm;

      string _value = "PART";
      string _resValue = string.Empty;
      bool wasResolved;
      bool useCached = false;

      if (cf != null) {
        scpm = cf.CustomPropertyManager;
      } else {
        scpm = gcpm;
      }
      int res;

      res = gcpm.Get5("Description", useCached, out _value, out _resValue, out wasResolved);
      if (_value == string.Empty) {
        res = scpm.Get5("Description", useCached, out _value, out _resValue, out wasResolved);
      }
      return _value;
    }

    /// <summary>
    /// Delete a LVL, and all sub LVLs.
    /// </summary>
    /// <param name="sender">Where'd the click come from?</param>
    /// <param name="e">Args?</param>
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

    /// <summary>
    /// Update status on selection.
    /// </summary>
    /// <param name="sender">Where'd the click come from?</param>
    /// <param name="e">Args?</param>
    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e) {
      if (label4.Text != string.Empty && cbRevision.Text != string.Empty) {
        int clid = PropertySet.CutlistData.GetCutlistID(label4.Text, cbRevision.Text);
        if (cbStatus.SelectedValue != null && clid != 0) {
          PropertySet.CutlistData.SetState(clid, (int)cbStatus.SelectedValue);
        }
      }
    }

    /// <summary>
    /// Nothing
    /// </summary>
    /// <param name="sender">unused</param>
    /// <param name="e">unused</param>
    private void cbRevision_SelectedIndexChanged(object sender, EventArgs e) {
    }

    /// <summary>
    /// Store latest selected customer.
    /// </summary>
    /// <param name="sender">Where'd the click come from?</param>
    /// <param name="e">Args?</param>
    private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e) {
      if (custo_clicked) {
        if (Properties.Settings.Default.RememberLastCustomer) {
          Properties.Settings.Default.LastCustomerSelection = (sender as ComboBox).SelectedIndex;
        } else {
          Properties.Settings.Default.LastCustomerSelection = 0;
        }
        Properties.Settings.Default.Save();
        custo_clicked = false;
      }
    }

    /// <summary>
    /// Determine whether there was a real user interation.
    /// </summary>
    /// <param name="sender">Where'd the click come from?</param>
    /// <param name="e">Args?</param>
    private void cbCustomer_MouseDown(object sender, MouseEventArgs e) {
      custo_clicked = true;
    }

    /// <summary>
    /// Copy label content to the clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void label4_Click(object sender, EventArgs e) {
      Redbrick.Clip(label4.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0]);
    }

    private void button1_Click_1(object sender, EventArgs e) {
      insert_BOM();
    }

    private void combobox_KeyDown(object sender, KeyEventArgs e) {
      if (sender is ComboBox)
        (sender as ComboBox).DroppedDown = false;
    }
  }
}