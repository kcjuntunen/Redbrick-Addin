using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class Ops2 : UserControl {
    private string partNum = string.Empty;

    private ENGINEERINGDataSetTableAdapters.OpTreeTableAdapter opTreeTableAdapter = 
      new ENGINEERINGDataSetTableAdapters.OpTreeTableAdapter();

    private ENGINEERINGDataSet.OpTreeDataTable otdt = 
      new ENGINEERINGDataSet.OpTreeDataTable();

    public Ops2(string partnum) {
      partNum = partnum;
      InitializeComponent();
      populate(partNum);
    }

    private void populate(string partnum) {
      if (partnum == string.Empty) {
        return;
      }
      CutlistData cd = new CutlistData();
      var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
      opTreeTableAdapter.FillByPartID(otdt, cd.GetPartID(partnum));
      foreach (DataRow item in otdt) {
        string topNodeString = string.Format(@"{0} Operation {1}", 
          t.ToTitleCase(item[@"TYPEDESC"].ToString().ToLower()), item[@"POPORDER"]);
        double setupTime = 0;
        double runTime = 0;
        TreeNode subNode1 = new TreeNode(t.ToTitleCase(item[@"OPDESCR"].ToString().ToLower()));
        TreeNode subNode2 = new TreeNode((bool)item[@"OPPROG"] ? @"Program required" : @"No program required");
        setupTime = (double)item[@"POPSETUP"] == 0.0 ? (double)item[@"OPSETUP"] : (double)item[@"POPSETUP"];
        runTime = (double)item[@"POPRUN"] == 0.0 ? (double)item[@"OPRUN"] : (double)item[@"POPRUN"];
        TreeNode subNode3 = new TreeNode(string.Format(@"Setup time: {0}", TimeSpan.FromHours(setupTime).ToString()));
        TreeNode subNode4 = new TreeNode(string.Format(@"Run time: {0}", TimeSpan.FromMinutes(runTime).ToString()));
        TreeNode topNode = new TreeNode(topNodeString, new TreeNode[] { subNode1, subNode2, subNode3, subNode4 });
        treeView1.Nodes.Add(topNode);
      }
    }

    private void writeToDB() {
      foreach (TreeNode node in treeView1.Nodes) {

      }
    }

    private int find_parent_index(TreeNode tn) {
      TreeNode node = tn;
      int res = 0;
      while (node.Parent != null) {
        node = tn.Parent;
        res = node.Index;
      }
      return res;
    }

    private void button1_Click(object sender, EventArgs e) {

    }

    private void button2_Click(object sender, EventArgs e) {
      int idx = find_parent_index(treeView1.SelectedNode);
      ENGINEERINGDataSet.OpTreeRow otr = (ENGINEERINGDataSet.OpTreeRow)otdt.Rows[idx];
      EditOp eo = new EditOp(partNum, otr, (int)otr[@"OPTYPE"]);
      eo.ShowDialog(this);
    }
  }
}
