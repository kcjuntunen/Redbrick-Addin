using System;
using Redbrick_Addin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Xml.Linq;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RedbrickTest {
  [TestClass]
  public class DataForm {
    M2MData m2m = new M2MData();
    CutlistData cd = new CutlistData();

    [TestMethod]
    public void M2MDataTable() {

      DataTable d = m2m.GetJobsDue();
      DataTable wc = cd.GetWCData();
      //DataTable x = cd.GetCutJobData();

      IEnumerable<DataRow> q1 = (from job in d.AsEnumerable()
                                 where job.Field<string>("fpartno").Contains("WC-122BA")
                                 select job);
      
      var q = from job in q1.AsEnumerable<DataRow>()
              join wkcen in wc.AsEnumerable() on job.Field<string>("fpro_id") equals wkcen.Field<string>("WC_ID")
              select new {
                JobNo = job["fjobno"],
                Status = job["fstatus"],
                PartNo = job["fpartno"],
                Rev = job["fpartrev"],
                Qty = job["foperqty"],
                DueDate = job["fddue_date"],
                WC = wkcen["WC_NAME"]
              };

      Redbrick_Addin.DataDisplay dd = new DataDisplay();
      dd.Left = 10;
      dd.Top = 10;
      dd.Width = 600;
      dd.Height = 300;
      dd.Grid.DataSource = ToDataTable(q.ToList());
      //dd.ShowDialog();
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

  }
}
