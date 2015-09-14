using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SolidWorks.Interop.swconst;

namespace redbrick.csproj
{
    public partial class GeneralProperties : UserControl
    {
        SwProperties propertySet;

        public GeneralProperties(ref SwProperties prop)
        {
            this.propertySet = prop;
            InitializeComponent();
            this.LinkControls();
        }

        private void LinkControls()
        {

            this.LinkControlToProperty("Description", this.tbDescription);
            this.LinkControlToProperty("LENGTH", this.tbLength);
            this.UpdateLengthRes(propertySet.GetProperty("LENGTH"));

            this.LinkControlToProperty("WIDTH", this.tbWidth);
            this.UpdateWidthRes(propertySet.GetProperty("WIDTH"));

            this.LinkControlToProperty("THICKNESS", this.tbThick);
            this.UpdateThickRes(propertySet.GetProperty("THICKNESS"));
            this.LinkControlToProperty("WALL THICKNESS", this.tbWallThick);
            this.UpdateWallThickRes(propertySet.GetProperty("WALL THICKNESS"));
            this.LinkControlToProperty("COMMENT", this.tbComment);

            this.UpdateLnW();
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

        public void ToggleFields(string opType)
        {
            bool wood = (opType != "METAL");
            this.labResWallThickness.Visible = !wood;
            this.lWallThickness.Visible = !wood;
            this.tbWallThick.Visible = !wood;
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
