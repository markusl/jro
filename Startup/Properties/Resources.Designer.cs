﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Startup.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Startup.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tiedosto tällä nimellä on jo levyllä {0}.
        /// </summary>
        internal static string AFileWithThisNameAlreadyExistsOnDisk {
            get {
                return ResourceManager.GetString("AFileWithThisNameAlreadyExistsOnDisk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to E18DD8F8-F23D-4eeb-9808-5D858F499EDC.
        /// </summary>
        internal static string AppId {
            get {
                return ResourceManager.GetString("AppId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valitse hakemisto jäsenrekisterille.
        /// </summary>
        internal static string ChooseDirectoryForRegistry {
            get {
                return ResourceManager.GetString("ChooseDirectoryForRegistry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tietokantaa ei voida avata. Tarkista salasana..
        /// </summary>
        internal static string DatabaseCannotBeOpenedInvalidPassword {
            get {
                return ResourceManager.GetString("DatabaseCannotBeOpenedInvalidPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Virhe avattaessa tietokantaa.
        /// </summary>
        internal static string ErrorOpeningDatabase {
            get {
                return ResourceManager.GetString("ErrorOpeningDatabase", resourceCulture);
            }
        }
    }
}