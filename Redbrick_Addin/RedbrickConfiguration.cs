using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class RedbrickConfiguration : Form {
    private CutlistData cd = new CutlistData();
    private bool initialated = false;
    public RedbrickConfiguration() {
      InitializeComponent();
      init();
    }

    private void init() {
      cbDefaultMaterial.DisplayMember = "DESCR";
      cbDefaultMaterial.ValueMember = "MATID";
      cbDefaultMaterial.DataSource = cd.Materials.Tables[0];
      cbDefaultMaterial.SelectedValue = Properties.Settings.Default.DefaultMaterial;

      cbDept.DisplayMember = "DEPT_NAME";
      cbDept.ValueMember = "DEPT_ID";
      cbDept.DataSource = cd.GetDepartments().Tables[0];

      for (int i = 0; i < 26; i++) {
        cbRevLimit.Items.Add("A" + (char)(i + 65));
      }

      chbDBEnabled.Checked = Properties.Settings.Default.EnableDBWrite;
      chbTestingMode.Checked = Properties.Settings.Default.Testing;
      cbDept.SelectedValue = Properties.Settings.Default.UserDept;
      cbRevLimit.SelectedIndex = Properties.Settings.Default.RevLimit - 1;
      initialated = true;
    }

    private void RedbrickConfiguration_Load(object sender, EventArgs e) {
      Location = Properties.Settings.Default.RBConfigLocation;
      Size = Properties.Settings.Default.RBConfigSize;
    }

    private void RedbrickConfiguration_FormClosing(object sender, FormClosingEventArgs e) {
    }

    private void cbDept_SelectedIndexChanged(object sender, EventArgs e) {
      if (initialated) {
        int tp = 0;
        if (int.TryParse(cbDept.SelectedValue.ToString(), out tp)) {
          Properties.Settings.Default.UserDept = tp;
        }
      }
    }

    private void cbRevLimit_SelectedIndexChanged(object sender, EventArgs e) {
      Properties.Settings.Default.RevLimit = (int)cbRevLimit.SelectedIndex + 1;
    }

    private void chbDBEnabled_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.EnableDBWrite = chbDBEnabled.Checked;
    }

    private void chbTestingMode_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.Testing = chbTestingMode.Checked;
    }

    private void btnOK_Click(object sender, EventArgs e) {
      Properties.Settings.Default.RBConfigLocation = Location;
      Properties.Settings.Default.RBConfigSize = Size;
      Properties.Settings.Default.Save();
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      Close();
    }

    private void cbDefaultMaterial_SelectedIndexChanged(object sender, EventArgs e) {
      int tp = Properties.Settings.Default.DefaultMaterial;
      if (initialated && int.TryParse(cbDefaultMaterial.SelectedValue.ToString(), out tp)) {
        Properties.Settings.Default.DefaultMaterial = tp;
      }
    }
  }
}