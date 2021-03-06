using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin {
  public class DrawingProperties : ICollection<SwProperty> {
    protected ArrayList _innerArray;

    public DrawingProperties(SldWorks sw) {
      this._swApp = sw;
      CutlistData = new CutlistData();
      this._innerArray = new ArrayList();
    }

    public void CreateDefaultSet() {
      this._innerArray.Add(new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoText, "$PRP:\"SW-File Name\"", true));
      this._innerArray.Add(new SwProperty("CUSTOMER", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("REVISION LEVEL", swCustomInfoType_e.swCustomInfoText, "100", true));
      this._innerArray.Add(new SwProperty("DrawnBy", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("DATE", swCustomInfoType_e.swCustomInfoText, DateTime.Now.ToShortDateString(), true));
      this._innerArray.Add(new SwProperty("M1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("FINISH 1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("M2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("FINISH 2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("M3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("FINISH 3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("M4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("FINISH 4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("M5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
      this._innerArray.Add(new SwProperty("FINISH 5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
    }

    public void UpdateProperty(SwProperty property) {
      foreach (SwProperty p in this._innerArray) {
        if (property.Name == p.Name) {
          p.Ctl = property.Ctl;
          p.Field = property.Field;
          p.Global = property.Global;
          p.ResValue = property.ResValue;
          p.Table = property.Table;
          p.Type = property.Type;
          p.Value = property.Value;
        }
      }
    }

    public SwProperty GetProperty(string name) {
      foreach (SwProperty p in this._innerArray) {
        if (name == p.Name) {
          return p;
        }
      }
      return null;
    }

    public void Read() {
      ModelDoc2 md = (ModelDoc2)this._swApp.ActiveDoc;
      CustomPropertyManager pm = md.Extension.get_CustomPropertyManager(string.Empty);
      int success = (int)swCustomInfoGetResult_e.swCustomInfoGetResult_ResolvedValue;
      int res;
      bool useCached = false;
      string valOut = string.Empty;
      string resValOut = string.Empty;
      bool wasResolved;


      res = pm.Get5("PartNo", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
        this.GetProperty("PartNo").ResValue = resValOut;
      }

      res = pm.Get5("CUSTOMER", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("CUSTOMER", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("REVISION LEVEL", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("REVISION LEVEL", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("DrawnBy", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("DrawnBy", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("DATE", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("DATE", swCustomInfoType_e.swCustomInfoDate, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("M1", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("M1", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }
      res = pm.Get5("FINISH 1", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("FINISH 1", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("M2", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("M2", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }
      res = pm.Get5("FINISH 2", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("FINISH 2", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("M3", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("M3", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }
      res = pm.Get5("FINISH 3", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("FINISH 3", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("M4", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("M4", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }
      res = pm.Get5("FINISH 4", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("FINISH 4", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }

      res = pm.Get5("M5", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("M5", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }
      res = pm.Get5("FINISH 5", useCached, out valOut, out resValOut, out wasResolved);
      if (res == success) {
        SwProperty x = new SwProperty("FINISH 5", swCustomInfoType_e.swCustomInfoText, valOut, true);
        x.SwApp = this._swApp;
        this._innerArray.Add(x);
      }
    }

    public void ClearProps() {
      ModelDoc2 md = (ModelDoc2)this._swApp.ActiveDoc;
      CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);
      string[] ss = (string[])glP.GetNames();

      if (ss != null) {
        foreach (string s in ss) {
          glP.Delete2(s);
        }
      }
    }

    public void ClearProps(ModelDoc2 md) {
      if (md.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
        CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);
        string[] ss = (string[])glP.GetNames();

        if (ss != null) {
          foreach (string s in ss) {
            glP.Delete2(s);
          }
        }
      }
    }

    public void Write() {
      ModelDoc2 md = (ModelDoc2)this._swApp.ActiveDoc;
      CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);

      this.ClearProps();
      foreach (SwProperty p in this._innerArray) {
        p.Write();
      }
    }

    public void Write(ModelDoc2 md) {
      CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);
      CutlistData.IncrementOdometer(CutlistData.Functions.GreenCheck);
      this.ClearProps(md);
      foreach (SwProperty p in this._innerArray) {
        p.Write(md);
      }
    }

    public void Write(SldWorks sw) {
      this._swApp = sw;
      ModelDoc2 md = (ModelDoc2)sw.ActiveDoc;
      CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);

      this.ClearProps();
      foreach (SwProperty p in this._innerArray) {
        p.Write(sw);
      }
    }

    public void UpdateFields() {
      foreach (SwProperty s in this._innerArray) {
        if (s.Ctl != null) {
          if (s.Ctl is System.Windows.Forms.ComboBox) {
            int si = 0;
            string ss = s.Value.Split(' ', '-')[0];
            if (s.Ctl.Name.Contains("cbM")) {
              ss = s.Value;
            }
            if (s.Ctl.Name.ToUpper().Contains("AUTHOR")) {
              si = (s.Ctl as System.Windows.Forms.ComboBox).FindString(CutlistData.GetAuthorFullName(ss));
            } else {
              si = (s.Ctl as System.Windows.Forms.ComboBox).FindString(ss);
            }

            if (si < 0 && !s.Ctl.Name.ToUpper().Contains("CUSTOMER")) {
              (s.Ctl as System.Windows.Forms.ComboBox).SelectedValue = ss;
            } else {
              (s.Ctl as System.Windows.Forms.ComboBox).SelectedIndex = si;
            }

            if ((s.Ctl as System.Windows.Forms.ComboBox).SelectedItem == null) {
              (s.Ctl as System.Windows.Forms.ComboBox).Text = ss;
            }
          } else if (s.Ctl is System.Windows.Forms.NumericUpDown) {
            decimal x = 0;
            if (decimal.TryParse(s.Value, out x)) {
              (s.Ctl as System.Windows.Forms.NumericUpDown).Value = x;
            } else {
              (s.Ctl as System.Windows.Forms.NumericUpDown).Value = 100;
            }
          } else {
            s.Ctl.Text = s.Value;
          }
        }
      }
    }

    public void ReadControls() {
      foreach (SwProperty s in this._innerArray) {
        if (s.Ctl != null) {
          if (s.Ctl is System.Windows.Forms.ComboBox) {
            if ((s.Ctl as System.Windows.Forms.ComboBox).SelectedValue != null) {
              if (s.Ctl.Name.Contains("cbCustomer")) {
                string[] cc = (s.Ctl as System.Windows.Forms.ComboBox).Text.Split('-', ' ');
                s.Value = cc[0] + " - " + cc[cc.Length - 1];
              } else {
                s.Value = (s.Ctl as System.Windows.Forms.ComboBox).SelectedValue.ToString().Substring(0, 2);
              }
            } else {
              string si;

              if ((s.Ctl as System.Windows.Forms.ComboBox).SelectedItem != null)
                si = (s.Ctl as System.Windows.Forms.ComboBox).SelectedItem.ToString();
              else
                si = string.Empty;

              // This will have to do, unless Chris wants to add a short customer name field.
              if (si.Length < 4 && !s.Ctl.Name.Contains("cbM") && !s.Ctl.Name.Contains("cbR")) {
                if ((s.Ctl as System.Windows.Forms.ComboBox).SelectedValue != null) {
                  s.Value = (s.Ctl as System.Windows.Forms.ComboBox).SelectedValue.ToString().Substring(0, 2);
                } else {
                  s.Value = (s.Ctl as System.Windows.Forms.ComboBox).Text;
                }
              } else if (si.Length > 12) {                                                            // Longer than 12? Must be customer codes.
                string[] cc = si.Split('-', ' ');
                s.Value = cc[0] + " - " + cc[cc.Length - 1];
              } else {                                                                                // Materials
                s.Value = (s.Ctl as System.Windows.Forms.ComboBox).Text;
              }
            }
          } else if (s.Ctl is System.Windows.Forms.DateTimePicker) {
            s.Value = (s.Ctl as System.Windows.Forms.DateTimePicker).Value.ToShortDateString();
          } else if (s.Ctl is System.Windows.Forms.NumericUpDown) {
            s.Value = (s.Ctl as System.Windows.Forms.NumericUpDown).Value.ToString();
          } else {
            s.Value = s.Ctl.Text;
          }
        }
      }
    }

    public CutlistData CutlistData { get; set; }

    private SldWorks _swApp;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }


    public override string ToString() {
      string ret = string.Empty;
      foreach (SwProperty p in this._innerArray) {
        ret += string.Format("{0}: {1}\n", p.Name, p.Value);
      }
      return ret;
    }

    #region ICollection<SwProperty> Members

    public void Add(SwProperty item) {
      this._innerArray.Add(item);
    }

    public void Clear() {
      this._innerArray.Clear();
    }

    public bool Contains(SwProperty item) {
      foreach (SwProperty p in this._innerArray) {
        if (item.Name == p.Name) {
          return true;
        }
      }
      return false;
    }

    public bool Contains(string name) {
      foreach (SwProperty p in this._innerArray) {
        if (name == p.Name) {
          return true;
        }
      }
      return false;
    }

    public void CopyTo(SwProperty[] array, int arrayIndex) {
      this._innerArray.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return this._innerArray.Count; }
    }

    public bool IsReadOnly {
      get { return this._innerArray.IsReadOnly; }
    }

    public bool Remove(SwProperty item) {
      int count = 0;
      foreach (SwProperty p in this._innerArray) {
        if (item.Name == p.Name) {
          this._innerArray.RemoveAt(count);
          return true;
        }
        count++;
      }
      return false;
    }

    public bool Remove(string name) {
      int count = 0;
      foreach (SwProperty p in this._innerArray) {
        if (name == p.Name) {
          this._innerArray.RemoveAt(count);
          return true;
        }
        count++;
      }
      return false;
    }

    #endregion

    #region IEnumerable<SwProperty> Members

    public IEnumerator<SwProperty> GetEnumerator() {
      return (new List<SwProperty>(this).GetEnumerator());
    }

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() {
      return this._innerArray.GetEnumerator();
    }

    #endregion
  }
}
