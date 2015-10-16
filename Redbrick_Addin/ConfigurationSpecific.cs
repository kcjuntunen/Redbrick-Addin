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

namespace Redbrick_Addin {
    public partial class ConfigurationSpecific : UserControl {
        private CutlistData cd;
        private SwProperties propertySet;

        public ConfigurationSpecific(ref SwProperties prop) {
            propertySet = prop;
            cd = prop.cutlistData;
            _edgeDiffL = 0.0;
            _edgeDiffW = 0.0;

            InitializeComponent();
            fillMat();
            ComboBox[] cc = { this.cbEf, this.cbEb, this.cbEl, this.cbEr };
            foreach (ComboBox c in cc)
                fillEdg((object)c);

            LinkControls();
        }

        public void Update(ref SwProperties prop) {
            propertySet = prop;
            configurationName = prop.modeldoc.ConfigurationManager.ActiveConfiguration.Name;
            cd = prop.cutlistData;
            _edgeDiffL = 0.0;
            _edgeDiffW = 0.0;
            LinkControls();
            ToggleFields(cd.OpType);
            UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
            UpdateDiff(cbEl, cbEr, ref _edgeDiffW);
        }

        private void LinkControls() {
            propertySet.LinkControlToProperty("MATID", false, this.cbMat);
            propertySet.LinkControlToProperty("EFID", false, this.cbEf);
            propertySet.LinkControlToProperty("EBID", false, this.cbEb);
            propertySet.LinkControlToProperty("ELID", false, this.cbEl);
            propertySet.LinkControlToProperty("ERID", false, this.cbEr);

            if (Properties.Settings.Default.Testing) {
                propertySet.LinkControlToProperty("CUTLIST MATERIAL", false, this.cbMat);
                propertySet.LinkControlToProperty("EDGE FRONT (L)", false, this.cbEf);
                propertySet.LinkControlToProperty("EDGE BACK (L)", false, this.cbEb);
                propertySet.LinkControlToProperty("EDGE LEFT (W)", false, this.cbEl);
                propertySet.LinkControlToProperty("EDGE RIGHT (W)", false, this.cbEr);
            }
        }

        private int GetIndex(DataTable dt, string val) {
            if (dt != null) {
                int count = 0;
                foreach (DataRow dr in dt.Rows) {
                    count++;
                    if (dr.ItemArray[0].ToString().Trim().ToUpper() == val.Trim().ToUpper())
                        return count;
                }
            }
            return -1;
        }

        private void fillComboBoxes() {
            Thread t = new Thread(new ThreadStart(fillMat));
            try {
                t.Start();
            } catch (ThreadStateException tse) {
                System.Windows.Forms.MessageBox.Show(tse.Message);
            } catch (OutOfMemoryException oome) {
                System.Windows.Forms.MessageBox.Show(oome.Message);
            }

            ComboBox[] cc = { cbEf, cbEb, cbEl, cbEr };
            foreach (ComboBox c in cc) {
                t = new Thread(new ParameterizedThreadStart(fillEdg));
                try {
                    t.Start((object)c);
                } catch (ThreadStateException tse) {
                    System.Windows.Forms.MessageBox.Show(tse.Message);
                } catch (OutOfMemoryException oome) {
                    System.Windows.Forms.MessageBox.Show(oome.Message);
                }
            }
        }

        private void fillMat() {
            cbMat.DataSource = cd.Materials.Tables[0];
            cbMat.DisplayMember = "DESCR";
            cbMat.ValueMember = "MATID";
            cbMat.Text = string.Empty;
        }

        private void fillEdg(object occ) {
            ComboBox c = (ComboBox)occ;
            c.DataSource = cd.Edges.Tables[0];
            c.DisplayMember = "DESCR";
            c.ValueMember = "EDGEID";
            c.Text = string.Empty;
        }

        public void ToggleFields(int opType) {
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

        private void UpdateDiff(ComboBox cb1, ComboBox cb2, ref double targ) {
            targ = 0.0;
            double t = 0.0;
            string thk = string.Empty;
            ComboBox[] cc = { cb1, cb2 };

            foreach (ComboBox c in cc) {
                try {
                    System.Data.DataRowView drv = (c.SelectedItem as System.Data.DataRowView);
                    if (drv != null)
                        thk = drv[3].ToString();
                } catch (InvalidCastException ice) {
                    ice.Data.Add("Not", "unused.");
                    thk = "0.0";
                } catch (NullReferenceException nre) {
                    nre.Data.Add("Not", "unused.");
                    thk = "0.0";
                }

                if (double.TryParse(thk, out t)) {
                    targ += t * -1;
                }
            }
        }

        private void cbMat_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbMat.SelectedValue != null)
                lMatColor.Text = (cbMat.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            else
                lMatColor.Text = string.Empty;
        }

        private void cbEf_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbEf.SelectedValue != null)
                leFColor.Text = (cbEf.SelectedItem as System.Data.DataRowView)[2].ToString();
            else
                leFColor.Text = string.Empty;

            UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
        }

        private void cbEb_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbEb.SelectedValue != null)
                leBColor.Text = (cbEb.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            else
                leBColor.Text = string.Empty;

            UpdateDiff(cbEf, cbEb, ref _edgeDiffL);
        }

        private void cbEl_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbEl.SelectedValue != null)
                leLColor.Text = (cbEl.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            else
                leLColor.Text = string.Empty;

            UpdateDiff(cbEl, cbEr, ref _edgeDiffW);
        }

        private void cbEr_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.cbEr.SelectedValue != null)
                leRColor.Text = (cbEr.SelectedItem as System.Data.DataRowView)["COLOR"].ToString();
            else
                leRColor.Text = string.Empty;

            UpdateDiff(cbEl, cbEr, ref _edgeDiffW);
        }

        public string configurationName { get; set; }

        public ComboBox GetCutlistMatBox() {
            return cbMat;
        }

        public ComboBox GetEdgeFrontBox() {
            return cbEf;
        }

        public ComboBox GetEdgeBackBox() {
            return cbEb;
        }

        public ComboBox GetEdgeLeftBox() {
            return cbEl;
        }

        public ComboBox GetEdgeRightBox() {
            return cbEr;
        }

        private double _edgeDiffL;

        public double EdgeDiffL {
            get { return _edgeDiffL; }
            set { _edgeDiffL = value; }
        }

        private double _edgeDiffW;

        public double EdgeDiffW {
            get { return _edgeDiffW; }
            set { _edgeDiffW = value; }
        }
    }
}
