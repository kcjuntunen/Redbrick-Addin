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
    public partial class MachineProperties : UserControl
    {
        SwProperties propertySet;
        public MachineProperties(ref SwProperties prop)
        {
            this.propertySet = prop;
            InitializeComponent();
            this.LinkControls();
        }

        public void Update(ref SwProperties p)
        {
            this.propertySet = p;
            this.LinkControls();
            this.ToggleFields(this.propertySet.cutlistData.OpType);
        }

        private void LinkControls()
        {
            this.tbCNC1.Text = string.Empty;
            this.tbCNC2.Text = string.Empty;
            this.tbOverL.Text = string.Empty;
            this.tbOverW.Text = string.Empty;
            this.tbBlankW.Text = string.Empty;
            this.tbBlankL.Text = string.Empty;

            this.propertySet.LinkControlToProperty("BLANK QTY", true, this.tbPPB);
            this.propertySet.LinkControlToProperty("CNC1", true, this.tbCNC1);
            this.propertySet.LinkControlToProperty("CNC2", true, this.tbCNC2);
            this.propertySet.LinkControlToProperty("OVERL", true, this.tbOverL);
            this.propertySet.LinkControlToProperty("OVERW", true, this.tbOverW);

            string tVal = "0.0";
            double dVal = 0.0;
            int edgef = 0;
            int edgeb = 0;
            int edgel = 0;
            int edger = 0;

            double finLen = 0.0;
            double blankLen = 0.0;
            double total_thickness = 0.0;

            if (this.propertySet.Contains("LENGTH"))
            {
                if (double.TryParse(this.propertySet.GetProperty("LENGTH").ResValue, out finLen))
                    blankLen += finLen;

                if (this.propertySet.Contains("EDGE FRONT (L)"))
                    if (int.TryParse(this.propertySet.GetProperty("EDGE FRONT (L)").Value, out edgef))
                        total_thickness -= this.propertySet.cutlistData.GetEdgeThickness(edgef);

                if (this.propertySet.Contains("EDGE BACK (L)"))
                    if (int.TryParse(this.propertySet.GetProperty("EDGE BACK (L)").Value, out edgeb))
                        total_thickness -= this.propertySet.cutlistData.GetEdgeThickness(edgeb);

                if (this.propertySet.Contains("OVERL"))
                    tVal = this.propertySet.GetProperty("OVERL").Value;

                if (double.TryParse(tVal, out dVal))
                    this._overL = dVal;

                this.tbBlankL.Text = (blankLen + dVal - total_thickness).ToString("N3");
            }

            finLen = 0.0;
            blankLen = 0.0;
            total_thickness = 0.0;
            if (this.propertySet.Contains("WIDTH"))
            {
                if (double.TryParse(this.propertySet.GetProperty("WIDTH").ResValue, out finLen))
                    blankLen += finLen;

                if (this.propertySet.Contains("EDGE LEFT (W)"))
                    if (int.TryParse(this.propertySet.GetProperty("EDGE LEFT (W)").Value, out edgel))
                        total_thickness -= this.propertySet.cutlistData.GetEdgeThickness(edgel);

                if (this.propertySet.Contains("EDGE RIGHT (W)"))
                    if (int.TryParse(this.propertySet.GetProperty("EDGE RIGHT (W)").Value, out edger))
                        total_thickness -= this.propertySet.cutlistData.GetEdgeThickness(edger);

                if (this.propertySet.Contains("OVERW"))
                    tVal = this.propertySet.GetProperty("OVERW").Value;

                dVal = 0.0;
                if (double.TryParse(tVal, out dVal))
                    this._overW = dVal;
                
                this.tbBlankW.Text = (blankLen + dVal - total_thickness).ToString("N3");
            }
        }

        private void LinkControlToProperty(string property, Control c)
        {
            SwProperty p = this.propertySet.GetProperty(property);
            if (this.propertySet.Contains(p))
            {
#if DEBUG
                System.Diagnostics.Debug.Print("Linking " + p.Name);
#endif
                p.Ctl = c;
                c.Text = p.Value;
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print("Creating " + p.Name);
#endif
                SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                x.Ctl = c;
            }
        }

        public void ToggleFields(int opType)
        {
            bool wood = (opType != 2);
            this.tbOverL.Enabled = wood;
            this.tbOverW.Enabled = wood;
            this.tbBlankL.Enabled = wood;
            this.tbBlankW.Enabled = wood;
            this.label4.Enabled = wood;
            this.label5.Enabled = wood;
            this.label6.Enabled = wood;
        }

        public TextBox GetCNC1Box()
        {
            return this.tbCNC1;
        }

        public TextBox GetCNC2Box()
        {
            return this.tbCNC2;
        }

        public TextBox GetPartsPerBlankBox()
        {
            return this.tbPPB;
        }

        public TextBox GetOverLBox()
        {
            return this.tbOverL;
        }

        public TextBox GetOverWBox()
        {
            return this.tbOverW;
        }

        public TextBox GetBlankLBox()
        {
            return this.tbBlankL;
        }

        public TextBox GetBlankWBox()
        {
            return this.tbBlankW;
        }

        private double _overL;

        public double OverL
        {
            get { return _overL; }
            set { _overL = value; }
        }

        private double _overW;

        public double OverW
        {
            get { return _overW; }
            set { _overW = value; }
        }
	
        private void tbOverL_TextChanged(object sender, EventArgs e)
        {
            this.tbOverL.Text = string.Format("{0:0.000}", this.tbOverL.Text);
        }

        private void tbOverW_TextChanged(object sender, EventArgs e)
        {
            this.tbOverW.Text = string.Format("{0:0.000}", this.tbOverW.Text);
        }

        private void tbOverL_Validated(object sender, EventArgs e)
        {
            string tVal = this.tbOverL.Text;
            double dVal = 0.0;
            if (double.TryParse(tVal, out dVal))
            {
                this._overL = dVal;
#if DEBUG
                System.Diagnostics.Debug.Print(double.Parse(tVal).ToString());
#endif
            }
        }

        private void tbOverW_Validated(object sender, EventArgs e)
        {
            string tVal = this.tbOverW.Text;
            double dVal = 0.0;
            if (double.TryParse(tVal, out dVal))
            {
                this._overW = dVal;
#if DEBUG
                System.Diagnostics.Debug.Print(tVal);
#endif
            }
        }
    }
}
