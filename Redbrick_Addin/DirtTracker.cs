using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redbrick_Addin {
  class DirtTracker {
    private UserControl _uc;

    public DirtTracker(UserControl uc) {
      IsDirty = false;
      _uc = uc;
      AssignHandlers(_uc.Controls);
    }

    private void AssignHandlers(Control.ControlCollection cc) {
      foreach (Control c in cc) {
        if ((c is TextBox) && (!c.Name.ToUpper().Contains("BLANK") && !c.Name.ToUpper().Contains("CUTLIST")))
          (c as TextBox).TextChanged += new EventHandler(DirtTracker_TextChanged);

        if (c is CheckBox)
          (c as CheckBox).CheckedChanged += new EventHandler(DirtTracker_CheckChanged);

        if ((c is ComboBox) && (!c.Name.ToUpper().Contains("CUTLIST") && !c.Name.ToUpper().Contains("STATUS")))
          (c as ComboBox).SelectedIndexChanged += new EventHandler(DirtTracker_SelectionChanged);

        if (c is TreeView)
          (c as TreeView).DrawNode += DirtTracker_DrawNode;

        if (c is DateTimePicker) {
          (c as DateTimePicker).ValueChanged += new EventHandler(DirtTracker_ValueChanged);
        }

        if (c.HasChildren)
          AssignHandlers(c.Controls);
      }
    }

    private void DirtTracker_DrawNode(object sender, DrawTreeNodeEventArgs e) {
      throw new NotImplementedException();
    }

    private void DirtTracker_ValueChanged(object sender, EventArgs e) {
      OnBesmirched(EventArgs.Empty);
    }
    
    private void DirtTracker_TextChanged(object sender, EventArgs e) {
      OnBesmirched(EventArgs.Empty);
    }

    private void DirtTracker_CheckChanged(object sender, EventArgs e) {
      OnBesmirched(EventArgs.Empty);
    }

    private void DirtTracker_SelectionChanged(object sender, EventArgs e) {
      OnBesmirched(EventArgs.Empty);
    }


    public event EventHandler Besmirched;

    protected virtual void OnBesmirched(EventArgs e) {
      if (Besmirched != null)
        Besmirched(this, e);
      IsDirty = true;
    }

    public bool IsDirty { get; set; }
  }
}
