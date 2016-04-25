using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class DataDisplay : Form {
    private string part = string.Empty;
    public DataDisplay() {
      InitializeComponent();
      Grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
    }

    public DataDisplay(List<object> items) {
      InitializeComponent();
      Grid.DataSource = ToDataTable(items);
    }

    public DataDisplay(string search) {
      InitializeComponent();
      Text = search;
    }

    public DataDisplay(DataTable dt, string search) {
      InitializeComponent();
      Text = string.Format(@"Where {0} is used...", search);
      dataGridView1.DataSource = dt;
    }

    public static DataTable ToDataTable<T>(List<T> items) {
      var tb = new DataTable(typeof(T).Name);
      System.Reflection.PropertyInfo[] props = typeof(T).GetProperties(
        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
      try {
        foreach (var prop in props) {
          tb.Columns.Add(prop.Name, prop.PropertyType);
        }

        foreach (var item in items) {
          var values = new object[props.Length];
          for (var i = 0; i < props.Length; i++) {
            values[i] = props[i].GetValue(item, null);
          }
          tb.Rows.Add(values);
        }
        return tb;
      } catch (Exception ex) {
        return null;
      }
    }

    private void DataDisplay_Load(object sender, EventArgs e) {
      Location = Properties.Settings.Default.DataDisplayLocation;
      Size = Properties.Settings.Default.DataDisplaySize;
      dataGridView1.AutoResizeRows();
      dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      dataGridView1.AutoResizeColumns();
      dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
    }

    private void DataDisplay_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.DataDisplayLocation = Location;
      Properties.Settings.Default.DataDisplaySize = Size;
      Properties.Settings.Default.Save();
    }

    public List<System.IO.FileInfo> PathIndex { get; set; }

    public SolidWorks.Interop.sldworks.SldWorks swApp { get; set; }

    public DataGridView Grid {
      get { return dataGridView1; }
      private set { dataGridView1 = value; }
    }

    public string GetPath() {
      return System.IO.Path.GetDirectoryName(
        (swApp.ActiveDoc as SolidWorks.Interop.sldworks.ModelDoc2).
        GetPathName());
    }

    private System.IO.FileInfo find_doc(string doc) {
      if (PathIndex != null) {
        foreach (System.IO.FileInfo fi in PathIndex) {
          if (fi.Name.ToUpper().Contains(doc.ToUpper()))
            return fi;
        }
      }

      System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(GetPath());
      System.IO.FileInfo[] fl = d.GetFiles();

      try {
        foreach (System.IO.FileInfo fi in fl) {
          if (fi.Name.ToUpper().Contains(doc.ToUpper())) {
            return fi;
          }
        }
      } catch (Exception ex) {
        swApp.SendMsgToUser2(string.Format("Couldn't find '{0}' in '{1}'.", doc, d.FullName),
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      }

      try {
        d = new System.IO.DirectoryInfo(GetPath() + @"\" + doc.Substring(0, 4));
        fl = d.GetFiles();

        foreach (System.IO.FileInfo fi in fl) {
          if (fi.Name.ToUpper().Contains(doc.ToUpper())) {
            return fi;
          }
        }
      } catch (Exception ex) {
        swApp.SendMsgToUser2(string.Format("Couldn't find '{0}' in '{1}'.", doc, d.FullName),
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      }

      return new System.IO.FileInfo((swApp.ActiveDoc as SolidWorks.Interop.sldworks.ModelDoc2).GetTitle());
    }

    private void OnClickOpenModel(System.Object sender, EventArgs e) {
      try {
        int err = 0;
        string t = find_doc(part).FullName;
        if (System.IO.File.Exists(t)) {
          swApp.ActivateDoc3(
            t,
            true,
            (int)SolidWorks.Interop.swconst.swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
          Close();
        } else {
          swApp.SendMsgToUser2(string.Format("Couldn't find '{0}'", t),
            (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
            (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
        }
      } catch (NullReferenceException nex) {
        swApp.SendMsgToUser2("You must select a row with something in it.\n" + nex.Message,
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      } catch (Exception ex) {
        swApp.SendMsgToUser2(ex.Message,
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      } finally {
        //
      }
    }

    private void OnClickOpenDrawing(System.Object sender, EventArgs e) {
      try {
        System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(GetPath());
        System.IO.FileInfo fi = find_doc(part);
        string t = fi.FullName.ToUpper();
        string ext = System.IO.Path.GetExtension(t).ToUpper();
        string fullpath = t.Replace(ext, ".SLDDRW");
        if (System.IO.File.Exists(fullpath)) {
          swApp.OpenDocSilent(fullpath,
            (int)SolidWorks.Interop.swconst.swDocumentTypes_e.swDocDRAWING,
            (int)SolidWorks.Interop.swconst.swOpenDocOptions_e.swOpenDocOptions_Silent);
          Close();
        } else {
          swApp.SendMsgToUser2(string.Format("Couldn't find '{0}'", fullpath),
            (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
            (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
        }
      } catch (NullReferenceException nex) {
        swApp.SendMsgToUser2("You must select a row with something in it.\n" + nex.Message,
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      } catch (Exception ex) {
        swApp.SendMsgToUser2(ex.Message,
          (int)SolidWorks.Interop.swconst.swMessageBoxIcon_e.swMbStop,
          (int)SolidWorks.Interop.swconst.swMessageBoxBtn_e.swMbOk);
      } finally {
        //
      }
    }

    private void DataDisplay_MouseClick(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Right && swApp != null) {
        try {
          ContextMenu m = new ContextMenu();
          int current_row = Grid.HitTest(e.X, e.Y).RowIndex;
          if (current_row >= 0) {
            part = Grid["Part", current_row].Value.ToString();
            m.MenuItems.Add(new MenuItem("Open Model...", OnClickOpenModel));
            m.MenuItems.Add(new MenuItem("Open Drawing...", OnClickOpenDrawing));
            //m.MenuItems.Add(new MenuItem(string.Format("Action specific to {0}...", Grid["Part", current_row].Value)));
          }

          m.Show(Grid, new Point(e.X, e.Y));
        } catch (Exception ex) {
          //
        }
      }
    }
  }
}
