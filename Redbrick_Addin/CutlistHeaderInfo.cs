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
    public CutlistHeaderInfo(Part p, CutlistData cd, string itemNo, string rev) {
      InitializeComponent();
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      Size = Properties.Settings.Default.CutlistHeaderSize;
      InitControlsWithPart(p, cd, itemNo, rev);
    }

    public CutlistHeaderInfo(Part p, CutlistData cd) {
      InitializeComponent();
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      Size = Properties.Settings.Default.CutlistHeaderSize;
      InitControlsWithPart(p, cd);
    }

    public CutlistHeaderInfo(DrawingProperties dp) {
      InitializeComponent();
      DrawingPropertySet = dp;
      CutlistData = dp.CutlistData;
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      Size = Properties.Settings.Default.CutlistHeaderSize;
      InitControlsWithDrawing();
    }

    private void InitControlsWithDrawing() {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;

      string[] title = (DrawingPropertySet.SwApp.ActiveDoc as ModelDoc2).GetTitle().Split(' ');
      cbDrawingReference.Text = title[0];
      cbItemNo.Text = title[0];
      cbCustomer.DataSource = CutlistData.Customers.Tables[0];
      cbCustomer.DisplayMember = "CUSTOMER";
      cbCustomer.ValueMember = "CUSTID";
      cbCustomer.SelectedIndex = cbCustomer.FindString(DrawingPropertySet.GetProperty("CUSTOMER").Value.Split('-')[0].Trim());

      cbSetupBy.DataSource = CutlistData.GetAuthors().Tables[0];
      cbSetupBy.DisplayMember = "NAME";
      cbSetupBy.ValueMember = "UID";
      cbSetupBy.SelectedValue = CutlistData.GetCurrentAuthor();

      InitTableData();
    }

    private void InitControlsWithPart(Part p, CutlistData cd, string itemNo, string Rev) {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;
      part = p;
      CutlistData = cd;

      cbCustomer.DataSource = CutlistData.Customers.Tables[0];
      cbCustomer.DisplayMember = "CUSTOMER";
      cbCustomer.ValueMember = "CUSTID";

      cbSetupBy.DataSource = CutlistData.GetAuthors().Tables[0];
      cbSetupBy.DisplayMember = "NAME";
      cbSetupBy.ValueMember = "UID";
      cbSetupBy.SelectedValue = CutlistData.GetCurrentAuthor();

      DataSet dr = CutlistData.GetCutlistData(itemNo, Rev);
      cbCustomer.SelectedValue = int.Parse(dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.CUSTID].ToString());
      cbItemNo.Text = dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.PARTNUM].ToString();
      cbDescription.Text = dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.DESCR].ToString();
      tbL.Text = dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.LENGTH].ToString();
      tbW.Text = dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.WIDTH].ToString();
      tbH.Text = dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.HEIGHT].ToString();
      cbDrawingReference.Text = dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.DRAWING].ToString();
      cbSetupBy.SelectedValue = int.Parse(dr.Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.SETUP_BY].ToString());
    }

    private void InitControlsWithPart(Part p, CutlistData cd) {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;
      part = p;
      CutlistData = cd;
      cbRev.Enabled = false;
      cbItemNo.DataSource = CutlistData.GetCutlists().Tables[0];
      cbItemNo.DisplayMember = "PARTNUM";
      cbItemNo.ValueMember = "CLID";

      cbCustomer.DataSource = CutlistData.Customers.Tables[0];
      cbCustomer.DisplayMember = "CUSTOMER";
      cbCustomer.ValueMember = "CUSTID";

      cbSetupBy.DataSource = CutlistData.GetAuthors().Tables[0];
      cbSetupBy.DisplayMember = "NAME";
      cbSetupBy.ValueMember = "UID";
      cbSetupBy.SelectedValue = CutlistData.GetCurrentAuthor();
    }

    private void InitTableData() {
      try {
        table = new swTableType.swTableType((DrawingPropertySet.SwApp.ActiveDoc as ModelDoc2),
          Properties.Settings.Default.MasterTableHash);
      } catch (Exception e) {
        
        throw;
      }
    }

    private void CutlistHeaderInfo_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.CutlistHeaderLocation = Location;
      Properties.Settings.Default.CutlistHeaderSize = Size;
      Properties.Settings.Default.Save();
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      Close();
    }

    private void btnCreate_Click(object sender, EventArgs e) {

      ushort itpRev = 0;
      ushort itpState = 0;
      ushort itpCust = 0;
      double dtpLength = 0.0f;
      double dtpWidth = 0.0f;
      double dtpHeight = 0.0f;

      try {
        itpRev = ushort.Parse(cbRev.Text);
        itpCust = ushort.Parse(cbCustomer.SelectedValue.ToString());
        itpState = ushort.Parse(Properties.Settings.Default.DefaultState.ToString());
      } catch (Exception) {

      }

      dtpLength = ParseFloat(tbL.Text);
      dtpWidth = ParseFloat(tbW.Text);
      dtpHeight = ParseFloat(tbH.Text);

      if (table != null) {
        CutlistData.UpdateCutlist(cbItemNo.Text, cbDrawingReference.Text, itpRev, cbDescription.Text,
          itpCust, dtpLength, dtpWidth, dtpHeight, itpState, table.GetParts());
      } else {
        Dictionary<string, Part> d = new Dictionary<string, Part>();
        d.Add(part.PartNumber, part);
        CutlistData.UpdateCutlist(cbItemNo.Text, cbDrawingReference.Text, itpRev, cbDescription.Text,
          itpCust, dtpLength, dtpWidth, dtpHeight, itpState, d);
      }
      Close();
    }

    private double ParseFloat(string number) {
      try {
        return double.Parse(number);
      } catch (Exception) {
        return 0.0f;
      }
    }

    public DrawingProperties DrawingPropertySet { get; set; }
    public Part part { get; set; }
    public CutlistData CutlistData { get; set; }
    public swTableType.swTableType table { get; set; }

    private void cbItemNo_SelectedIndexChanged(object sender, EventArgs e) {
      cbCustomer.SelectedValue = int.Parse((cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.CUSTID].ToString());

      cbDescription.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.DESCR].ToString();
      tbL.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.LENGTH].ToString();
      tbW.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.WIDTH].ToString();
      tbH.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.HEIGHT].ToString();
      cbDrawingReference.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.DRAWING].ToString();
      cbSetupBy.SelectedValue = int.Parse((cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.SETUP_BY].ToString());
    }
  }
}