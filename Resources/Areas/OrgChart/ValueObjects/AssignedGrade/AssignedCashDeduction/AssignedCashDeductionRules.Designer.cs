﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashDeduction {
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
    public class AssignedCashDeductionRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AssignedCashDeductionRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashDeduction.Assigne" +
                            "dCashDeductionRules", typeof(AssignedCashDeductionRules).Assembly);
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
        ///   Looks up a localized string similar to Active Date Field Is Required.
        /// </summary>
        public static string ActiveDateReq {
            get {
                return ResourceManager.GetString("ActiveDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Active Date Must Be Less Than Inactive Date.
        /// </summary>
        public static string ActiveDateRule1 {
            get {
                return ResourceManager.GetString("ActiveDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inactive Date Field Is Required.
        /// </summary>
        public static string InactiveDateReq {
            get {
                return ResourceManager.GetString("InactiveDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inactive Date Must Be Greater Than Current Date.
        /// </summary>
        public static string InactiveDateRule1 {
            get {
                return ResourceManager.GetString("InactiveDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Occurrence Field Is Required.
        /// </summary>
        public static string OccurrenceReq {
            get {
                return ResourceManager.GetString("OccurrenceReq", resourceCulture);
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
        ///   Looks up a localized string similar to Type Field Is Required.
        /// </summary>
        public static string TypeReq {
            get {
                return ResourceManager.GetString("TypeReq", resourceCulture);
            }
        }
    }
}
