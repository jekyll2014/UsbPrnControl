﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UsbPrnControl.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("866")]
        public int CodePage {
            get {
                return ((int)(this["CodePage"]));
            }
            set {
                this["CodePage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Codepages list: https://msdn.microsoft.com/en-us/library/system.text.encoding(v=v" +
            "s.110).aspx")]
        public string CPDescString {
            get {
                return ((string)(this["CPDescString"]));
            }
            set {
                this["CPDescString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("28D78FAD-5A12-11D1-AE5B-0000F803A8C2")]
        public string GUID_PRINT {
            get {
                return ((string)(this["GUID_PRINT"]));
            }
            set {
                this["GUID_PRINT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool checkBox_hexCommand {
            get {
                return ((bool)(this["checkBox_hexCommand"]));
            }
            set {
                this["checkBox_hexCommand"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string textBox_command {
            get {
                return ((string)(this["textBox_command"]));
            }
            set {
                this["textBox_command"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool checkBox_hexParam {
            get {
                return ((bool)(this["checkBox_hexParam"]));
            }
            set {
                this["checkBox_hexParam"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string textBox_param {
            get {
                return ((string)(this["textBox_param"]));
            }
            set {
                this["textBox_param"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5B51A523-CEAD-4468-99DD-BF85B80E0A64")]
        public string GUID_SCAN {
            get {
                return ((string)(this["GUID_SCAN"]));
            }
            set {
                this["GUID_SCAN"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int USBReadInterval {
            get {
                return ((int)(this["USBReadInterval"]));
            }
            set {
                this["USBReadInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public int LineBreakTimeout {
            get {
                return ((int)(this["LineBreakTimeout"]));
            }
            set {
                this["LineBreakTimeout"] = value;
            }
        }
    }
}
