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
            this.LinkControlToProperty();
            this.PropertySet.cutlistData.OpType = this.OpType;
            int idx = this.GetIndex((this.cbDepartment.DataSource as DataTable), (this.OpType).ToString());
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
                this.PropertySet.cutlistData.OpType = tp;
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
        }

        private int GetIndex(DataTable dt, string val)
        {
            int count = 1;
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    count++;

                    if (dr.ItemArray[0].ToString().Trim().ToUpper() == val.Trim().ToUpper())
                        return count + 1;
                }
            }
            return 1;
        }

        public SwProperties PropertySet { get; set; }
        public int OpType { get; set; }
        public SolidWorks.Interop.sldworks.SldWorks SwApp { get; set; }
    }
}
