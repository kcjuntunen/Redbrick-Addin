using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class MachineProgramManager : Form {
    private bool p1_clicked = false;
    private bool p2_clicked = false;
    private bool p3_clicked = false;
    private CutlistData cutlistData;
    private SwProperties prop;

    private DataTable wubp;
    private DataTable gpbc;
    private DataTable dtP1;
    private DataTable dtP2;
    private DataTable dtP3;
    private DataTable machList;

    public MachineProgramManager() {
      InitializeComponent();
    }


    public MachineProgramManager(SwProperties p, string searchterm) {
      InitializeComponent();
      prop = p;
      cutlistData = p.cutlistData;
      string s = OnlyDigits(searchterm);
      dtP1 = cutlistData.GetMachinesByProg(s, 1);
      dtP2 = cutlistData.GetMachinesByProg(s, 2);
      dtP3 = cutlistData.GetMachinesByProg(s, 3);
      wubp = cutlistData.GetWhereUsedByProg(s);
      gpbc = cutlistData.GetPartsByCNC(s);

      init();
    }

    private void init() {
      machList = cutlistData.GetMachines();

      foreach (ListBox lb in new ListBox[] { lbPri1, lbPri2, lbPri3 })
        PopulatePriorityBox(lb);

      FillBox(dtP1, lbPri1);
      FillBox(dtP2, lbPri2);
      FillBox(dtP3, lbPri3);

      lbAssocCutl.DisplayMember = "PARTNUM";
      lbAssocPart.DisplayMember = "PARTNUM";

      lbAssocCutl.DataSource = wubp;
      lbAssocPart.DataSource = gpbc;
    }

    private void PopulatePriorityBox(ListBox lb) {
      lb.DisplayMember = "MACHNAME";
      lb.ValueMember = "MACHID";
      lb.DataSource = machList.Copy();
    }

    private void FillBox(DataTable dt, ListBox lb) {
      lb.SelectedIndex = -1;
      string comp_string1 = string.Empty; 
      string comp_string2 = string.Empty;
      if (dt.Rows.Count > 0) {
        foreach (DataRow dr in dt.Rows) {
          comp_string1 = dr["MACHID"].ToString();
          for (int i = 0; i < lb.Items.Count; i++) {
            comp_string2 = (lb.Items[i] as DataRowView)["MACHID"].ToString();

            if (lb.SelectedValue.ToString() == comp_string1) {
              lb.SetSelected(i, true);
            } else {
              lb.SetSelected(i, false);
            }
          }
        }
      } else {
        for (int i = 0; i < lb.Items.Count; i++) {
          lb.SetSelected(i, false);
        }
      }
    }

    private void UpdateDB() {
      int i = 0;
      int prtID = cutlistData.GetPartID(prop.PartName);
      foreach (DataRowView s in lbPri1.Items) {
        if (lbPri1.GetSelected(i)) {
          cutlistData.SetProgramPriority(prtID, (int)s["MACHID"], 1);
        }

        if (lbPri2.GetSelected(i)) {
          cutlistData.SetProgramPriority(prtID, (int)s["MACHID"], 2);
        }

        if (lbPri3.GetSelected(i)) {
          cutlistData.SetProgramPriority(prtID, (int)s["MACHID"], 3);
        }

        if (!lbPri1.GetSelected(i) && !lbPri2.GetSelected(i) && !lbPri3.GetSelected(i)) {
          cutlistData.SetProgramExists(prtID, (int)s["MACHID"], false);
        }
        i++;
      }
    }

    public static string OnlyDigits(string input_string) {
      string res = string.Empty;
      try {
        Regex r = new Regex(@"[^\d]");
        res = r.Replace(input_string, "");
      } catch (ArgumentException) {
        // Bad Regex
      }
      return res;
    }
    
    private void lbPri1_SelectedIndexChanged(object sender, EventArgs e) {
      if (p1_clicked) {
        for (int i = 0; i < lbPri1.Items.Count; i++) {
          if (lbPri1.GetSelected(i)) {
            lbPri2.SetSelected(i, false);
            lbPri3.SetSelected(i, false);
          }
        }
        p1_clicked = false;
      }
    }

    private void lbPri2_SelectedIndexChanged(object sender, EventArgs e) {
      if (p2_clicked) {
        for (int i = 0; i < lbPri2.Items.Count; i++) {
          if (lbPri2.GetSelected(i)) {
            lbPri1.SetSelected(i, false);
            lbPri3.SetSelected(i, false);
          }
        }
        p2_clicked = false;
      }
    }

    private void lbPri3_SelectedIndexChanged(object sender, EventArgs e) {
      if (p3_clicked) {
        for (int i = 0; i < lbPri3.Items.Count; i++) {
          if (lbPri3.GetSelected(i)) {
            lbPri1.SetSelected(i, false);
            lbPri2.SetSelected(i, false);
          }
        }
        p3_clicked = false;
      }
    }

    private void lbPri1_MouseDown(object sender, MouseEventArgs e) {
      p1_clicked = true;
      p2_clicked = false;
      p3_clicked = false;
    }

    private void lbPri2_MouseDown(object sender, MouseEventArgs e) {
      p1_clicked = false;
      p2_clicked = true;
      p3_clicked = false;
    }

    private void lbPri3_MouseDown(object sender, MouseEventArgs e) {
      p1_clicked = false;
      p2_clicked = false;
      p3_clicked = true;
    }

    private void MachineProgramManager_Load(object sender, EventArgs e) {
      Location = Properties.Settings.Default.MPMLocation;
    }

    private void MachineProgramManager_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.MPMLocation = Location;
      Properties.Settings.Default.Save();
    }

    private void btnCncl_Click(object sender, EventArgs e) {
      Close();
    }

    private void btnOK_Click(object sender, EventArgs e) {
      UpdateDB();
      Close();
    }

  }
}