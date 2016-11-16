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
    }

    /// <summary>
    /// Our own init.
    /// </summary>
    private void init() {
      label1.Click += label1_Click;

      fillMat();
      fillStatus();
      ComboBox[] cc = { this.cbEf, this.cbEb, this.cbEl, this.cbEr };
      foreach (ComboBox c in cc)
        fillEdg((object)c);

      LinkControls();
    }

    /// <summary>
    /// Populate possible statuses.
    /// </summary>
    private void fillStatus() {
      cbStatus.DataSource = propertySet.cutlistData.States.Tables[0];
      cbStatus.DisplayMember = "STATE";
      cbStatus.ValueMember = "ID";
    }

    /// <summary>
    /// Update fields with data from the SwProperties we receive.
    /// </summary>
    /// <param name="prop">An SwProperties object.</param>
    public void Update(ref SwProperties prop) {
      propertySet = prop;
      configurationName = prop.modeldoc.ConfigurationManager.ActiveConfiguration.Name;
      cd = prop.cutlistData;

      Updte();
      Redbrick.unselect(Controls);
      Redbrick.unselect(tableLayoutPanel1.Controls);
    }

    /// <summary>
    /// This is a sub-function of Update(). I broke it out so it could be executed by itself.
    /// </summary>
    public void Updte() {
      _edgeDiffL = 0.0;
      _edgeDiffW = 0.0;
      LinkControls();
      ToggleFields(cd.OpType);
      UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
      UpdateDiff(cbEl, cbEr, ref _edgeDiffW);

      UpdateCutlistBox();
    }

    /// <summary>
    /// Figure out where an item is used, and select the last one the user had selected.
    /// </summary>
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
          nudQ.Enabled = true;
          cbStatus.Enabled = true;
          cbStatus.SelectedValue = s;
          cbStatus.Text = cd.GetStateByID(s);
        }
      } else {
        cbStatus.Text = string.Empty;
        nudQ.Enabled = false;
        cbStatus.Enabled = false;
        nudQ.Value = 1;
      }

      if (cbCutlist.SelectedItem != null && int.TryParse((cbCutlist.SelectedItem as DataRowView)[(int)CutlistData.WhereUsedRes.QTY].ToString(), out s)) {
        nudQ.Value = s;
      }

      if (cbCutlist.Text == string.Empty) {
        propertySet.CutlistID = 0;
        bRemove.Enabled = false;
      } else {
        int sv = 0;
        if (int.TryParse(cbCutlist.SelectedValue.ToString(), out sv))
          propertySet.CutlistID = sv;
        bRemove.Enabled = true;
      }
    }

    /// <summary>
    /// Link controls to properties.
    /// </summary>
    private void LinkControls() {
      propertySet.LinkControlToProperty("MATID", false, cbMat);
      propertySet.LinkControlToProperty("EFID", false, cbEf);
      propertySet.LinkControlToProperty("EBID", false, cbEb);
      propertySet.LinkControlToProperty("ELID", false, cbEl);
      propertySet.LinkControlToProperty("ERID", false, cbEr);
      propertySet.CutlistQuantity = nudQ.Value.ToString();

      if (Properties.Settings.Default.Testing) {
        propertySet.LinkControlToProperty("CUTLIST MATERIAL", false, cbMat);
        propertySet.LinkControlToProperty("EDGE FRONT (L)", false, cbEf);
        propertySet.LinkControlToProperty("EDGE BACK (L)", false, cbEb);
        propertySet.LinkControlToProperty("EDGE LEFT (W)", false, cbEl);
        propertySet.LinkControlToProperty("EDGE RIGHT (W)", false, cbEr);
      }

      if (propertySet.cutlistData.ReturnHash(propertySet) == propertySet.Hash) {
        propertySet.Primary = true;
      } else {
        propertySet.Primary = false;
      }
    }

    /// <summary>
    /// Find the index of a val.
    /// </summary>
    /// <param name="dt">A DataTable object.</param>
    /// <param name="val">The string, of which we want the index.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Fill material combobox with materials.
    /// </summary>
    private void fillMat() {
      cbMat.DataSource = cd.Materials.Tables[0];
      cbMat.DisplayMember = "DESCR";
      cbMat.ValueMember = "MATID";
      cbMat.Text = string.Empty;
    }

    /// <summary>
    /// Fill edging combobox with edgings.
    /// </summary>
    /// <param name="occ"></param>
    private void fillEdg(object occ) {
      ComboBox c = (ComboBox)occ;
      c.DataSource = cd.Edges.Tables[0];
      c.DisplayMember = "DESCR";
      c.ValueMember = "EDGEID";
      c.Text = string.Empty;
    }

    /// <summary>
    /// Use all the Controls so long as it's not a metal part.
    /// </summary>
    /// <param name="opType"></param>
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

    /// <summary>
    /// Subtract edging thickness from part size.
    /// </summary>
    /// <param name="cb1"></param>
    /// <param name="cb2"></param>
    /// <param name="targ"></param>
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

    /// <summary>
    /// Select material; update color.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbMat_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbMat.SelectedValue != null)
        lMatColor.Text = (cbMat.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        lMatColor.Text = string.Empty;
    }

    /// <summary>
    /// Select edge; update color.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbEf_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbEf.SelectedValue != null)
        leFColor.Text = (cbEf.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        leFColor.Text = string.Empty;

      UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
    }

    /// <summary>
    /// Select edge; update color.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbEb_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbEb.SelectedValue != null)
        leBColor.Text = (cbEb.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        leBColor.Text = string.Empty;

      UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
    }

    /// <summary>
    /// Select edge; update color.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbEl_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbEl.SelectedValue != null)
        leLColor.Text = (cbEl.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
      else
        leLColor.Text = string.Empty;

      UpdateDiff(cbEl, cbEr, ref _edgeDiffW);
    }

    /// <summary>
    /// Select edge; update color.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Select cutlist; determine status; update fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbCutlist_SelectedIndexChanged(object sender, EventArgs e) {
      if (changingwithmouse) {
        int cc = 0;
        if (int.TryParse(cbCutlist.SelectedValue.ToString(), out cc) /* && cc != 0 */) {
          Properties.Settings.Default.CurrentCutlist = int.Parse(cbCutlist.SelectedValue.ToString());
          propertySet.CutlistID = Properties.Settings.Default.CurrentCutlist;
          Properties.Settings.Default.Save();
        }

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
          propertySet.CutlistID = 0;
          nudQ.Enabled = false;
          cbStatus.Enabled = false;
          bRemove.Enabled = false;
        } else {
          nudQ.Enabled = true;
          cbStatus.Enabled = true;
          bRemove.Enabled = true;
        }

        changingwithmouse = false;
      }
    }

    /// <summary>
    /// Cutlist changed programmatically. I don't want events triggered without real user interaction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbCutlist_MouseClick(object sender, MouseEventArgs e) {
      changingwithmouse = true;
    }

    /// <summary>
    /// Save if unsaved, and add Cutlist to DB.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnMakeOriginal_Click(object sender, EventArgs e) {
      int hash = propertySet.cutlistData.GetHash(propertySet.PartName);

      if (propertySet.modeldoc.GetPathName() == string.Empty) {
        propertySet.modeldoc.Extension.RunCommand(
          (int)SolidWorks.Interop.swcommands.swCommands_e.swCommands_SaveAs,
          propertySet.modeldoc.GetTitle());
        if (propertySet.modeldoc.GetPathName() == string.Empty) {
          throw new Exception("Unsaved models cannot be added to a cutlist.");
        }
      }

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

    /// <summary>
    /// Pop up the CutlistHeaderInfo form.
    /// </summary>
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

    /// <summary>
    /// Set status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e) {
      if (cbStatus.SelectedValue != null && cbCutlist.SelectedValue != null) {
        propertySet.cutlistData.SetState((int)cbCutlist.SelectedValue, (int)cbStatus.SelectedValue);
      }
    }

    /// <summary>
    /// Tell property of current qty.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void nudQ_ValueChanged(object sender, EventArgs e) {
      propertySet.CutlistQuantity = nudQ.Value.ToString();
    }

    /// <summary>
    /// Remove part from particular cutlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void bRemove_Click(object sender, EventArgs e) {
      string[] cl = cbCutlist.Text.Split(new string[] { "REV" }, StringSplitOptions.None);
      if (cl.Length > 1) {
        int clid = cd.GetCutlistID(cl[0].Trim(), cl[1].Trim());
        if (cd.RemovePartFromCutlist(clid, propertySet) > 0)
          UpdateCutlistBox();
      }

      if (Properties.Settings.Default.MakeSounds)
        System.Media.SystemSounds.Beep.Play();
    }

    /// <summary>
    /// Select item matching to entered text.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbMat_TextChanged(object sender, EventArgs e) {
      cbMat.SelectedIndex = cbMat.FindString(cbMat.Text.Trim());
    }

    /// <summary>
    /// Resolve entered text to a ComboBox item.
    /// </summary>
    /// <param name="sender">What item triggered this? Looking for a ComboBox.</param>
    /// <param name="e">Args?</param>
    private void ResolveText(object sender, EventArgs e) {

    }

    /// <summary>
    /// You'd think focus would go with the thing you clicked on, or entered text into. Well, apparently not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FocusHere(object sender, MouseEventArgs e) {
      if (sender is ComboBox) {
        if ((sender as ComboBox).DroppedDown) {
          //
        } else {
          (sender as ComboBox).Focus();
        }
      } else if (sender is TextBox) {
        (sender as TextBox).Focus();
      }
    }

    /// <summary>
    /// Send label1 contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void label1_Click(object sender, EventArgs e) {
      Redbrick.Clip(cbCutlist.Text.Split(new string[] { "REV" }, StringSplitOptions.None)[0].Trim());
    }

    /// <summary>
    /// Send lEr contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lEr_Click(object sender, EventArgs e) {
      Redbrick.Clip(cbEr.Text);
    }

    /// <summary>
    /// Send lEl contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lEl_Click(object sender, EventArgs e) {
      Redbrick.Clip(cbEl.Text);
    }

    /// <summary>
    /// Send lEb contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lEb_Click(object sender, EventArgs e) {
      Redbrick.Clip(cbEb.Text);
    }

    /// <summary>
    /// Send lEf contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lEf_Click(object sender, EventArgs e) {
      Redbrick.Clip(cbEf.Text);
    }

    /// <summary>
    /// Send lMat contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lMat_Click(object sender, EventArgs e) {
      Redbrick.Clip(cbMat.Text);
    }

    /// <summary>
    /// Send lMatColor contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lMatColor_Click(object sender, EventArgs e) {
      Redbrick.Clip(lMatColor.Text);
    }

    /// <summary>
    /// Send leFColor contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void leFColor_Click(object sender, EventArgs e) {
      Redbrick.Clip(leFColor.Text);
    }

    /// <summary>
    /// Send leBColor contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void leBColor_Click(object sender, EventArgs e) {
      Redbrick.Clip(leBColor.Text);
    }

    /// <summary>
    /// Send leLColor contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void leLColor_Click(object sender, EventArgs e) {
      Redbrick.Clip(leLColor.Text);
    }

    /// <summary>
    /// Send leRColor contents to clipboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void leRColor_Click(object sender, EventArgs e) {
      Redbrick.Clip(leRColor.Text);
    }
  }
}
