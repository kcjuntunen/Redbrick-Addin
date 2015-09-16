using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin
{
    public partial class DepartmentSelector : UserControl
    {
        public DepartmentSelector(ref SwProperties p)
        {
            this.PropertySet = p;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.cbDepartment.ValueMember = "TYPEID";
            this.cbDepartment.DisplayMember = "TYPEDESC";
            this.cbDepartment.DataSource = this.PropertySet.cutlistData.OpTypes.Tables[0];
        }

        public void updateDept()
        {
            int idx = this.GetIndex((this.cbDepartment.DataSource as DataTable), this.OpType.ToString()) - 1;
            this.cbDepartment.SelectedIndex = idx;
            this.cbDepartment.DisplayMember = "TYPEDESC";
        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tp = 1; // wood
            string val = ((this.cbDepartment.SelectedItem as DataRowView)[this.cbDepartment.ValueMember]).ToString();
            if (int.TryParse(val, out tp))
            {
                this.OpType = tp;   
            }
        }

        private int GetIndex(DataTable dt, string val)
        {
            if (dt != null)
            {
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    count++;

                    if (dr.ItemArray[0].ToString().Trim().ToUpper() == val.Trim().ToUpper())
                        return count;
                }
            }
            return -1;
        }

        public SwProperties PropertySet { get; set; }
        public int OpType { get; set; }
    }
}
