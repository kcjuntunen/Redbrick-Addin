using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class EditRev : Form {
    private int nodeCount;
    private CutlistData cutlist_data;
    private SwProperty revision;
    public bool new_rev = false;

    public EditRev(ref DrawingRevs revs, int NodeCount) {
      System.Diagnostics.Debug.Print(NodeCount.ToString());
      nodeCount = NodeCount;
      _revs = revs;
      InitializeComponent();
      Init();
    }

    public EditRev(ref DrawingRevs revs, int NodeCount, CutlistData cd, SwProperty rev) {
      System.Diagnostics.Debug.Print(NodeCount.ToString());
      nodeCount = NodeCount;
      cutlist_data = cd;
      revision = rev;
      _revs = revs;
      InitializeComponent();
      Init();
    }

    private void Init() {
      CutlistData cd;
      if (cutlist_data != null) {
        cd = cutlist_data;
      } else {
        cd = new CutlistData();
      }

      cbBy.DataSource = cd.GetAuthors().Tables[0];
      cbBy.DisplayMember = "NAME";
      cbBy.ValueMember = "USERNAME";

      if (!Revs.Contains("REVISION " + (char)(nodeCount + 65))) {
        cbBy.SelectedValue = System.Environment.UserName;
        string theRev = "REVISION " + (char)(nodeCount + 65);
        Text = "Creating new " + theRev + "...";
        if (nodeCount < 1) {
          tbDesc.Text = "RELEASED";
          tbECO.Text = "NA";
        }
      } else {
        string theRev = "REVISION " + (char)(nodeCount + 65);
        DrawingRev r = Revs.GetRev(theRev);
        tbECO.Text = r.Eco.Value;
        tbDesc.Text = r.Description.Value;
        cbBy.SelectedValue = cd.GetAuthorUserName(r.List.Value);
        System.DateTime dt = new System.DateTime();
        if (DateTime.TryParse(r.Date.Value, out dt)) {
          dtpDate.Value = dt;
        }
        Text = "Editing " + theRev + "...";
      }

      for (int i = 0; i < Properties.Settings.Default.RevLimit; i++)
        cbRev.Items.Add("A" + (char)(i + 65));

      cbRev.SelectedIndex = (nodeCount);
    }

    private int GetIndex(DataTable dt, string val) {
      if (dt != null) {
        int count = 0;
        foreach (DataRow dr in dt.Rows) {
          count++;
          if (dr.ItemArray[1].ToString().Trim().ToUpper() == val.Trim().ToUpper())
            return count - 1;
        }
      }
      return -1;
    }

    private int GetIndex(DataTable dt, string val, int column) {
      if (dt != null) {
        int count = 0;
        foreach (DataRow dr in dt.Rows) {
          count++;
          if (dr.ItemArray[column].ToString().Trim().ToUpper() == val.Trim().ToUpper())
            return count - 1;
        }
      }
      return -1;
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      Close();
    }

    private void btnOK_Click(object sender, EventArgs e) {
      DrawingRev r = null;
      swCustomInfoType_e tType = swCustomInfoType_e.swCustomInfoText;
      SwProperty rev = new SwProperty("REVISION " + (char)(nodeCount + 65), tType, cbRev.Text, true);
      SwProperty eco = new SwProperty("ECO " + (nodeCount + 1).ToString(), tType, tbECO.Text, true);
      // I usually only apply this filter on insertion, but this field is only ever 
      // displayed on drawings. We only want all caps for that as well.
      SwProperty desc = new SwProperty("DESCRIPTION " + (nodeCount + 1).ToString(), tType, CutlistData.FilterString(tbDesc.Text, Properties.Settings.Default.FlameWar), true);
      cbBy.ValueMember = "INITIAL";
      
      SwProperty list;
      if (cbBy.SelectedValue != null) {
        list = new SwProperty("LIST " + (nodeCount + 1).ToString(), tType, (cbBy.SelectedValue as string).Substring(0, 2), true);
      } else {
        list = new SwProperty("LIST " + (nodeCount + 1).ToString(), tType, string.Empty, true);
      }

      SwProperty date = new SwProperty("DATE " + (nodeCount + 1).ToString(), tType, dtpDate.Value.ToShortDateString(), true);

      if (Revs.Contains("REVISION " + (char)(nodeCount + 65))) {
        r = Revs.GetRev("REVISION " + (char)(nodeCount + 65));
        r.Revision = rev;
        r.Eco = eco;
        r.Description = desc;
        r.List = list;
        r.Date = date;
      } else {
        r = new DrawingRev(rev, eco, desc, list, date);
        Revs.Add(r);
      }

      r.SwApp = Revs.SwApp;

      if (new_rev) {
        try {
          AddECRItem(r);
        } catch (Exception ex) {
          r.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
        }
      }

      this.Close();
    }


    public delegate void AddedECREventHandler(object sender, EventArgs e);
    public event EventHandler Added;
    public event EventHandler AddedLvl;

    protected virtual void OnAdded(EventArgs e) {
      if (Added != null)
        Added(this, e);
    }

    protected virtual void OnAddedLvl(EventArgs e) {
      if (AddedLvl != null)
        AddedLvl(this, e);
    }

    private void AddECRItem(DrawingRev r) {
      ModelDoc2 md = (ModelDoc2)r.SwApp.ActiveDoc;
      string partpath = md.GetPathName();
      string partfilename = System.IO.Path.GetFileNameWithoutExtension(partpath);
      if (md.GetPathName() == string.Empty) {
        md.Extension.RunCommand((int)swCommands_e.swCommands_SaveAs, md.GetTitle());
        if (md.GetPathName() == string.Empty) {
          throw new Exception("Unsaved drawings cannot be added to an ECR.");
        }
        partpath = md.GetPathName();
        partfilename = System.IO.Path.GetFileNameWithoutExtension(partpath);
      }
      string[] partarr = partfilename.Split(new string[] { "REV" }, StringSplitOptions.RemoveEmptyEntries);
      string partno = partarr[0].Trim();
      string rv = revision.Value;
      if (partarr.Length > 1) {
        rv = partarr[1].Trim();
      }
      string question = string.Format(Properties.Resources.InsertIntoEcrItems,
        System.IO.Path.GetFileName(partno),
        r.Eco.Value);

      swMessageBoxResult_e mbr = swMessageBoxResult_e.swMbHitNo;

      int en = 0;
      if (int.TryParse(r.Eco.Value, out en) && 
        !cutlist_data.ECRIsBogus(r.Eco.Value) &&
        !cutlist_data.ECRItemExists(en, partno, revision.Value)) {

        mbr = (swMessageBoxResult_e)r.SwApp.SendMsgToUser2(question,
            (int)swMessageBoxIcon_e.swMbQuestion,
            (int)swMessageBoxBtn_e.swMbYesNo);
      }

      if (mbr == swMessageBoxResult_e.swMbHitYes) {
        OnAdded(EventArgs.Empty);
        ArchivePDF.csproj.ArchivePDFWrapper apw = new ArchivePDF.csproj.ArchivePDFWrapper(r.SwApp);
        apw.Archive();
        int parttype = 7;
        // M2M
        M2MData m = new M2MData();
        if (m.GetPartCount(partno, revision.Value) > 0) {
          parttype = m.GetPartType(partno, revision.Value);
        } else {
          // Cutlist
          int prtid = 0;
          if (int.TryParse(
            cutlist_data.GetCutlistData(partno, revision.Value).Tables[0].Columns["CLID"].ToString(), out prtid)) {
            if (prtid > 0) {
              parttype = 5;
            }
          } else {
            if (cutlist_data.PartUsed(partno)) {
              parttype = 6;
            }
          }
        }
        cutlist_data.InsertECRItem(en,
          partno,
          rv,
          parttype,
          r.Revision.Value,
          partpath,
          r.Eco.Value);
        }

      OnAddedLvl(new EventArgs());
      }

    private void EditRev_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.EditRevLocation = Location;
      Properties.Settings.Default.Save();
    }

    private DrawingRevs _revs;

    public DrawingRevs Revs {
      get { return _revs; }
      set { _revs = value; }
    }
  }
}