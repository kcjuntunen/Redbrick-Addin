using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  public partial class GeneralProperties : UserControl {
    SwProperties propertySet;
    private System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Settings.Default.ClipboardSound);

    public GeneralProperties(ref SwProperties prop) {
      propertySet = prop;
      InitializeComponent();
    }

    public void Update(ref SwProperties p) {
      propertySet = p;
      LinkControls();
      ToggleFields(propertySet.cutlistData.OpType);

      if (Properties.Settings.Default.MakeSounds) {
        try {
          sp.LoadAsync();
        } catch (Exception ex) {
          propertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
        }
      }

    }

    private void LinkControls() {
      tbLength.Text = string.Empty;
      tbWidth.Text = string.Empty;
      tbThick.Text = string.Empty;
      tbWallThick.Text = string.Empty;
      tbComment.Text = string.Empty;

      propertySet.LinkControlToProperty("DESCRIPTION", true, tbDescription);
      propertySet.LinkControlToProperty("LENGTH", true, tbLength);
      propertySet.LinkControlToProperty("WIDTH", true, tbWidth);
      propertySet.LinkControlToProperty("THICKNESS", true, tbThick);
      propertySet.LinkControlToProperty("WALL THICKNESS", true, tbWallThick);
      propertySet.LinkControlToProperty("COMMENT", true, tbComment);

      UpdateRes(propertySet.GetProperty("LENGTH"), labResLength);
      UpdateRes(propertySet.GetProperty("WIDTH"), labResWidth);
      UpdateRes(propertySet.GetProperty("THICKNESS"), labResThickness);

      if (propertySet.GetProperty("WALL THICKNESS") != null)
        UpdateRes(propertySet.GetProperty("WALL THICKNESS"), labResWallThickness);

      UpdateLnW();
    }

    private void UpdateRes(SwProperty p, Control c) {
      double tp = 0.0f;
      if (double.TryParse(p.ResValue, out tp)) {
        c.Text = string.Format("{0:0.000}", double.Parse(p.ResValue)); 
      }
    }

    private void LinkControlToProperty(string property, Control c) {
      SwProperty p = propertySet.GetProperty(property);
      if (propertySet.Contains(p)) {
        p.Ctl = c;
        c.Text = p.Value;
      } else {
        SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
        x.Ctl = c;
      }
    }

    private void UpdateLnW() {
      string tVal;
      double dVal;

      tVal = this.labResLength.Text;
      if (double.TryParse(tVal, out dVal))
        _length = dVal;
      else
        _length = 0.0;

      tVal = this.labResWidth.Text;
      if (double.TryParse(tVal, out dVal))
        _width = dVal;
      else
        _width = 0.0;
    }

    public void ToggleFields(int opType) {
      bool wood = (opType != 2);
      labResWallThickness.Enabled = !wood;
      lWallThickness.Enabled = !wood;
      tbWallThick.Enabled = !wood;
    }

    public TextBox GetDescriptionBox() {
      return tbDescription;
    }

    public TextBox GetLengthBox() {
      return tbLength;
    }

    public TextBox GetWidthBox() {
      return tbWidth;
    }

    public TextBox GetThicknessBox() {
      return tbThick;
    }

    public TextBox GetWallThicknessBox() {
      return tbWallThick;
    }

    public TextBox GetCommentBox() {
      return tbComment;
    }

    private void tbLength_Leave(object sender, EventArgs e) {
      propertySet.GetProperty("LENGTH").Value = tbLength.Text;
      propertySet.GetProperty("LENGTH").Write();
      propertySet.GetProperty("LENGTH").Get(propertySet.modeldoc, propertySet.cutlistData);
      double tp = 0.0;
      string resVal = propertySet.GetProperty("LENGTH").ResValue;
      labResLength.Text = double.TryParse(resVal, out tp) ? string.Format("{0:0.000}",  tp) : resVal;
    }

    private void tbWidth_Leave(object sender, EventArgs e) {
      propertySet.GetProperty("WIDTH").Value = tbWidth.Text;
      propertySet.GetProperty("WIDTH").Write();
      propertySet.GetProperty("WIDTH").Get(propertySet.modeldoc, propertySet.cutlistData);
      double tp = 0.0;
      string resVal = propertySet.GetProperty("WIDTH").ResValue;
      labResWidth.Text = double.TryParse(resVal, out tp) ? string.Format("{0:0.000}", tp) : resVal;
    }

    private void tbThick_Leave(object sender, EventArgs e) {
      propertySet.GetProperty("THICKNESS").Value = tbThick.Text;
      propertySet.GetProperty("THICKNESS").Write();
      propertySet.GetProperty("THICKNESS").Get(propertySet.modeldoc, propertySet.cutlistData);
      double tp = 0.0;
      string resVal = propertySet.GetProperty("THICKNESS").ResValue;
      labResThickness.Text = double.TryParse(resVal, out tp) ? string.Format("{0:0.000}", tp) : resVal;
    }

    private void tbWallThick_Leave(object sender, EventArgs e) {
      propertySet.GetProperty("WALL THICKNESS").Value = tbWallThick.Text;
      propertySet.GetProperty("WALL THICKNESS").Write();
      propertySet.GetProperty("WALL THICKNESS").Get(propertySet.modeldoc, propertySet.cutlistData);
      double tp = 0.0;
      string resVal = propertySet.GetProperty("WALL THICKNESS").ResValue;
      labResWallThickness.Text = double.TryParse(resVal, out tp) ? string.Format("{0:0.000}", tp) : resVal;
    }

    private double _length;

    public double PartLength {
      get { return _length; }
      set { _length = value; }
    }

    private double _width;

    public double PartWidth {
      get { return _width; }
      set { _width = value; }
    }

    private void labResLength_TextChanged(object sender, EventArgs e) {
      UpdateLnW();
    }

    private void labResWidth_TextChanged(object sender, EventArgs e) {
      UpdateLnW();
    }

    private void bCopy_Click(object sender, EventArgs e) {
      System.Windows.Forms.Clipboard.SetText(tbDescription.Text);
    }

    private void tbComment_TextChanged(object sender, EventArgs e) {
      CutlistData.FilterTextForControl(tbComment);
    }

    private void tbDescription_TextChanged(object sender, EventArgs e) {
      CutlistData.FilterTextForControl(tbDescription);
    }

    private void btnSwap_Click(object sender, EventArgs e) {
      string temp = tbWidth.Text;
      tbWidth.Text = tbLength.Text;
      tbLength.Text = temp;
      tbLength_Leave(sender, e);
      tbWidth_Leave(sender, e);
    }

    private void Clip(string to_clip) {
      if (to_clip != null && to_clip != string.Empty) {
        System.Windows.Forms.Clipboard.SetText(to_clip);

        if (Properties.Settings.Default.MakeSounds) {
          try {
            sp.PlaySync();
          } catch (Exception ex) {
            propertySet.SwApp.SendMsgToUser2(ex.Message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
          }
        }

      } else {
        //
      }
    }

    public string Length { 
      get { return labResLength.Text; }
      private set { labResLength.Text = value; }
    }

    public string Width {
      get { return labResWidth.Text; }
      private set { labResWidth.Text = value; }
    }

    public string Thickness {
      get { return labResThickness.Text; }
      private set { labResThickness.Text = value; }
    }

    public string WallThickness {
      get { return labResWallThickness.Text; }
      private set { labResWallThickness.Text = value; }
    }

  }
}
