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
    private int porder = 1;
    private ENGINEERINGDataSetTableAdapters.OpDataTableAdapter opDataTableAdapter =
      new ENGINEERINGDataSetTableAdapters.OpDataTableAdapter();
    private EditFunction func = EditFunction.NewOp;

    private SwProperties propertySet;

    enum EditFunction {
      NewOp,
      EditOp
    }

    public EditOp(string partnum, ref SwProperties propset, int idx) {
      func = EditFunction.NewOp;
      partNum = partnum;
      propertySet = propset;
      porder = idx + 1;
      department = propset.cutlistData.OpType;
      CutlistData cd = propertySet.cutlistData;
      InitializeComponent();
      partID = cd.GetPartID(partnum);
      dataRowView = newDataRow();
    }

    public EditOp(string partnum, ENGINEERINGDataSet.OpDataRow row, ref SwProperties propset) {
      func = EditFunction.EditOp;
      InitializeComponent();
      cpota.FillByPOPID(cpodt, (int)row[@"POPID"]);
      partNum = partnum;
      propertySet = propset;
      department = department = propset.cutlistData.OpType;
      try {
        dataRowView = (ENGINEERINGDataSet.CUT_PART_OPSRow)cpodt.Rows[0];
      } catch (Exception) {
        dataRowView = newDataRow();
      }
    }

    private ENGINEERINGDataSet.CUT_PART_OPSRow newDataRow() {
      ENGINEERINGDataSet.CUT_PART_OPSRow cpor = (ENGINEERINGDataSet.CUT_PART_OPSRow)cpodt.NewRow();
      cpor[@"POPPART"] = partID;
      cpor[@"POPORDER"] = porder;
      cpor[@"POPOP"] = 1;
      cpor[@"POPSETUP"] = 0.0F;
      cpor[@"POPRUN"] = 0.0F;
      return (ENGINEERINGDataSet.CUT_PART_OPSRow)cpor;
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
      SwProperty new_p = propertySet.GetProperty(@"DictOps");
      SwProperty p = propertySet.GetProperty(string.Format(@"OP{0}ID", porder));
      SwProperty old_p = propertySet.GetProperty(string.Format(@"OP{0}", porder));
      switch (func) {
        case EditFunction.NewOp:
          p.ID = comboBox1.SelectedValue.ToString();
          p.Value = propertySet.cutlistData.GetOpAbbreviationByID(p.ID);
          p.ResValue = propertySet.cutlistData.GetOpByID(p.ID);
          p.Descr = p.ResValue;
          p.Old = false;
          old_p.ID = comboBox1.SelectedValue.ToString();
          old_p.Value = p.Value;
          old_p.ResValue = p.ResValue;
          old_p.Descr = p.ResValue;
          old_p.Old = true;
          new_p.Write2(propertySet.modeldoc);
          new_p.Value = Ops2.DataRowToUri(dataRowView);
          new_p.ResValue = p.Value;
          new_p.Descr = p.Value;
          if (propertySet.cutlistData.GetPartID(propertySet.PartName) > 0) {
            cpota.Insert(partID, porder, (int)comboBox1.SelectedValue,
              popSetup / 60.0, popRun / 60.0);
            omh.PartOpAdd();
          }
          break;
        case EditFunction.EditOp:
          p.ID = comboBox1.SelectedValue.ToString();
          p.Value = propertySet.cutlistData.GetOpAbbreviationByID(p.ID);
          p.ResValue = propertySet.cutlistData.GetOpByID(p.ID);
          p.Old = false;
          old_p.ID = comboBox1.SelectedValue.ToString();
          old_p.Value = p.Value;
          old_p.ResValue = p.ResValue;
          old_p.Descr = p.ResValue;
          old_p.Old = true;
          dataRowView[@"POPOP"] = comboBox1.SelectedValue;
          dataRowView[@"POPSETUP"] = popSetup / 60;
          dataRowView[@"POPRUN"] = popRun / 60;
          p.Write2(propertySet.modeldoc);
          if (propertySet.cutlistData.GetPartID(propertySet.PartName) > 0) {
            cpota.Update(dataRowView);
            omh.UpdateCutlistTime();
          }
          break;
        default:
          break;
      }
      Close();
    }
  }
}