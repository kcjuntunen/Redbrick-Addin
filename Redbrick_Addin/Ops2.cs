using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Web;
using System.Collections.Specialized;

namespace Redbrick_Addin {
  public partial class Ops2 : UserControl {
    private string partNum = string.Empty;
    private SwProperties propertySet = default(SwProperties);
    private int OpType = 1;
    private int count = 0;
    private bool has_cnc_op = false;
    private bool has_eb_op = false;
    private List<string> op_names = new List<string>();

    private ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cUT_PART_OPSTableAdapter =
      new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

    private ENGINEERINGDataSet.CUT_PART_OPSDataTable cUT_PART_OPSDataTable =
      new ENGINEERINGDataSet.CUT_PART_OPSDataTable();

    private ENGINEERINGDataSetTableAdapters.OpDataTableAdapter opDataTableAdapter =
      new ENGINEERINGDataSetTableAdapters.OpDataTableAdapter();

    private ENGINEERINGDataSet.OpDataDataTable otdt =
      new ENGINEERINGDataSet.OpDataDataTable();

    public Ops2(ref SwProperties p) {
      propertySet = p;
      partNum = p.PartName;
      InitializeComponent();
      populate(partNum);
      treeView1.ItemDrag += treeView1_ItemDrag;
      treeView1.DragEnter += treeView1_DragEnter;
      treeView1.DragDrop += treeView1_DragDrop;
    }

    void treeView1_DragDrop(object sender, DragEventArgs e) {
      TreeNodeWithData newNode;
      if (e.Data.GetDataPresent(@"Redbrick_Addin.TreeNodeWithData", false)) {
        Point pt = (sender as TreeView).PointToClient(new Point(e.X, e.Y));
        TreeNodeWithData destinationNode = find_parent_node((sender as TreeView).GetNodeAt(pt));
        newNode = find_parent_node((TreeNodeWithData)e.Data.GetData(@"Redbrick_Addin.TreeNodeWithData"));
        newNode.OpData = OpData.Rows[newNode.Index];
        newNode.PropertySet = propertySet;
        TreeNodeWithData nodeClone = (TreeNodeWithData)newNode.Clone();
        nodeClone.OpData = newNode.OpData;
        nodeClone.PropertySet = newNode.PropertySet;
        treeView1.Nodes.Insert(destinationNode.Index, nodeClone);
        newNode.Remove();
        update_from_nodes();
      }
    }

    public static string DataRowToUri(DataRow dr) {
      int cnt = 0;
      int colcnt = dr.Table.Columns.Count; 
      string output = string.Empty;
      foreach (var item in dr.Table.Columns) {
        output += string.Format(@"{0}={1}", item.ToString(), dr[item.ToString()].ToString());
        if (cnt++ < colcnt) {
          output += "&";
        }
      }
      return output;
    }

    private List<DataRow> WriteSWProps(List<TreeNodeWithData> nodes) {
      List<DataRow> newRows = new List<DataRow>();
      for (int i = 0; i < nodes.Count; i++) {
        TreeNodeWithData tn = (TreeNodeWithData)treeView1.Nodes[i];
        tn.OpData[@"POPORDER"] = tn.Index + 1;
        SwProperty p = tn.PropertySet.GetProperty(string.Format(@"OP{0}ID", i + 1));
        p.ID = tn.OpData[@"POPOP"].ToString();
        p.Value = tn.OpData[@"POPOP"].ToString();
        p.ResValue = tn.OpData[@"POPOP"].ToString();
        p.Descr = tn.OpData[@"OPNAME"].ToString();
        p.Old = false;
        p.Write2(propertySet.modeldoc);
        if (Properties.Settings.Default.Testing) {
          SwProperty old_p = tn.PropertySet.GetProperty(string.Format(@"OP{0}", i + 1));
          old_p.ID = tn.OpData[@"POPOP"].ToString();
          old_p.Value = tn.OpData[@"OPNAME"].ToString();
          old_p.ResValue = tn.OpData[@"OPNAME"].ToString();
          old_p.Descr = tn.OpData[@"OPNAME"].ToString();
          old_p.Old = true;
          old_p.Write2(propertySet.modeldoc);
        }
        newRows.Add(tn.OpData);
      }
      return newRows;
    }

