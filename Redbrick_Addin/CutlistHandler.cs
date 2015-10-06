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
                this.cbCutlist.SelectedIndex = GetIndex(Properties.Settings.Default.CurrentCutlist);
            }
            else
            {
            }
        }

        public void Write()
        {
            int tp = 0;
            if (int.TryParse((this.cbCutlist.SelectedItem as DataRowView)[0].ToString(), out tp))
            {
                Properties.Settings.Default.CurrentCutlist = tp;
                Properties.Settings.Default.Save();
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
