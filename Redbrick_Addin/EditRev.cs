using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin
{
    public partial class EditRev : Form
    {
        private int nodeCount;

        public EditRev(ref DrawingRevs revs, int NodeCount)
        {
            System.Diagnostics.Debug.Print(NodeCount.ToString());
            nodeCount = NodeCount;
            _revs = revs;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            CutlistData cd = new CutlistData();

            cbBy.DataSource = cd.GetAuthors().Tables[0];
            cbBy.DisplayMember = "INITIAL";
            cbBy.ValueMember = "USERNAME";

            if (!Revs.Contains("REVISION " + (char)(nodeCount + 65)))
            {
                cbBy.SelectedValue = Environment.UserName;
                string theRev = "REVISION " + (char)(nodeCount + 65);
                Text = "Creating new " + theRev + "...";
            }
            else
            {
                string theRev = "REVISION " + (char)(nodeCount + 65);
                DrawingRev r = Revs.GetRev(theRev);
                tbECO.Text = r.Eco.Value;
                tbDesc.Text = r.Description.Value;
                cbBy.SelectedValue = cd.GetAuthorUserName(r.List.Value);
                Text = "Editing " + theRev + "...";
            }

            for (int i = 0; i < Properties.Settings.Default.RevLimit; i++)
                cbRev.Items.Add("A" + (char)(i+65));

            cbRev.SelectedIndex = (nodeCount);
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
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DrawingRev r;

            SolidWorks.Interop.swconst.swCustomInfoType_e tType = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText;
            SwProperty rev = new SwProperty("REVISION " + (char)(nodeCount + 65), tType, cbRev.Text, true);
            SwProperty eco = new SwProperty("ECO " + (nodeCount + 1).ToString(), tType, tbECO.Text, true);
            SwProperty desc = new SwProperty("DESCRIPTION " + (nodeCount + 1).ToString(), tType, tbDesc.Text, true);
            cbBy.ValueMember = "INITIAL";
            SwProperty list = new SwProperty("LIST " + (nodeCount + 1).ToString(), tType, cbBy.Text.Substring(0, 2), true);
            cbBy.ValueMember = "LAST";
            SwProperty date = new SwProperty("DATE " + (nodeCount + 1).ToString(), tType, dtpDate.Value.ToShortDateString(), true);

            if (Revs.Contains("REVISION " + (char)(nodeCount + 65)))
            {
                r = Revs.GetRev("REVISION " + (char)(nodeCount + 65));
                r.Revision = rev;
                r.Eco = eco;
                r.Description = desc;
                r.List = list;
                r.Date = date;
            }
            else
            {
                r = new DrawingRev(rev, eco, desc, list, date);
                Revs.Add(r);
            }
            this.Close();
        }


        private void EditRev_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.EditRevLocation = Location;
            Properties.Settings.Default.Save();
        }

        private DrawingRevs _revs;

        public DrawingRevs Revs
        {
            get { return _revs; }
            set { _revs = value; }
        }
    }
}