    public void update_from_nodes() {
      DataRow[] rows = OpData.Select();
      List<TreeNodeWithData> nodes = new List<TreeNodeWithData>();
      string dictOps = string.Empty;
      foreach (TreeNodeWithData item in treeView1.Nodes) {
        nodes.Add(item);
      }

      for (int i = 0; i < nodes.Count; i++) {
        dictOps += DataRowToUri(nodes[i].OpData);
        if (i < nodes.Count) {
          dictOps += @"|";
        }
      }

      SwProperty p = propertySet.GetProperty(@"DictOps");
      p.Value = dictOps;
      p.ResValue = dictOps;
      p.Descr = dictOps;
      p.Write2(propertySet.modeldoc);

      if (propertySet.cutlistData.GetPartID(partNum) > 0) {
        opDataTableAdapter.Update(WriteSWProps(nodes).ToArray());
      }
      populate(partNum);
    }

    void treeView1_DragEnter(object sender, DragEventArgs e) {
      e.Effect = DragDropEffects.Move;
    }

    void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
      DoDragDrop(e.Item, DragDropEffects.Move);
    }

    public void Update(ref SwProperties p) {
      HasCNCOp = false;
      HasEBOp = false;
      OpNameList.Clear();
      propertySet = p;
      partNum = p.PartName;
      OpType = p.cutlistData.OpType;
      LinkControls();
      populate(partNum);
    }

    private void LinkControls() {
      int cnt = 1;
      int oldcnt = 1;

      for (int i = 1; i < Count + 1; i++) {
        propertySet.GetProperty(string.Format(@"OP{0}ID", i)).Ctl = this;
      }

      foreach (SwProperty p in propertySet) {
        if (p.Name.StartsWith(@"OP") && !p.Old) {
          if (cnt > Count) {
            //propertySet.Remove(p);
            //p.Del(propertySet.modeldoc);
          }
          cnt++;
        }

        if (p.Name.StartsWith(@"OP") && p.Old) {
          if (oldcnt > Count) {
            //propertySet.Remove(p);
            //p.Del(propertySet.modeldoc);
          }
          oldcnt++;
        }

      }
    }

    private void populate(string partnum) {
      treeView1.Nodes.Clear();
      if (partnum == string.Empty) {
        return;
      }
      try {
        opDataTableAdapter.FillByPartNum(otdt, partnum);
        cUT_PART_OPSTableAdapter.FillByPartID(cUT_PART_OPSDataTable, (int)otdt.Rows[0][@"POPPART"]);
        for (int i = 0; i < otdt.Count; i++) {
          add_node(i);
        }
      } catch (Exception) {
        add_nodes_from_SW();
      }
    }

        //    foreach (string key in nvc) {
        //  string topNodeString = string.Format(@"Step {0}: {1} Operation", ++cnt,
        //    t.ToTitleCase(propertySet.cutlistData.GetOpByID(p.ID).ToString().ToLower()));
        //  ENGINEERINGDataSet.OpDataRow dr = od.NewOpDataRow();
        //  ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter fco =
        //    new ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter();
        //  ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow odr =
        //    (ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow)fco.GetDataByTypeOp(propertySet.cutlistData.OpType, int.Parse(p.ID)).Rows[cnt];
        //  dr[@"POPPART"] = (int)propertySet.cutlistData.GetPartID(propertySet.PartName);
        //  dr[@"PARTNUM"] = (string)propertySet.PartName;
        //  dr[@"POPORDER"] = cnt;
        //  dr[@"POPOP"] = int.Parse(p.ID);
        //  dr[@"POPSETUP"] = 0.0F;
        //  dr[@"POPRUN"] = 0.0F;
        //  dr[@"OTTYPE"] = propertySet.cutlistData.OpType;
        //  dr[@"TYPEDESC"] = odr[@"TYPEDESC"];
        //  dr[@"OPSETUP"] = odr[@"OPSETUP"];
        //  dr[@"OPRUN"] = odr[@"OPRUN"];
        //  dr[@"OPPROG"] = odr[@"OPPROG"];
        //  dr[@"OPDESCR"] = odr[@"FRIENDLYDESCR"].ToString().Split('-')[1].Trim();
        //  dr[@"OPNAME"] = propertySet.cutlistData.GetOpByID(p.ID);
        //  TreeNodeWithData tn = new TreeNodeWithData(dr, propertySet, topNodeString, new TreeNode[] { });
        //  treeView1.Nodes.Add(tn);
        //  if ((int)dr[@"POPPART"] > 0) {
        //    opDataTableAdapter.Update(dr);
        //  }
        //}

