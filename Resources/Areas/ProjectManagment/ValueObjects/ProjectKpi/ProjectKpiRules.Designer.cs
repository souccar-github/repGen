//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.ProjectManagment.ValueObjects.ProjectKpi {
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
    public class ProjectKpiRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProjectKpiRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.ProjectManagment.ValueObjects.ProjectKpi.ProjectKpiRules", typeof(ProjectKpiRules).Assembly);
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
        ///   Looks up a localized string similar to Type Field Is Required.
        /// </summary>
        public static string TypeReq {
            get {
                return ResourceManager.GetString("TypeReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value Field Can&apos;t Equal Zero.
        /// </summary>
        public static string ValueRule1 {
            get {
                return ResourceManager.GetString("ValueRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value Can&apos;t Be Negative Number.
        /// </summary>
        public static string ValueRule2 {
            get {
                return ResourceManager.GetString("ValueRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value Can Not Be Greater Than 100.
        /// </summary>
        public static string ValueRule3 {
            get {
                return ResourceManager.GetString("ValueRule3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Weight Field Can&apos;t Equal Zero.
        /// </summary>
        public static string WeightRule1 {
            get {
                return ResourceManager.GetString("WeightRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Weight Can&apos;t Be Negative Number.
        /// </summary>
        public static string WeightRule2 {
            get {
                return ResourceManager.GetString("WeightRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Weight Can Not Be Greater Than 100.
        /// </summary>
        public static string WeightRule3 {
            get {
                return ResourceManager.GetString("WeightRule3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to total KPI weights for this Project exceeds 100 !.
        /// </summary>
        public static string WeightRule4 {
            get {
                return ResourceManager.GetString("WeightRule4", resourceCulture);
            }
        }
    }
}
