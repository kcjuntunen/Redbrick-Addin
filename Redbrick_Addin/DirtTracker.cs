using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redbrick_Addin
{
    class DirtTracker
    {
        private UserControl _uc;

        public DirtTracker(UserControl uc)
        {
            this.IsDirty = false;
            this._uc = uc;
            AssignHandlers(this._uc.Controls);
        }

        private void AssignHandlers(Control.ControlCollection cc)
        {
            foreach (Control c in cc)
            {
                if ((c is TextBox) && (!c.Name.ToUpper().Contains("BLANK") || !c.Name.ToUpper().Contains("CUTLIST")))
                    (c as TextBox).TextChanged += new EventHandler(DirtTracker_TextChanged);

                if (c is CheckBox)
                    (c as CheckBox).CheckedChanged += new EventHandler(DirtTracker_CheckChanged);

                if (c is ComboBox)
                    (c as ComboBox).SelectedIndexChanged += new EventHandler(DirtTracker_SelectionChanged);

                if (c.HasChildren)
                    this.AssignHandlers(c.Controls);
            }
        }

        private void DirtTracker_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        private void DirtTracker_CheckChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        private void DirtTracker_SelectionChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        public bool IsDirty { get; set; }
    }
}
