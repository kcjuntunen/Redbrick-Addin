#undef DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using SolidWorks.Interop.swconst;

namespace Redbrick_Addin
{
    public partial class ConfigurationSpecific : UserControl
    {
        private CutlistData cd;
        private SwProperties propertySet;

        //private List<ComboBox> cc = new List<ComboBox>();
        public ConfigurationSpecific(ref SwProperties prop)
        {
            propertySet = prop;
            cd = prop.cutlistData;
            this._edgeDiffL = 0.0;
            this._edgeDiffW = 0.0;

            InitializeComponent();
            //this.fillComboBoxes();
            this.fillMat();
            ComboBox[] cc = { this.cbEf, this.cbEb, this.cbEl, this.cbEr };
            foreach (ComboBox c in cc)
            {
                this.fillEdg((object)c);
                
            }
            
            this.LinkControls();
        }

        private void LinkControls()
        {

            this.LinkControlToProperty("CUTLIST MATERIAL", this.cbMat);
            this.LinkControlToProperty("EDGE FRONT (L)", this.cbEf);
            this.LinkControlToProperty("EDGE BACK (L)", this.cbEb);
            this.LinkControlToProperty("EDGE LEFT (W)", this.cbEl);
            this.LinkControlToProperty("EDGE RIGHT (W)", this.cbEr);
        }

        private void LinkControlToProperty(string property, Control c)
        {
            SwProperty p = this.propertySet.GetProperty(property);
            ComboBox cb = (c as ComboBox);
            if (this.propertySet.Contains(p))
            {
                p.Type = swCustomInfoType_e.swCustomInfoText;
                p.Ctl = c;
                int idx = this.GetIndex((cb.DataSource as DataTable), p.Value.Trim());
#if DEBUG
                System.Diagnostics.Debug.Print(string.Format("Linking {0}\nGot index {1} for value {2}.", p.Name, idx, p.Value));
#endif
                if (idx > cb.Items.Count - 1) idx = -1;

                (c as ComboBox).SelectedIndex = idx;
#if DEBUG
                System.Diagnostics.Debug.Print(p.Value + " " + idx);
#endif
            }
            else
            {
#if DEBUG
                //System.Diagnostics.Debug.Print("Creating " + p.Name);
#endif
                SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                x.Type = swCustomInfoType_e.swCustomInfoText;
                x.Ctl = c;
                this.propertySet.Add(x);
            }
        }

