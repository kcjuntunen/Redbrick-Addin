using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class DataDisplay : Form {
    public DataDisplay() {
      InitializeComponent();
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
      dataGridView1.AutoResizeColumns();

      dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
    }

    private void DataDisplay_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.DataDisplayLocation = Location;
      Properties.Settings.Default.DataDisplaySize = Size;
      Properties.Settings.Default.Save();
    }

    public DataGridView Grid {
      get { return dataGridView1; }
      private set { dataGridView1 = value; }
    }
  }
}