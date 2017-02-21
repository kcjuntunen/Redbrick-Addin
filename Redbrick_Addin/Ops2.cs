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
    private ENGINEERINGDataSetTableAdapters.OpTreeTableAdapter opTreeTableAdapter;

    public Ops2() {
      InitializeComponent();
    }

    private void populate(string partnum) {
      if (partnum == string.Empty) {
        return;
      }
      ENGINEERINGDataSet.OpTreeDataTable dt = new ENGINEERINGDataSet.OpTreeDataTable();
      opTreeTableAdapter.FillByPartID(dt, 129);
      foreach (DataRow item in dt) {
        string topNodeString = string.Format(@"Operation {1} ({0})", item[@"TYPEDESC"], item[@"POPORDER"]);
        TreeNode subNode1 = new TreeNode((string)item[@"OPDESCR"]);
        TreeNode subNode2 = new TreeNode((bool)item[@"OPPROG"] ? @"Program required" : @"No program needed");
        TreeNode topNode = new TreeNode(topNodeString, new TreeNode[] { subNode1, subNode2 });
        treeView1.Nodes.Add(topNode);
      }
    }

    private void button1_Click(object sender, EventArgs e) {
      EditOp eo = new EditOp();
      eo.ShowDialog(this);
    }
  }
}
