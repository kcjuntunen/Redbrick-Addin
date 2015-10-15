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
            this.PropertySet = prop;
            InitializeComponent();

            for (int i = 100; i < 106; i++)
                cbRev.Items.Add(i.ToString());
        }

        public enum WhereUsedRes {
            CLID,
            PARTNUM,
            REV,
            DESCR,
            LENGTH,
            WIDTH,
            HEIGHT,
            CDATE,
            CUSTID,
            SETUP_BY,
            STATE_BY,
            DRAWING,
            QTY
        }

        public void Update(ref SwProperties prop) {
            PropertySet = prop;
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
                    cbRev.Text = drv[(int)WhereUsedRes.REV].ToString();
                    tbQty.Text = drv[(int)WhereUsedRes.QTY].ToString();
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
                    this.PropertySet.cutlistData.InsertIntoCutlist(this.PropertySet);
                }
                btnOriginal.Enabled = false;
            } else {
                // We've affected more than one row. Uh-oh.
            }
        }

        private void btnInsert_Click(object sender, EventArgs e) {
            PropertySet.cutlistData.InsertIntoCutlist(PropertySet);
        }

        public System.Windows.Forms.ComboBox CutlistComboBox { get { return cbCutlist; } }
        public System.Windows.Forms.ComboBox RevComboBox { get { return cbRev; } }
    }
}
