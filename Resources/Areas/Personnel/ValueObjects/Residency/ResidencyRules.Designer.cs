﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.Personnel.ValueObjects.Residency {
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
    public class ResidencyRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResidencyRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.Personnel.ValueObjects.Residency.ResidencyRules", typeof(ResidencyRules).Assembly);
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
        ///   Looks up a localized string similar to expiry date is required.
        /// </summary>
        public static string ExpiryDateReq {
            get {
                return ResourceManager.GetString("ExpiryDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to expiry date can&apos;t be less than current date.
        /// </summary>
        public static string ExpiryDateRule1 {
            get {
                return ResourceManager.GetString("ExpiryDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to residency &quot;{0}&quot; has expired.
        /// </summary>
        public static string IdRule1 {
            get {
                return ResourceManager.GetString("IdRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to issuance date is required.
        /// </summary>
        public static string IssuanceDateReq {
            get {
                return ResourceManager.GetString("IssuanceDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to issuance date can&apos;t be equal or greater than expire date.
        /// </summary>
        public static string IssuanceDateRule1 {
            get {
                return ResourceManager.GetString("IssuanceDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to issuance date can&apos;t be greater than current date.
        /// </summary>
        public static string IssuanceDateRule2 {
            get {
                return ResourceManager.GetString("IssuanceDateRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to nationality is required.
        /// </summary>
        public static string NationalityReq {
            get {
                return ResourceManager.GetString("NationalityReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to type is required.
        /// </summary>
        public static string TypeReq {
            get {
                return ResourceManager.GetString("TypeReq", resourceCulture);
            }
        }
    }
}
