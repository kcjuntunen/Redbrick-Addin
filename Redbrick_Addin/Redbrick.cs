using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
//using SolidWorksTools;

using System.Runtime.InteropServices;

namespace Redbrick_Addin {
  public class Redbrick : ISwAddin {
    public SldWorks swApp;
    private int cookie;
    private TaskpaneView taskpaneView;
    private SWTaskPaneHost taskpaneHost;
    private KeyValuePair<Version, Uri> publicVersion;
    private Version currentVersion;
    private string UpdateMessage = string.Empty;

    public bool ConnectToSW(object ThisSW, int Cookie) {
      swApp = (SldWorks)ThisSW;
      cookie = Cookie;

      bool res = swApp.SetAddinCallbackInfo(0, this, cookie);
      if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
        UISetup();
        return true;
      } else {
        return false;
      }
    }

    public bool DisconnectFromSW() {
      this.UITearDown();
      return true;
    }

    private void UISetup() {
      try {
        taskpaneView = swApp.CreateTaskpaneView2(Properties.Settings.Default.NetPath +
            Properties.Settings.Default.Icon,
            Properties.Resources.Title);

        taskpaneHost = (SWTaskPaneHost)taskpaneView.AddControl(SWTaskPaneHost.SWTASKPANE_PROGID, string.Empty);
        taskpaneHost.OnRequestSW += new Func<SldWorks>(delegate { return this.swApp; });

        bool result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK");
        result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Options, "Configuration");
        //result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close");
        result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.RefreshIcon, "Refresh");
        result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.ArchiveIcon, "Archive PDF");
        result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.HelpIcon, "Usage Help");

        taskpaneView.TaskPaneToolbarButtonClicked += taskpaneView_TaskPaneToolbarButtonClicked;
        CheckUpdate();
        taskpaneHost.cookie = cookie;
        taskpaneHost.Start();
      } catch (Exception e) {
        RedbrickErr.ErrMsg em = new RedbrickErr.ErrMsg(e);
        em.ShowDialog();
      }
    }

    int taskpaneView_TaskPaneToolbarButtonClicked(int ButtonIndex) {
      switch (ButtonIndex) {
        case 0:
          taskpaneHost.Write();
          if (Properties.Settings.Default.MakeSounds)
            System.Media.SystemSounds.Beep.Play();
          break;
        case 1:
          RedbrickConfiguration rbc = new RedbrickConfiguration();
          rbc.ShowDialog();
          taskpaneHost.ConnectSelection();
          break;
        case 2:
          taskpaneHost.ReStart();
          break;
        case 3:
          CutlistData cd = new CutlistData();
          cd.IncrementOdometer(CutlistData.Functions.ArchivePDF);
          cd.Dispose();
          cd = null;
          ArchivePDF.csproj.ArchivePDFWrapper apw = new ArchivePDF.csproj.ArchivePDFWrapper(swApp);
          apw.Archive();
          break;
        case 4:
          System.Diagnostics.Process.Start(Properties.Settings.Default.UsageLink);
          break;
        default:
          break;
      }
      return 1;
    }

    private void UITearDown() {
      taskpaneHost = null;
      taskpaneView.DeleteView();
      Marshal.ReleaseComObject(taskpaneView);
      taskpaneView = null;
    }

    public void CopyInstaller() {
      System.IO.FileInfo nfi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
      nfi.CopyTo(Properties.Settings.Default.EngineeringDir + @"\InstallRedBrick.exe", true);
    }

    private void GetPublicData(string t) {
      System.IO.FileInfo pi = new System.IO.FileInfo(t);
      Version v = new Version();
      Uri u = new Uri(pi.FullName);
      string m = string.Empty;
      string elementName = string.Empty;

      if (pi.Exists) {
        using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(pi.FullName)) {
          while (r.Read()) {
            if (r.NodeType == System.Xml.XmlNodeType.Element) {
              elementName = r.Name;
            } else {
              if (r.NodeType == System.Xml.XmlNodeType.Text && r.HasValue) {
                switch (elementName) {
                  case "version":
                    v = new Version(r.Value);
                    break;
                  case "url":
                    u = new Uri(r.Value);
                    break;
                  case "message":
                    m = r.Value;
                    break;
                  default:
                    break;
                }
              }
            }
          }
        }
      }
      publicVersion = new KeyValuePair<Version, Uri>(v, u);
      UpdateMessage = m;
    }

    public bool Old() {
      System.IO.FileInfo pi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
      if (true) {
        GetPublicData(pi.DirectoryName + @"\version.xml");
        currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        if (currentVersion.CompareTo(publicVersion.Key) < 0)
          return true;
      }

      return false;
    }

    public void Update() {
      string chge = string.Format(Properties.Resources.Update, currentVersion.ToString(), publicVersion.Key.ToString());
      swMessageBoxResult_e res = (swMessageBoxResult_e)swApp.SendMsgToUser2(
        string.Format("{0}\n\nCHANGE: {1}", chge, UpdateMessage),
        (int)swMessageBoxIcon_e.swMbQuestion,
        (int)swMessageBoxBtn_e.swMbYesNo);

      switch (res) {
        case swMessageBoxResult_e.swMbHitAbort:
          break;
        case swMessageBoxResult_e.swMbHitCancel:
          break;
        case swMessageBoxResult_e.swMbHitIgnore:
          break;
        case swMessageBoxResult_e.swMbHitNo:
          break;
        case swMessageBoxResult_e.swMbHitOk:
          break;
        case swMessageBoxResult_e.swMbHitRetry:
          break;
        case swMessageBoxResult_e.swMbHitYes:
          swApp.DestroyNotify += swApp_DestroyNotify;
          swApp.SendMsgToUser2(Properties.Resources.Restart,
            (int)swMessageBoxIcon_e.swMbWarning,
            (int)swMessageBoxBtn_e.swMbOk);
          break;
        default:
          break;
      }
    }

    private int swApp_DestroyNotify() {
      System.Diagnostics.Process.Start(publicVersion.Value.ToString());
      return 0;
    }

    public void CheckUpdate() {
      if (Old()) {
        Update();
      }
    }

    private void DoUpdate() {
      System.Diagnostics.Process p = new System.Diagnostics.Process();
      p.StartInfo.FileName = Properties.Settings.Default.EngineeringDir + @"\InstallRedBrick.exe";
      p.Start();
    }

    static public int GetHash(string fullPath) {

      DamienG.Security.Cryptography.Crc32 crc = new DamienG.Security.Cryptography.Crc32();

      byte[] b = new byte[fullPath.Length];
      string hash = string.Empty;

      for (int i = 0; i < fullPath.Length; i++)
        b[i] = (byte)fullPath[i];

      foreach (byte byt in crc.ComputeHash(b))
        hash += byt.ToString("x2").ToLower();

      try {
        return int.Parse(hash, System.Globalization.NumberStyles.HexNumber);
      } catch (Exception) {
        return 0;
      }
    }

    static public void Clip(string to_clip) {
      if ((to_clip != null && to_clip != string.Empty) && to_clip != System.Windows.Forms.Clipboard.GetText()) {
        System.Windows.Forms.Clipboard.SetText(to_clip.Replace("*", ""));
        if (Properties.Settings.Default.MakeSounds) {
          try {
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Settings.Default.ClipboardSound);
            sp.PlaySync();
          } catch (Exception ex) {
            RedbrickErr.ErrMsg em = new RedbrickErr.ErrMsg(ex);
            em.ShowDialog();
          }
        }
      } else {
        if (Properties.Settings.Default.MakeSounds) {
          System.Media.SystemSounds.Asterisk.Play();
        }
      }
    }


    static public void unselect(System.Windows.Forms.Control.ControlCollection controls) {
      foreach (System.Windows.Forms.Control c in controls) {
        if (c is System.Windows.Forms.ComboBox) {
          (c as System.Windows.Forms.ComboBox).SelectionLength = 0;
        }
      }
    }

    [ComRegisterFunction()]
    private static void ComRegister(Type t) {
      Properties.Settings.Default.Upgrade();
      string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

      using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath)) {
        rk.SetValue(null, 1); // Load at startup
        rk.SetValue("Title", "Redbrick");
        rk.SetValue("Description", "Change properties the Amstore way.");
      }
    }

    [ComUnregisterFunction()]
    private static void ComUnregister(Type t) {
      string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
      Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
    }
  }
}
