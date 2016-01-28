using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class ConfigurationSpecific : UserControl {
    private CutlistData cd;
    private SwProperties propertySet;
    private bool changingwithmouse = false;

    public ConfigurationSpecific(ref SwProperties prop) {
      propertySet = prop;
      cd = prop.cutlistData;
      _edgeDiffL = 0.0;
      _edgeDiffW = 0.0;

      InitializeComponent();
      init();
      fillMat();
      fillStatus();
      ComboBox[] cc = { this.cbEf, this.cbEb, this.cbEl, this.cbEr };
      foreach (ComboBox c in cc)
        fillEdg((object)c);

      LinkControls();
    }

    private void init() {
      lMat.Click += lMat_Click;
      lEf.Click += lEf_Click;
      lEb.Click += lEb_Click;
      lEl.Click += lEl_Click;
      lEr.Click += lEr_Click;
      label1.Click += label1_Click;
    }

    void label1_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(cbCutlist.Text.Split(new string[] {"REV"},StringSplitOptions.None)[0].Trim());
    }

    void lEr_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(cbEr.Text);
    }

    void lEl_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(cbEl.Text);
    }

    void lEb_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(cbEb.Text);
    }

    void lEf_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(cbEf.Text);
    }

    void lMat_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(cbMat.Text);
    }

    private void fillStatus() {
      cbStatus.DataSource = propertySet.cutlistData.States.Tables[0];
      cbStatus.DisplayMember = "STATE";
      cbStatus.ValueMember = "ID";
    }

    public void Update(ref SwProperties prop) {
      propertySet = prop;
      configurationName = prop.modeldoc.ConfigurationManager.ActiveConfiguration.Name;
      cd = prop.cutlistData;

      Updte();
    }

    public void Updte() {

      _edgeDiffL = 0.0;
      _edgeDiffW = 0.0;
      LinkControls();
      ToggleFields(cd.OpType);
      UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
      UpdateDiff(cbEl, cbEr, ref _edgeDiffW);

      UpdateCutlistBox();
    }

    private void UpdateCutlistBox() {
      DataSet ds = cd.GetWherePartUsed(propertySet.PartName);
      int s = 1;

      cbCutlist.DataSource = ds.Tables[(int)CutlistData.WhereUsedRes.CLID];
      cbCutlist.DisplayMember = "PARTNUM";
      cbCutlist.ValueMember = "CLID";
      
      cbCutlist.Text = string.Empty;

      cbCutlist.SelectedValue = Properties.Settings.Default.CurrentCutlist;

      if (cbCutlist.SelectedItem != null) {
        cbCutlist.SelectedText = (cbCutlist.SelectedItem as DataRowView)[(int)CutlistData.WhereUsedRes.PARTNUM].ToString();
        cbCutlist.SelectedValue = Properties.Settings.Default.CurrentCutlist;

        if (cbCutlist.SelectedItem != null && int.TryParse((cbCutlist.SelectedItem as DataRowView)[(int)CutlistData.WhereUsedRes.STATEID].ToString(), out s)) {
          cbStatus.SelectedValue = s;
          cbStatus.Text = cd.GetStateByID(s);
        }
      } else {
        cbStatus.Text = string.Empty;
        nudQ.Value = 1;
      }

      if (cbCutlist.SelectedItem != null && int.TryParse((cbCutlist.SelectedItem as DataRowView)[(int)CutlistData.WhereUsedRes.QTY].ToString(), out s)) {
        nudQ.Value = s;
      }

      if (cbCutlist.Text == string.Empty) {
        bRemove.Enabled = false;
      } else {
        bRemove.Enabled = true;
      }
    }

    private void LinkControls() {
      propertySet.LinkControlToProperty("MATID", false, this.cbMat);
      propertySet.LinkControlToProperty("EFID", false, this.cbEf);
      propertySet.LinkControlToProperty("EBID", false, this.cbEb);
      propertySet.LinkControlToProperty("ELID", false, this.cbEl);
      propertySet.LinkControlToProperty("ERID", false, this.cbEr);
      propertySet.CutlistQuantity = nudQ.Value.ToString();

      if (Properties.Settings.Default.Testing) {
        propertySet.LinkControlToProperty("CUTLIST MATERIAL", false, this.cbMat);
        propertySet.LinkControlToProperty("EDGE FRONT (L)", false, this.cbEf);
        propertySet.LinkControlToProperty("EDGE BACK (L)", false, this.cbEb);
        propertySet.LinkControlToProperty("EDGE LEFT (W)", false, this.cbEl);
        propertySet.LinkControlToProperty("EDGE RIGHT (W)", false, this.cbEr);
      }

      if (propertySet.cutlistData.ReturnHash(propertySet) == propertySet.Hash) {
        propertySet.Primary = true;
      } else {
        propertySet.Primary = false;
      }
    }

    private int GetIndex(DataTable dt, string val) {
      if (dt != null) {
        int count = 0;
        foreach (DataRow dr in dt.Rows) {
          count++;
          if (dr.ItemArray[0].ToString().Trim().ToUpper() == val.Trim().ToUpper())
            return count;
        }
      }
      return -1;
    }

    private void fillComboBoxes() {
      Thread t = new Thread(new ThreadStart(fillMat));
      try {
        t.Start();
      } catch (ThreadStateException tse) {
        System.Windows.Forms.MessageBox.Show(tse.Message);
      } catch (OutOfMemoryException oome) {
        System.Windows.Forms.MessageBox.Show(oome.Message);
      }

      ComboBox[] cc = { cbEf, cbEb, cbEl, cbEr };
      foreach (ComboBox c in cc) {
        t = new Thread(new ParameterizedThreadStart(fillEdg));
        try {
          t.Start((object)c);
        } catch (ThreadStateException tse) {
          System.Windows.Forms.MessageBox.Show(tse.Message);
        } catch (OutOfMemoryException oome) {
          System.Windows.Forms.MessageBox.Show(oome.Message);
        }
      }
    }

    private void fillMat() {
      cbMat.DataSource = cd.Materials.Tables[0];
      cbMat.DisplayMember = "DESCR";
      cbMat.ValueMember = "MATID";
      cbMat.Text = string.Empty;
    }

    private void fillEdg(object occ) {
      ComboBox c = (ComboBox)occ;
      c.DataSource = cd.Edges.Tables[0];
      c.DisplayMember = "DESCR";
      c.ValueMember = "EDGEID";
      c.Text = string.Empty;
    }

    public void ToggleFields(int opType) {
      bool wood = (opType != 2);
      lEf.Enabled = wood;
      leFColor.Enabled = wood;
      cbEf.Enabled = wood;

      lEb.Enabled = wood;
      leBColor.Enabled = wood;
      cbEb.Enabled = wood;

      lEl.Enabled = wood;
      leLColor.Enabled = wood;
      cbEl.Enabled = wood;

      lEr.Enabled = wood;
      leRColor.Enabled = wood;
      cbEr.Enabled = wood;
    }

    private void UpdateDiff(ComboBox cb1, ComboBox cb2, ref double targ) {
      targ = 0.0;
      double t = 0.0;
      string thk = string.Empty;
      ComboBox[] cc = { cb1, cb2 };

      foreach (ComboBox c in cc) {
        try {
          System.Data.DataRowView drv = (c.SelectedItem as System.Data.DataRowView);
          if (drv != null)
            thk = drv[3].ToString();
        } catch (InvalidCastException ice) {
          ice.Data.Add("Not", "unused.");
          thk = "0.0";
        } catch (NullReferenceException nre) {
          nre.Data.Add("Not", "unused.");
          thk = "0.0";
        }

        if (double.TryParse(thk, out t)) {
          targ += t * -1;
        }
      }
    }

    private void cbMat_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbMat.SelectedValue != null)
        lMatColor.Text = (cbMat.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        lMatColor.Text = string.Empty;
    }

    private void cbEf_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbEf.SelectedValue != null)
        leFColor.Text = (cbEf.SelectedItem as System.Data.DataRowView)[2].ToString();
      else
        leFColor.Text = string.Empty;

      UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
    }

    private void cbEb_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbEb.SelectedValue != null)
        leBColor.Text = (cbEb.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        leBColor.Text = string.Empty;

      UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
    }

    private void cbEl_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbEl.SelectedValue != null)
        leLColor.Text = (cbEl.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        leLColor.Text = string.Empty;

      UpdateDiff(cbEl, cbEr, ref _edgeDiffW);
    }

    private void cbEr_SelectedIndexChanged(object sender, EventArgs e) {
      if (this.cbEr.SelectedValue != null)
        leRColor.Text = (cbEr.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        leRColor.Text = string.Empty;

      UpdateDiff(cbEl, cbEr, ref _edgeDiffW);
    }

    public string configurationName { get; set; }

    public ComboBox GetCutlistMatBox() {
      return cbMat;
    }

    public ComboBox GetEdgeFrontBox() {
      return cbEf;
    }

    public ComboBox GetEdgeBackBox() {
      return cbEb;
    }

    public ComboBox GetEdgeLeftBox() {
      return cbEl;
    }

    public ComboBox GetEdgeRightBox() {
      return cbEr;
    }

    private double _edgeDiffL;

    public double EdgeDiffL {
      get { return _edgeDiffL; }
      set { _edgeDiffL = value; }
    }

    private double _edgeDiffW;

    public double EdgeDiffW {
      get { return _edgeDiffW; }
      set { _edgeDiffW = value; }
    }

    private void cbCutlist_SelectedIndexChanged(object sender, EventArgs e) {
      if (changingwithmouse) {
        Properties.Settings.Default.CurrentCutlist = int.Parse(cbCutlist.SelectedValue.ToString());
        propertySet.CutlistID = Properties.Settings.Default.CurrentCutlist;
        Properties.Settings.Default.Save();

        if (cbCutlist.SelectedItem != null) {
          cbStatus.SelectedValue = (cbCutlist.SelectedItem as DataRowView)[(int)CutlistData.WhereUsedRes.STATEID];
          int t = 0;
          if (cbStatus.SelectedItem != null && int.TryParse(cbStatus.SelectedValue.ToString(), out t)) {
            cbStatus.Text = cd.GetStateByID(t); 
          }
        }

        int s = 1;
        if (cbCutlist.SelectedItem != null && int.TryParse((cbCutlist.SelectedItem as DataRowView)[(int)CutlistData.WhereUsedRes.QTY].ToString(), out s)) {
          nudQ.Value = s;
        }

        if (cbCutlist.Text == string.Empty) {
          bRemove.Enabled = false;
        } else {
          bRemove.Enabled = true;
        }

        changingwithmouse = false;
      }
    }

    private void cbCutlist_MouseClick(object sender, MouseEventArgs e) {
      changingwithmouse = true;
    }

    private void btnMakeOriginal_Click(object sender, EventArgs e) {
      int hash = propertySet.cutlistData.GetHash(propertySet.PartName);
      if (hash != propertySet.Hash) {
        if (hash != 0) {
          string question = string.Format(Properties.Resources.AlreadyInOtherLocation, propertySet.PartName);

          swMessageBoxResult_e res = (swMessageBoxResult_e)propertySet.SwApp.SendMsgToUser2(question,
            (int)swMessageBoxIcon_e.swMbQuestion,
            (int)swMessageBoxBtn_e.swMbYesNo);
          switch (res) {
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
              propertySet.cutlistData.MakeOriginal(propertySet);
              AddToCutlist();
              break;
            default:
              break;
          }
        } else {
          AddToCutlist();
        }

      } else {
        AddToCutlist();
      }
      Updte();
    }

    private void AddToCutlist() {
      string question = string.Format(Properties.Resources.AddToExistingCutlist, propertySet.PartName);
      swMessageBoxResult_e res = (swMessageBoxResult_e)propertySet.SwApp.SendMsgToUser2(question,
        (int)swMessageBoxIcon_e.swMbQuestion,
        (int)swMessageBoxBtn_e.swMbYesNoCancel);
      switch (res) {
        case swMessageBoxResult_e.swMbHitAbort:
          break;
        case swMessageBoxResult_e.swMbHitCancel:
          break;
        case swMessageBoxResult_e.swMbHitIgnore:
          break;
        case swMessageBoxResult_e.swMbHitNo:
          CutlistHeaderInfo chin = new CutlistHeaderInfo(CutlistData.MakePartFromPropertySet(propertySet, Convert.ToUInt16(nudQ.Value)), 
            propertySet.cutlistData, CutlistHeaderInfo.CutlistFunction.CreateNew);
          chin.Text = "Creating new cutlist...";
          chin.ShowDialog();
          break;
        case swMessageBoxResult_e.swMbHitOk:
          break;
        case swMessageBoxResult_e.swMbHitRetry:
          break;
        case swMessageBoxResult_e.swMbHitYes:
            CutlistHeaderInfo chiy = new CutlistHeaderInfo(CutlistData.MakePartFromPropertySet(propertySet, Convert.ToUInt16(nudQ.Value)), 
              propertySet.cutlistData, CutlistHeaderInfo.CutlistFunction.AddToExistingNotSelected);
            chiy.Text = "Adding to cutlist...";
            chiy.ShowDialog();
          break;
        default:
          break;
      }
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbStatus.SelectedValue != null && cbCutlist.SelectedValue != null) {
        propertySet.cutlistData.SetState((int)cbCutlist.SelectedValue, (int)cbStatus.SelectedValue);
      }
    }

    private void nudQ_ValueChanged(object sender, EventArgs e) {
      propertySet.CutlistQuantity = nudQ.Value.ToString();
    }

    private void bRemove_Click(object sender, EventArgs e) {
      string[] cl = cbCutlist.Text.Split(new string[] { "REV" }, StringSplitOptions.None);
      if (cl.Length > 1) {
        int clid = cd.GetCutlistID(cl[0].Trim(), cl[1].Trim());
        if (cd.RemovePartFromCutlist(clid, propertySet) > 0)
          UpdateCutlistBox();
      }
    }
  }
}
