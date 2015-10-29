using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin {
  public partial class tvRevs : UserControl {
    private CutlistData cd;
    public DrawingRevs revSet;
    private DrawingProperties propertySet;

    public tvRevs(ref DrawingProperties prop, ref DrawingRevs dr) {
      propertySet = prop;
      cd = prop.CutlistData;
      revSet = dr;

      InitializeComponent();
      Init();
    }

    private void Init() {
      this.tvRevisions.Nodes.Clear();
      foreach (DrawingRev r in revSet) {
        TreeNode tnList = new TreeNode(r.List.Value);
        TreeNode tnDate = new TreeNode(r.Date.Value);
        TreeNode tnECO = new TreeNode(r.Eco.Value);
        TreeNode tnC = new TreeNode(r.Revision.Value);
        int test = 0;
        eco e = new eco();
        e = cd.GetECOData(r.Eco.Value);

        if (int.TryParse(r.Eco.Value, out test) && test > Properties.Settings.Default.LastLegacyECR) {
          TreeNode tnD = new TreeNode("Error Description: " + e.ErrDescription, 0, 0);
          TreeNode tnRB = new TreeNode("Requested by: " + e.RequestedBy, 0, 0);
          TreeNode tnR = new TreeNode("Revision Description:" + e.Revision, 0, 0);
          TreeNode tnS = new TreeNode("Status: " + e.Status, 0, 0);
          tnC = new TreeNode("Changes: " + e.Changes);
          TreeNode[] ts = { tnC, tnD, tnRB, tnR, tnS };
          TreeNode tnDesc = new TreeNode(r.Description.Value);
          tnECO = new TreeNode(r.Eco.Value, ts);
          TreeNode[] tt = { tnECO, tnDesc, tnList, tnDate };
          TreeNode tn = new TreeNode(r.Revision.Value, tt);
          this.tvRevisions.Nodes.Add(tn);
        } else if (r.Eco.Value.Contains("NA") || r.Eco.Value.Trim().Equals(string.Empty)) {
          tnECO = new TreeNode(r.Eco.Value);
          TreeNode tnDesc = new TreeNode(r.Description.Value);
          TreeNode[] tt = { tnECO, tnDesc, tnList, tnDate };

          TreeNode tn = new TreeNode(r.Revision.Value, tt);
          this.tvRevisions.Nodes.Add(tn);
        } else {
          if ((e.Changes != null) && e.Changes.Contains("\n")) {
            List<TreeNode> nodes = new List<TreeNode>();
            string[] changeNodes = e.Changes.Split('\n');
            foreach (string s in changeNodes) {
              nodes.Add(new TreeNode(s));
            }
            tnC = new TreeNode("Changes ", nodes.ToArray());
          } else {
            tnC = new TreeNode("Changes: " + e.Changes);

            TreeNode tnRB = new TreeNode("Finished by: " + e.RequestedBy, 0, 0);
            string[] affectedParts = e.Revision.Split(',', '&');
            TreeNode tnT = new TreeNode("");

            List<TreeNode> nodes = new List<TreeNode>();
            foreach (string item in affectedParts) {
              nodes.Add(new TreeNode(item.Trim()));
            }
            TreeNode tnR = new TreeNode("Affected Parts:", nodes.ToArray());

            TreeNode tnS = new TreeNode("Status: " + e.Status, 0, 0);
            TreeNode[] ts = { tnC, tnRB, tnR, tnS };
            tnECO = new TreeNode(r.Eco.Value, ts);

            TreeNode tnDesc = new TreeNode(r.Description.Value);
            TreeNode[] tt = { tnECO, tnDesc, tnList, tnDate };

            TreeNode tn = new TreeNode(r.Revision.Value, tt);
            this.tvRevisions.Nodes.Add(tn);
          }
        }
      }
    }

    private void btnNewRev_Click(object sender, EventArgs e) {
      EditRev er = new EditRev(ref this.revSet, this.tvRevisions.Nodes.Count);
      er.ShowDialog();
      this.Init();
    }

    private void btnEditRev_Click(object sender, EventArgs e) {
      TreeNode node = this.tvRevisions.SelectedNode;
      if (node != null) {
        while (node.Parent != null) {
          node = node.Parent;
        }
        EditRev er = new EditRev(ref this.revSet, node.Index);
        er.ShowDialog();
        this.Init();
      } else {
        EditRev er = new EditRev(ref this.revSet, 0);
        er.ShowDialog();
        this.Init();
      }
    }

    private void btnDelRev_Click(object sender, EventArgs e) {
      TreeNode node = this.tvRevisions.SelectedNode;
      if (node != null) {
        while (node.Parent != null) {
          node = node.Parent;
        }

        DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure?", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        if (dr == DialogResult.Yes) {
          string revToDel = string.Empty;
          while (this.tvRevisions.Nodes.Count > node.Index) {
            revToDel = "REVISION " + this.tvRevisions.Nodes[this.tvRevisions.Nodes.Count - 1].Text.Substring(1, 1);
            this.revSet.Remove(revToDel);
            this.tvRevisions.Nodes.Remove(this.tvRevisions.Nodes[this.tvRevisions.Nodes.Count - 1]);
          }
        }
      } else {
        System.Windows.Forms.MessageBox.Show("You must make a selection.");
      }
    }
  }
}