    private void add_nodes_from_SW() {
      int cnt = 0;
      var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
      ENGINEERINGDataSet.OpDataDataTable od = new ENGINEERINGDataSet.OpDataDataTable();
      if (propertySet.Contains(@"DictOps")) {
        SwProperty p = propertySet.GetProperty(@"DictOps");
        NameValueCollection nvc = HttpUtility.ParseQueryString(p.Value);
        string topNodeString = string.Format(@"Step {0}: {1} Operation", ++cnt,
          t.ToTitleCase(propertySet.cutlistData.GetOpByID(p.ID).ToString().ToLower()));
        ENGINEERINGDataSet.OpDataRow dr = od.NewOpDataRow();
        ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter fco =
          new ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter();
        //ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow odr =
        //  (ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow)fco.GetDataByTypeOp(propertySet.cutlistData.OpType, int.Parse(p.ID)).Rows[0];

        foreach (string k in nvc) {
          if (k != null) {
            dr[k] = int.Parse(nvc[k]);
          }
        }

        TreeNodeWithData tn = new TreeNodeWithData(dr, propertySet, topNodeString, new TreeNode[] { });
        treeView1.Nodes.Add(tn);
        try {
          if ((int)dr[@"POPPART"] > 0) {
            opDataTableAdapter.Update(dr);
          }
        } catch (Exception) {
          //
        }
      } else {
        foreach (SwProperty p in propertySet) {
          if (p.Name.StartsWith(@"OP") && !p.Old) {
            if (int.Parse(p.ID) > 0) {
              string topNodeString = string.Format(@"Step {0}: {1} Operation", ++cnt,
                t.ToTitleCase(propertySet.cutlistData.GetOpByID(p.ID).ToString().ToLower()));
              ENGINEERINGDataSet.OpDataRow dr = od.NewOpDataRow();
              ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter fco =
                new ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter();
              ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow odr =
                (ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow)fco.GetDataByTypeOp(propertySet.cutlistData.OpType, int.Parse(p.ID)).Rows[0];
              dr[@"POPPART"] = (int)propertySet.cutlistData.GetPartID(propertySet.PartName);
              dr[@"PARTNUM"] = (string)propertySet.PartName;
              dr[@"POPORDER"] = cnt;
              dr[@"POPOP"] = int.Parse(p.ID);
              dr[@"POPSETUP"] = 0.0F;
              dr[@"POPRUN"] = 0.0F;
              dr[@"OTTYPE"] = propertySet.cutlistData.OpType;
              dr[@"TYPEDESC"] = odr[@"TYPEDESC"];
              dr[@"OPSETUP"] = odr[@"OPSETUP"];
              dr[@"OPRUN"] = odr[@"OPRUN"];
              dr[@"OPPROG"] = odr[@"OPPROG"];
              dr[@"OPDESCR"] = odr[@"FRIENDLYDESCR"].ToString().Split('-')[1].Trim();
              dr[@"OPNAME"] = propertySet.cutlistData.GetOpByID(p.ID);
              TreeNodeWithData tn = new TreeNodeWithData(dr, propertySet, topNodeString, new TreeNode[] { });
              treeView1.Nodes.Add(tn);
              if ((int)dr[@"POPPART"] > 0) {
                opDataTableAdapter.Update(dr);
              }
            }
          }
        }
      }
    }

