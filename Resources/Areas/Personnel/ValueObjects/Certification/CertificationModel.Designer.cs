//------------------------------------------------------------------------------
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
    public class CertificationModel {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CertificationModel() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Areas.Personnel.ValueObjects.Certification.CertificationModel", typeof(CertificationModel).Assembly);
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
        ///   Looks up a localized string similar to Certification Type.
        /// </summary>
        public static string CertificationType {
            get {
                return ResourceManager.GetString("CertificationType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Issuance Date.
        /// </summary>
        public static string DateOfIssuance {
            get {
                return ResourceManager.GetString("DateOfIssuance", resourceCulture);
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
        ///   Looks up a localized string similar to Expiry Date.
        /// </summary>
        public static string ExpirationDate {
            get {
                return ResourceManager.GetString("ExpirationDate", resourceCulture);
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
        ///   Looks up a localized string similar to Comments.
        /// </summary>
        public static string Notes {
            get {
                return ResourceManager.GetString("Notes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to comments  must be a text with a maximum length of 255 characters.
        /// </summary>
        public static string NotesLength {
            get {
                return ResourceManager.GetString("NotesLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Issuance Place.
        /// </summary>
        public static string PlaceOfIssuance {
            get {
                return ResourceManager.GetString("PlaceOfIssuance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Status.
        /// </summary>
        public static string Status {
            get {
                return ResourceManager.GetString("Status", resourceCulture);
            }
        }
    }
}
