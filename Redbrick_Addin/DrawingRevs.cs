using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin {
  public class DrawingRevs : ICollection<DrawingRev> {
    protected ArrayList _innerArray;

    public DrawingRevs(SldWorks sw) {
      this.SwApp = sw;
      this._innerArray = new ArrayList(15);
    }

    public override string ToString() {
      string ret = string.Empty;
      if (this._innerArray != null) {
        foreach (DrawingRev p in this._innerArray) {
          ret += p.ToString();
        }
      }
      return ret;
    }

    public void Read() {
      ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
      CustomPropertyManager pm = md.Extension.get_CustomPropertyManager(string.Empty);

      int revLetterOffset = 64;
      int revLimit = Properties.Settings.Default.RevLimit;

      for (int i = 1; i <= revLimit; i++) {
        string r = "REVISION " + (char)(i + revLetterOffset);
        string e = "ECO " + i.ToString();
        string de = "DESCRIPTION " + i.ToString();
        string l = "LIST " + i.ToString();
        string da = "DATE " + i.ToString();
        //System.Diagnostics.Debug.Print(string.Format("{0}: {1}: {2}: {3}", r, e, de, l));

        SwProperty rp = this.AssignProperty(pm, r);
        if (rp.Value == "NULL" || rp.Value == string.Empty)
          break;

        SwProperty ep = this.AssignProperty(pm, e);
        SwProperty dep = this.AssignProperty(pm, de);
        SwProperty lp = this.AssignProperty(pm, l);
        SwProperty dap = this.AssignProperty(pm, da);

        rp.SwApp = this.SwApp;
        ep.SwApp = this.SwApp;
        dep.SwApp = this.SwApp;
        lp.SwApp = this.SwApp;
        dap.SwApp = this.SwApp;

        if ((rp.Value != "NULL")) {
          DrawingRev dr = new DrawingRev(rp, ep, dep, lp, dap);
          if (dr != null) {
            this._innerArray.Add(dr);
          }
        } else
          break;
      }
    }

    public void ClearProps() {
      ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
      CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);
      int res;

      for (int i = 1; i <= Properties.Settings.Default.RevLimit; i++) {
        res = glP.Delete2("REVISION " + (char)(i + 65));
        res = glP.Delete2("ECO " + i.ToString());
        res = glP.Delete2("DESCRIPTION " + i.ToString());
        res = glP.Delete2("LIST " + i.ToString());
        res = glP.Delete2("DATE " + i.ToString());
      }
    }

    public void ClearProps(ModelDoc2 md) {
      CustomPropertyManager glP = md.Extension.get_CustomPropertyManager(string.Empty);
      int res;

      for (int i = 1; i <= Properties.Settings.Default.RevLimit; i++) {
        res = glP.Delete2("REVISION " + (char)(i + 65));
        res = glP.Delete2("ECO " + i.ToString());
        res = glP.Delete2("DESCRIPTION " + i.ToString());
        res = glP.Delete2("LIST " + i.ToString());
        res = glP.Delete2("DATE " + i.ToString());
      }
    }

    public void Write() {
      this.ClearProps();

      foreach (DrawingRev dr in this._innerArray) {
        dr.Write();
      }
    }

    public void Write(ModelDoc2 md) {
      this.ClearProps(md);

      foreach (DrawingRev dr in this._innerArray) {
        dr.Write(md);
      }
    }

    public void Write(SldWorks sw) {
      this.ClearProps();

      foreach (DrawingRev dr in this._innerArray) {
        dr.Write(sw);
      }
    }

    public void ReadControls() {
      DataTable dt = (DataTable)this.listBox.DataSource;

      this._innerArray.Clear();
      int count = 1;

      foreach (System.Windows.Forms.DataGridViewRow dr in this.listBox.Rows) {
        if (dr.Cells[0].Value != null && dr.Cells[2].Value != null) {
          SwProperty rev = new SwProperty("REVISION " + (char)(count + 64), swCustomInfoType_e.swCustomInfoText, dr.Cells[0].Value.ToString(), true);
          SwProperty eco = new SwProperty("ECO " + count.ToString(), swCustomInfoType_e.swCustomInfoText, dr.Cells[1].Value.ToString(), true);
          SwProperty des = new SwProperty("DESCRIPTION " + count.ToString(), swCustomInfoType_e.swCustomInfoText, dr.Cells[2].Value.ToString(), true);
          SwProperty lis = new SwProperty("LIST " + count.ToString(), swCustomInfoType_e.swCustomInfoText, dr.Cells[3].Value.ToString(), true);
          SwProperty dat = new SwProperty("DATE " + count.ToString(), swCustomInfoType_e.swCustomInfoDate, dr.Cells[4].Value.ToString(), true);

          DrawingRev r = new DrawingRev(rev, eco, des, lis, dat);
          r.Del(this.SwApp);

          this._innerArray.Add(r);

          count++;
        }
      }
    }

    public void DelAll() {
      foreach (DrawingRev dr in this._innerArray) {
        dr.Del();
      }
    }

    public void DelAll(SldWorks sw) {
      foreach (DrawingRev dr in this._innerArray) {
        dr.Del(sw);
      }
    }

    private SwProperty AssignProperty(CustomPropertyManager pm, string name) {
      int res;
      int success = (int)swCustomInfoGetResult_e.swCustomInfoGetResult_ResolvedValue;
      bool useCached = false;
      string valOut = string.Empty;
      string resValOut = string.Empty;
      bool wasResolved;

      SwProperty rp = new SwProperty();
      rp.SwApp = this.SwApp;

      if (!InThere(pm, name)) {
        return rp;
      } else {
      }

      res = pm.Get5(name, useCached, out valOut, out resValOut, out wasResolved);

      if (res == success) {
        System.Diagnostics.Debug.WriteLine(string.Format("Found {0}.", name));
        rp.Name = name;
        rp.Value = valOut;
        rp.ResValue = resValOut;
        rp.Type = swCustomInfoType_e.swCustomInfoText;
        rp.Global = true;
      }
      return rp;
    }

    private bool InThere(CustomPropertyManager c, string name) {
      string[] ss = (string[])c.GetNames();
      if (ss != null) {
        foreach (string p in ss) {
          if (p == name) {
            return true;
          }
        }
      }
      return false;
    }

    #region ICollection<SwProperty> Members

    public void Add(DrawingRev item) {
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

    public bool Contains(DrawingRev rev) {
      foreach (DrawingRev r in this._innerArray) {
        if (r.Revision.Name == rev.Revision.Name) {
          return true;
        }
      }
      return false;
    }

    public bool Contains(string name) {
      foreach (DrawingRev r in this._innerArray) {
        if (name == r.Revision.Name) {
          return true;
        }
      }
      return false;
    }

    public void CopyTo(DrawingRev[] array, int arrayIndex) {
      this._innerArray.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return this._innerArray.Count; }
    }

    public bool IsReadOnly {
      get { return this._innerArray.IsReadOnly; }
    }

    public bool Remove(DrawingRev rev) {
      int count = 0;
      foreach (DrawingRev r in this._innerArray) {
        if (r.Revision.Name == rev.Revision.Name) {
          this._innerArray.RemoveAt(count);
          return true;
        }
        count++;
      }
      return false;
    }

    public bool Remove(string name) {
      int count = 0;
      foreach (DrawingRev p in this._innerArray) {
        if (name == p.Revision.Name) {
          System.Diagnostics.Debug.Print("=>" + p.Revision.Name);
          this._innerArray.RemoveAt(count);
          return true;
        } else
          System.Diagnostics.Debug.Print(name + ": " + p.Revision.Name);

        count++;
      }
      return false;
    }

    public DrawingRev GetRev(string name) {
      foreach (DrawingRev r in this._innerArray) {
        if (r.Revision.Name == name) {
          return r;
        }
      }
      return null;
    }

    #endregion

    #region IEnumerable<DrawingRev> Members

    public IEnumerator<DrawingRev> GetEnumerator() {
      return (new List<DrawingRev>(this).GetEnumerator());
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return this._innerArray.GetEnumerator();
    }

    #endregion



    private System.Windows.Forms.DataGridView _lb;

    public System.Windows.Forms.DataGridView listBox {
      get { return _lb; }
      set { _lb = value; }
    }

    private System.Windows.Forms.TreeView _tv;

    public System.Windows.Forms.TreeView treeView {
      get { return _tv; }
      set { _tv = value; }
    }

    private SldWorks _swApp;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }

  }
}
