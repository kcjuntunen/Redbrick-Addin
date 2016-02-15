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

      CalculateBlankSize(l, w);
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

    private void LinkControls() {
      tbCNC1.Text = string.Empty;
      tbCNC2.Text = string.Empty;
      tbOverL.Text = string.Empty;
      tbOverW.Text = string.Empty;
      tbBlankW.Text = string.Empty;
      tbBlankL.Text = string.Empty;

      propertySet.LinkControlToProperty("BLANK QTY", true, this.tbPPB);
      propertySet.LinkControlToProperty("CNC1", true, this.tbCNC1);
      propertySet.LinkControlToProperty("CNC2", true, this.tbCNC2);
      propertySet.LinkControlToProperty("OVERL", true, this.tbOverL);
      propertySet.LinkControlToProperty("OVERW", true, this.tbOverW);
      propertySet.LinkControlToProperty("UPDATE CNC", true, this.chUpdate);

      if (propertySet.GetProperty("UPDATE CNC").Value.ToUpper().Contains("YES"))
        chUpdate.Checked = true;
      else
        chUpdate.Checked = false;
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

    private void tbOverL_TextChanged(object sender, EventArgs e) {
      tbOverL.Text = string.Format("{0:0.000}", tbOverL.Text);
    }

    private void tbOverW_TextChanged(object sender, EventArgs e) {
      tbOverW.Text = string.Format("{0:0.000}", tbOverW.Text);
    }

    private void tbOverL_Validated(object sender, EventArgs e) {
      string tVal = tbOverL.Text;
      double dVal = 0.0;
      if (double.TryParse(tVal, out dVal))
        _overL = dVal;
    }

    private void tbOverW_Validated(object sender, EventArgs e) {
      string tVal = tbOverW.Text;
      double dVal = 0.0;
      if (double.TryParse(tVal, out dVal))
        _overW = dVal;
    }

    private void btnWhere_Click(object sender, EventArgs e) {
      //DataDisplay dd = new DataDisplay(propertySet.cutlistData.GetWhereProgUsed(tbCNC1.Text), tbCNC1.Text);
      //dd.ShowDialog();
      MachineProgramManager mpm = new MachineProgramManager(propertySet, tbCNC1.Text);
      mpm.ShowDialog();
    }
  }
}
