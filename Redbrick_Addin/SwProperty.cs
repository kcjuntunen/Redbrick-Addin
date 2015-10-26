using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin {
  public class SwProperty {
    /// <summary>
    /// Contructor with vars.
    /// </summary>
    /// <param name="PropertyName">This is more or less permanent, and yan't add one if it already exists.</param>
    /// <param name="swType">This gets reassigned appropriately on write because so many old models have it wrong.</param>
    /// <param name="testValue">Unresolved val.</param>
    /// <param name="global">This gets reassigned appropriately on write because so many old models have it wrong.</param>
    public SwProperty(string PropertyName, swCustomInfoType_e swType, string testValue, bool global) {
      Name = PropertyName;
      Type = swType;
      Value = testValue;
      Global = global;
    }

    /// <summary>
    /// Contructs a stub property.
    /// </summary>
    public SwProperty() {
      string n = "STUB" + DateTime.Now.ToLongTimeString();
      Name = n;
      Type = swCustomInfoType_e.swCustomInfoText;
      Value = "NULL";
      ResValue = "NULL";
      Global = false;
      Descr = "NULL";
      ID = "0";
      Field = "[Nope]";
      Table = "[No]";
    }

    /// <summary>
    /// Rename this property.
    /// </summary>
    /// <param name="NewName">The new name. What else?</param>
    public void Rename(string NewName) {
      Name = NewName;
    }

    /// <summary>
    /// Write assuming we already have SwApp defined.
    /// </summary>
    public void Write() {
      if (this.SwApp != null) {
        ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
        Write(md);
      }
    }

    /// <summary>
    /// Define this.SwApp, then use it to write on the active document.
    /// </summary>
    /// <param name="sw">SwApp</param>
    public void Write(SldWorks sw) {
      if (sw != null) {
        SwApp = sw;
        Write();
      } else {
        if (SwApp != null)
          Write();
      }
    }

    /// <summary>
    /// Writes data using the custom property managers of a selected ModelDoc2.
    /// </summary>
    /// <param name="md">A ModelDoc2 object</param>
    public void Write(ModelDoc2 md) {
      if (md != null) {
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
        if (Global) {
          // This is a global prop that gets a db ID #, so instead of an actual description, we get the # from the datarow in the combobox.
          if ((((Name.ToUpper().StartsWith("OP") && !Name.ToUpper().EndsWith("ID"))) || Name.ToUpper().Contains("DEPARTMENT"))
              && Ctl != null) {
            System.Data.DataRowView drv = ((Ctl as System.Windows.Forms.ComboBox).SelectedItem as System.Data.DataRowView);
            string v = Descr;
            res = gcpm.Add3(Name, (int)swCustomInfoType_e.swCustomInfoText, v, (int)ao);
          }

          if (((Name.ToUpper().EndsWith("ID")) ||
              Name.ToUpper().Contains("DEPT")) && Ctl != null) {
            string v = ID;
            res = gcpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
          } else if (this.Name.ToUpper().Contains("UPDATE")) {
            this.Type = swCustomInfoType_e.swCustomInfoYesOrNo;
            if ((Ctl as System.Windows.Forms.CheckBox).Checked)
              res = gcpm.Add3(Name, (int)Type, "Yes", (int)ao);
            else
              res = gcpm.Add3(Name, (int)Type, "NO", (int)ao);
          } else // Regular text, double, and date type global props can just be written.
                    {
            res = gcpm.Add3(Name, (int)Type, Value, (int)ao);
          }
        } else {
          // Configuration specific props.
          if (this.Name.Contains("CUTLIST MATERIAL")) {
            string v = Descr;
            res = scpm.Add3(Name, (int)swCustomInfoType_e.swCustomInfoText, v, (int)ao);
          }

          if (this.Name.Contains("EDGE")) {
            string v = Descr;
            res = scpm.Add3(Name, (int)swCustomInfoType_e.swCustomInfoText, v, (int)ao);
          }
        }
      } else {
      }
    }

    public void Write2(ModelDoc2 md) {
      if (md != null) {
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
        switch (this.Type) {
          case swCustomInfoType_e.swCustomInfoDate:
            res = gcpm.Add3(Name, (int)Type, Value, (int)ao);
            break;
          case swCustomInfoType_e.swCustomInfoDouble:
            res = gcpm.Add3(Name, (int)Type, Value, (int)ao);
            break;
          case swCustomInfoType_e.swCustomInfoNumber:
            if (this.Global)
              if (this.Name.ToUpper().Contains("BLANK"))
                res = gcpm.Add3(Name, (int)this.Type, Value, (int)ao);
              else
                res = gcpm.Add3(Name, (int)Type, ID, (int)ao);
            else
              scpm.Add3(Name, (int)Type, ID, (int)ao);
            break;
          case swCustomInfoType_e.swCustomInfoText:
            if (this.Global)
              if (Name.ToUpper().StartsWith("OP") && !Name.ToUpper().EndsWith("ID"))
                res = gcpm.Add3(Name, (int)Type, Descr, (int)ao);
              else if (Name.ToUpper().EndsWith("ID"))
                res = gcpm.Add3(Name, (int)swCustomInfoType_e.swCustomInfoNumber, ID, (int)ao);
              else if (Name.ToUpper().Contains("UPDATE"))
                res = gcpm.Add3(Name, (int)swCustomInfoType_e.swCustomInfoYesOrNo, (ID == "-1" ? "Yes" : "N"), (int)ao);
              else
                res = gcpm.Add3(Name, (int)Type, Value, (int)ao);
            else
              res = scpm.Add3(Name, (int)Type, Value, (int)ao);
            break;
          case swCustomInfoType_e.swCustomInfoUnknown:
            break;
          case swCustomInfoType_e.swCustomInfoYesOrNo:
            if (Ctl != null) {
              if ((Ctl as System.Windows.Forms.CheckBox).Checked)
                res = gcpm.Add3(Name, (int)Type, "Yes", (int)ao);
              else
                res = gcpm.Add3(Name, (int)Type, "N", (int)ao);
            } else {
              res = gcpm.Add3(Name, (int)Type, Value, (int)ao);
            }
            break;
          default:
            if (!Name.StartsWith("STUB")) {
              if (Global)
                res = gcpm.Add3(Name, (int)Type, Value, (int)ao);
              else
                res = scpm.Add3(Name, (int)Type, Value, (int)ao);
            }
            break;
        }
      }
    }

    /// <summary>
    /// Directly draws from SW.
    /// </summary>
    public void Get() {
      if (SwApp != null) {
        ModelDoc2 md = (ModelDoc2)this.SwApp.ActiveDoc;
        ConfigurationManager cfMgr = md.ConfigurationManager;
        Configuration cf = cfMgr.ActiveConfiguration;

        CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
        CustomPropertyManager scpm;

        bool wasResolved;
        bool useCached = false;

        if (cf != null) {
          scpm = cf.CustomPropertyManager;
        } else {
          scpm = gcpm;
        }
        int res;

        if (this.Global) {
          res = gcpm.Get5(Name, useCached, out _value, out _resValue, out wasResolved);
          this.Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);


          if (Type == swCustomInfoType_e.swCustomInfoNumber && Name.ToUpper().Contains("OVER"))
            Type = swCustomInfoType_e.swCustomInfoDouble;
        } else {
          res = scpm.Get5(Name, useCached, out _value, out _resValue, out wasResolved);
          this.Type = (swCustomInfoType_e)gcpm.GetType2(Name);
        }
      } else {
        throw new NullReferenceException("sw is null");
      }
    }

    /// <summary>
    /// Directly draws from SW, assinging SwApp.
    /// </summary>
    /// <param name="sw">SwApp</param>
    public void Get(SldWorks sw) {
      if (sw != null) {
        this.SwApp = sw;
        ModelDoc2 md = (ModelDoc2)sw.ActiveDoc;
        Configuration cf = md.ConfigurationManager.ActiveConfiguration;

        CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
        CustomPropertyManager scpm;

        bool wasResolved;
        bool useCached = false;

        if (cf != null) {
          scpm = md.Extension.get_CustomPropertyManager(cf.Name);
        } else {
          scpm = md.Extension.get_CustomPropertyManager(string.Empty);
        }
        int res;

        if (this.Global) {
          res = gcpm.Get5(this.Name, useCached, out _value, out _resValue, out wasResolved);
          Type = (swCustomInfoType_e)gcpm.GetType2(this.Name);


          if (Type == swCustomInfoType_e.swCustomInfoNumber && Name.ToUpper().Contains("OVER"))
            Type = swCustomInfoType_e.swCustomInfoDouble;
        } else {
          res = scpm.Get5(Name, useCached, out _value, out _resValue, out wasResolved);
          this.Type = (swCustomInfoType_e)gcpm.GetType2(Name);
        }
      } else {
        throw new NullReferenceException("sw is null");
      }
    }

    /// <summary>
    /// Directly draws from SW, assinging SwApp. Why do I have all these?
    /// </summary>
    /// <param name="md">A ModelDoc2.</param>
    /// <param name="cd">The Cutlist handler.</param>
    public void Get(ModelDoc2 md, CutlistData cd) {
      Configuration cf = md.ConfigurationManager.ActiveConfiguration;

      CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
      CustomPropertyManager scpm;

      bool wasResolved;
      bool useCached = false;

      if (cf != null) {
        scpm = md.Extension.get_CustomPropertyManager(cf.Name);
      } else {
        scpm = md.Extension.get_CustomPropertyManager(string.Empty);
      }
      int res;

      if (this.Global) {
        res = gcpm.Get5(Name, useCached, out _value, out _resValue, out wasResolved);
        Type = (swCustomInfoType_e)gcpm.GetType2(Name);


        if (Type == swCustomInfoType_e.swCustomInfoNumber && Name.ToUpper().Contains("OVER"))
          Type = swCustomInfoType_e.swCustomInfoDouble;

        if (this.Name.Contains("OP")) {
          int tp = 0;

          if (int.TryParse(this._value, out tp)) {
            ID = _resValue;
            _value = cd.GetOpByID(_resValue);
          } else {
            ID = cd.GetOpIDByName(_resValue).ToString();
            _value = cd.GetOpByID(ID);
          }
        }
      } else {
        res = scpm.Get5(Name, useCached, out _value, out _resValue, out wasResolved);
        Type = (swCustomInfoType_e)gcpm.GetType2(Name);
        if (Name.ToUpper().Contains("CUTLIST MATERIAL")) {
          int tp = 0;
          if (int.TryParse(_value, out tp)) {
            ID = _resValue;
            _value = cd.GetMaterialByID(_resValue);
          } else {
            ID = cd.GetMaterialID(_value).ToString();
          }
        }

        if (Name.ToUpper().Contains("EDGE")) {
          int tp = 0;
          if (int.TryParse(_value, out tp)) {
            ID = _resValue;
            _value = cd.GetEdgeByID(_resValue);
          } else {
            ID = cd.GetMaterialID(_value).ToString();
          }
        }
      }
    }

    /// <summary>
    /// Deletes the prop from the SW doc.
    /// </summary>
    public void Del() {
      if (SwApp != null) {
        ModelDoc2 md = (ModelDoc2)this.SwApp.ActiveDoc;
        Configuration cf = md.ConfigurationManager.ActiveConfiguration;

        CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
        CustomPropertyManager scpm;

        if (cf != null) {
          scpm = md.Extension.get_CustomPropertyManager(cf.Name);
        } else {
          scpm = md.Extension.get_CustomPropertyManager(string.Empty);
        }
        int res;

        if (this.Global)
          res = gcpm.Delete2(Name);
        else
          res = scpm.Delete2(Name);
      } else {
        throw new NullReferenceException("sw is null");
      }
    }

    /// <summary>
    /// Deletes the prop from the SW doc, assigning the SwApp object.
    /// </summary>
    /// <param name="sw"></param>
    public void Del(SldWorks sw) {
      if (sw != null) {
        SwApp = sw;
        ModelDoc2 md = (ModelDoc2)sw.ActiveDoc;
        Configuration cf = md.ConfigurationManager.ActiveConfiguration;

        CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
        CustomPropertyManager scpm;
        if (cf != null) {
          scpm = md.Extension.get_CustomPropertyManager(cf.Name);
        } else {
          scpm = md.Extension.get_CustomPropertyManager(string.Empty);
        }

        int res;

        if (Global)
          res = gcpm.Delete2(Name);
        else
          res = scpm.Delete2(Name);
      } else {
        throw new NullReferenceException("sw is null");
      }
    }

    /// <summary>
    /// Deletes the prop from the selected ModelDoc2 object.
    /// </summary>
    /// <param name="md">A ModelDoc2 object.</param>
    public void Del(ModelDoc2 md) {
      Configuration cf = md.ConfigurationManager.ActiveConfiguration;

      CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
      CustomPropertyManager scpm;
      if (cf != null) {
        scpm = md.Extension.get_CustomPropertyManager(cf.Name);
      } else {
        scpm = md.Extension.get_CustomPropertyManager(string.Empty);
      }

      int res;

      if (this.Global)
        res = gcpm.Delete2(Name);
      else
        res = scpm.Delete2(Name);
    }

    private string _id;

    public string ID {
      get { return _id; }
      set { _id = value; }
    }

    private string _descr;

    public string Descr {
      get { return _descr; }
      set { _descr = value; }
    }

    private string _propName;

    public string Name {
      get { return _propName; }
      set { _propName = value; }
    }

    private swCustomInfoType_e _propType;

    public swCustomInfoType_e Type {
      get { return _propType; }
      set { _propType = value; }
    }

    private string _value;

    public string Value {
      get { return _value; }
      set { _value = value; }
    }

    private string _resValue;

    public string ResValue {
      get { return _resValue; }
      set { _resValue = value; }
    }


    private string _field;

    public string Field {
      get { return _field; }
      set { _field = value; }
    }

    private string _table;

    public string Table {
      get { return _table; }
      set { _table = value; }
    }

    private System.Windows.Forms.Control _ctl;

    public System.Windows.Forms.Control Ctl {
      get { return _ctl; }
      set { _ctl = value; }
    }

    private bool _global;

    public bool Global {
      get { return _global; }
      set { _global = value; }
    }

    public override string ToString() {
      return string.Format("{0}: {1} => {2} | {3} - {4}\n", Name, Value, ResValue, ID, Descr);
    }

    public override bool Equals(object obj) {
      if (obj == null || !(obj is SwProperty))
        return false;

      return (Name == (obj as SwProperty).Name) && (Value == (obj as SwProperty).Value);
    }

    public override int GetHashCode() {
      return this.Name.GetHashCode() ^ this.Value.GetHashCode();
    }

    private SldWorks _swApp;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }

  }
}
