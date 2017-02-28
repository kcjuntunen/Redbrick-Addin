using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Redbrick_Addin {
  class TreeNodeWithData : TreeNode {
    public DataRow OpData { get; set; }
    public SwProperties PropertySet { get; set; }

    public TreeNodeWithData() {
    }

    public TreeNodeWithData(DataRow dataRow, SwProperties props, string text, TreeNode [] children) 
      : base(text, children) {
      OpData = dataRow;
      PropertySet = props;
    }
  }
}
