﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.Objective.Entities.BasicObjective {
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
    public class BasicObjectiveRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BasicObjectiveRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveRules", typeof(BasicObjectiveRules).Assembly);
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
        ///   Looks up a localized string similar to actual starting date must be less than actual closing date.
        /// </summary>
        public static string ActualStartingDateRule1 {
            get {
                return ResourceManager.GetString("ActualStartingDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to creation date must be equal to date of today.
        /// </summary>
        public static string CreationDateRule1 {
            get {
                return ResourceManager.GetString("CreationDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to owner is required.
        /// </summary>
        public static string EmployeeReq {
            get {
                return ResourceManager.GetString("EmployeeReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned closing date is required.
        /// </summary>
        public static string PlannedClosingDateReq {
            get {
                return ResourceManager.GetString("PlannedClosingDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned starting date is required.
        /// </summary>
        public static string PlannedStartingDateReq {
            get {
                return ResourceManager.GetString("PlannedStartingDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned starting date must be less than planned closing date.
        /// </summary>
        public static string PlannedStartingDateRule1 {
            get {
                return ResourceManager.GetString("PlannedStartingDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned starting date must be in first quarter [1 Jan - 31 Mar] .
        /// </summary>
        public static string PlannedStartingDateRule2 {
            get {
                return ResourceManager.GetString("PlannedStartingDateRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned starting date must be in second quarter [1 Apr - 30 Jun] of &quot;{0}&quot;.
        /// </summary>
        public static string PlannedStartingDateRule3 {
            get {
                return ResourceManager.GetString("PlannedStartingDateRule3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned starting date must be in third quarter [1 Jul - 30 Sept] of &quot;{0}&quot;.
        /// </summary>
        public static string PlannedStartingDateRule4 {
            get {
                return ResourceManager.GetString("PlannedStartingDateRule4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to planned starting date must be in fourth quarter [1 Oct - 31 Dec] of &quot;{0}&quot;.
        /// </summary>
        public static string PlannedStartingDateRule5 {
            get {
                return ResourceManager.GetString("PlannedStartingDateRule5", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to priority is required.
        /// </summary>
        public static string PriorityReq {
            get {
                return ResourceManager.GetString("PriorityReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to quarter is required.
        /// </summary>
        public static string QuarterReq {
            get {
                return ResourceManager.GetString("QuarterReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to weight is required.
        /// </summary>
        public static string WeightReq {
            get {
                return ResourceManager.GetString("WeightReq", resourceCulture);
            }
        }
    }
}
