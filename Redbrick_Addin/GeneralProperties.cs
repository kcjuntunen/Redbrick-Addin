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
            propertySet = p;
            LinkControls();
            ToggleFields(this.propertySet.cutlistData.OpType);
        }

        private void LinkControls()
        {
            tbLength.Text = string.Empty;
            tbWidth.Text = string.Empty;
            tbThick.Text = string.Empty;
            tbWallThick.Text = string.Empty;
            tbComment.Text = string.Empty;

            propertySet.LinkControlToProperty("Description", true, tbDescription);
            propertySet.LinkControlToProperty("LENGTH", true, tbLength);
            propertySet.LinkControlToProperty("WIDTH", true, tbWidth);
            propertySet.LinkControlToProperty("THICKNESS", true, tbThick);
            propertySet.LinkControlToProperty("WALL THICKNESS", true, tbWallThick);
            propertySet.LinkControlToProperty("COMMENT", true, tbComment);

            UpdateRes(propertySet.GetProperty("LENGTH"), labResLength);
            UpdateRes(propertySet.GetProperty("WIDTH"), labResWidth);
            UpdateRes(propertySet.GetProperty("THICKNESS"), labResThickness);

            if (propertySet.GetProperty("WALL THICKNESS") != null)
                UpdateRes(propertySet.GetProperty("WALL THICKNESS"), labResWallThickness);

            UpdateLnW();
        }

        private void UpdateRes(SwProperty p, Control c)
        {
            c.Text = string.Format("{0:N3}", p.ResValue);
        }

        private void LinkControlToProperty(string property, Control c)
        {
            SwProperty p = propertySet.GetProperty(property);
            if (propertySet.Contains(p))
            {
                p.Ctl = c;
                c.Text = p.Value;
            }
            else
            {
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
                _length = dVal;
            else
                _length = 0.0;

            tVal = this.labResWidth.Text;
            if (double.TryParse(tVal, out dVal))
                _width = dVal;
            else
                _width = 0.0;
        }

        public void ToggleFields(int opType)
        {
            bool wood = (opType != 2);
            labResWallThickness.Enabled = !wood;
            lWallThickness.Enabled = !wood;
            tbWallThick.Enabled = !wood;
        }

        public TextBox GetDescriptionBox()
        {
            return tbDescription;
        }

        public TextBox GetLengthBox()
        {
            return tbLength;
        }

        public TextBox GetWidthBox()
        {
            return tbWidth;
        }

        public TextBox GetThicknessBox()
        {
            return tbThick;
        }

        public TextBox GetWallThicknessBox()
        {
            return tbWallThick;
        }

        public TextBox GetCommentBox()
        {
            return tbComment;
        }

        public void UpdateLengthRes(SwProperty p)
        {
            labResLength.Text = p.ResValue;
        }

        public void UpdateWidthRes(SwProperty p)
        {
            labResWidth.Text = p.ResValue;
        }

        public void UpdateThickRes(SwProperty p)
        {
            labResThickness.Text = p.ResValue;
        }

        public void UpdateWallThickRes(SwProperty p)
        {
            if (p != null)
                labResWallThickness.Text = p.ResValue;
        }

        private void tbLength_Leave(object sender, EventArgs e)
        {
            propertySet.GetProperty("LENGTH").Value = tbLength.Text;
            propertySet.GetProperty("LENGTH").Write();
            propertySet.GetProperty("LENGTH").Get();
            labResLength.Text = propertySet.GetProperty("LENGTH").ResValue;
        }

        private void tbWidth_Leave(object sender, EventArgs e)
        {
            propertySet.GetProperty("WIDTH").Value = tbWidth.Text;
            propertySet.GetProperty("WIDTH").Write();
            propertySet.GetProperty("WIDTH").Get();
            labResWidth.Text = propertySet.GetProperty("WIDTH").ResValue;
        }

        private void tbThick_Leave(object sender, EventArgs e)
        {
            propertySet.GetProperty("THICKNESS").Value = tbThick.Text;
            propertySet.GetProperty("THICKNESS").Write();
            propertySet.GetProperty("THICKNESS").Get();
            labResThickness.Text = propertySet.GetProperty("THICKNESS").ResValue;
        }

        private void tbWallThick_Leave(object sender, EventArgs e)
        {
            propertySet.GetProperty("WALL THICKNESS").Value = tbWallThick.Text;
            propertySet.GetProperty("WALL THICKNESS").Write();
            propertySet.GetProperty("WALL THICKNESS").Get();
            labResWallThickness.Text = propertySet.GetProperty("WALL THICKNESS").ResValue;
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
