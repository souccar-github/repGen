﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.ProjectManagment.ValueObjects.ProjectPhase {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ProjectPhaseRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProjectPhaseRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.ProjectManagment.ValueObjects.ProjectPhase.ProjectPhaseRules", typeof(ProjectPhaseRules).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End Date Field Is Required.
        /// </summary>
        public static string EndDateReq {
            get {
                return ResourceManager.GetString("EndDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End Date should be less than or equal project planned closing date.
        /// </summary>
        public static string EndDateRule1 {
            get {
                return ResourceManager.GetString("EndDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Date Field Is Required.
        /// </summary>
        public static string StartDateReq {
            get {
                return ResourceManager.GetString("StartDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Date Must Be Less Than End Date.
        /// </summary>
        public static string StartDateRule1 {
            get {
                return ResourceManager.GetString("StartDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Date should be greater than or equal project planned starting date.
        /// </summary>
        public static string StartDateRule2 {
            get {
                return ResourceManager.GetString("StartDateRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Status Field Is Required.
        /// </summary>
        public static string StatusReq {
            get {
                return ResourceManager.GetString("StatusReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Owner Field Is Required.
        /// </summary>
        public static string TeamMemberReq {
            get {
                return ResourceManager.GetString("TeamMemberReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Team Field Is Required.
        /// </summary>
        public static string TeamReq {
            get {
                return ResourceManager.GetString("TeamReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Team Role  Field Is Required.
        /// </summary>
        public static string TeamRoleReq {
            get {
                return ResourceManager.GetString("TeamRoleReq", resourceCulture);
            }
        }
    }
}