        public void GetProperties()
        {
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
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

        private void fillComboBoxes()
        {
            Thread t = new Thread(new ThreadStart(this.fillMat));
            try
            {
                t.Start();
            }
            catch (ThreadStateException tse)
            {
                System.Windows.Forms.MessageBox.Show(tse.Message);
            }
            catch (OutOfMemoryException oome)
            {
                System.Windows.Forms.MessageBox.Show(oome.Message);
            }

            ComboBox[] cc = { this.cbEf, this.cbEb, this.cbEl, this.cbEr };
            foreach (ComboBox c in cc)
            {
                t = new Thread(new ParameterizedThreadStart(this.fillEdg));
                try
                {
                    t.Start((object)c);
                }
                catch (ThreadStateException tse)
                {
                    System.Windows.Forms.MessageBox.Show(tse.Message);
                }
                catch (OutOfMemoryException oome)
                {
                    System.Windows.Forms.MessageBox.Show(oome.Message);
                }
            }
        }

        private void fillMat()
        {
            //System.Windows.Forms.MessageBox.Show("fillMat()");
            this.cbMat.DataSource = cd.Materials.Tables[0];
            this.cbMat.DisplayMember = "DESCR";
            this.cbMat.ValueMember = "MATID";
        }

        private void fillEdg(object occ)
        {
            ComboBox c = (ComboBox)occ;
            c.DataSource = cd.Edges.Tables[0];
            c.DisplayMember = "DESCR";
            c.ValueMember = "EDGEID";
        }

        public void ToggleFields(string opType)
        {
            bool wood = (opType != "METAL");
            lEf.Visible = wood;
            leFColor.Visible = wood;
            cbEf.Visible = wood;

            lEb.Visible = wood;
            leBColor.Visible = wood;
            cbEb.Visible = wood;

            lEl.Visible = wood;
            leLColor.Visible = wood;
            cbEl.Visible = wood;

            lEr.Visible = wood;
            leRColor.Visible = wood;
            cbEr.Visible = wood;

            if (opType != "WOOD")
            {
                for (int row = this.tableLayoutPanel1.RowCount - 1; row >= 0; row--)
                {
                    bool hasControl = false;
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
			        {
                        if (this.tableLayoutPanel1.GetControlFromPosition(col, row) != null)
                        {
                            hasControl = true;
                            break;
                        }
			        }
                    if (!hasControl)
	                {
                        this.tableLayoutPanel1.RowStyles.RemoveAt(row);
                        tableLayoutPanel1.RowCount--;
	                }
                }
            }
            else
            {
            }
        }

        private void UpdateDiffW(ComboBox cbl, ComboBox cbr)
        {
            this._edgeDiffW = 0.0;
            double t = 0.0;
            string thk = string.Empty;
            try
            {
                System.Data.DataRowView drv = (System.Data.DataRowView)cbl.SelectedItem;
                if (drv != null)
                    thk = drv[3].ToString();
            }
            catch (InvalidCastException ice)
            {
                ice.Data.Add("Not", "unused.");
                thk = "0.0";
            }
            catch (NullReferenceException nre)
            {
                nre.Data.Add("Not", "unused.");
                thk = "0.0";
            }

            if (double.TryParse(thk, out t))
            {
                this._edgeDiffW += t * -1;
            }

            try
            {
                System.Data.DataRowView drv = (System.Data.DataRowView)cbr.SelectedItem;
                if (drv != null)
                    thk = drv[3].ToString();
            }
            catch (InvalidCastException ice)
            {
                ice.Data.Add("Not", "unused.");
                thk = "0.0";
            }
            catch (NullReferenceException nre)
            {
                nre.Data.Add("Not", "unused.");
                thk = "0.0";
            }

            if (double.TryParse(thk, out t))
            {
                this._edgeDiffW += t * -1;
            }
        }

        private void UpdateDiffL(ComboBox cbf, ComboBox cbb)
        {
            this._edgeDiffL = 0.0;
            double t = 0.0;
            string thk = string.Empty;
            try
            {
                System.Data.DataRowView drv = (System.Data.DataRowView)cbf.SelectedItem;
                if (drv != null)
                    thk = drv[3].ToString();
            }
            catch (InvalidCastException ice)
            {
                ice.Data.Add("Not", "unused.");
                thk = "0.0";
            }
            catch (NullReferenceException nre)
            {
                nre.Data.Add("Not", "unused.");
                thk = "0.0";
            }

            if (double.TryParse(thk, out t))
            {
                this._edgeDiffL += t * -1;
            }

            try
            {
                System.Data.DataRowView drv = (System.Data.DataRowView)cbb.SelectedItem;
                if (drv != null)
                    thk = drv[3].ToString();
            }
            catch (InvalidCastException ice)
            {
                ice.Data.Add("Not", "unused.");
                thk = "0.0";
            }
            catch (NullReferenceException nre)
            {
                nre.Data.Add("Not", "unused.");
                thk = "0.0";
            }

            if (double.TryParse(thk, out t))
            {
                this._edgeDiffL += t * -1;
            }
        }

        private void cbMat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbMat.SelectedValue != null)
                this.lMatColor.Text = (this.cbMat.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            else
                this.lMatColor.Text = string.Empty;
        }

        private void cbEf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbEf.SelectedValue != null)
            {
                this.leFColor.Text = (this.cbEf.SelectedItem as System.Data.DataRowView)[2].ToString();
            }
            else
            {
                this.leFColor.Text = string.Empty;
            }

            this.UpdateDiffL(this.cbEf, this.cbEb);
        }

        private void cbEb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbEb.SelectedValue != null)
            {
                this.leBColor.Text = (this.cbEb.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            }
            else
            {
                this.leBColor.Text = string.Empty;
            }

            this.UpdateDiffL(this.cbEf, this.cbEb);
        }

        private void cbEl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbEl.SelectedValue != null)
            {
                this.leLColor.Text = (this.cbEl.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            }
            else
            {
                this.leLColor.Text = string.Empty;
            }

            this.UpdateDiffW(this.cbEl, this.cbEr);
        }

        private void cbEr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbEr.SelectedValue != null)
            {
                this.leRColor.Text = (this.cbEr.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            }
            else
            {
                this.leRColor.Text = string.Empty;
            }

            this.UpdateDiffW(this.cbEl, this.cbEr);
        }

        public ComboBox GetCutlistMatBox()
        {
            return this.cbMat;
        }

        public ComboBox GetEdgeFrontBox()
        {
            return this.cbEf;
        }

        public ComboBox GetEdgeBackBox()
        {
            return this.cbEb;
        }

        public ComboBox GetEdgeLeftBox()
        {
            return this.cbEl;
        }

        public ComboBox GetEdgeRightBox()
        {
            return this.cbEr;
        }

        private double _edgeDiffL;

        public double EdgeDiffL
        {
            get { return _edgeDiffL; }
            set { _edgeDiffL = value; }
        }

        private double _edgeDiffW;

        public double EdgeDiffW
        {
            get { return _edgeDiffW; }
            set { _edgeDiffW = value; }
        }
    }
}
