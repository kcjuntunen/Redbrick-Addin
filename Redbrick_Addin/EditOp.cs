using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class EditOp : Form {
    private ENGINEERINGDataSet.OpTreeRow dataRowView = default(ENGINEERINGDataSet.OpTreeRow);
    private string partNum = string.Empty;
    private int department = 1;
    private ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cUT_PART_OPSTableAdapter = 
      new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

    enum EditFunction {
      NewOp,
      EditOp
    }

    public EditOp() {
      InitializeComponent();
    }

    public EditOp(string partnum) {
      partNum = partnum;
      InitializeComponent();
      textBox1.Text = cUT_PART_OPSTableAdapter.SetupTimeByPart(partnum, 1).ToString();
      DataRowView drv = (comboBox1.SelectedItem as DataRowView);
      if (textBox1.Text == string.Empty && drv != null) {
        textBox1.Text = drv[@"OPSETUP"].ToString();
      }
      textBox2.Text = cUT_PART_OPSTableAdapter.RunTimeByPart(partnum, 1).ToString();
      if (textBox2.Text == string.Empty && drv != null) {
        textBox2.Text = drv[@"OPRUN"].ToString();
      }
    }

    public EditOp(string partnum, ENGINEERINGDataSet.OpTreeRow row, int dept) {
      InitializeComponent();
      partNum = partnum;
      department = dept;
      dataRowView = row;
    }

    private void cUT_OPSBindingNavigatorSaveItem_Click(object sender, EventArgs e) {
      //this.Validate();
      //this.cUT_OPSBindingSource.EndEdit();
      //this.tableAdapterManager.UpdateAll(this.eNGINEERINGDataSet);

    }

    private void EditOp_Load(object sender, EventArgs e) {
      // TODO: This line of code loads data into the 'eNGINEERINGDataSet.FRIENDLY_CUT_OPS' table. You can move, or remove it, as needed.
      this.fRIENDLY_CUT_OPSTableAdapter.Fill(this.eNGINEERINGDataSet.FRIENDLY_CUT_OPS);
      // TODO: This line of code loads data into the 'eNGINEERINGDataSet.CUT_PART_TYPES' table. You can move, or remove it, as needed.
      this.cUT_PART_TYPESTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_PART_TYPES);
      comboBox2.SelectedIndex = department - 1;
      fRIENDLYCUTOPSBindingSource.Filter = string.Format(@"TYPEID = {0}", comboBox2.SelectedValue);
      if (dataRowView != null) {
        comboBox1.SelectedValue = (int)dataRowView[@"POPOP"];
        Text = string.Format("{0} - Step {1}", partNum, dataRowView[@"POPORDER"]);
        textBox1.Text = dataRowView[@"POPSETUP"].ToString();
        textBox2.Text = dataRowView[@"POPRUN"].ToString();
      }
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
      fRIENDLYCUTOPSBindingSource.Filter = string.Format(@"TYPEID = {0}", comboBox2.SelectedValue);
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
      DataRowView drv = (comboBox1.SelectedItem as DataRowView);
      if (textBox1.Text == string.Empty && drv != null) {
        textBox1.Text = drv[@"OPSETUP"].ToString();
      }
      textBox2.Text = cUT_PART_OPSTableAdapter.RunTimeByPart(partNum, 1).ToString();
      if (textBox2.Text == string.Empty && drv != null) {
        textBox2.Text = drv[@"OPRUN"].ToString();
      }
    }

    private void comboBox1_KeyDown(object sender, KeyEventArgs e) {
      (sender as ComboBox).DroppedDown = false;
    }
  }
}