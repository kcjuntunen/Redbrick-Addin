using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace redbrick.csproj
{
    public class SwProperty
    {

        public SwProperty(string PropertyName, swCustomInfoType_e swType, string testValue, bool global)
        {
            this.Name = PropertyName;
            this.Type = swType;
            this.Value = testValue;
            this.Global = global;
        }

        public SwProperty()
        {
            string n = "STUB" + DateTime.Now.ToLongTimeString();
            this.Name = n;
            this.Type = swCustomInfoType_e.swCustomInfoText;
            this.Value = "NULL";
            this.Global = false;

            this.ID = "0";
            this.Field = "[Nope]";
            this.Table = "[No]";
        }

        public void Write()
        {
            if (this.SwApp != null)
            {
                ModelDoc2 md = (ModelDoc2)this.SwApp.ActiveDoc;
                Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                CustomPropertyManager scpm = md.Extension.get_CustomPropertyManager(string.Empty);
                
                if (cf != null)
                    scpm = md.Extension.get_CustomPropertyManager(cf.Name);
                
                
                swCustomPropertyAddOption_e ao = swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd;
                
                int res;
                if (this.Global)
                {
                    if (this.Name.Contains("OP"))
                    {
                        string v = ((this.Ctl as System.Windows.Forms.ComboBox).SelectedItem as System.Data.DataRowView).Row.ItemArray[0].ToString();
                        res = gcpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
#if DEBUG
                        System.Diagnostics.Debug.Print(string.Format("Writing {0} to {1}: {2}", this.Name, v, this.Value));
#endif
                    }
                    else
                    {
                        res = gcpm.Add3(this.Name, (int)this.Type, this.Value.ToUpper(), (int)ao);
                    }
                }
                else
                {
                    if (this.Name.Contains("EDGE") || this.Name.Contains("CUTLIST MATERIAL"))
                    {
                        string v = "0";
                        if ((this.Ctl as System.Windows.Forms.ComboBox).SelectedItem != null)
                            v = ((this.Ctl as System.Windows.Forms.ComboBox).SelectedItem as System.Data.DataRowView).Row.ItemArray[0].ToString();

                        res = scpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
#if DEBUG
                        System.Diagnostics.Debug.Print(this.Name + " <-- " + this.Value);
#endif
                    }
                }
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print(string.Format("{0}: SwApp is undefined", this.Name));
#endif
            }
        }

        public void Write(SldWorks sw)
        {
            if (sw != null)
            {
                this.SwApp = sw;
                ModelDoc2 md = (ModelDoc2)sw.ActiveDoc;
                Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                CustomPropertyManager scpm;
                if (cf != null)
                {
                    scpm = md.Extension.get_CustomPropertyManager(cf.Name);
                }
                else
                {
                    scpm = md.Extension.get_CustomPropertyManager(string.Empty);
                }

                swCustomPropertyAddOption_e ao = swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd;
                int res;

                if (this.Global)
                    res = gcpm.Add3(this.Name, (int)this.Type, this.Value, (int)ao);
                else
                    res = scpm.Add3(this.Name, (int)this.Type, this.Value, (int)ao);
#if DEBUG
                System.Diagnostics.Debug.Print(this.Name + " <-- " + this.Value);
#endif
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print("SwApp is undefined");
#endif
            }
        }

        public void Get()
        {
            if (this.SwApp != null)
                if (this.SwApp != null)
                {
                    ModelDoc2 md = (ModelDoc2)this.SwApp.ActiveDoc;
                    Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                    CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                    CustomPropertyManager scpm;

                    bool wasResolved;
                    bool useCached = false;

                    if (cf != null)
                    {
                        scpm = md.Extension.get_CustomPropertyManager(cf.Name);
                    }
                    else
                    {
                        scpm = md.Extension.get_CustomPropertyManager(string.Empty);
                    }
                    int res;

                    if (this.Global)
                    {
                        res = gcpm.Get5(this.Name, useCached, out this._value, out this._resValue, out wasResolved);
                        this.Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);


                        if (this.Type == swCustomInfoType_e.swCustomInfoNumber && this.Name.ToUpper().Contains("OVER"))
                            this.Type = swCustomInfoType_e.swCustomInfoDouble;
                    }
                    else
                    {
                        res = scpm.Get5(this.Name, useCached, out this._value, out this._resValue, out wasResolved);
                        this.Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);
                    }
#if DEBUG
                    System.Diagnostics.Debug.Print(this.Name + " --> " + this.Value);
#endif
                }
                else
                {
#if DEBUG
                    System.Diagnostics.Debug.Print("SwApp is undefined");
#endif
                }
        }

        public void Get(SldWorks sw)
        {
            if (sw != null)
            {
                this.SwApp = sw;
                ModelDoc2 md = (ModelDoc2)sw.ActiveDoc;
                Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                CustomPropertyManager scpm;

                bool wasResolved;
                bool useCached = false;

                if (cf != null)
                {
                    scpm = md.Extension.get_CustomPropertyManager(cf.Name);
                }
                else
                {
                    scpm = md.Extension.get_CustomPropertyManager(string.Empty);
                }
                int res;

                if (this.Global)
                {
                    res = gcpm.Get5(this.Name, useCached, out this._value, out this._resValue, out wasResolved);
                    this.Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);


                    if (this.Type == swCustomInfoType_e.swCustomInfoNumber && this.Name.ToUpper().Contains("OVER"))
                        this.Type = swCustomInfoType_e.swCustomInfoDouble;
                }
                else
                {
                    res = scpm.Get5(this.Name, useCached, out this._value, out this._resValue, out wasResolved);
                    this.Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);
                }
