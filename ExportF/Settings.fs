module Settings

let FoDocumentHeader = """
<?xml version="1.0" encoding="UTF-8"?>
<fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <fo:layout-master-set>
    <fo:simple-page-master master-name="first"
          margin-right="1.5cm"
          margin-left="1.5cm"
          margin-bottom="2cm"
          margin-top="1cm"
          page-width="21cm"
          page-height="29.7cm">
      <fo:region-body margin-top="4cm"/>
      <fo:region-before extent="7cm"/>
      <fo:region-after extent="1cm"/>
    </fo:simple-page-master>
  </fo:layout-master-set>

  <fo:page-sequence master-reference="first">

  <fo:static-content flow-name="xsl-region-before">
      <fo:list-block font-size="10pt" provisional-label-separation="0pt">
          <fo:list-item>
              <fo:list-item-label end-indent="label-end()">
                  <fo:block text-align="left">
                      <fo:block>{0}</fo:block>
                      <fo:block>{1}</fo:block>
                  </fo:block>
              </fo:list-item-label>
              <fo:list-item-body start-indent="body-start()">
                  <fo:list-block provisional-label-separation="0pt">
                      <fo:list-item>
                          <fo:list-item-label end-indent="label-end()">
                              <fo:block text-align="center">
                                  <fo:block>{2}</fo:block>
                                  <fo:block>{3}</fo:block>
                              </fo:block>
                          </fo:list-item-label>
                          <fo:list-item-body start-indent="body-start()">
                              <fo:block text-align="right">
                                <fo:page-number/> (<fo:page-number-citation ref-id="last-page"/>)
                              </fo:block>
                              <fo:block>{4}</fo:block>
                          </fo:list-item-body>
                      </fo:list-item>
                  </fo:list-block>
              </fo:list-item-body>
          </fo:list-item>
      </fo:list-block>
  </fo:static-content>
 
  <fo:static-content flow-name="xsl-region-after">
  </fo:static-content>

    <fo:flow flow-name="xsl-region-body">
"""
let FoDocumentEnd = """
      <fo:block id="last-page">
      </fo:block>
    </fo:flow>
  </fo:page-sequence>
</fo:root>
"""
let Fo4ColumnTableStart = """
      <fo:table font-size="9pt">
        <fo:table-column column-width="4cm"/>
        <fo:table-column column-width="5cm"/>
        <fo:table-column column-width="4.5cm"/>
        <fo:table-column column-width="4.5cm"/>
      
        <fo:table-header background-color="#BBB">
          <fo:table-row>
            <fo:table-cell>
              <fo:block font-weight="bold" text-align="center" vertical-align="middle">
              {0}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight="bold" text-align="center" vertical-align="middle">
              {1}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight="bold" text-align="center" vertical-align="middle">
              {2}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight="bold" text-align="center" vertical-align="middle">
              {3}
              </fo:block>
            </fo:table-cell>
          </fo:table-row>
        </fo:table-header>

        <fo:table-body>
"""
let Fo4ColumnTableEvenRow = """
          <fo:table-row>
            <fo:table-cell padding="2pt">
              {0}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {1}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {2}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {3}
            </fo:table-cell>
          </fo:table-row>
"""
let FoFirstBoldBlock = """
<fo:block font-weight="bold" border-right-width="0.5pt" text-align="left" vertical-align="middle">
              {0}
</fo:block>
"""
let FoBlockItalic = """
<fo:block font-style="italic">{0}</fo:block>
"""
let FoBlockBold = """
<fo:block font-weight="bold">{0}</fo:block>
"""
let FoBlock = """
<fo:block>{0}</fo:block>
"""
let FoTableEnd = """
        </fo:table-body>
      </fo:table>
"""
let Fo4ColumnTableOddRow = """
          <fo:table-row background-color="#EEE">
            <fo:table-cell padding="2pt">
              {0}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {1}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {2}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {3}
            </fo:table-cell>
          </fo:table-row>
"""
let Fo2ColumnTableStart = """
      <fo:table font-size="9pt">
        <fo:table-column column-width="6cm"/>
        <fo:table-column column-width="6cm"/>
      
        <fo:table-header background-color="#BBB">
          <fo:table-row>
            <fo:table-cell>
              <fo:block font-weight="bold" text-align="center" vertical-align="middle">
              {0}
              </fo:block>
            </fo:table-cell>
            <fo:table-cell>
              <fo:block font-weight="bold" text-align="center" vertical-align="middle">
              {1}
              </fo:block>
            </fo:table-cell>
          </fo:table-row>
        </fo:table-header>

        <fo:table-body>
"""
let Fo2ColumnTableEvenRow = """
          <fo:table-row>
            <fo:table-cell padding="2pt">
              {0}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {1}
            </fo:table-cell>
          </fo:table-row>
"""
let Fo2ColumnTableOddRow = """
          <fo:table-row background-color="#EEE">
            <fo:table-cell padding="2pt">
              {0}
            </fo:table-cell>
            <fo:table-cell padding="2pt">
              {1}
            </fo:table-cell>
          </fo:table-row>
"""
