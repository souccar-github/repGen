﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.Personnel.ValueObjects.MilitaryService {
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
    public class MilitaryServiceRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MilitaryServiceRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.Personnel.ValueObjects.MilitaryService.MilitaryServiceRules", typeof(MilitaryServiceRules).Assembly);
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
        ///   Looks up a localized string similar to days can not be greater than 31.
        /// </summary>
        public static string DaysRule1 {
            get {
                return ResourceManager.GetString("DaysRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to days must be number.
        /// </summary>
        public static string DaysRule2 {
            get {
                return ResourceManager.GetString("DaysRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to not allowed to add.. males only.
        /// </summary>
        public static string IdRule1 {
            get {
                return ResourceManager.GetString("IdRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to months must be greater than zero.
        /// </summary>
        public static string MonthsRule1 {
            get {
                return ResourceManager.GetString("MonthsRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to months must be number.
        /// </summary>
        public static string MonthsRule2 {
            get {
                return ResourceManager.GetString("MonthsRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to months can not be greater than 50.
        /// </summary>
        public static string MonthsRule3 {
            get {
                return ResourceManager.GetString("MonthsRule3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to months must be number.
        /// </summary>
        public static string MonthsRule4 {
            get {
                return ResourceManager.GetString("MonthsRule4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to service start date is required.
        /// </summary>
        public static string ServiceStartDateRule1 {
            get {
                return ResourceManager.GetString("ServiceStartDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to service start date can not  be greater than current date.
        /// </summary>
        public static string ServiceStartDateRule2 {
            get {
                return ResourceManager.GetString("ServiceStartDateRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to this employee is about to finish his service..
        /// </summary>
        public static string ServiceStartDateRule3 {
            get {
                return ResourceManager.GetString("ServiceStartDateRule3", resourceCulture);
            }
        }
    }
}
