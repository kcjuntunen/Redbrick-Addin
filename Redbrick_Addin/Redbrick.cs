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
    private bool askedToUpdate = false;
    private string UpdateMessage = string.Empty;

    public bool ConnectToSW(object ThisSW, int Cookie) {
      swApp = (SldWorks)ThisSW;
      cookie = Cookie;

      bool res = swApp.SetAddinCallbackInfo(0, this, cookie);
      if (CheckNetwork()) {
        UISetup();
        return true;
      } else {
        swApp.SendMsgToUser2(Properties.Resources.NetworkNotAvailable,
          (int)swMessageBoxIcon_e.swMbWarning,
          (int)swMessageBoxBtn_e.swMbOk);
        return false;
      }
    }

    public static bool CheckNetwork() {
      bool res = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
      System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
      System.Net.NetworkInformation.PingReply pr = null;
      string host = Properties.Settings.Default.NetPath.ToString().Split('\\')[2];
      try {
        pr = p.Send(host, 1000);

        switch (pr.Status) {
          case System.Net.NetworkInformation.IPStatus.Success:
            res &= true;
            break;
          case System.Net.NetworkInformation.IPStatus.TimedOut:
            res &= false;
            break;
          case System.Net.NetworkInformation.IPStatus.DestinationHostUnreachable:
            res &= false;
            break;
          case System.Net.NetworkInformation.IPStatus.Unknown:
            res &= false;
            break;
          default:
            res &= false;
            break;
        }

      } catch (Exception) {
        res &= false;
      }

      return res;
    }

    public bool DisconnectFromSW() {
      CheckUpdate();
      this.UITearDown();
      return true;
    }

    private void UISetup() {
      try {
        Version cv = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        string ver = cv.ToString();

        taskpaneView = swApp.CreateTaskpaneView2(Properties.Settings.Default.NetPath +
            Properties.Settings.Default.Icon,
            string.Format(Properties.Resources.Title, ver));

        taskpaneHost = (SWTaskPaneHost)taskpaneView.AddControl(SWTaskPaneHost.SWTASKPANE_PROGID, string.Empty);
        taskpaneHost.OnRequestSW += new Func<SldWorks>(delegate { return this.swApp; });

        bool result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK");
        result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Options, "Configuration");
        //result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close");
        result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.RefreshIcon, "Refresh");
        result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.ArchiveIcon, "Archive PDF");
        result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.HelpIcon, "Usage Help");

        taskpaneView.TaskPaneToolbarButtonClicked += taskpaneView_TaskPaneToolbarButtonClicked;
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
          ArchivePDF.csproj.ArchivePDFWrapper apw = new ArchivePDF.csproj.ArchivePDFWrapper(swApp, GeneratePathSet());
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

    /// <summary>
    /// Copy the installer.
    /// </summary>
    public void CopyInstaller() {
      System.IO.FileInfo nfi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
      nfi.CopyTo(Properties.Settings.Default.EngineeringDir + @"\InstallRedBrick.exe", true);
    }

    public static DateTime GetOdometerStart(string t) {
      DateTime dt = Properties.Settings.Default.OdometerStart;
      System.IO.FileInfo pi = new System.IO.FileInfo(t);
      string elementName = string.Empty;
      using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(pi.FullName)) {
        while (r.Read())
        {
          if (r.NodeType == System.Xml.XmlNodeType.Element) {
            elementName = r.Name;
          } else if (r.NodeType == System.Xml.XmlNodeType.Text && r.HasValue && elementName == @"OdometerStart") {
              DateTime.TryParse(r.Value, out dt);
          }
        }
      }
      return dt;
    }

    /// <summary>
    /// Get data from the version XML file that goes with the installer.
    /// </summary>
    /// <param name="t">Full path of target XML file.</param>
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

    /// <summary>
    /// Determine whether we're using a version older than the one on the network.
    /// </summary>
    /// <returns>A bool.</returns>
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

    /// <summary>
    /// Ask user if update is wanted.
    /// </summary>
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
          //swApp.DestroyNotify += swApp_DestroyNotify;
          swApp_DestroyNotify();
          //swApp.SendMsgToUser2(Properties.Resources.Restart,
          //  (int)swMessageBoxIcon_e.swMbWarning,
          //  (int)swMessageBoxBtn_e.swMbOk);
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// Fire install process.
    /// </summary>
    /// <returns>0</returns>
    private int swApp_DestroyNotify() {
      System.Diagnostics.Process.Start(publicVersion.Value.ToString());
      return 0;
    }

    /// <summary>
    /// Update if old.
    /// </summary>
    public void CheckUpdate() {
      if (Old() && !askedToUpdate) {
        Update();
        askedToUpdate = true;
      }
    }

    /// <summary>
    /// Execute update.
    /// </summary>
    private void DoUpdate() {
      System.Diagnostics.Process p = new System.Diagnostics.Process();
      p.StartInfo.FileName = Properties.Settings.Default.EngineeringDir + @"\InstallRedBrick.exe";
      p.Start();
    }

    /// <summary>
    /// Hash a string.
    /// </summary>
    /// <param name="fullPath">Full path of a part.</param>
    /// <returns>A 32-bit int.</returns>
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

    /// <summary>
    /// Copy to_clip to clipboard.
    /// </summary>
    /// <param name="to_clip">A string.</param>
    static public void Clip(string to_clip) {
      if ((to_clip != null && to_clip != string.Empty) && to_clip != System.Windows.Forms.Clipboard.GetText()) {
        System.Windows.Forms.Clipboard.SetText(to_clip.Replace(Properties.Settings.Default.NotSavedMark, string.Empty));
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

    /// <summary>
    /// De-blue comboboxes.
    /// </summary>
    /// <param name="controls">A ControlCollection object.</param>
    static public void unselect(System.Windows.Forms.Control.ControlCollection controls) {
      foreach (System.Windows.Forms.Control c in controls) {
        if (c is System.Windows.Forms.ComboBox) {
          (c as System.Windows.Forms.ComboBox).SelectionLength = 0;
        }
      }
    }

    static public ArchivePDF.csproj.PathSet GeneratePathSet() {
      ArchivePDF.csproj.PathSet ps = new ArchivePDF.csproj.PathSet();
      ps.GaugePath = Properties.Settings.Default.GaugePath;
      ps.ShtFmtPath = Properties.Settings.Default.ShtFmtPath;
      ps.JPGPath = Properties.Settings.Default.JPGPath;
      ps.KPath = Properties.Settings.Default.KPath;
      ps.GPath = Properties.Settings.Default.GPath;
      ps.MetalPath = Properties.Settings.Default.MetalPath;
      ps.SaveFirst = Properties.Settings.Default.SaveFirst;
      ps.SilenceGaugeErrors = Properties.Settings.Default.SilenceGaugeErrors;
      ps.ExportPDF = Properties.Settings.Default.ExportPDF;
      ps.ExportEDrw = Properties.Settings.Default.ExportEDrw;
      ps.ExportImg = Properties.Settings.Default.ExportImg;
      ps.WriteToDb = Properties.Settings.Default.EnableDBWrite;
      ps.Initialated = true;
      return ps;
    }

    public static void InsertBOM(SldWorks swApp) {
      ModelDoc2 md = (ModelDoc2)swApp.ActiveDoc;
      DrawingDoc dd = (DrawingDoc)swApp.ActiveDoc;
      ModelDocExtension ex = (ModelDocExtension)md.Extension;
      int bom_type = (int)swBomType_e.swBomType_PartsOnly;
      int bom_numbering = (int)swNumberingType_e.swNumberingType_Flat;
      int bom_anchor = (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopLeft;
      SolidWorks.Interop.sldworks.View v = GetFirstView(swApp);

      if (dd.ActivateView(v.Name)) {
        v.InsertBomTable4(
          false,
          Properties.Settings.Default.BOMLocationX, Properties.Settings.Default.BOMLocationY,
          bom_anchor,
          bom_type,
          v.ReferencedConfiguration,
          Properties.Settings.Default.BOMTemplatePath,
          false,
          bom_numbering,
          false);
      }
    }

    /// </summary>
    /// <param name="sw">Active SldWorks object.</param>
    /// <returns>A View object.</returns>
    public static SolidWorks.Interop.sldworks.View GetFirstView(SldWorks sw) {
      ModelDoc2 swModel = (ModelDoc2)sw.ActiveDoc;
      SolidWorks.Interop.sldworks.View v;
      DrawingDoc d = (DrawingDoc)swModel;
      string[] shtNames = (String[])d.GetSheetNames();
      string message = string.Empty;

      //This should find the first page with something on it.
      int x = 0;
      do {
        try {
          d.ActivateSheet(shtNames[x]);
        } catch (IndexOutOfRangeException e) {
          throw new IndexOutOfRangeException("Went beyond the number of sheets.", e);
        } catch (Exception e) {
          throw e;
        }
        v = (SolidWorks.Interop.sldworks.View)d.GetFirstView();
        v = (SolidWorks.Interop.sldworks.View)v.GetNextView();
        x++;
      } while ((v == null) && (x < d.GetSheetCount()));

      message = (string)v.GetName2() + ":\n";

      if (v == null) {
        throw new Exception("I couldn't find a model anywhere in this document.");
      }
      return v;
    }

    /// <summary>
    /// Convert a specialized StringCollection to an array of strings.
    /// </summary>
    static public string[] BOMFilter {
      get {
        string[] regex_patterns = new string[Properties.Settings.Default.BOMFilter.Count];
        Properties.Settings.Default.BOMFilter.CopyTo(regex_patterns, 0);
        return regex_patterns;
      }
    }

    /// <summary>
    /// Convert a specialized StringCollection to an array of strings.
    /// </summary>
    static public string[] MasterHashes {
      get {
        string[] hs = new string[Properties.Settings.Default.MasterTableHashes.Count];
        Properties.Settings.Default.MasterTableHashes.CopyTo(hs, 0);
        return hs;
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
