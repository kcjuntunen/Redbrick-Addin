﻿using System;
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
    private DateTime odometerStart;
    private double workDays;

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

    /// <summary>
    /// Pull data from config resources and populate the form.
    /// </summary>
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
      textBox2.Text = Properties.Settings.Default.GaugePath;
      textBox3.Text = Properties.Settings.Default.BOMTemplatePath;
      checkBox3.Checked = Properties.Settings.Default.SaveFirst;
      checkBox4.Checked = Properties.Settings.Default.SilenceGaugeErrors;
      checkBox5.Checked = Properties.Settings.Default.ExportEDrw;
      checkBox6.Checked = Properties.Settings.Default.ExportImg;
      checkBox7.Checked = Properties.Settings.Default.CutlistNotSelectedWarning;
      checkBox8.Checked = Properties.Settings.Default.AutoOpenPriority;
      chbFilterBOM.Checked = Properties.Settings.Default.FilterBOM;
      textBox4.Text = Properties.Settings.Default.DrawingTemplate;
      textBox5.Text = Properties.Settings.Default.DraftingStandard;

      ToolTip tt = new ToolTip();
      tt.ShowAlways = true;
      tt.SetToolTip(label4, "You probably don't want to mess with this.");
      var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
      string currentUserName = t.ToTitleCase(cd.GetAuthorFullName(cd.GetCurrentAuthorInitial()).ToLower());
      dataGridView1.AutoResizeRows();
      dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      dataGridView1.AutoResizeColumns();
      dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridView1.DataSource = get_stats();
      dataGridView1.Columns["Avg Daily Usage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      dataGridView1.Columns["Total Since Reset"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      dataGridView1.Columns["Your Avg Daily Usage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      dataGridView1.Columns["Your Total Since Reset"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      dataGridView1.Columns["σ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

      dataGridView1.Columns["Avg Daily Usage"].ToolTipText = string.Format(@"{0} work days since {1}", workDays, odometerStart.ToShortDateString());
      dataGridView1.Columns["Total Since Reset"].ToolTipText = @"Reset @ " + odometerStart.ToShortDateString();
      dataGridView1.Columns["Your Avg Daily Usage"].ToolTipText = string.Format(@"{0}'s average", currentUserName);
      dataGridView1.Columns["Your Total Since Reset"].ToolTipText =
        string.Format(@"{0}'s total, reset @ {1}", currentUserName, odometerStart.ToShortDateString());
      dataGridView1.Columns["σ"].ToolTipText = @"Standard deviation";

      dataGridView1.Columns["Avg Daily Usage"].DefaultCellStyle.Format = @"#.###";
      dataGridView1.Columns["Your Avg Daily Usage"].DefaultCellStyle.Format = @"#.###";
      dataGridView1.Columns["σ"].DefaultCellStyle.Format = @"#.###";
      initialated = true;
    }

    private double WorkDays() {
      System.IO.FileInfo pi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
      odometerStart = Redbrick.GetOdometerStart(pi.DirectoryName + @"\version.xml");
      DateTime end = DateTime.Now;

      int dowStart = ((int)odometerStart.DayOfWeek == 0 ? 7 : (int)odometerStart.DayOfWeek);
      int dowEnd = ((int)end.DayOfWeek == 0 ? 7 : (int)end.DayOfWeek);
      TimeSpan tSpan = end - odometerStart;
      if (dowStart <= dowEnd) {
        return (((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
      }
      return (((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));
    }

    private DataView get_stats() {
      DataTable dt = new DataTable();
      dt.Columns.Add("Function");
      dt.Columns.Add("Avg Daily Usage", typeof(double));
      dt.Columns.Add("Total Since Reset", typeof(int));
      dt.Columns.Add("Your Avg Daily Usage", typeof(double));
      dt.Columns.Add("Your Total Since Reset", typeof(int));
      dt.Columns.Add("σ", typeof(double));
      workDays = WorkDays();
      foreach (object item in Enum.GetValues(typeof(CutlistData.Functions))) {
        string x = Enum.GetName(typeof(CutlistData.Functions), (CutlistData.Functions)item);
        int f = cd.GetOdometerTotalValue((CutlistData.Functions)item);
        int mf = cd.GetOdometerValue((CutlistData.Functions)item);
        double y = f / workDays;
        double my = mf / workDays;
        double σ = cd.GetOdometerStdDev((CutlistData.Functions)item);
        if (y > 0) {
          DataRow dr = dt.NewRow();
          dr["Function"] = x;
          dr["Avg Daily Usage"] = y;
          dr["Total Since Reset"] = f;
          dr["Your Avg Daily Usage"] = my;
          dr["Your Total Since Reset"] = mf;
          dr["σ"] = σ;
          dt.Rows.Add(dr);
        }
      }
      dt.DefaultView.Sort = "[Avg Daily Usage] DESC";
      return dt.DefaultView;
    }

    /// <summary>
    /// On load, restore position and size.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void RedbrickConfiguration_Load(object sender, EventArgs e) {
      Location = Properties.Settings.Default.RBConfigLocation;
      Size = Properties.Settings.Default.RBConfigSize;
    }

    /// <summary>
    /// I was saving data here, but it didn't seem to keep.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void RedbrickConfiguration_FormClosing(object sender, FormClosingEventArgs e) {
    }

    /// <summary>
    /// Update user department data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void cbDept_SelectedIndexChanged(object sender, EventArgs e) {
      if (initialated) {
        int tp = 0;
        if (int.TryParse(cbDept.SelectedValue.ToString(), out tp)) {
          Properties.Settings.Default.UserDept = tp;
        }
      }
    }

    /// <summary>
    /// Update rev limit data
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void cbRevLimit_SelectedIndexChanged(object sender, EventArgs e) {
      Properties.Settings.Default.RevLimit = (int)cbRevLimit.SelectedIndex + 1;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbDBEnabled_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.EnableDBWrite = chbDBEnabled.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbTestingMode_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.Testing = chbTestingMode.Checked;
    }

    /// <summary>
    /// Save data and close.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void btnOK_Click(object sender, EventArgs e) {
      System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
      sc.Add(textBox1.Text);

      Properties.Settings.Default.DrawingTemplate = textBox4.Text;
      Properties.Settings.Default.DraftingStandard = textBox5.Text;

      Properties.Settings.Default.BOMFilter = sc;
      Properties.Settings.Default.RBConfigLocation = Location;
      Properties.Settings.Default.RBConfigSize = Size;
      Properties.Settings.Default.Save();
      Close();
    }

    /// <summary>
    /// Just close.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void btnCancel_Click(object sender, EventArgs e) {
      Close();
    }

    /// <summary>
    /// What material should we default to?
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void cbDefaultMaterial_SelectedIndexChanged(object sender, EventArgs e) {
      int tp = Properties.Settings.Default.DefaultMaterial;
      if (initialated && int.TryParse(cbDefaultMaterial.SelectedValue.ToString(), out tp)) {
        Properties.Settings.Default.DefaultMaterial = tp;
      }
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbFlameWar_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.FlameWar = chbFlameWar.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbWarnings_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.Warn = chbWarnings.Checked;
      tableLayoutPanel5.Enabled = chbWarnings.Checked;
    }

    /// <summary>
    /// Update checkbox data. Also triggers a file selection box.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
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

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbSounds_Click(object sender, EventArgs e) {
      sound_clicked = true;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbIdiotLight_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.IdiotLight = chbIdiotLight.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbOnlyActive_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.OnlyActiveAuthors = chbOnlyActive.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbOnlyActiveCustomers_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.OnlyCurrentCustomers = chbOnlyActiveCustomers.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbCustomerWarn_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.RememberLastCustomer = chbRememberCustomer.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void chbOpWarnings_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.ProgWarn = chbOpWarnings.Checked;
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void checkBox1_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.WarnExcludeAssy = checkBox1.Checked;
    }

    /// <summary>
    /// Update textbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void textBox1_Leave(object sender, EventArgs e) {
    }

    /// <summary>
    /// Update checkbox data.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void checkBox2_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.CHIHideLWH = checkBox2.Checked;
    }

    /// <summary>
    /// On doubleclick, select xml file.
    /// </summary>
    /// <param name="sender">Who triggered this event?</param>
    /// <param name="e">Any data come with it?</param>
    private void textBox2_DoubleClick(object sender, EventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.GaugePath);
      ofd.FileName = System.IO.Path.GetFileName(Properties.Settings.Default.GaugePath);
      ofd.Filter = "XML Data (*.xml)|*.xml";
      if (ofd.ShowDialog() == DialogResult.OK) {
        textBox2.Text = ofd.FileName;
        Properties.Settings.Default.GaugePath = ofd.FileName;
        Properties.Settings.Default.Save();
      } else {

      }
    }

    private void checkBox3_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.SaveFirst = checkBox3.Checked;
    }

    private void checkBox4_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.SilenceGaugeErrors = checkBox4.Checked;
    }

    private void checkBox5_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.ExportEDrw = checkBox5.Checked;
    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.ExportImg = checkBox6.Checked;
    }

    private void checkBox7_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.CutlistNotSelectedWarning = checkBox7.Checked;
    }

    private void textBox3_MouseDoubleClick(object sender, MouseEventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.BOMTemplatePath);
      ofd.FileName = System.IO.Path.GetFileName(Properties.Settings.Default.BOMTemplatePath);
      ofd.Filter = "Table Template Files (*.sldbomtbt)|*.sldbomtbt";
      if (ofd.ShowDialog() == DialogResult.OK) {
        textBox3.Text = ofd.FileName;
        Properties.Settings.Default.BOMTemplatePath = ofd.FileName;
        Properties.Settings.Default.Save();
      } else {

      }
    }

    private void checkBox1_CheckedChanged_1(object sender, EventArgs e) {
      Properties.Settings.Default.WarnExcludeAssy = checkBox1.Checked;
    }

    private void combobox_KeyDown(object sender, KeyEventArgs e) {
      if (sender is ComboBox)
        (sender as ComboBox).DroppedDown = false;
    }

    private void checkBox8_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.AutoOpenPriority = checkBox8.Checked;
    }

    private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {

    }

    private void button1_Click(object sender, EventArgs e) {
      AboutBox ab = new AboutBox();
      ab.ShowDialog();
    }

    private void chbFilterBOM_CheckedChanged(object sender, EventArgs e) {
      Properties.Settings.Default.FilterBOM = (sender as CheckBox).Checked;
    }
  }
}