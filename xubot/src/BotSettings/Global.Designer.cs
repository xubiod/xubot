﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace xubot.src.BotSettings {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Global : global::System.Configuration.ApplicationSettingsBase {
        
        private static Global defaultInstance = ((Global)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Global())));
        
        public static Global Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DMsAlwaysNSFW {
            get {
                return ((bool)(this["DMsAlwaysNSFW"]));
            }
            set {
                this["DMsAlwaysNSFW"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public int EmbedListMaxLength {
            get {
                return ((int)(this["EmbedListMaxLength"]));
            }
            set {
                this["EmbedListMaxLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DisableRedditOnStart {
            get {
                return ((bool)(this["DisableRedditOnStart"]));
            }
            set {
                this["DisableRedditOnStart"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("./Exceptions/")]
        public string ExceptionLogLocation {
            get {
                return ((string)(this["ExceptionLogLocation"]));
            }
            set {
                this["ExceptionLogLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SendStacktraceOnError {
            get {
                return ((bool)(this["SendStacktraceOnError"]));
            }
            set {
                this["SendStacktraceOnError"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SendBigStacktraceOnError {
            get {
                return ((bool)(this["SendBigStacktraceOnError"]));
            }
            set {
                this["SendBigStacktraceOnError"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool BotwideNSFWEnabled {
            get {
                return ((bool)(this["BotwideNSFWEnabled"]));
            }
            set {
                this["BotwideNSFWEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("💭")]
        public string WorkingReaction {
            get {
                return ((string)(this["WorkingReaction"]));
            }
            set {
                this["WorkingReaction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("✅")]
        public string WorkCompletedReaction {
            get {
                return ((string)(this["WorkCompletedReaction"]));
            }
            set {
                this["WorkCompletedReaction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("🕒")]
        public string WorkTakingLongerReaction {
            get {
                return ((string)(this["WorkTakingLongerReaction"]));
            }
            set {
                this["WorkTakingLongerReaction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int TakingLongerMilliseconds {
            get {
                return ((int)(this["TakingLongerMilliseconds"]));
            }
            set {
                this["TakingLongerMilliseconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int TaskPollLength {
            get {
                return ((int)(this["TaskPollLength"]));
            }
            set {
                this["TaskPollLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("notinteresting")]
        public string StartingSubreddit {
            get {
                return ((string)(this["StartingSubreddit"]));
            }
            set {
                this["StartingSubreddit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string StartingRedditQuery {
            get {
                return ((string)(this["StartingRedditQuery"]));
            }
            set {
                this["StartingRedditQuery"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int StartingRedditSorting {
            get {
                return ((int)(this["StartingRedditSorting"]));
            }
            set {
                this["StartingRedditSorting"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool StartingRedditHideOutput {
            get {
                return ((bool)(this["StartingRedditHideOutput"]));
            }
            set {
                this["StartingRedditHideOutput"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SuperSimpleTypes {
            get {
                return ((bool)(this["SuperSimpleTypes"]));
            }
            set {
                this["SuperSimpleTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("xub>")]
        public string HardcodedPrefix {
            get {
                return ((string)(this["HardcodedPrefix"]));
            }
            set {
                this["HardcodedPrefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[>")]
        public string DefaultPrefix {
            get {
                return ((string)(this["DefaultPrefix"]));
            }
            set {
                this["DefaultPrefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("d>")]
        public string DefaultDevPrefix {
            get {
                return ((string)(this["DefaultDevPrefix"]));
            }
            set {
                this["DefaultDevPrefix"] = value;
            }
        }
    }
}
