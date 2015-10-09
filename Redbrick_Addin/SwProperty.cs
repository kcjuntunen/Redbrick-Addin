using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin
{
    public class SwProperty
    {
        /// <summary>
        /// Contructor with vars.
        /// </summary>
        /// <param name="PropertyName">This is more or less permanent, and yan't add one if it already exists.</param>
        /// <param name="swType">This gets reassigned appropriately on write because so many old models have it wrong.</param>
        /// <param name="testValue">Unresolved val.</param>
        /// <param name="global">This gets reassigned appropriately on write because so many old models have it wrong.</param>
        public SwProperty(string PropertyName, swCustomInfoType_e swType, string testValue, bool global)
        {
            this.Name = PropertyName;
            this.Type = swType;
            this.Value = testValue;
            this.Global = global;
        }

        /// <summary>
        /// Contructs a stub property.
        /// </summary>
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

        /// <summary>
        /// Rename this property.
        /// </summary>
        /// <param name="NewName">The new name. What else?</param>
        public void Rename(string NewName)
        {
            this.Name = NewName;
        }

        /// <summary>
        /// Write assuming we already have SwApp defined.
        /// </summary>
        public void Write()
        {
            if (this.SwApp != null)
            {
                ModelDoc2 md = (ModelDoc2)this.SwApp.ActiveDoc;
                this.Write(md);
            }
        }

        /// <summary>
        /// Define this.SwApp, then use it to write on the active document.
        /// </summary>
        /// <param name="sw">SwApp</param>
        public void Write(SldWorks sw)
        {
            if (sw != null)
            {
                this.SwApp = sw;
                this.Write();
            }
            else
            {
                if (this.SwApp != null)
                    this.Write();
#if DEBUG
                System.Diagnostics.Debug.Print("SwApp is undefined");
#endif
            }
        }

        /// <summary>
        /// Writes data using the custom property managers of a selected ModelDoc2.
        /// </summary>
        /// <param name="md">A ModelDoc2 object</param>
        public void Write(ModelDoc2 md)
        {
            if (md != null)
            {
                Configuration cf = md.ConfigurationManager.ActiveConfiguration;

                CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
                CustomPropertyManager scpm = md.Extension.get_CustomPropertyManager(string.Empty);

                // Null reference on drawings. Not good. Let's just make everything global if there's no config.
                if (cf != null)
                    scpm = md.Extension.get_CustomPropertyManager(cf.Name);

                // Rather than changing values, we'll just completely overwrite them.
                swCustomPropertyAddOption_e ao = swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd;

                // This is for checking if the writing actually happened. It usually does. Don't know what I'd do if it didn't.
                int res;
                if (this.Global)
                {
                    // This is a global prop that gets a db ID #, so instead of an actual description, we get the # from the datarow in the combobox.
                    if (this.Name.ToUpper().Contains("OP") && !this.Name.ToUpper().Contains("ID") && this.Ctl != null)
                    {
                        System.Data.DataRowView drv = ((this.Ctl as System.Windows.Forms.ComboBox).SelectedItem as System.Data.DataRowView);
                        string v = this.Descr; //drv.Row.ItemArray[1].ToString();
                        res = gcpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
#if DEBUG
                        System.Diagnostics.Debug.Print(string.Format("Writing {0} to {1}: {2}", this.Name, v, this.Value));
#endif
                    }

                    if (((this.Name.ToUpper().Contains("OP") && this.Name.ToUpper().Contains("ID")) || 
                        this.Name.ToUpper().Contains("DEPARTMENT")) && this.Ctl != null)
                    {
                        //System.Data.DataRowView drv = ((this.Ctl as System.Windows.Forms.ComboBox).SelectedItem as System.Data.DataRowView);
                        string v = this.ID; //drv.Row.ItemArray[0].ToString();
                        res = gcpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
    #if DEBUG
                        System.Diagnostics.Debug.Print(string.Format("Writing {0} to {1}: {2}", this.Name, v, this.Value));
    #endif
                    }
                    else if (this.Name.ToUpper().Contains("UPDATE"))
                    {
                        this.Type = swCustomInfoType_e.swCustomInfoYesOrNo;
                        if ((this.Ctl as System.Windows.Forms.CheckBox).Checked)
                            res = gcpm.Add3(this.Name, (int)this.Type, "Yes", (int)ao);
                        else
                            res = gcpm.Add3(this.Name, (int)this.Type, "NO", (int)ao);
                    }
                    else // Regular text, double, and date type global props can just be written.
                    {
                        res = gcpm.Add3(this.Name, (int)this.Type, this.Value, (int)ao);
                    }
                }
                else // Configuration specific props.
                {
                    // We only want material and edging here. It'll get ID #s from the datarow in the combobox.
                    if (this.Name.Contains("ID"))
                    {
                        string v = this.ID;
                        res = scpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
#if DEBUG
                        System.Diagnostics.Debug.Print(this.Name + " <-- " + this.Value);
#endif
                    }

                    if (this.Name.Contains("CUTLIST MATERIAL"))
                    {
                        string v = this.Descr;
                        res = scpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoText, v, (int)ao);
#if DEBUG
                        System.Diagnostics.Debug.Print(this.Name + " <-- " + this.Value);
#endif
                    }

                    if (this.Name.Contains("EDGE"))
                    {
                        string v = this.Descr;
                        res = scpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoText, v, (int)ao);
#if DEBUG
                        System.Diagnostics.Debug.Print(this.Name + " <-- " + this.Value);
#endif
                    }
                }
            }
            else
            {
    #if DEBUG
                System.Diagnostics.Debug.Print(string.Format("{0}: ModelDoc2 md is undefined", this.Name));
    #endif
            }
        }

        /// <summary>
        /// Directly draws from SW.
        /// </summary>
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

        /// <summary>
        /// Directly draws from SW, assinging SwApp.
        /// </summary>
        /// <param name="sw">SwApp</param>
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

        /// <summary>
        /// Directly draws from SW, assinging SwApp. Why do I have all these?
        /// </summary>
        /// <param name="md">A ModelDoc2.</param>
        /// <param name="cd">The Cutlist handler.</param>
        public void Get(ModelDoc2 md, CutlistData cd)
        {
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

                if (this.Name.Contains("OP"))
                {
                    int tp = 0;

                    if (int.TryParse(this._value, out tp))
                    {
                        this.ID = this._resValue;
                        this._value = cd.GetOpByID(this._resValue);
                    }
                    else
                    {
                        this.ID = cd.GetOpIDByName(this._resValue).ToString();
                        this._value = cd.GetOpByID(this.ID);
                    }
                }
            }
            else
            {
                res = scpm.Get5(this.Name, useCached, out this._value, out this._resValue, out wasResolved);
                this.Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);
                if (this.Name.ToUpper().Contains("CUTLIST MATERIAL"))
                {
                    int tp = 0;
                    if (int.TryParse(this._value, out tp))
                    {
                        this.ID = this._resValue;
                        this._value = cd.GetMaterialByID(this._resValue);
                    }
                    else
                    {
                        this.ID = cd.GetMaterialID(this._value).ToString();
                    }
                }

                if (this.Name.ToUpper().Contains("EDGE"))
                {
                    int tp = 0;
                    if (int.TryParse(this._value, out tp))
                    {
                        this.ID = this._resValue;
                        this._value = cd.GetEdgeByID(this._resValue);
                    }
                    else
                    {
                        this.ID = cd.GetMaterialID(this._value).ToString();
                    }
                }
            }
#if DEBUG
            System.Diagnostics.Debug.Print(this.Name + " --> " + this.Value);
#endif
        }

        /// <summary>
        /// Deletes the prop from the SW doc.
        /// </summary>
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

        /// <summary>
        /// Deletes the prop from the SW doc, assigning the SwApp object.
        /// </summary>
        /// <param name="sw"></param>
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

        /// <summary>
        /// Deletes the prop from the selected ModelDoc2 object.
        /// </summary>
        /// <param name="md">A ModelDoc2 object.</param>
        public void Del(ModelDoc2 md)
        {
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

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _descr;

        public string Descr
        {
            get { return _descr; }
            set { _descr = value; }
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
