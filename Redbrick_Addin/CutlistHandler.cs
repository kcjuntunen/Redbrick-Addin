using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

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

        public void Update(ref SwProperties prop)
        {
            this.PropertySet = prop;
            this.cbCutlist.DisplayMember = "PARTNUM";
            this.cbCutlist.ValueMember = "CLID";
            DataSet ds = prop.cutlistData.GetWherePartUsed(PropertySet.PartName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.cbCutlist.DataSource = ds.Tables[0];
                int idx = GetIndex(Properties.Settings.Default.CurrentCutlist);

                if (idx < this.cbCutlist.Items.Count)
                    this.cbCutlist.SelectedIndex = idx;

                this.tbDescription.Text = (this.cbCutlist.SelectedItem as DataRowView)[3].ToString();
                this.tbL.Text = (this.cbCutlist.SelectedItem as DataRowView)[4].ToString();
                this.tbW.Text = (this.cbCutlist.SelectedItem as DataRowView)[5].ToString();
                this.tbH.Text = (this.cbCutlist.SelectedItem as DataRowView)[6].ToString();
                this.tbRef.Text = (this.cbCutlist.SelectedItem as DataRowView)[11].ToString();
                this.dateTimePicker1.Text = (this.cbCutlist.SelectedItem as DataRowView)[7].ToString();
                this.cbRev.Text = ((this.cbCutlist.SelectedItem as DataRowView)[2].ToString());
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

        private void cbCutlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
