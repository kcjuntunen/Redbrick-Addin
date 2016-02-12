using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class MachineProgramManager : Form {
    private bool p1_clicked = false;
    private bool p2_clicked = false;
    private bool p3_clicked = false;

    public MachineProgramManager() {
      InitializeComponent();
    }

    private void lbPri1_SelectedIndexChanged(object sender, EventArgs e) {
      if (p1_clicked) {

      }
    }

    private void lbPri2_SelectedIndexChanged(object sender, EventArgs e) {
      if (p2_clicked) {

      }
    }

    private void lbPri3_SelectedIndexChanged(object sender, EventArgs e) {
      if (p3_clicked) {

      }
    }
  }
}