﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlackBoot.Services.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class AppResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BlackBoot.Services.Resources.AppResource", typeof(AppResource).Assembly);
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
        ///   Looks up a localized string similar to Sorry! We reach to out investment goal. Please check your email for further investment phase..
        /// </summary>
        public static string CrowdSaleEnded {
            get {
                return ResourceManager.GetString("CrowdSaleEnded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to InvalidUser.
        /// </summary>
        public static string InvalidUser {
            get {
                return ResourceManager.GetString("InvalidUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry! Minimum payment is ${0}..
        /// </summary>
        public static string MinimumPayment {
            get {
                return ResourceManager.GetString("MinimumPayment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NewAndConfirmPasswordsDoNotMatch.
        /// </summary>
        public static string NewAndConfirmPasswordsDoNotMatch {
            get {
                return ResourceManager.GetString("NewAndConfirmPasswordsDoNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PreviousPasswordsDoNotMatch.
        /// </summary>
        public static string PreviousPasswordsDoNotMatch {
            get {
                return ResourceManager.GetString("PreviousPasswordsDoNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current Transaction is failed! Please try again later..
        /// </summary>
        public static string TransactionFailed {
            get {
                return ResourceManager.GetString("TransactionFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UserNotFound.
        /// </summary>
        public static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
    }
}
