﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Redbrick_Addin.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int Top {
            get {
                return ((int)(this["Top"]));
            }
            set {
                this["Top"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int Left {
            get {
                return ((int)(this["Left"]));
            }
            set {
                this["Left"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public int RevLimit {
            get {
                return ((int)(this["RevLimit"]));
            }
            set {
                this["RevLimit"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>BS</string>
  <string>CD</string>
  <string>CW</string>
  <string>DF</string>
  <string>DH</string>
  <string>ED</string>
  <string>JB</string>
  <string>KJ</string>
  <string>KL</string>
  <string>LF</string>
  <string>SP</string>
  <string>TH</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Authors {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Authors"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string />
  <string>ACRYLIC</string>
  <string>ALUMINUM</string>
  <string>BENDER</string>
  <string>CRS</string>
  <string>FELT</string>
  <string>GLASS FL</string>
  <string>GLASS LAM</string>
  <string>GLASS TMP</string>
  <string>HPL</string>
  <string>HRS</string>
  <string>MDF</string>
  <string>MEL</string>
  <string>OFF ALL</string>
  <string>PB</string>
  <string>PC</string>
  <string>PE</string>
  <string>PETG</string>
  <string>PLWD</string>
  <string>POPLAR</string>
  <string>POSTFRM MDF</string>
  <string>PVC</string>
  <string>SLAB</string>
  <string>SOLID SURF</string>
  <string>SST</string>
  <string>STYRENE</string>
  <string>VAC PRESS</string>
  <string>VENEER</string>
  <string>VINYL</string>
  <string>VINYL WRAP</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Materials {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Materials"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>JCPENNEY - 220</string>
  <string>KMART - 239</string>
  <string>KOHLS - 201</string>
  <string>MACY'S  - 495</string>
  <string>SEARS - 210</string>
  <string>STERLING-491</string>
  <string>TARGET - 274</string>
  <string>WALGREENS - 510</string>
  <string>WALMART - 505</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Customers {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Customers"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100, 100")]
        public global::System.Drawing.Point EditRevLocation {
            get {
                return ((global::System.Drawing.Point)(this["EditRevLocation"]));
            }
            set {
                this["EditRevLocation"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Driver={SQL Server};SERVER=AMSTORE-SQL-05;DATABASE=ENGINEERING")]
        public string ConnectionString {
            get {
                return ((string)(this["ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("kcjuntunen@amstore.com")]
        public string Dev {
            get {
                return ((string)(this["Dev"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Error in property wizard")]
        public string SubjectLine {
            get {
                return ((string)(this["SubjectLine"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-22")]
        public string NetPath {
            get {
                return ((string)(this["NetPath"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\setup.bmp")]
        public string Icon {
            get {
                return ((string)(this["Icon"]));
            }
            set {
                this["Icon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("645")]
        public int CurrentCutlist {
            get {
                return ((int)(this["CurrentCutlist"]));
            }
            set {
                this["CurrentCutlist"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int UserDept {
            get {
                return ((int)(this["UserDept"]));
            }
            set {
                this["UserDept"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EnableDBWrite {
            get {
                return ((bool)(this["EnableDBWrite"]));
            }
            set {
                this["EnableDBWrite"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Testing {
            get {
                return ((bool)(this["Testing"]));
            }
            set {
                this["Testing"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool OnlyCurrentCustomers {
            get {
                return ((bool)(this["OnlyCurrentCustomers"]));
            }
            set {
                this["OnlyCurrentCustomers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8999")]
        public int LastLegacyECR {
            get {
                return ((int)(this["LastLegacyECR"]));
            }
            set {
                this["LastLegacyECR"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int CurrentRev {
            get {
                return ((int)(this["CurrentRev"]));
            }
            set {
                this["CurrentRev"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int DefaultState {
            get {
                return ((int)(this["DefaultState"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point CutlistHeaderLocation {
            get {
                return ((global::System.Drawing.Point)(this["CutlistHeaderLocation"]));
            }
            set {
                this["CutlistHeaderLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size CutlistHeaderSize {
            get {
                return ((global::System.Drawing.Size)(this["CutlistHeaderSize"]));
            }
            set {
                this["CutlistHeaderSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7")]
        public int RevNoLimit {
            get {
                return ((int)(this["RevNoLimit"]));
            }
            set {
                this["RevNoLimit"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("B2-01-6F-32-BD-A0-D4-35-11-4D-40-09-16-58-0B-2F")]
        public string MasterTableHash {
            get {
                return ((string)(this["MasterTableHash"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point DataDisplayLocation {
            get {
                return ((global::System.Drawing.Point)(this["DataDisplayLocation"]));
            }
            set {
                this["DataDisplayLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size DataDisplaySize {
            get {
                return ((global::System.Drawing.Size)(this["DataDisplaySize"]));
            }
            set {
                this["DataDisplaySize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point RBConfigLocation {
            get {
                return ((global::System.Drawing.Point)(this["RBConfigLocation"]));
            }
            set {
                this["RBConfigLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size RBConfigSize {
            get {
                return ((global::System.Drawing.Size)(this["RBConfigSize"]));
            }
            set {
                this["RBConfigSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3030")]
        public int DefaultMaterial {
            get {
                return ((int)(this["DefaultMaterial"]));
            }
            set {
                this["DefaultMaterial"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>AMS</string>\r\n  <string>CUS</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection LayerHeads {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["LayerHeads"]));
            }
            set {
                this["LayerHeads"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>.1-5</string>
  <string>.6-10</string>
  <string>.11-15</string>
  <string>.16-20</string>
  <string>.21-25</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection LayerTails {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["LayerTails"]));
            }
            set {
                this["LayerTails"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\Refresh-icon.bmp")]
        public string RefreshIcon {
            get {
                return ((string)(this["RefreshIcon"]));
            }
            set {
                this["RefreshIcon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-02\\shared\\shared\\general\\RedBrick\\InstallRedBrick.exe")]
        public string InstallerNetworkPath {
            get {
                return ((string)(this["InstallerNetworkPath"]));
            }
            set {
                this["InstallerNetworkPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Program Files\\RedBrick\\RemoveRedbrick.exe")]
        public string UninstallerPath {
            get {
                return ((string)(this["UninstallerPath"]));
            }
            set {
                this["UninstallerPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Engineering")]
        public string EngineeringDir {
            get {
                return ((string)(this["EngineeringDir"]));
            }
            set {
                this["EngineeringDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FlameWar {
            get {
                return ((bool)(this["FlameWar"]));
            }
            set {
                this["FlameWar"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("SERVER=AMSTORE-SQL-07;DATABASE=M2MDATA01;Trusted_Connection=True")]
        public string M2MConnection {
            get {
                return ((string)(this["M2MConnection"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point MPMLocation {
            get {
                return ((global::System.Drawing.Point)(this["MPMLocation"]));
            }
            set {
                this["MPMLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size MPMSize {
            get {
                return ((global::System.Drawing.Size)(this["MPMSize"]));
            }
            set {
                this["MPMSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int LastCustomerSelection {
            get {
                return ((int)(this["LastCustomerSelection"]));
            }
            set {
                this["LastCustomerSelection"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MakeSounds {
            get {
                return ((bool)(this["MakeSounds"]));
            }
            set {
                this["MakeSounds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Warn {
            get {
                return ((bool)(this["Warn"]));
            }
            set {
                this["Warn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Windows\\Media\\tada.wav")]
        public string ClipboardSound {
            get {
                return ((string)(this["ClipboardSound"]));
            }
            set {
                this["ClipboardSound"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.001")]
        public double CheckEpsilon {
            get {
                return ((double)(this["CheckEpsilon"]));
            }
            set {
                this["CheckEpsilon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IdiotLight {
            get {
                return ((bool)(this["IdiotLight"]));
            }
            set {
                this["IdiotLight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>B2-01-6F-32-BD-A0-D4-35-11-4D-40-09-16-58-0B-2F</string>
  <string>7D-37-E7-57-82-09-28-71-D3-0B-94-7D-AC-44-D2-0F</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection MasterTableHashes {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["MasterTableHashes"]));
            }
            set {
                this["MasterTableHashes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool OnlyActiveAuthors {
            get {
                return ((bool)(this["OnlyActiveAuthors"]));
            }
            set {
                this["OnlyActiveAuthors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool RememberLastCustomer {
            get {
                return ((bool)(this["RememberLastCustomer"]));
            }
            set {
                this["RememberLastCustomer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-02\\shared\\shared\\general\\Engineering Utility\\ECR Drawings\\")]
        public string ECRDrawingsDestination {
            get {
                return ((string)(this["ECRDrawingsDestination"]));
            }
            set {
                this["ECRDrawingsDestination"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\Archive PDF.bmp")]
        public string ArchiveIcon {
            get {
                return ((string)(this["ArchiveIcon"]));
            }
            set {
                this["ArchiveIcon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>CEB</string>
  <string>CNC</string>
  <string>GIO</string>
  <string>LAC</string>
  <string>WEE</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection CNCOps {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["CNCOps"]));
            }
            set {
                this["CNCOps"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>EB</string>\r\n  <string>CEB</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection EBOps {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["EBOps"]));
            }
            set {
                this["EBOps"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ProgWarn {
            get {
                return ((bool)(this["ProgWarn"]));
            }
            set {
                this["ProgWarn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\help_and_support.bmp")]
        public string HelpIcon {
            get {
                return ((string)(this["HelpIcon"]));
            }
            set {
                this["HelpIcon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://kcjuntunen.github.io/readbrick_readme.html")]
        public string UsageLink {
            get {
                return ((string)(this["UsageLink"]));
            }
            set {
                this["UsageLink"] = value;
            }
        }
    }
}
