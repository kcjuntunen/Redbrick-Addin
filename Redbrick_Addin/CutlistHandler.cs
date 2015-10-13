using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


using SolidWorks.Interop.swconst;

namespace Redbrick_Addin
{
    public partial class CutlistHandler : UserControl
    {
        public CutlistHandler(ref SwProperties prop)
        {
            this.PropertySet = prop;
            InitializeComponent();

            for (int i = 100; i < 106; i++)
                this.cbRev.Items.Add(i.ToString());
        }

        public enum WhereUsedRes
        {
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

        public void Update(ref SwProperties prop)
        {
            this.PropertySet = prop;
            this.cbCutlist.DisplayMember = "PARTNUM";
            this.cbCutlist.ValueMember = "CLID";
            DataSet ds = prop.cutlistData.GetWherePartUsed(PropertySet.PartName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.cbCutlist.DataSource = ds.Tables[(int)WhereUsedRes.CLID];
                int idx = GetIndex(Properties.Settings.Default.CurrentCutlist);

                if (idx < this.cbCutlist.Items.Count)
                    this.cbCutlist.SelectedIndex = idx;
                DataRowView drv = (this.cbCutlist.SelectedItem as DataRowView);
                this.tbDescription.Text = drv[(int)WhereUsedRes.DESCR].ToString();
                this.tbL.Text = drv[(int)WhereUsedRes.LENGTH].ToString();
                this.tbW.Text = drv[(int)WhereUsedRes.WIDTH].ToString();
                this.tbH.Text = drv[(int)WhereUsedRes.HEIGHT].ToString();
                this.tbRef.Text = drv[(int)WhereUsedRes.DRAWING].ToString();
                this.dateTimePicker1.Text = drv[(int)WhereUsedRes.CDATE].ToString();
                this.cbRev.Text = drv[(int)WhereUsedRes.REV].ToString();
                this.tbQty.Text = drv[(int)WhereUsedRes.QTY].ToString();
                prop.CutlistQuantity = drv[(int)WhereUsedRes.QTY].ToString();
                prop.CutlistID = drv[(int)WhereUsedRes.CLID].ToString();

                if (prop.cutlistData.ReturnHash(prop) == prop.Hash)
                {
                    this.btnOriginal.Enabled = false;
                    //this.btnOriginal.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.btnOriginal.Enabled = true;
                    //this.btnOriginal.BackColor = System.Drawing.Color.Red;
                }
            }
            else
            {
            }
        }

        public void Write()
        {
            int tp = 0;
            if (this.cbCustomer.SelectedItem != null)
            {
                if (int.TryParse((this.cbCutlist.SelectedItem as DataRowView)[0].ToString(), out tp))
                {
                    Properties.Settings.Default.CurrentCutlist = tp;
                    Properties.Settings.Default.Save();
                }   
            }
        }

        private int GetIndex(int clID)
        {
            int idx = 0;
            int tp = 0;
            foreach (DataRowView item in this.cbCutlist.Items)
            {
                if (int.TryParse(item[0].ToString(), out tp))
                {
                    if (tp == clID)
                    {
                        return idx;
                    }   
                }
                idx++;
            }
            return idx;
        }

        public SwProperties PropertySet { get; set; }

        public string CutlistID 
        {
            get { return (this.cbCutlist.SelectedItem as DataRowView)[(int)WhereUsedRes.CLID].ToString(); }
            set { CutlistID = value; }
        }

        public string CutlistQty
        {
            get { return this.tbQty.Text; }
            set { CutlistQty = value; }
        }

        private void cbCutlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOriginal_Click(object sender, EventArgs e)
        {
            int affrow =this.PropertySet.cutlistData.MakeOriginal(this.PropertySet);
            if (affrow == 1)
            {   
                //success
            }
            else if (affrow < 1)
            {
                // Part didn't exist
                int diaRes = this.PropertySet.SwApp.SendMsgToUser2("Insert " + this.PropertySet.PartName + " into Cutlist?",
                    (int)swMessageBoxIcon_e.swMbQuestion,
                    (int)swMessageBoxBtn_e.swMbYesNo);
                if (diaRes == (int)swMessageBoxResult_e.swMbHitYes)
                {
                    this.PropertySet.cutlistData.InsertIntoCutlist(this.PropertySet);
                }
                this.btnOriginal.Enabled = false;
            }
            else
            {
                // We've affected more than one row. Uh-oh.
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            this.PropertySet.cutlistData.InsertIntoCutlist(this.PropertySet);
        }
    }
}
