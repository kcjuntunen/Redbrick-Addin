using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Redbrick_Addin {
  public partial class DepartmentSelector : UserControl {
    public delegate void DepartmentSelected(object d, EventArgs e);
    public event DepartmentSelected Selected;

    public DepartmentSelector(ref SwProperties p) {
      PropertySet = p;
      SwApp = p.SwApp;
      InitializeComponent();
      Init();
    }

    private void Init() {
      cbDepartment.ValueMember = "TYPEID";
      cbDepartment.DisplayMember = "TYPEDESC";
      cbDepartment.DataSource = this.PropertySet.cutlistData.OpTypes.Tables[0];
      cbDepartment.SelectedIndexChanged += cbDepartment_SelectedIndexChanged;

      Selected = new DepartmentSelected(OnSelected);
    }

    public void Update(ref SwProperties p) {
      PropertySet = p;
      LinkControlToProperty();
      PropertySet.cutlistData.OpType = OpType;
      cbDepartment.SelectedValue = OpType;
    }

    public void LinkControls() {
      PropertySet.GetProperty("DEPT").Type = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoNumber;
      PropertySet.GetProperty("DEPTARTMENT").Type = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText;
      PropertySet.LinkControlToProperty("DEPT", true, cbDepartment);
      PropertySet.LinkControlToProperty("DEPARTMENT", true, cbDepartment);
    }

    private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e) {
      if (Selected != null) {
        Selected(this, e);
      }
    }

    private void OnSelected(object sender, EventArgs e) {
      OpType = this.cbDepartment.SelectedIndex + 1;
      PropertySet.cutlistData.OpType = this.OpType;
      PropertySet.GetProperty("DEPARTMENT").Value = this.OpType.ToString();
      PropertySet.GetProperty("DEPARTMENT").ResValue = this.cbDepartment.Text;
      int idx = this.OpType - 1; // Don't sort the table, and this works well.
      cbDepartment.SelectedIndex = idx;
      cbDepartment.DisplayMember = "TYPEDESC";
    }

    private void LinkControlToProperty() {
      string pn = "DEPARTMENT";
      if (!Properties.Settings.Default.Testing)
        pn = "DEPTID";

      string dept;
      if (PropertySet.Contains(pn)) {
        PropertySet.GetProperty(pn).Ctl = cbDepartment;
        dept = PropertySet.GetProperty(pn).Value;
        int tp = 1;

        if (int.TryParse(dept, out tp)) {
          OpType = tp;
        } else {
          OpType = PropertySet.cutlistData.GetOpTypeIDByName(dept);
        }
        dept = tp.ToString();
      } else {
        SolidWorks.Interop.swconst.swCustomInfoType_e t = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoNumber;
        SwProperty p = new SwProperty(pn, t, "1", true);
        p.SwApp = SwApp;
        p.Ctl = cbDepartment;
        PropertySet.Add(p);
        OpType = 1;
      }
    }

    public SwProperties PropertySet { get; set; }
    public int OpType { get; set; }
    public SolidWorks.Interop.sldworks.SldWorks SwApp { get; set; }
  }
}
