﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
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
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int UserDept {
            get {
                return ((int)(this["UserDept"]));
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
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
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
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7")]
        public int RevNoLimit {
            get {
                return ((int)(this["RevNoLimit"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C6-2E-80-9C-3B-A4-97-7C-2A-98-B9-D6-48-9A-BF-A1")]
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
    }
}
