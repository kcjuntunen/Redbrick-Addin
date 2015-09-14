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
    public partial class MachineProperties : UserControl
    {
        SwProperties propertySet;
        public MachineProperties(ref SwProperties prop)
        {
            this.propertySet = prop;
            InitializeComponent();
            this.LinkControls();
        }

        private void LinkControls()
        {
            this.LinkControlToProperty("BLANK QTY", this.tbPPB);
            this.LinkControlToProperty("CNC1", this.tbCNC1);
            this.LinkControlToProperty("CNC2", this.tbCNC2);
            this.LinkControlToProperty("OVERL", this.tbOverL);

            string tVal = this.propertySet.GetProperty("OVERL").Value;
            double dVal = 0.0;
            if (double.TryParse(tVal, out dVal))
            {
                this._overL = dVal;
            }

            this.LinkControlToProperty("OVERW", this.tbOverW);

            tVal = this.propertySet.GetProperty("OVERW").Value;
            dVal = 0.0;
            if (double.TryParse(tVal, out dVal))
            {
                this._overW = dVal;
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
