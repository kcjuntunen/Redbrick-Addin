using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RedbrickErr {
  public partial class ErrMsg : Form {
    private Redbrick_Addin.CutlistData cd;
    public ErrMsg() {
      InitializeComponent();
    }

    public ErrMsg(Exception e) {
      InitializeComponent();
      System.Media.SystemSounds.Exclamation.Play();
      tbxMsg.Text = ComposeMessage(e);
    }

    public ErrMsg(Exception e, Redbrick_Addin.CutlistData d) {
      InitializeComponent();
      System.Media.SystemSounds.Exclamation.Play();
      cd = d;
      tbxMsg.Text = ComposeMessage(e);
    }

    private string ComposeMessage(Exception e) {
      Text = String.Format("Error in {0}", e.TargetSite);
      StringBuilder msg = new StringBuilder();
      msg.AppendFormat("{0} caused an error: {1}\r\n\r\n in {2}", e.Source, e.Message, e.TargetSite);
      msg.Append("\r\n\r\n");
      msg.AppendFormat("Stack trace:\r\n{0}\r\n", e.StackTrace);

      if (cd != null) {
        cd.InsertError(e.HResult, e.Message, e.TargetSite.ToString());
      }

      if (e.Data.Count > 0) {
        msg.AppendFormat("\r\n\r\nData:\r\n");
        foreach (KeyValuePair<object, object> kp in e.Data)
          msg.AppendFormat("{0} => {1}", kp.Key.ToString(), kp.Value.ToString());
      }

      if (e.InnerException != null)
        msg.AppendFormat(GetInnerException(e.InnerException));

      return msg.ToString();
    }

    private string GetInnerException(Exception e) {
      StringBuilder msg = new StringBuilder();
      msg.AppendFormat("{0} caused an error: {1}\r\n\r\n in {2}", e.Source, e.Message, e.TargetSite);
      msg.Append("\r\n\r\n");
      msg.AppendFormat("Stack trace:\r\n{0}\r\n", e.StackTrace);

      if (e.Data.Count > 0) {
        msg.AppendFormat("\r\n\r\nData:\r\n");

        foreach (KeyValuePair<object, object> kp in e.Data)
          msg.AppendFormat("{0} => {1}", kp.Key.ToString(), kp.Value.ToString());
      }

      if (e.InnerException != null)
        msg.AppendFormat(GetInnerException(e.InnerException));

      return msg.ToString();
    }

    private void bSend_Click(object sender, EventArgs e) {
      //Clipboard.SetText(textBox1.Text);

      string sMailToLink = @"mailto:" + Redbrick_Addin.Properties.Settings.Default.Dev +
          "subject=" + Redbrick_Addin.Properties.Settings.Default.SubjectLine + "&body=" + tbxMsg.Text;
      System.Diagnostics.Process.Start(sMailToLink.Replace("\r\n", "%0A"));
      this.Close();
    }

    private void bClose_Click(object sender, EventArgs e) {
      this.Close();
    }
  }
}
