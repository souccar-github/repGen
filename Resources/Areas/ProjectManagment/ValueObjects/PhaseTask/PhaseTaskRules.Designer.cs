﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.ProjectManagment.ValueObjects.PhaseTask {
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
    public class PhaseTaskRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PhaseTaskRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.ProjectManagment.ValueObjects.PhaseTask.PhaseTaskRules", typeof(PhaseTaskRules).Assembly);
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
        ///   Looks up a localized string similar to Actual Closing Date Can&apos;t Be Greater Than Current Date.
        /// </summary>
        public static string ActualClosingDateRule1 {
            get {
                return ResourceManager.GetString("ActualClosingDateRule1", resourceCulture);
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
        
        /// <summary>
        ///   Looks up a localized string similar to Weight Can&apos;t Be Less Than 0.
        /// </summary>
        public static string WeightRule1 {
            get {
                return ResourceManager.GetString("WeightRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Weight Can Not Be Greater Than 100.
        /// </summary>
        public static string WeightRule2 {
            get {
                return ResourceManager.GetString("WeightRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total Weight Can Not Be Greater Than 100.
        /// </summary>
        public static string WeightRule3 {
            get {
                return ResourceManager.GetString("WeightRule3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total Weight is exceeding 100...Please updat the Weight in other Phase tasks and try again.
        /// </summary>
        public static string WeightRule4 {
            get {
                return ResourceManager.GetString("WeightRule4", resourceCulture);
            }
        }
    }
}
