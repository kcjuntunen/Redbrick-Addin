using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class MachineProperties : UserControl {
    SwProperties propertySet;
    //private System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Settings.Default.ClipboardSound);

    string cnc1 = string.Empty;
    string cnc2 = string.Empty;
    bool? chkupd = null;
    int ppb = 1;
    string ol = string.Empty;
    string ow = string.Empty;

    public MachineProperties(ref SwProperties prop) {
      propertySet = prop;
      InitializeComponent();
    }

    public void Update(ref SwProperties p) {
      propertySet = p;
      LinkControls();
      ToggleFields(this.propertySet.cutlistData.OpType);
    }

    public void Update(ref SwProperties p, double l, double w) {
      propertySet = p;
      LinkControls();
      ToggleFields(this.propertySet.cutlistData.OpType);

      if (tbCNC1.Text.Trim() == "NA" || tbCNC1.Text.Trim() == string.Empty)
        btnWhere.Enabled = false;
      else
        btnWhere.Enabled = true;

      if (cnc1 == string.Empty || cnc2 == string.Empty) {
        cnc1 = propertySet.GetProperty("CNC1").Value;
        cnc2 = propertySet.GetProperty("CNC2").Value;
      }

      if (ol == string.Empty || ow == string.Empty) {
        ol = propertySet.GetProperty("OVERL").Value;
        ow = propertySet.GetProperty("OVERW").Value;
      }

      if (chkupd == null) {
        chkupd = propertySet.GetProperty("UPDATE CNC").Value.ToUpper().Contains("YES");
      }

      CalculateBlankSize(l, w);

      tbCNC1.Text = cnc1;
      tbCNC2.Text = cnc2;
      tbOverL.Text = ol;
      tbOverW.Text = ow;
      tbPPB.Text = ppb.ToString();
      chUpdate.Checked = chkupd.Value;
    }

    private void CalculateBlankSize(double edgeL, double edgeW) {
      double dVal = 0.0;

      double finLen = 0.0;
      double blankLen = 0.0;

      if (propertySet.Contains("LENGTH")) {
        if (double.TryParse(propertySet.GetProperty("LENGTH").ResValue, out finLen))
          blankLen = finLen;

        if (double.TryParse(tbOverL.Text, out dVal))
          _overL = dVal;

        this.tbBlankL.Text = Math.Round((blankLen + dVal + edgeW), 3).ToString("N3");
      }

      blankLen = 0.0;
      if (propertySet.Contains("WIDTH")) {
        if (double.TryParse(propertySet.GetProperty("WIDTH").ResValue, out finLen))
          blankLen = finLen;

        dVal = 0.0;
        if (double.TryParse(tbOverW.Text, out dVal))
          _overW = dVal;

        tbBlankW.Text = Math.Round((blankLen + dVal + edgeL), 3).ToString("N3");
      }
    }

    //private void CalculateBlankSize(double edgeL, double edgeW) {
    //  double dVal = 0.0;
    //  double blankLen = 0.0;
    //  bool calcMultiBlankL = ol.Contains(@"*");
    //  bool calcMultiBlankW = ow.Contains(@"*");

    //  if (propertySet.Contains("LENGTH")) {
    //    double.TryParse(propertySet.GetProperty("LENGTH").ResValue, out blankLen);
    //    string[] arr = ol.Split('*');

    //    if (double.TryParse(arr[0], out dVal))
    //      _overL = dVal;

    //    double _blnksize = (blankLen + _overL + edgeW);

    //    if (calcMultiBlankL) {
    //      double _gapSize = 0.0F;
    //      int _ppb = 1;

    //      if (double.TryParse(arr[1].Trim(), out _gapSize) &&
    //          int.TryParse(tbPPB.Text, out _ppb)) {
    //            _overL = ((((blankLen + edgeW) * (_ppb - 1)) + ((_ppb - 1) * _gapSize)) + _overL);
    //            tbOverL.Text = _overL.ToString("N3");
    //            propertySet.GetProperty("OVERL").Value = tbOverL.Text;
    //            propertySet.GetProperty("OVERL").ResValue = tbOverL.Text;
    //            tbBlankL.Text = (blankLen + _overL + edgeW).ToString("N3");
    //      }
    //    } else {
    //      this.tbBlankL.Text = _blnksize.ToString("N3");
    //    }
    //  }

    //  blankLen = 0.0;
    //  if (propertySet.Contains("WIDTH")) {
    //    double.TryParse(propertySet.GetProperty("WIDTH").ResValue, out blankLen);

    //    string[] arr = ow.Split('*');
    //    dVal = 0.0;

    //    if (double.TryParse(arr[0], out dVal))
    //      _overW = dVal;

    //    double _blnksize = (blankLen + _overW + edgeL);

    //    if (calcMultiBlankW) {
    //      double _gapSize = 0.0F;
    //      int _ppb = 1;

    //      if (double.TryParse(arr[1].Trim(), out _gapSize) &&
    //          int.TryParse(tbPPB.Text, out _ppb)) {
    //            _overW = ((((blankLen + edgeL) * (_ppb - 1)) + ((_ppb - 1) * _gapSize)) + _overW);
    //            tbOverW.Text = _overW.ToString("N3");
    //            propertySet.GetProperty("OVERW").Value = tbOverW.Text;
    //            propertySet.GetProperty("OVERW").ResValue = tbOverW.Text;
    //            tbBlankW.Text = (blankLen + _overW + edgeL).ToString("N3");
    //      }
    //    } else {
    //      tbBlankW.Text = _blnksize.ToString("N3");
    //    }
    //  }
    //}

    private void LinkControls() {
      cnc1 = tbCNC1.Text;
      cnc2 = tbCNC2.Text;
      ol = tbOverL.Text;
      ow = tbOverW.Text;
      if (chkupd != null) {
        chkupd = chUpdate.Checked;
      }

      tbCNC1.Text = string.Empty;
      tbCNC2.Text = string.Empty;
      tbOverL.Text = string.Empty;
      tbOverW.Text = string.Empty;
      tbBlankW.Text = string.Empty;
      tbBlankL.Text = string.Empty;

      int.TryParse(tbPPB.Text, out ppb);
      propertySet.LinkControlToProperty("BLANK QTY", true, this.tbPPB);
      propertySet.LinkControlToProperty("CNC1", true, this.tbCNC1);
      propertySet.LinkControlToProperty("CNC2", true, this.tbCNC2);
      propertySet.LinkControlToProperty("OVERL", true, this.tbOverL);
      propertySet.LinkControlToProperty("OVERW", true, this.tbOverW);
      propertySet.LinkControlToProperty("UPDATE CNC", true, this.chUpdate);
      propertySet.GetProperty("UPDATE CNC").Type = swCustomInfoType_e.swCustomInfoYesOrNo;

      propertySet.UpdCheckedAtStart = propertySet.GetProperty("UPDATE CNC").Value.ToUpper().Contains("YES");
    }

    private void LinkControlToProperty(string property, Control c) {
      SwProperty p = this.propertySet.GetProperty(property);
      if (propertySet.Contains(p)) {
        p.Ctl = c;
        c.Text = p.Value;
      } else {
        SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
        x.Ctl = c;
      }
    }

    public void ToggleFields(int opType) {
      bool wood = (opType != 2);
      tbOverL.Enabled = wood;
      tbOverW.Enabled = wood;
      tbBlankL.Enabled = wood;
      tbBlankW.Enabled = wood;
      label4.Enabled = wood;
      label5.Enabled = wood;
      label6.Enabled = wood;
    }

    public TextBox GetCNC1Box() {
      return tbCNC1;
    }

    public TextBox GetCNC2Box() {
      return tbCNC2;
    }

    public TextBox GetPartsPerBlankBox() {
      return tbPPB;
    }

    public TextBox GetOverLBox() {
      return tbOverL;
    }

    public TextBox GetOverWBox() {
      return tbOverW;
    }

    public TextBox GetBlankLBox() {
      return tbBlankL;
    }

    public TextBox GetBlankWBox() {
      return tbBlankW;
    }

    private double _overL;

    public double OverL {
      get { return _overL; }
      set { _overL = value; }
    }

    private double _overW;

    public double OverW {
      get { return _overW; }
      set { _overW = value; }
    }

    private void tbOver_TextChanged(object sender, EventArgs e) {
      //
    }

    private void tbOverL_Validated(object sender, EventArgs e) {
      string tVal = tbOverL.Text;
      ol = tVal;
      double dVal = 0.0;
      if (double.TryParse(tVal, out dVal)) {
        (sender as TextBox).Text = dVal.ToString("N3");
        _overL = dVal;
      }
    }

    private void tbOverW_Validated(object sender, EventArgs e) {
      string tVal = tbOverW.Text;
      ow = tVal;
      double dVal = 0.0;
      if (double.TryParse(tVal, out dVal)) {
        (sender as TextBox).Text = dVal.ToString("N3");
        _overW = dVal;
      }
    }

    private void btnWhere_Click(object sender, EventArgs e) {
      SolidWorks.Interop.sldworks.ModelDoc2 md = (SolidWorks.Interop.sldworks.ModelDoc2)propertySet.modeldoc;
      System.IO.FileInfo fi = new System.IO.FileInfo(md.GetPathName());
      string name = fi.Name.Replace(fi.Extension, string.Empty);
      Machine_Priority_Control.MachinePriority mp = new Machine_Priority_Control.MachinePriority(name);
      mp.ShowDialog(this.ParentForm);
    }
    
    private void label6_Click(object sender, EventArgs e) {
      string clipping = string.Format("{0} X {1}", tbBlankL.Text, tbBlankW.Text);
      Redbrick.Clip(clipping);
    }

    private void label1_Click(object sender, EventArgs e) {
      Redbrick.Clip(tbCNC1.Text);
    }

    private void label2_Click(object sender, EventArgs e) {
      Redbrick.Clip(tbCNC2.Text);
    }

    private void label4_Click(object sender, EventArgs e) {
      Redbrick.Clip(tbOverL.Text);
    }

    private void label5_Click(object sender, EventArgs e) {
      Redbrick.Clip(tbOverW.Text);
    }

    private void chUpdate_CheckedChanged(object sender, EventArgs e) {
      //chkupd = chUpdate.Checked;
      //SwProperty p = propertySet.GetProperty("UPDATE CNC");
      //p.Value = chUpdate.Checked ? "Y" : "N";
      //p.ResValue = chUpdate.Checked ? "Y" : "N";
      //p.Write();
    }
  }
}
