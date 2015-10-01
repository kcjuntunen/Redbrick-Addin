using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SolidWorks.Interop.swconst;

namespace Redbrick_Addin
{
    public partial class GeneralProperties : UserControl
    {
        SwProperties propertySet;

        public GeneralProperties(ref SwProperties prop)
        {
            this.propertySet = prop;
            InitializeComponent();
        }

        public void Update(ref SwProperties p)
        {
            this.propertySet = p;
            this.LinkControls();
            this.ToggleFields(this.propertySet.cutlistData.OpType);
        }

        private void LinkControls()
        {
            this.tbLength.Text = string.Empty;
            this.tbWidth.Text = string.Empty;
            this.tbThick.Text = string.Empty;
            this.tbWallThick.Text = string.Empty;
            this.tbComment.Text = string.Empty;

            this.propertySet.LinkControlToProperty("Description", true, this.tbDescription);
            this.propertySet.LinkControlToProperty("LENGTH", true, this.tbLength);
            this.propertySet.LinkControlToProperty("WIDTH", true, this.tbWidth);
            this.propertySet.LinkControlToProperty("THICKNESS", true, this.tbThick);
            this.propertySet.LinkControlToProperty("WALL THICKNESS", true, this.tbWallThick);
            this.propertySet.LinkControlToProperty("COMMENT", true, this.tbComment);

            this.UpdateRes(this.propertySet.GetProperty("LENGTH"), this.labResLength);
            this.UpdateRes(this.propertySet.GetProperty("WIDTH"), this.labResWidth);
            this.UpdateRes(this.propertySet.GetProperty("THICKNESS"), this.labResThickness);

            if (this.propertySet.GetProperty("WALL THICKNESS") != null)
                this.UpdateRes(this.propertySet.GetProperty("WALL THICKNESS"), this.labResWallThickness);

            this.UpdateLnW();
        }

        private void UpdateRes(SwProperty p, Control c)
        {
            c.Text = string.Format("{0:N3}", p.ResValue);
        }

        private void LinkControlToProperty(string property, Control c)
        {
            SwProperty p = this.propertySet.GetProperty(property);
            if (this.propertySet.Contains(p))
            {
#if DEBUG
                System.Diagnostics.Debug.Print("Linking " + p.Name + ": " + p.Value);
#endif
                p.Ctl = c;
                c.Text = p.Value;
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print("Creating " + property);
#endif
                SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                x.Ctl = c;
            }
        }

        private void UpdateLnW()
        {
            string tVal;
            double dVal;

            tVal = this.labResLength.Text;
            if (double.TryParse(tVal, out dVal))
                this._length = dVal;
            else
                this._length = 0.0;

            tVal = this.labResWidth.Text;
            if (double.TryParse(tVal, out dVal))
                this._width = dVal;
            else
                this._width = 0.0;
        }

        public void ToggleFields(int opType)
        {
            bool wood = (opType != 2);
            this.labResWallThickness.Enabled = !wood;
            this.lWallThickness.Enabled = !wood;
            this.tbWallThick.Enabled = !wood;
        }

        public TextBox GetDescriptionBox()
        {
            return this.tbDescription;
        }

        public TextBox GetLengthBox()
        {
            return this.tbLength;
        }

        public TextBox GetWidthBox()
        {
            return this.tbWidth;
        }

        public TextBox GetThicknessBox()
        {
            return this.tbThick;
        }

        public TextBox GetWallThicknessBox()
        {
            return this.tbWallThick;
        }

        public TextBox GetCommentBox()
        {
            return this.tbComment;
        }

        public void UpdateLengthRes(SwProperty p)
        {
            this.labResLength.Text = p.ResValue;
        }

        public void UpdateWidthRes(SwProperty p)
        {
            this.labResWidth.Text = p.ResValue;
        }

        public void UpdateThickRes(SwProperty p)
        {
            this.labResThickness.Text = p.ResValue;
        }

        public void UpdateWallThickRes(SwProperty p)
        {
            if (p != null)
                this.labResWallThickness.Text = p.ResValue;
        }

        private void tbLength_Leave(object sender, EventArgs e)
        {
            this.propertySet.GetProperty("LENGTH").Value = this.tbLength.Text;
            this.propertySet.GetProperty("LENGTH").Write();
            this.propertySet.GetProperty("LENGTH").Get();
            this.labResLength.Text = this.propertySet.GetProperty("LENGTH").ResValue;
        }

        private void tbWidth_Leave(object sender, EventArgs e)
        {
            this.propertySet.GetProperty("WIDTH").Value = this.tbWidth.Text;
            this.propertySet.GetProperty("WIDTH").Write();
            this.propertySet.GetProperty("WIDTH").Get();
            this.labResWidth.Text = this.propertySet.GetProperty("WIDTH").ResValue;
        }

        private void tbThick_Leave(object sender, EventArgs e)
        {
            this.propertySet.GetProperty("THICKNESS").Value = this.tbThick.Text;
            this.propertySet.GetProperty("THICKNESS").Write();
            this.propertySet.GetProperty("THICKNESS").Get();
            this.labResThickness.Text = this.propertySet.GetProperty("THICKNESS").ResValue;
        }

        private void tbWallThick_Leave(object sender, EventArgs e)
        {
            this.propertySet.GetProperty("WALL THICKNESS").Value = this.tbWallThick.Text;
            this.propertySet.GetProperty("WALL THICKNESS").Write();
            this.propertySet.GetProperty("WALL THICKNESS").Get();
            this.labResWallThickness.Text = this.propertySet.GetProperty("WALL THICKNESS").ResValue;
        }

        private double _length;

        public double PartLength
        {
            get { return _length; }
            set { _length = value; }
        }

        private double _width;

        public double PartWidth
        {
            get { return _width; }
            set { _width = value; }
        }

        private void labResLength_TextChanged(object sender, EventArgs e)
        {
            this.UpdateLnW();
        }

        private void labResWidth_TextChanged(object sender, EventArgs e)
        {
            this.UpdateLnW();
        }

        private void bCopy_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(this.tbDescription.Text);
        }
    }
}
