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

namespace Redbrick_Addin
{
    public partial class Ops : UserControl
    {
        private CutlistData cd = new CutlistData();
        public SwProperties propertySet;

        public Ops(ref SwProperties prop)
        {
            this.cd = prop.cutlistData;
            this.propertySet = prop;

            InitializeComponent();
            this.OpType = 1;
            this.RefreshOps(this.OpType);
        }

        public void GetProperties()
        {
            for (int i = 0; i < 6; i++)
            {
                string op = string.Format("OP{0}", i.ToString());

                foreach (Control c in this.tableLayoutPanel1.Controls)
                {
                    if ((c is ComboBox) && c.Name.ToUpper().Contains(op))
                    {
                        ComboBox cb = (c as ComboBox);

                        if (!this.propertySet.Contains(op))
                        {
                            SwProperty ps = new SwProperty(op, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                            ps.ID = (cb.SelectedItem as DataRowView).Row.ItemArray[0].ToString();
                            ps.Value = (cb.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                            ps.ResValue = (cb.SelectedItem as DataRowView).Row.ItemArray[2].ToString();

                            ps.Table = "CUT_PARTS";
                            ps.Field = string.Format("OP{0}ID", c.Name.Split('p')[1]);
                            ps.SwApp = this.propertySet.SwApp;
                            this.propertySet.Add(ps);
                        }

                        this.propertySet.GetProperty(op).Ctl = c;
                        cb.DisplayMember = "OPID";

                        int idx = this.GetIndex((cb.DataSource as DataTable),
                            this.propertySet.GetProperty(op).Value);

                        System.Diagnostics.Debug.Print(this.GetIndex((cb.DataSource as DataTable),
                            this.propertySet.GetProperty(op).Value).ToString());

                        if (idx > cb.Items.Count - 1) idx = 0;

                        cb.SelectedIndex = idx;
                        cb.DisplayMember = "OPDESCR";

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

        public void LinkControls()
        {
            for (int i = 0; i < 6; i++)
            {
                string op = string.Format("OP{0}", i.ToString());

                foreach (Control c in this.tableLayoutPanel1.Controls)
                {
                    if ((c is ComboBox) && c.Name.ToUpper().Contains(op))
                    {
                        ComboBox cb = (c as ComboBox);

                        if (!this.propertySet.Contains(op))
                        {
                            SwProperty ps = new SwProperty(op, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                            ps.ID = (cb.SelectedItem as DataRowView).Row.ItemArray[0].ToString();
                            ps.Value = (cb.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                            ps.ResValue = (cb.SelectedItem as DataRowView).Row.ItemArray[2].ToString();

                            ps.Table = "CUT_PARTS";
                            ps.Field = string.Format("OP{0}ID", c.Name.Split('p')[1]);
                            ps.SwApp = this.propertySet.SwApp;
                            this.propertySet.Add(ps);
                        }

                        this.propertySet.GetProperty(op).Ctl = c;
                        cb.DisplayMember = "OPID";

                        int idx = this.GetIndex((cb.DataSource as DataTable),
                            this.propertySet.GetProperty(op).Value);

                        System.Diagnostics.Debug.Print(this.GetIndex((cb.DataSource as DataTable),
                            this.propertySet.GetProperty(op).Value).ToString());

                        if (idx > cb.Items.Count - 1) idx = 0;

                        cb.SelectedIndex = idx;
                        cb.DisplayMember = "OPDESCR";

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

        private void fillBox(object occ)
        {
            ComboBox c = (ComboBox)occ;
            //c.DataSource = cd.GetOps(this.OpType).Tables[0];
            c.DisplayMember = "OPDESCR";
            c.ValueMember = "OPNAME";
            this.propertySet.cutlistData.OpType = this.OpType;
            c.DataSource = this.propertySet.cutlistData.Ops.Tables[0];
        }

        public void RefreshOps(int opType)
        {
            this.OpType = opType;
            ComboBox[] cc = { this.cbOp1, this.cbOp2, this.cbOp3, this.cbOp4, this.cbOp5 };
            foreach (ComboBox c in cc)
            {
                this.fillBox((object)c);
            }
            this.GetProperties();
        }

        public EventArgs RefreshOpBoxes(int opType)
        {
            EventArgs e = new EventArgs();
            this.RefreshOps(opType);
            return e;
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

        public Control GetOp1Box()
        {
            return this.cbOp1;
        }

        public Control GetOp2Box()
        {
            return this.cbOp2;
        }

        public Control GetOp3Box()
        {
            return this.cbOp3;
        }

        public Control GetOp4Box()
        {
            return this.cbOp4;
        }

        public Control GetOp5Box()
        {
            return this.cbOp5;
        }

        public int OpType { get; set; }
    }
}
