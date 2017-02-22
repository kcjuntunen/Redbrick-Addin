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
    private SwProperties propertySet = default(SwProperties);
    private int OpType = 1;

    private ENGINEERINGDataSetTableAdapters.OpTreeTableAdapter opTreeTableAdapter = 
      new ENGINEERINGDataSetTableAdapters.OpTreeTableAdapter();

    private ENGINEERINGDataSet.OpTreeDataTable otdt = 
      new ENGINEERINGDataSet.OpTreeDataTable();

    public Ops2(ref SwProperties p) {
      propertySet = p;
      partNum = p.PartName;
      InitializeComponent();
      populate(partNum);
    }

    public void Update(ref SwProperties p) {
      propertySet = p;
      partNum = p.PartName;
      OpType = p.cutlistData.OpType;
      treeView1.Nodes.Clear();
      populate(partNum);
    }

    private void populate(string partnum) {
      if (partnum == string.Empty) {
        return;
      }
      opTreeTableAdapter.FillByPartID(otdt, propertySet.cutlistData.GetPartID(partnum));
      foreach (DataRow item in otdt) {
        add_node(item);
      }
    }

    private int add_node(DataRow row) {
      var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
      string topNodeString = string.Format(@"{0} Operation {1}",
        t.ToTitleCase(row[@"TYPEDESC"].ToString().ToLower()), row[@"POPORDER"]);
      double setupTime = 0;
      double runTime = 0;
      TreeNode subNode1 = new TreeNode(t.ToTitleCase(row[@"OPDESCR"].ToString().ToLower()));
      TreeNode subNode2 = new TreeNode((bool)row[@"OPPROG"] ? @"Program required" : @"No program required");
      setupTime = (double)row[@"POPSETUP"] == 0.0 ? (double)row[@"OPSETUP"] : (double)row[@"POPSETUP"];
      runTime = (double)row[@"POPRUN"] == 0.0 ? (double)row[@"OPRUN"] : (double)row[@"POPRUN"];
      TreeNode subNode3 = new TreeNode(string.Format(@"Setup time: {0}", TimeSpan.FromHours(setupTime).ToString()));
      TreeNode subNode4 = new TreeNode(string.Format(@"Run time: {0}", TimeSpan.FromMinutes(runTime).ToString()));
      TreeNode topNode = new TreeNode(topNodeString, new TreeNode[] { subNode1, subNode2, subNode3, subNode4 });
      treeView1.Nodes.Add(topNode);
      return topNode.Index;
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

    public ENGINEERINGDataSet.OpTreeDataTable OpData {
      get { return otdt; }
      set { } }
  }
}
