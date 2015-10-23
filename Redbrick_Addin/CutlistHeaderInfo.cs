using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin {
  public partial class CutlistHeaderInfo : Form {
    public CutlistHeaderInfo() {
      InitializeComponent();
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      Size = Properties.Settings.Default.CutlistHeaderSize;
    }

    public CutlistHeaderInfo(DrawingProperties dp) {
      InitializeComponent();
      DrawingPropertySet = dp;
      CutlistData = dp.CutlistData;
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      Size = Properties.Settings.Default.CutlistHeaderSize;
      InitControls();
    }

    private void InitControls() {
      string[] title = (DrawingPropertySet.SwApp.ActiveDoc as ModelDoc2).GetTitle().Split(new string[] { " " }, StringSplitOptions.None);
      cbDrawingReference.Text = title[0];
      cbItemNo.Text = title[0];
      cbCustomer.DataSource = CutlistData.Customers.Tables[0];
      cbCustomer.DisplayMember = "CUSTOMER";
      cbCustomer.ValueMember = "CUSTID";

      cbSetupBy.DataSource = CutlistData.GetAuthors().Tables[0];
      cbSetupBy.DisplayMember = "NAME";
      cbSetupBy.ValueMember = "UID";
      cbSetupBy.SelectedValue = CutlistData.GetCurrentAuthor();
    }

    private void InitTableData() {
      table = new swTableType.swTableType(DrawingPropertySet.SwApp.ActiveDoc as ModelDoc2);
    }

    private void CutlistHeaderInfo_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.CutlistHeaderLocation = Location;
      Properties.Settings.Default.CutlistHeaderSize = Size;
      Properties.Settings.Default.Save();
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      Close();
    }

    public DrawingProperties DrawingPropertySet { get; set; }
    public CutlistData CutlistData { get; set; }
    public swTableType.swTableType table { get; set; }

    private void btnCreate_Click(object sender, EventArgs e) {
      uint
      CutlistData.UpdateCutlist(cbItemNo.Text, cbDrawingReference.Text, 100, cbDescription.Text,
        cbCustomer.SelectedValue, tbL.Text, tbW.Text, tbH.Text, 0, Properties.Settings.Default.DefaultState, table.GetParts() );
    }
  }
}