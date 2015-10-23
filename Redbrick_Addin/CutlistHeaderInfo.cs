using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class CutlistHeaderInfo : Form {
    public CutlistHeaderInfo() {
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      InitializeComponent();
    }

    private void CutlistHeaderInfo_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.CutlistHeaderLocation = Location;
      Properties.Settings.Default.Save();
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      Close();
    }
  }
}