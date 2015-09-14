using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace redbrick.csproj
{
    public partial class EditRev : Form
    {
        private int nodeCount;

        public EditRev(ref DrawingRevs revs, int NodeCount)
        {
            System.Diagnostics.Debug.Print(NodeCount.ToString());
            nodeCount = NodeCount;
            this._revs = revs;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            CutlistData cd = new CutlistData();
            
            this.cbBy.DataSource = cd.GetAuthors().Tables[0];
            this.cbBy.DisplayMember = "INITIAL";
            this.cbBy.ValueMember = "INITIAL";

            if (!this.Revs.Contains("REVISION " + (char)(nodeCount + 65)))
            {
                this.cbBy.SelectedIndex = this.GetIndex((cbBy.DataSource as DataTable), Environment.UserName);
                string theRev = "REVISION " + (char)(nodeCount + 65);
                this.Text = "Creating new " + theRev + "...";
            }
            else
            {
                string theRev = "REVISION " + (char)(nodeCount + 65);
                DrawingRev r = this.Revs.GetRev(theRev);
                this.tbECO.Text = r.Eco.Value;
                this.tbDesc.Text = r.Description.Value;
                this.cbBy.SelectedIndex = this.GetIndex((cbBy.DataSource as DataTable), this.cbBy.Text);
                this.Text = "Editing " + theRev + "...";
            }

            for (int i = 0; i < Properties.Settings.Default.RevLimit; i++)
            {
                this.cbRev.Items.Add("A" + (char)(i+65));
            }
            this.cbRev.SelectedIndex = (nodeCount);
        }

        private int GetIndex(DataTable dt, string val)
        {
            if (dt != null)
            {
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    count++;
                    if (dr.ItemArray[1].ToString().Trim().ToUpper() == val.Trim().ToUpper())
                        return count - 1;
                }
            }
            return -1;
        }

        private int GetIndex(DataTable dt, string val, int column)
        {
            if (dt != null)
            {
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    count++;
                    if (dr.ItemArray[column].ToString().Trim().ToUpper() == val.Trim().ToUpper())
                        return count - 1;
                }
            }
            return -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DrawingRev r;

            SolidWorks.Interop.swconst.swCustomInfoType_e tType = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText;
            SwProperty rev = new SwProperty("REVISION " + (char)(nodeCount + 65), tType, this.cbRev.Text, true);
            SwProperty eco = new SwProperty("ECO " + (nodeCount + 1).ToString(), tType, this.tbECO.Text, true);
            SwProperty desc = new SwProperty("DESCRIPTION " + (nodeCount + 1).ToString(), tType, this.tbDesc.Text, true);
            this.cbBy.ValueMember = "INITIAL";
            SwProperty list = new SwProperty("LIST " + (nodeCount + 1).ToString(), tType, this.cbBy.Text.Substring(0, 2), true);
            this.cbBy.ValueMember = "LAST";
            SwProperty date = new SwProperty("DATE " + (nodeCount + 1).ToString(), tType, this.dtpDate.Value.ToShortDateString(), true);

            if (this.Revs.Contains("REVISION " + (char)(nodeCount + 65)))
            {
                r = this.Revs.GetRev("REVISION " + (char)(nodeCount + 65));
                r.Revision = rev;
                r.Eco = eco;
                r.Description = desc;
                r.List = list;
                r.Date = date;
            }
            else
            {
                r = new DrawingRev(rev, eco, desc, list, date);
                this.Revs.Add(r);
            }
#if DEBUG
            System.Windows.Forms.MessageBox.Show(this.Revs.ToString());
#endif
            this.Close();
        }

        private DrawingRevs _revs;

        public DrawingRevs Revs
        {
            get { return _revs; }
            set { _revs = value; }
        }
	
    }
}