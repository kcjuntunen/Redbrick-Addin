using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class DataDisplay : Form {
    public DataDisplay() {
      InitializeComponent();
    }

    public DataDisplay(DataTable dt) {
      InitializeComponent();
      dataGridView1.DataSource = dt;
    }

    private void DataDisplay_Load(object sender, EventArgs e) {
      Location = Properties.Settings.Default.DataDisplayLocation;
      Size = Properties.Settings.Default.DataDisplaySize;
    }

    private void DataDisplay_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.DataDisplayLocation = Location;
      Properties.Settings.Default.DataDisplaySize = Size;
      Properties.Settings.Default.Save();
    }
  }
}