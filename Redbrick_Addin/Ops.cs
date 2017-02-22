using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class Ops : UserControl {
    private CutlistData cd = new CutlistData();
    public SwProperties propertySet;

    public Ops(ref SwProperties prop) {
      cd = prop.cutlistData;
      propertySet = prop;

      InitializeComponent();
    }

    public void Update(ref SwProperties p) {
      propertySet = p;
      OpType = p.cutlistData.OpType;
      RefreshOps(OpType);
      LinkControls();
    }

    private void LinkControls() {
      propertySet.LinkControlToProperty("OP1ID", true, cbOp1);
      propertySet.LinkControlToProperty("OP2ID", true, cbOp2);
      propertySet.LinkControlToProperty("OP3ID", true, cbOp3);
      propertySet.LinkControlToProperty("OP4ID", true, cbOp4);
      propertySet.LinkControlToProperty("OP5ID", true, cbOp5);

      if (Properties.Settings.Default.Testing) {
        propertySet.LinkControlToProperty("OP1", true, cbOp1);
        propertySet.LinkControlToProperty("OP2", true, cbOp2);
        propertySet.LinkControlToProperty("OP3", true, cbOp3);
        propertySet.LinkControlToProperty("OP4", true, cbOp4);
        propertySet.LinkControlToProperty("OP5", true, cbOp5);
      }
    }

    public void GetProperties() {
      for (int i = 0; i < 6; i++) {
        string op = string.Format("OP{0}", i.ToString());

        foreach (Control c in this.tableLayoutPanel1.Controls) {
          if ((c is ComboBox) && c.Name.ToUpper().Contains(op)) {
            ComboBox cb = (c as ComboBox);

            propertySet.GetProperty(op).Ctl = c;

            cb.ValueMember = "OPID";
            cb.DisplayMember = "OPDESCR";

            cb.SelectedValue = int.Parse(propertySet.GetProperty(op).Value);

            SwProperty p = this.propertySet.GetProperty(op);
            p.ID = (cb.SelectedItem as DataRowView).Row.ItemArray[0].ToString();
            p.Value = (cb.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
            p.ResValue = (cb.SelectedItem as DataRowView).Row.ItemArray[2].ToString();

            p.Table = "CUT_PARTS";
            p.Field = string.Format("OP{0}ID", c.Name.Split('p')[1]);
          }
        }
      }
    }

    private void fillBox(object occ) {
      ComboBox c = (ComboBox)occ;
      propertySet.cutlistData.OpType = OpType;
      if (!c.IsDisposed) {
        c.DisplayMember = "OPDESCR";
        c.ValueMember = "OPID";
        c.DataSource = propertySet.cutlistData.Ops.Tables[0];
        c.SelectedText = string.Empty;
        c.SelectedValue = 0;
      }
    }

    public void RefreshOps(int opType) {
      OpType = opType;
      ComboBox[] cc = { cbOp1, cbOp2, cbOp3, cbOp4, cbOp5 };
      foreach (ComboBox c in cc) {
        fillBox((object)c);
      }
      //this.GetProperties();
    }

    public EventArgs RefreshOpBoxes(int opType) {
      EventArgs e = new EventArgs();
      RefreshOps(opType);
      return e;
    }

    private int GetIndex(DataTable dt, string val) {
      if (dt != null) {
        int count = 0;
        foreach (DataRow dr in dt.Rows) {
          count++;

          if (dr.ItemArray[0].ToString().Trim().ToUpper() == val.Trim().ToUpper())
            return count;
        }
      }
      return -1;
    }

    private void ResolveText(object sender, EventArgs e) {
      if (sender is ComboBox) {
        ComboBox s = (sender as ComboBox);
        if (s.Text.Trim() == string.Empty) {
          s.SelectedIndex = s.Items.Count - 1;
        } else {
          s.SelectedIndex = s.FindString(s.Text.Trim());
        }
        s.SelectionStart = 0;
        s.SelectionLength = s.Text.Length;
      }
    }

    private void FocusHere(object sender, MouseEventArgs e) {
      if (sender is ComboBox) {
        if ((sender as ComboBox).DroppedDown) {
          //
        } else {
          (sender as ComboBox).Focus();
          ResolveText((sender as ComboBox), new EventArgs());
        }
      } else if (sender is TextBox) {
        (sender as TextBox).Focus();
      }
    }

    private void combobox_KeyDown(object sender, KeyEventArgs e) {
      if (sender is ComboBox)
        (sender as ComboBox).DroppedDown = false;
    }

    public Control GetOp1Box() {
      return cbOp1;
    }

    public Control GetOp2Box() {
      return cbOp2;
    }

    public Control GetOp3Box() {
      return cbOp3;
    }

    public Control GetOp4Box() {
      return cbOp4;
    }

    public Control GetOp5Box() {
      return cbOp5;
    }

    public int OpType { get; set; }

    private void label1_Click(object sender, EventArgs e) {
      Form f = new Form();
      Ops2 op2 = new Ops2(ref propertySet);
      f.Text = propertySet.PartName;
      op2.Dock = DockStyle.Fill;
      f.Controls.Add(op2);
      f.Show(this);
    }
  }
}