#if DEBUG
                System.Diagnostics.Debug.Print(this.Name + " --> " + this.Value);
#endif
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print("SwApp is undefined");
#endif
            }
        }

        public void Del()
        {
            if (this.SwApp != null)
            {
                ModelDoc2 md = (ModelDoc2)this.SwApp.ActiveDoc;
                Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                CustomPropertyManager scpm;

                if (cf != null)
                {
                    scpm = md.Extension.get_CustomPropertyManager(cf.Name);
                }
                else
                {
                    scpm = md.Extension.get_CustomPropertyManager(string.Empty);
                }
                int res;

                if (this.Global)
                    res = gcpm.Delete2(this.Name);
                else
                    res = scpm.Delete2(this.Name);
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print("SwApp is undefined");
#endif
            }
        }

        public void Del(SldWorks sw)
        {
            if (sw != null)
            {
                this.SwApp = sw;
                ModelDoc2 md = (ModelDoc2)sw.ActiveDoc;
                Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                CustomPropertyManager scpm;
                if (cf != null)
                {
                    scpm = md.Extension.get_CustomPropertyManager(cf.Name);
                }
                else
                {
                    scpm = md.Extension.get_CustomPropertyManager(string.Empty);
                }

                int res;

                if (this.Global)
                    res = gcpm.Delete2(this.Name);
                else
                    res = scpm.Delete2(this.Name);
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Print("SwApp is undefined");
#endif
            }
        }

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
	

        private string _propName;

        public string Name
        {
            get { return _propName; }
            set { _propName = value; }
        }

        private swCustomInfoType_e _propType;

        public swCustomInfoType_e Type
        {
            get { return _propType; }
            set { _propType = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _resValue;

        public string ResValue
        {
            get { return _resValue; }
            set { _resValue = value; }
        }
	

        private string _field;

        public string Field
        {
            get { return _field; }
            set { _field = value; }
        }

        private string _table;

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        private System.Windows.Forms.Control _ctl;

        public System.Windows.Forms.Control Ctl
        {
            get { return _ctl; }
            set { _ctl = value; }
        }

        private bool _global;

        public bool Global
        {
            get { return _global; }
            set { _global = value; }
        }

        public override string ToString()
        {
            return this.Name + ": " + this.Value + " => " + this.ResValue;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SwProperty))
                return false;

            return (this.Name == (obj as SwProperty).Name) && (this.Value == (obj as SwProperty).Value);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Value.GetHashCode();
        }

        private SldWorks _swApp;

        public SldWorks SwApp
        {
            get { return _swApp; }
            set { _swApp = value; }
        }
	
    }
}
