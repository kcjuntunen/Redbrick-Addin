using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class EditOp : Form {
    private ENGINEERINGDataSet.CUT_PART_OPSRow dataRowView = default(ENGINEERINGDataSet.CUT_PART_OPSRow);
    private ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota = new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();
    private ENGINEERINGDataSet.CUT_PART_OPSDataTable cpodt = new ENGINEERINGDataSet.CUT_PART_OPSDataTable();
    private bool initialated;
    private string partNum = string.Empty;
    private int partID = 0;
    private int department = 1;
    private ENGINEERINGDataSetTableAdapters.OpDataTableAdapter opDataTableAdapter =
      new ENGINEERINGDataSetTableAdapters.OpDataTableAdapter();
    private EditFunction func = EditFunction.NewOp;

    enum EditFunction {
      NewOp,
      EditOp
    }

    public EditOp(string partnum, int dept, int idx) {
      func = EditFunction.NewOp;
      partNum = partnum;
      department = dept;
      ENGINEERINGDataSet.CUT_PART_OPSRow cpor = (ENGINEERINGDataSet.CUT_PART_OPSRow)cpodt.NewRow();
      CutlistData cd = new CutlistData();
      InitializeComponent();
      partID = cd.GetPartID(partnum);
      cpor[@"POPPART"] = partID;
      cpor[@"POPORDER"] = idx + 1;
      cpor[@"POPOP"] = 1;
      cpor[@"POPSETUP"] = 0.0F;
      cpor[@"POPRUN"] = 0.0F;
      dataRowView = (ENGINEERINGDataSet.CUT_PART_OPSRow)cpor;
    }

    public EditOp(string partnum, ENGINEERINGDataSet.OpDataRow row, int dept) {
      func = EditFunction.EditOp;
      InitializeComponent();
      cpota.FillByPOPID(cpodt, (int)row[@"POPID"]);
      partNum = partnum;
      department = dept;
      dataRowView = (ENGINEERINGDataSet.CUT_PART_OPSRow)cpodt.Rows[0];
    }

    private void cUT_OPSBindingNavigatorSaveItem_Click(object sender, EventArgs e) {
      //this.Validate();
      //this.cUT_OPSBindingSource.EndEdit();
      //this.tableAdapterManager.UpdateAll(this.eNGINEERINGDataSet);

    }

    private void EditOp_Load(object sender, EventArgs e) {
      this.cUT_OPS_METHODSTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_OPS_METHODS);
      this.fRIENDLY_CUT_OPSTableAdapter.Fill(this.eNGINEERINGDataSet.FRIENDLY_CUT_OPS);
      this.cUT_PART_TYPESTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_PART_TYPES);
      comboBox2.SelectedValue = department;
      fRIENDLYCUTOPSBindingSource.Filter = string.Format(@"TYPEID = {0}", comboBox2.SelectedValue);
      if (dataRowView != null) {
        comboBox1.SelectedValue = (int)dataRowView[@"POPOP"];
        Text = string.Format("{0} - Step {1}", partNum, dataRowView[@"POPORDER"]);
        textBox1.Text = ((double)dataRowView[@"POPSETUP"] * 60).ToString();
        textBox2.Text = ((double)dataRowView[@"POPRUN"] * 60).ToString();
      }

      DataRowView drv = (comboBox1.SelectedItem as DataRowView);
      if (textBox1.Text == string.Empty && drv != null) {
        textBox1.Text = drv[@"OPSETUP"].ToString();
      }

      if (textBox2.Text == string.Empty && drv != null) {
        textBox2.Text = drv[@"OPRUN"].ToString();
      }
      initialated = true;
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
      fRIENDLYCUTOPSBindingSource.Filter = string.Format(@"TYPEID = {0}", comboBox2.SelectedValue);
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
      if (initialated) {
        DataRowView drv = (comboBox1.SelectedItem as DataRowView);
        comboBox3.SelectedValue = drv[@"OPMETHOD"];
        textBox1.Text = dataRowView[@"POPSETUP"].ToString();
        if (textBox1.Text == string.Empty && drv != null) {
          textBox1.Text = drv[@"OPSETUP"].ToString();
        }
        textBox2.Text = dataRowView[@"POPRUN"].ToString();
        if (textBox2.Text == string.Empty && drv != null) {
          textBox2.Text = drv[@"OPRUN"].ToString();
        }
      }
    }

    private void comboBox1_KeyDown(object sender, KeyEventArgs e) {
      (sender as ComboBox).DroppedDown = false;
    }

    private void button1_Click(object sender, EventArgs e) {
      Close();
    }

    private void delPartOp() {
      cpota.Delete((int)dataRowView[@"POPID"], (int)dataRowView[@"POPPART"], (int)dataRowView[@"POPORDER"],
        (int)dataRowView[@"POPOP"], (int)dataRowView[@"POPSETUP"], (int)dataRowView[@"POPRUN"]);
    }

    private void button2_Click(object sender, EventArgs e) {
      double popSetup = 0.0F;
      double popRun = 0.0F;
      double.TryParse(textBox1.Text, out popSetup);
      double.TryParse(textBox2.Text, out popRun);
      OpMethodHandler omh = new OpMethodHandler((int)dataRowView[@"POPPART"], (int)comboBox1.SelectedValue,
        (int)dataRowView[@"POPORDER"], popSetup / 60.0, popRun / 60.0);
      switch (func) {
        case EditFunction.NewOp:
          omh.PartOpAdd();
          break;
        case EditFunction.EditOp:
          double res = 0.0F;
          dataRowView[@"POPOP"] = comboBox1.SelectedValue;

          if (double.TryParse(textBox1.Text, out res)) {
            dataRowView[@"POPSETUP"] = res / 60;
          }
          if (double.TryParse(textBox2.Text, out res)) {
            dataRowView[@"POPRUN"] = res / 60;
          }
          cpota.Update(dataRowView);
          omh.UpdateCutlistTime();
          break;
        default:
          break;
      }
      Close();
    }
  }
}