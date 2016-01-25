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
    private bool clicked = false;

    public enum CutlistFunction {
      AddToExistingAlreadySelected,
      AddToExistingNotSelected,
      CreateNew
    }

    public CutlistHeaderInfo(Part p, CutlistData cd, CutlistFunction c) {
      InitializeComponent();
      Location = Properties.Settings.Default.CutlistHeaderLocation;
      Size = Properties.Settings.Default.CutlistHeaderSize;

      switch (c) {
        case CutlistFunction.AddToExistingAlreadySelected:
          InitControlsWithPart(p, cd);
          break;
        case CutlistFunction.AddToExistingNotSelected:
          InitControlsWithPart(p, cd);
          break;
        case CutlistFunction.CreateNew:
          InitControlsWithNewPart(p, cd);
          break;
        default:
          break;
      }
    }
    
    public CutlistHeaderInfo(Part p, CutlistData cd) { // New Cutlist
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
      Text = "Add/Update Cutlist for " + title[0] + "...";
      cbDrawingReference.Text = title[0];
      //cbItemNo.DataSource = CutlistData.GetCutlists().Tables[0];
      cbItemNo.DisplayMember = "PARTNUM";
      cbItemNo.ValueMember = "CLID";
      cbDrawingReference.Text = DrawingPropertySet.GetProperty("PartNo").ResValue;
      cbItemNo.Text = cbDrawingReference.Text;

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

    private void InitControlsWithNewPart(Part p, CutlistData cd) {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;
      btnCreate.Text = "Create Cutlist";
      part = p;
      CutlistData = cd;
      cbRev.Enabled = true;

      cbCustomer.DataSource = CutlistData.Customers.Tables[0];
      cbCustomer.DisplayMember = "CUSTOMER";
      cbCustomer.ValueMember = "CUSTID";

      cbSetupBy.DataSource = CutlistData.GetAuthors().Tables[0];
      cbSetupBy.DisplayMember = "NAME";
      cbSetupBy.ValueMember = "UID";
      cbSetupBy.SelectedValue = CutlistData.GetCurrentAuthor();
    }

    private void InitControlsWithPart(Part p, CutlistData cd, string itemNo, string Rev) {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;
      btnCreate.Text = "Add";
      part = p;
      CutlistData = cd;

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

    private void xInitControlsWithPart(Part p, CutlistData cd, string itemNo, string Rev) {
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;
      btnCreate.Text = "Add";
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
      Text = "Add to cutlist...";
      for (int i = 100; i < 100 + Properties.Settings.Default.RevNoLimit; i++)
        cbRev.Items.Add(i.ToString());
      cbRev.SelectedIndex = 0;
      btnCreate.Text = "Add";
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

      // Not classy, but it makes the boxes update the first time around.
      clicked = true;
      cbItemNo.SelectedIndex = cbItemNo.FindString(
        cd.GetCutlistData(Properties.Settings.Default.CurrentCutlist).Tables[0].Rows[0][(int)CutlistData.CutlistDataFields.PARTNUM].ToString());
    }

    private void InitTableData() {
      try {
        table = new swTableType.swTableType((DrawingPropertySet.SwApp.ActiveDoc as ModelDoc2),
          Properties.Settings.Default.MasterTableHash);
      } catch (Exception e) {
        RedbrickErr.ErrMsg em = new RedbrickErr.ErrMsg(e);
        em.ShowDialog();
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
      string[] clData = cbItemNo.Text.Split(new string[] { "REV" }, StringSplitOptions.None);
      if (clData.Length > 1) {
        if (CutlistData.GetCutlistID(clData[0].Trim(), clData[1].Trim()) > 0) {
          UpdateCutlist(clData[0].Trim(), clData[1].Trim());
        }
      } else {
        UpdateCutlist(cbItemNo.Text, cbRev.Text);
      }

      Close();
    }

    private void UpdateExistingCutlist(string itemNo, string rev) {

    }

    private void UpdateCutlist(string itemNo, string rev) {
      ushort itpState = 0;
      ushort itpCust = 0;
      double dtpLength = 0.0f;
      double dtpWidth = 0.0f;
      double dtpHeight = 0.0f;

      try {
        itpCust = ushort.Parse(cbCustomer.SelectedValue.ToString());
        itpState = ushort.Parse(Status.ToString());
      } catch (Exception) {

      }

      dtpLength = ParseFloat(tbL.Text);
      dtpWidth = ParseFloat(tbW.Text);
      dtpHeight = ParseFloat(tbH.Text);

      if (table != null) {
        CutlistData.UpdateCutlist(itemNo, cbDrawingReference.Text, rev, cbDescription.Text,
          itpCust, dtpLength, dtpWidth, dtpHeight, itpState, table.GetParts());
      } else if (part != null) {
        Dictionary<string, Part> d = new Dictionary<string, Part>();
        d.Add(part.PartNumber, part);
        CutlistData.UpdateCutlist(itemNo, cbDrawingReference.Text, rev, cbDescription.Text,
          itpCust, dtpLength, dtpWidth, dtpHeight, itpState, d);
      } else {
        DrawingPropertySet.SwApp.SendMsgToUser2("Failed to read table or part.",
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      }
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
    public int Status { get; set; }

    private void cbItemNo_SelectedIndexChanged(object sender, EventArgs e) {
      if (clicked) {
        if (cbItemNo.SelectedValue != null) {
          dpDate.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.CDATE].ToString();
          cbDescription.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.DESCR].ToString();
          tbL.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.LENGTH].ToString();
          tbW.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.WIDTH].ToString();
          tbH.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.HEIGHT].ToString();

          int status = 1;
          int custid = 1;

          if (int.TryParse((cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.CUSTID].ToString(), out custid))
            cbCustomer.SelectedValue = custid;

          if (int.TryParse((cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.STATEID].ToString(), out status))
            Status = status;

          string[] itnre = { string.Empty };

          itnre = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.PARTNUM].ToString()
            .Split(new string[] { "REV" }, StringSplitOptions.None);

          if (itnre.Length > 1)
            cbRev.Text = itnre[1];

          cbDrawingReference.Text = (cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.DRAWING].ToString();
          cbSetupBy.SelectedValue = int.Parse((cbItemNo.SelectedItem as DataRowView).Row.ItemArray[(int)CutlistData.CutlistDataFieldsJoined.SETUP_BY].ToString());
        }
        clicked = false;
      }
    }

    private void cbDescription_TextChanged(object sender, EventArgs e) {
      CutlistData.FilterTextForControl(cbDescription);
    }

    private void cbItemNo_MouseClick(object sender, MouseEventArgs e) {
      clicked = true;
    }
  }
}