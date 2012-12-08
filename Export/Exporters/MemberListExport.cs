using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.IO;

namespace Export
{
    /// <summary>
    /// Exports the complete member list containing the name and living place of the member.
    /// </summary>
    public class MemberListExport : PdfExport
    {
        public override int PagesPer100Members { get { return 1; } }
        public MemberListExport()
        {
            DocumentTitle = "JÄSENLUETTELO";
            DocumentHeader = "JÄSENLUETTELO";
            Name = "Yhdistyksen jäsenluettelo (nimi, kotipaikka)";
        }

        public override string ToString() { return Name; }

        protected override StringBuilder BuildDocument(IEnumerable<Member> members)
        {
            StringBuilder sb = new StringBuilder();

            Properties.Export export = global::Export.Properties.Export.Default;

            sb.AppendFormat(export.FoDocumentHeader, OrgName, "", DocumentTitle, DocumentDate, "");

            sb.AppendFormat(export.FoBlockBold, String.Format("{0} - {1}", DocumentHeader, OrgName));

            sb.AppendFormat(export.Fo2ColumnTableStart, "Nimi", "Kotipaikka");

            int counter = 0;
            foreach (Member row in members.OrderBy(o => o.lastname))
            {
                counter = AppendMember(sb, export, counter, row);
            }

            sb.Append(export.FoTableEnd);
            sb.AppendFormat(export.FoBlockBold, String.Format("{0} jäsentä listattu", counter));
            sb.Append(export.FoDocumentEnd);
            return sb;
        }

        private static int AppendMember(StringBuilder sb, Properties.Export export, int counter, Member member)
        {
            StringBuilder col1Builder = new StringBuilder();
            col1Builder.AppendFormat(export.FoFirstBlock, member.lastname + " " + member.firstname);

            StringBuilder col2Builder = new StringBuilder();
            col2Builder.AppendFormat(export.FoBlock, member.Address.city);

            string foTableRow = ++counter % 2 == 0 ? export.Fo2ColumnTableOddRow : export.Fo2ColumnTableEvenRow;
            sb.AppendFormat(foTableRow, col1Builder, col2Builder);
            return counter;
        }
    }
}
