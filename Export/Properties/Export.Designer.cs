﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.35317
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Export.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Export : global::System.Configuration.ApplicationSettingsBase {
        
        private static Export defaultInstance = ((Export)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Export())));
        
        public static Export Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<fo:root xmlns:fo=\"http://www.w3.org/1999" +
            "/XSL/Format\">\r\n  <fo:layout-master-set>\r\n    <fo:simple-page-master master-name=" +
            "\"first\"\r\n          margin-right=\"1.5cm\"\r\n          margin-left=\"1.5cm\"\r\n        " +
            "  margin-bottom=\"2cm\"\r\n          margin-top=\"1cm\"\r\n          page-width=\"21cm\"\r\n" +
            "          page-height=\"29.7cm\">\r\n      <fo:region-body margin-top=\"4cm\"/>\r\n     " +
            " <fo:region-before extent=\"7cm\"/>\r\n      <fo:region-after extent=\"1cm\"/>\r\n    </" +
            "fo:simple-page-master>\r\n  </fo:layout-master-set>\r\n\r\n  <fo:page-sequence master-" +
            "reference=\"first\">\r\n\r\n  <fo:static-content flow-name=\"xsl-region-before\">\r\n     " +
            " <fo:list-block font-size=\"10pt\" provisional-label-separation=\"0pt\">\r\n          " +
            "<fo:list-item>\r\n              <fo:list-item-label end-indent=\"label-end()\">\r\n   " +
            "               <fo:block text-align=\"left\">\r\n                      <fo:block>{0}" +
            "</fo:block>\r\n                      <fo:block>{1}</fo:block>\r\n                  <" +
            "/fo:block>\r\n              </fo:list-item-label>\r\n              <fo:list-item-bod" +
            "y start-indent=\"body-start()\">\r\n                  <fo:list-block provisional-lab" +
            "el-separation=\"0pt\">\r\n                      <fo:list-item>\r\n                    " +
            "      <fo:list-item-label end-indent=\"label-end()\">\r\n                           " +
            "   <fo:block text-align=\"center\">\r\n                                  <fo:block>{" +
            "2}</fo:block>\r\n                                  <fo:block>{3}</fo:block>\r\n     " +
            "                         </fo:block>\r\n                          </fo:list-item-l" +
            "abel>\r\n                          <fo:list-item-body start-indent=\"body-start()\">" +
            "\r\n                              <fo:block text-align=\"right\">\r\n                 " +
            "               <fo:page-number/> (<fo:page-number-citation ref-id=\"last-page\"/>)" +
            "\r\n                              </fo:block>\r\n                              <fo:b" +
            "lock>{4}</fo:block>\r\n                          </fo:list-item-body>\r\n           " +
            "           </fo:list-item>\r\n                  </fo:list-block>\r\n              </" +
            "fo:list-item-body>\r\n          </fo:list-item>\r\n      </fo:list-block>\r\n  </fo:st" +
            "atic-content>\r\n \r\n  <fo:static-content flow-name=\"xsl-region-after\">\r\n  </fo:sta" +
            "tic-content>\r\n\r\n    <fo:flow flow-name=\"xsl-region-body\">")]
        public string FoDocumentHeader {
            get {
                return ((string)(this["FoDocumentHeader"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("      <fo:block id=\"last-page\">\r\n      </fo:block>\r\n    </fo:flow>\r\n  </fo:page-s" +
            "equence>\r\n</fo:root>\r\n")]
        public string FoDocumentEnd {
            get {
                return ((string)(this["FoDocumentEnd"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"      <fo:table font-size=""9pt"">
        <fo:table-column column-width=""4cm""/>
        <fo:table-column column-width=""5cm""/>
        <fo:table-column column-width=""4.5cm""/>
        <fo:table-column column-width=""4.5cm""/>
      
        <fo:table-header background-color=""#BBB"">
          <fo:table-row>
            <fo:table-cell>
              <fo:block font-weight=""bold"" text-align=""center"" vertical-align=""middle"">
              {0}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight=""bold"" text-align=""center"" vertical-align=""middle"">
              {1}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight=""bold"" text-align=""center"" vertical-align=""middle"">
              {2}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight=""bold"" text-align=""center"" vertical-align=""middle"">
              {3}
              </fo:block>
            </fo:table-cell>
          </fo:table-row>
        </fo:table-header>

        <fo:table-body>
")]
        public string Fo4ColumnTableStart {
            get {
                return ((string)(this["Fo4ColumnTableStart"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"          <fo:table-row>
            <fo:table-cell padding=""2pt"">
              {0}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {1}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {2}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {3}
            </fo:table-cell>
          </fo:table-row>
")]
        public string Fo4ColumnTableEvenRow {
            get {
                return ((string)(this["Fo4ColumnTableEvenRow"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<fo:block font-weight=\"bold\" border-right-width=\"0.5pt\" text-align=\"left\" vertica" +
            "l-align=\"middle\">\r\n              {0}\r\n</fo:block>")]
        public string FoFirstBoldBlock {
            get {
                return ((string)(this["FoFirstBoldBlock"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<fo:block border-right-width=\"0.5pt\" text-align=\"left\" vertical-align=\"middle\">\r\n" +
            "              {0}\r\n</fo:block>\r\n")]
        public string FoFirstBlock {
            get {
                return ((string)(this["FoFirstBlock"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<fo:block>{0}</fo:block>\r\n")]
        public string FoBlock {
            get {
                return ((string)(this["FoBlock"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("        </fo:table-body>\r\n      </fo:table>")]
        public string FoTableEnd {
            get {
                return ((string)(this["FoTableEnd"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"          <fo:table-row background-color=""#EEE"">
            <fo:table-cell padding=""2pt"">
              {0}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {1}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {2}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {3}
            </fo:table-cell>
          </fo:table-row>
")]
        public string Fo4ColumnTableOddRow {
            get {
                return ((string)(this["Fo4ColumnTableOddRow"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<fo:block font-weight=\"bold\">{0}</fo:block>\r\n")]
        public string FoBlockBold {
            get {
                return ((string)(this["FoBlockBold"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"      <fo:table font-size=""9pt"">
        <fo:table-column column-width=""6cm""/>
        <fo:table-column column-width=""6cm""/>
      
        <fo:table-header background-color=""#BBB"">
          <fo:table-row>
            <fo:table-cell>
              <fo:block font-weight=""bold"" text-align=""center"" vertical-align=""middle"">
              {0}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight=""bold"" text-align=""center"" vertical-align=""middle"">
              {1}
              </fo:block>
            </fo:table-cell>
          </fo:table-row>
        </fo:table-header>

        <fo:table-body>
")]
        public string Fo2ColumnTableStart {
            get {
                return ((string)(this["Fo2ColumnTableStart"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("          <fo:table-row>\r\n            <fo:table-cell padding=\"2pt\">\r\n            " +
            "  {0}\r\n            </fo:table-cell>\r\n            <fo:table-cell padding=\"2pt\">\r\n" +
            "              {1}\r\n            </fo:table-cell>\r\n          </fo:table-row>\r\n")]
        public string Fo2ColumnTableEvenRow {
            get {
                return ((string)(this["Fo2ColumnTableEvenRow"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"          <fo:table-row background-color=""#EEE"">
            <fo:table-cell padding=""2pt"">
              {0}
            </fo:table-cell>
            <fo:table-cell padding=""2pt"">
              {1}
            </fo:table-cell>
          </fo:table-row>")]
        public string Fo2ColumnTableOddRow {
            get {
                return ((string)(this["Fo2ColumnTableOddRow"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<fo:block font-style=\"italic\">{0}</fo:block>\r\n")]
        public string FoBlockItalic {
            get {
                return ((string)(this["FoBlockItalic"]));
            }
        }
    }
}
