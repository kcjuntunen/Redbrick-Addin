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

        public ConfigurationSpecific(ref SwProperties prop)
        {
            this.propertySet = prop;
            this.cd = prop.cutlistData;
            this._edgeDiffL = 0.0;
            this._edgeDiffW = 0.0;

            InitializeComponent();
            this.fillMat();
            ComboBox[] cc = { this.cbEf, this.cbEb, this.cbEl, this.cbEr };
            foreach (ComboBox c in cc)
            {
                this.fillEdg((object)c);
                
            }
            
            this.LinkControls();
        }

        public void Update(ref SwProperties prop)
        {
            this.propertySet = prop;
            this.configurationName = prop.modeldoc.ConfigurationManager.ActiveConfiguration.Name;
            this.cd = prop.cutlistData;
            this._edgeDiffL = 0.0;
            this._edgeDiffW = 0.0;
            this.LinkControls();
            this.ToggleFields(cd.OpType);
        }

        private void LinkControls()
        {
            this.propertySet.LinkControlToProperty("CUTLIST MATERIAL", false, this.cbMat);
            this.propertySet.LinkControlToProperty("EDGE FRONT (L)", false, this.cbEf);
            this.propertySet.LinkControlToProperty("EDGE BACK (L)", false, this.cbEb);
            this.propertySet.LinkControlToProperty("EDGE LEFT (W)", false, this.cbEl);
            this.propertySet.LinkControlToProperty("EDGE RIGHT (W)", false, this.cbEr);
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
            this.cbMat.DataSource = cd.Materials.Tables[0];
            this.cbMat.DisplayMember = "DESCR";
            this.cbMat.ValueMember = "MATID";
            this.cbMat.Text = string.Empty;
        }

        private void fillEdg(object occ)
        {
            ComboBox c = (ComboBox)occ;
            c.DataSource = cd.Edges.Tables[0];
            c.DisplayMember = "DESCR";
            c.ValueMember = "EDGEID";
            c.Text = string.Empty;
        }

        public void ToggleFields(int opType)
        {
            bool wood = (opType != 2);
            lEf.Enabled = wood;
            leFColor.Enabled = wood;
            cbEf.Enabled = wood;

            lEb.Enabled = wood;
            leBColor.Enabled = wood;
            cbEb.Enabled = wood;

            lEl.Enabled = wood;
            leLColor.Enabled = wood;
            cbEl.Enabled = wood;

            lEr.Enabled = wood;
            leRColor.Enabled = wood;
            cbEr.Enabled = wood;
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

        public string configurationName { get; set; }

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