    private int add_node(int idx) {
      DataRow row = OpData.Rows[idx];
      DataRow opData = OpData.Rows[idx];
      var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
      string topNodeString = string.Format(@"Step {0}: {1} Operation", row[@"POPORDER"],
        t.ToTitleCase(row[@"OPDESCR"].ToString().ToLower()));
      double setupTime = 0;
      double runTime = 0;
      TreeNode subNode1 = new TreeNode(string.Format(@"Department: {0}", t.ToTitleCase(row[@"TYPEDESC"].ToString().ToLower())));
      TreeNode subNode2 = new TreeNode((bool)row[@"OPPROG"] ? @"Program required" : @"No program required");
      setupTime = (double)row[@"POPSETUP"] == 0.0 ? (double)row[@"OPSETUP"] : (double)row[@"POPSETUP"];
      runTime = (double)row[@"POPRUN"] == 0.0 ? (double)row[@"OPRUN"] : (double)row[@"POPRUN"];
      TreeNode subNode3 = new TreeNode(string.Format(@"Setup time: {0}", TimeSpan.FromHours(setupTime).ToString()));
      TreeNode subNode4 = new TreeNode(string.Format(@"Run time: {0}", TimeSpan.FromHours(runTime).ToString()));
      TreeNodeWithData topNode = new TreeNodeWithData(opData, propertySet, topNodeString, 
        new TreeNode[] { subNode1, subNode2, subNode3, subNode4 });

      HasCNCOp |= IsCNCOp(topNode);
      HasEBOp |= IsEBOp(topNode);
      OpNameList.Add(topNode.OpData[@"OPNAME"].ToString());

      treeView1.Nodes.Add(topNode);
      return topNode.Index;
    }

    private bool IsCNCOp(TreeNodeWithData tn) {
      bool is_cnc = false;
      foreach (string op in Properties.Settings.Default.CNCOps) {
        if (tn.OpData[@"OPNAME"].ToString() == op) {
          is_cnc |= true;
        }
      }
      return is_cnc;
    }

    private bool IsEBOp(TreeNodeWithData tn) {
      bool is_eb = false;
      foreach (string op in Properties.Settings.Default.EBOps) {
        if (tn.OpData[@"OPNAME"].ToString() == op) {
          is_eb |= true;
        }
      }
      return is_eb;
    }

    private void writeToDB() {
      foreach (TreeNode node in treeView1.Nodes) {
        
      }
    }

    private TreeNodeWithData find_parent_node(TreeNode tn) {
      TreeNode node = tn;
      while (!(node is TreeNodeWithData) && node.Parent != null) {
        node = node.Parent;
      }
      return (TreeNodeWithData)node;
    }

    private int find_parent_index(TreeNode tn) {
      return find_parent_node(tn).Index;
    }

    private void delPartOp() {
      TreeNodeWithData tn = find_parent_node(treeView1.SelectedNode);
      opDataTableAdapter.DeleteQuery((int)tn.OpData[@"POPID"]);
      OpMethodHandler omh = new OpMethodHandler((int)tn.OpData[@"POPPART"],
        (int)tn.OpData[@"POPOP"], (int)tn.OpData[@"POPORDER"], (double)tn.OpData[@"POPSETUP"],
        (double)tn.OpData[@"POPRUN"]);
      omh.UpdateCutlistTime();
    }

    private void button1_Click(object sender, EventArgs e) {
      EditOp eo = new EditOp(partNum, ref propertySet, treeView1.Nodes.Count);
      eo.ShowDialog(this);
      populate(partNum);
    }

    private void button2_Click(object sender, EventArgs e) {
      //int idx = find_parent_index(treeView1.SelectedNode);
      TreeNodeWithData tn = find_parent_node(treeView1.SelectedNode);
      ENGINEERINGDataSet.OpDataRow otr = (ENGINEERINGDataSet.OpDataRow)tn.OpData;
      EditOp eo = new EditOp(partNum, otr, ref propertySet);
      eo.ShowDialog(this);
      populate(partNum);
    }

    private void button3_Click(object sender, EventArgs e) {
      delPartOp();
      populate(partNum);
    }

    public bool HasCNCOp {
      get {
        return has_cnc_op;
      }
      private set {
        has_cnc_op = value;
      }
    }

    public bool HasEBOp {
      get {
        return has_eb_op;
      }
      private set {
        has_eb_op = value;
      }
    }

    public List<string> OpNameList {
      get {
        return op_names;
      }
      private set {
        op_names = value;
      }
    }

    public int Count {
      get {
        return treeView1.Nodes.Count;
      }
      private set {
          count = value;
      }
    }

    public ENGINEERINGDataSet.OpDataDataTable OpData {
      get { return otdt; }
      set { } }
  }
}
