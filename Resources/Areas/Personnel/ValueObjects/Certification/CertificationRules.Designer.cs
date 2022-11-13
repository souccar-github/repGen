﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.Areas.Personnel.ValueObjects.Certification {
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
    public class CertificationRules {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CertificationRules() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.Personnel.ValueObjects.Certification.CertificationRules", typeof(CertificationRules).Assembly);
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
        ///   Looks up a localized string similar to issuance date is required.
        /// </summary>
        public static string DateOfIssuanceReq {
            get {
                return ResourceManager.GetString("DateOfIssuanceReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to issuance date can&apos;t be equal or greater than expire date.
        /// </summary>
        public static string DateOfIssuanceRule1 {
            get {
                return ResourceManager.GetString("DateOfIssuanceRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to issuance date can&apos;t be greater than current date.
        /// </summary>
        public static string DateOfIssuanceRule2 {
            get {
                return ResourceManager.GetString("DateOfIssuanceRule2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to expiry date is required.
        /// </summary>
        public static string ExpirationDateReq {
            get {
                return ResourceManager.GetString("ExpirationDateReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to certification &quot;{0}&quot; has expired.
        /// </summary>
        public static string ExpirationDateRule1 {
            get {
                return ResourceManager.GetString("ExpirationDateRule1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to issuance place is required.
        /// </summary>
        public static string PlaceOfIssuanceReq {
            get {
                return ResourceManager.GetString("PlaceOfIssuanceReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to status is required.
        /// </summary>
        public static string StatusReq {
            get {
                return ResourceManager.GetString("StatusReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to certification type is required.
        /// </summary>
        public static string TypeReq {
            get {
                return ResourceManager.GetString("TypeReq", resourceCulture);
            }
        }
    }
}
