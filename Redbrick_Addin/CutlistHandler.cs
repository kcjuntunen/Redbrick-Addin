using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class CutlistHandler : UserControl {
    public CutlistHandler(ref SwProperties prop) {
      PropertySet = prop;
      InitializeComponent();
      FillBoxes();
    }

    private void FillBoxes() {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());

      cbStatus.DataSource = PropertySet.cutlistData.States.Tables[0];
      cbStatus.DisplayMember = "STATE";
      cbStatus.ValueMember = "ID";
    }

    public enum WhereUsedRes {
      CLID,
      PARTNUM,
      //REV,
      DESCR,
      LENGTH, WIDTH, HEIGHT,
      CDATE,
      CUSTID,
      SETUP_BY, STATE_BY,
      DRAWING,
      QTY,
      STATEID
    }

    private void BlankCtrls(Control ctrl) {
      foreach (Control c in ctrl.Controls) {
        if (c is ComboBox) {
          (c as ComboBox).SelectedIndex = -1;
        } else if (c is TextBox) {
          c.Text = string.Empty;
        }
        if (c.HasChildren) {
          BlankCtrls(c);
        }
      }
    }

    public void Update(ref SwProperties prop) {
      PropertySet = prop;
      BlankCtrls(this);
      DataSet ds = prop.cutlistData.GetWherePartUsed(PropertySet.PartName);
      cbCutlist.DataSource = ds.Tables[(int)WhereUsedRes.CLID];
      cbCutlist.DisplayMember = "PARTNUM";
      cbCutlist.ValueMember = "CLID";

      cbCustomer.DataSource = prop.cutlistData.Customers.Tables[0];
      cbCustomer.ValueMember = "CUSTID";
      cbCustomer.DisplayMember = "CUSTOMER";

      if (ds.Tables[0].Rows.Count > 0) {
        if (cbCutlist.Items.Contains(Properties.Settings.Default.CurrentCutlist))
          cbCutlist.SelectedValue = Properties.Settings.Default.CurrentCutlist;
        
        if (cbCutlist.SelectedValue != null) {
          DataRowView drv = (this.cbCutlist.SelectedItem as DataRowView);
          tbDescription.Text = drv[(int)WhereUsedRes.DESCR].ToString();
          tbL.Text = drv[(int)WhereUsedRes.LENGTH].ToString();
          tbW.Text = drv[(int)WhereUsedRes.WIDTH].ToString();
          tbH.Text = drv[(int)WhereUsedRes.HEIGHT].ToString();
          tbRef.Text = drv[(int)WhereUsedRes.DRAWING].ToString();
          dateTimePicker1.Text = drv[(int)WhereUsedRes.CDATE].ToString();
          //cbRev.Text = drv[(int)WhereUsedRes.REV].ToString();
          tbQty.Text = drv[(int)WhereUsedRes.QTY].ToString();
          cbStatus.SelectedValue = drv[(int)WhereUsedRes.STATEID].ToString();
          prop.CutlistQuantity = drv[(int)WhereUsedRes.QTY].ToString();
          prop.CutlistID = drv[(int)WhereUsedRes.CLID].ToString();

          cbCustomer.SelectedValue = int.Parse(drv[(int)WhereUsedRes.CUSTID].ToString());
        }

        if (prop.cutlistData.ReturnHash(prop) == prop.Hash) {
          prop.Primary = true;
          btnOriginal.Enabled = false;
        } else {
          prop.Primary = false;
          btnOriginal.Enabled = true;
        }
      } else {
      }
    }

    public void Write() {
      int tp = 0;
      if (this.cbCutlist.SelectedItem != null) {
        if (int.TryParse((this.cbCutlist.SelectedItem as DataRowView)[0].ToString(), out tp)) {
          Properties.Settings.Default.CurrentCutlist = tp;
          Properties.Settings.Default.Save();
        }
      }
    }

    private int GetIndex(int ID, ComboBox c) {
      int idx = 0;
      int tp = 0;
      foreach (DataRowView item in c.Items) {
        if (int.TryParse(item[0].ToString(), out tp)) {
          if (tp == ID) {
            return idx;
          }
        }
        idx++;
      }
      return idx;
    }

    public SwProperties PropertySet { get; set; }

    public string CutlistID {
      get {
        if (this.cbCutlist.SelectedItem != null)
          return (this.cbCutlist.SelectedItem as DataRowView)[(int)WhereUsedRes.CLID].ToString();
        else
          return "0";
      }
      set { CutlistID = value; }
    }

    public string CutlistQty {
      get {
        if (tbQty.Text != string.Empty)
          return this.tbQty.Text;
        else
          return "0";
      }
      set { CutlistQty = value; }
    }

    private void cbCutlist_SelectedIndexChanged(object sender, EventArgs e) {
    }

    private void btnOriginal_Click(object sender, EventArgs e) {
      int affrow = this.PropertySet.cutlistData.MakeOriginal(this.PropertySet);
      if (affrow == 1) {
        //success
        PropertySet.Primary = true;
        btnOriginal.Enabled = false;
      } else if (affrow < 1) {
        // Part didn't exist
        int diaRes = this.PropertySet.SwApp.SendMsgToUser2("Insert " + this.PropertySet.PartName + " into Cutlist?",
            (int)swMessageBoxIcon_e.swMbQuestion,
            (int)swMessageBoxBtn_e.swMbYesNo);
        if (diaRes == (int)swMessageBoxResult_e.swMbHitYes) {
          //PropertySet.cutlistData.InsertIntoCutlist(MakePartFromPropertySet(PropertySet));
        }
        btnOriginal.Enabled = false;
      } else {
        // We've affected more than one row. Uh-oh.
      }
    }

    private void btnInsert_Click(object sender, EventArgs e) {
      string[] s = cbCutlist.Text.Split(new string[] { "REV" }, StringSplitOptions.None);
      if (s.Length > 1) {
        CutlistHeaderInfo chi = new CutlistHeaderInfo(MakePartFromPropertySet(PropertySet), PropertySet.cutlistData, s[0].Trim(), s[1].Trim());
        chi.ShowDialog();
      } else {
        CutlistHeaderInfo chi = new CutlistHeaderInfo(MakePartFromPropertySet(PropertySet), PropertySet.cutlistData);
        chi.ShowDialog();
      }
    }

    Part MakePartFromPropertySet(SwProperties swp) {
      Part p = new Part();
      p.Hash = (uint)swp.Hash;
      p.FileInformation = swp.PartFileInfo;
      p.SetBlankQty(swp.CutlistQuantity);
      p.SetDeptID(swp.GetProperty("DEPT").ID);
      p.SetEdgeFrontID(swp.GetProperty("EFID").ID);
      p.SetEdgeBackID(swp.GetProperty("EBID").ID);
      p.SetEdgeLeftID(swp.GetProperty("ELID").ID);
      p.SetEdgeRightID(swp.GetProperty("ERID").ID);
      p.SetLength(swp.GetProperty("LENGTH").ResValue);
      p.SetWidth(swp.GetProperty("WIDTH").ID);
      p.SetThickness(swp.GetProperty("THICKNESS").ID);
      p.SetMaterialID(swp.GetProperty("MATID").ID);
      p.SetOpID(swp.GetProperty("OP1ID").ID, 0);
      p.SetOpID(swp.GetProperty("OP2ID").ID, 1);
      p.SetOpID(swp.GetProperty("OP3ID").ID, 2);
      p.SetOpID(swp.GetProperty("OP4ID").ID, 3);
      p.SetOpID(swp.GetProperty("OP5ID").ID, 4);
      p.SetOverL(swp.GetProperty("OVERL").Value);
      p.SetOverW(swp.GetProperty("OVERW").Value);
      p.SetQuantity(swp.CutlistQuantity);
      p.SetUpdateCNC(swp.GetProperty("UPDATE CNC").ID);
      string hash = string.Format("{0:X}", swp.GetProperty("CRC32").Value);
      p.Hash = uint.Parse(hash, System.Globalization.NumberStyles.HexNumber);
      return p;
    }

    public System.Windows.Forms.ComboBox CutlistComboBox { get { return cbCutlist; } }
    public System.Windows.Forms.ComboBox RevComboBox { get { return cbRev; } }
  }
}
