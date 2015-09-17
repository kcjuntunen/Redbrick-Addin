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
            this.SwApp = p.SwApp;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.cbDepartment.ValueMember = "TYPEID";
            this.cbDepartment.DisplayMember = "TYPEDESC";
            this.cbDepartment.DataSource = this.PropertySet.cutlistData.OpTypes.Tables[0];
        }

        public void Update(ref SwProperties p)
        {
            this.PropertySet = p;
            int idx = this.GetIndex((this.cbDepartment.DataSource as DataTable), this.OpType.ToString()) - 1;
            this.cbDepartment.SelectedIndex = idx;
            this.cbDepartment.DisplayMember = "TYPEDESC";
            this.LinkControlToProperty();
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

        private void LinkControlToProperty()
        {
            string pn = "DEPARTMENT";
            string dept;
            if (this.PropertySet.Contains(pn))
            {
                dept = this.PropertySet.GetProperty(pn).Value;
                int tp = 1;

                if (int.TryParse(dept, out tp))
                {
                    this.OpType = tp;
                }
                else
                {
                    this.OpType = this.PropertySet.cutlistData.GetOpTypeIDByName(dept);
                }
                dept = tp.ToString();
            }
            else
            {
                SolidWorks.Interop.swconst.swCustomInfoType_e t = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoNumber;
                SwProperty p = new SwProperty(pn, t, "1", true);
                p.SwApp = this.SwApp;
                p.Ctl = this.cbDepartment;
                this.PropertySet.Add(p);
                this.OpType = 1;
            }
            this.updateDept();
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
        public SolidWorks.Interop.sldworks.SldWorks SwApp { get; set; }
    }
}
