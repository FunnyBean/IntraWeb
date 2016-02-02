﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntraWeb.Resources.Email {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///    A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class EmailStringTable {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        internal EmailStringTable() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("IntraWeb.Resources.Email.EmailStringTable", typeof(EmailStringTable).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///    Overrides the current thread's CurrentUICulture property for all
        ///    resource lookups using this strongly typed resource class.
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
        ///    Looks up a localized string similar to Krosbook.
        /// </summary>
        internal static string MailboxNameFrom {
            get {
                return ResourceManager.GetString("MailboxNameFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Krosbook&apos;s user.
        /// </summary>
        internal static string MailboxNameTo {
            get {
                return ResourceManager.GetString("MailboxNameTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Vitaj na Krosbooku.
        /// </summary>
        internal static string TemplateBodySalutation {
            get {
                return ResourceManager.GetString("TemplateBodySalutation", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to  Nejhorší ze všeho je promarněná příležitost: Správně napsaný uvítací e-mail má 4x vyšší míru přečtení, 5x vyšší počet kliků a 8x vyšší návratnost přepočtenou na jeden e-mail. Jak ho tedy správně napsat?
        ///
        ///Na své webové stránce máte formulář na zasílání e-mailových zpráv. To znamená, že někdo byl na vašem webu dřív, než jste svou firmu aktivně propagovali. Kujte tedy železo, dokud je žhavé: Oslovte zákazníka, dokud vás má ještě v hlavě. Okamžitě zaslaný mail má celkem slušnou šanci, že bude otevřen.
        ///
        ///
        ///Kr [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TemplateBodyText {
            get {
                return ResourceManager.GetString("TemplateBodyText", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to http://funnybean.cloudapp.net.
        /// </summary>
        internal static string TemplateCompanyWebSite {
            get {
                return ResourceManager.GetString("TemplateCompanyWebSite", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to © 2016 Krosbook.
        /// </summary>
        internal static string TemplateFooterCopyright {
            get {
                return ResourceManager.GetString("TemplateFooterCopyright", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to Sleduj firemný život.
        /// </summary>
        internal static string TemplateHeaderSubCaption {
            get {
                return ResourceManager.GetString("TemplateHeaderSubCaption", resourceCulture);
            }
        }
    }
}
