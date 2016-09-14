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
    private bool sound_clicked = false;

    public RedbrickConfiguration() {
      InitializeComponent();
      Version cv = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      string ver = cv.ToString();
#if DEBUG
      ver += "DEBUG";
#endif

      Text = "Redbrick Configuration v" + ver;
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
      chbFlameWar.Checked = Properties.Settings.Default.FlameWar;
      chbTestingMode.Checked = Properties.Settings.Default.Testing;
      cbDept.SelectedValue = Properties.Settings.Default.UserDept;
      cbRevLimit.SelectedIndex = Properties.Settings.Default.RevLimit - 1;
      chbSounds.Checked = Properties.Settings.Default.MakeSounds;
      chbWarnings.Checked = Properties.Settings.Default.Warn;
      chbOpWarnings.Checked = Properties.Settings.Default.ProgWarn;
      chbIdiotLight.Checked = Properties.Settings.Default.IdiotLight;
      chbOnlyActive.Checked = Properties.Settings.Default.OnlyActiveAuthors;
      chbOnlyActiveCustomers.Checked = Properties.Settings.Default.OnlyCurrentCustomers;
      chbRememberCustomer.Checked = Properties.Settings.Default.RememberLastCustomer;
      checkBox1.Checked = Properties.Settings.Default.WarnExcludeAssy;
      checkBox2.Checked = Properties.Settings.Default.CHIHideLWH;
      textBox1.Text = Properties.Settings.Default.BOMFilter[0].ToString();

      ToolTip tt = new ToolTip();
      tt.ShowAlways = true;
      tt.SetToolTip(label4, "You probably don't want to mess with this.");

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

    private void chbFlameWar_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.FlameWar = chbFlameWar.Checked;
    }

    private void chbWarnings_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.Warn = chbWarnings.Checked;
    }

    private void chbSounds_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.MakeSounds = chbSounds.Checked;
      if (chbSounds.Checked && sound_clicked) {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.ClipboardSound);
        ofd.FileName = System.IO.Path.GetFileName(Properties.Settings.Default.ClipboardSound);
        ofd.Filter = "Audio Files (*.wav)|*.wav";
        if (ofd.ShowDialog() == DialogResult.OK) {
          Properties.Settings.Default.ClipboardSound = ofd.FileName;
        } else {
          chbSounds.Checked = false;
        }
        sound_clicked = false;
      }
    }

    private void chbSounds_Click(object sender, EventArgs e) {
      sound_clicked = true;
    }

    private void chbIdiotLight_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.IdiotLight = chbIdiotLight.Checked;
    }

    private void chbOnlyActive_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.OnlyActiveAuthors = chbOnlyActive.Checked;
    }

    private void chbOnlyActiveCustomers_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.OnlyCurrentCustomers = chbOnlyActiveCustomers.Checked;
    }

    private void chbCustomerWarn_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.RememberLastCustomer = chbRememberCustomer.Checked;
    }

    private void chbOpWarnings_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.ProgWarn = chbOpWarnings.Checked;
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.WarnExcludeAssy = checkBox1.Checked;
    }

    private void textBox1_Leave(object sender, EventArgs e) {
      System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
      sc.Add(textBox1.Text);
      Properties.Settings.Default.BOMFilter = sc;
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.CHIHideLWH = checkBox2.Checked;
    }
  }
}