using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.Globalization;

namespace Export
{
    /// <summary>
    /// Class for exporting address list of members.
    /// </summary>
    public class MemberAddressExport : PdfExport
    {
        public override int PagesPer100Members { get { return 4; } }
        public MemberAddressExport()
        {
            Name = "Jäsenlista valituista (yhteystiedot)";
            DocumentTitle = "OTE JÄSENREKISTERISTÄ";
        }

        public override string ToString() { return Name; }

        protected override StringBuilder BuildDocument(IEnumerable<Member> selection)
        {
            IEnumerable<Member> actualMembers = selection.Where(m => m.IsMember());
            if (actualMembers == null || actualMembers.Count() == 0)
                throw new ArgumentException("", "selection");

            var sb = new StringBuilder();

            Properties.Export export = global::Export.Properties.Export.Default;

            sb.AppendFormat(export.FoDocumentHeader, OrgName, "", DocumentTitle, DocumentDate, "");

            sb.AppendFormat(export.Fo2ColumnTableStart, "Jäsen", "Yhteystiedot");

            int counter = 0;
            foreach (Member row in actualMembers)
            {
                AppendMember(sb, export, ++counter, row);
            }

            sb.Append(export.FoTableEnd);
            sb.AppendFormat(export.FoBlockBold, String.Format("{0} jäsentä listattu", counter));
            sb.Append(export.FoDocumentEnd);

            return sb;
        }

        private static void AppendMember(StringBuilder sb, Properties.Export export, int counter, Member member)
        {
            var col1Builder = new StringBuilder();
            col1Builder.AppendFormat(export.FoFirstBoldBlock, member.lastname + " " + member.firstname);
            col1Builder.AppendFormat(export.FoBlock, DateTime.Parse(member.birthdate, CultureInfo.CurrentCulture).AsDate());
            col1Builder.AppendFormat(export.FoBlock, member.mobile);
            col1Builder.AppendFormat(export.FoBlock, member.email);

            var col2Builder = new StringBuilder();
            col2Builder.AppendFormat(export.FoFirstBlock, member.Address.address);
            col2Builder.AppendFormat(export.FoBlock, member.Address.postalcode + " " + member.Address.city);

            if (counter % 2 == 0)
                sb.AppendFormat(export.Fo2ColumnTableOddRow, col1Builder, col2Builder);
            else
                sb.AppendFormat(export.Fo2ColumnTableEvenRow, col1Builder, col2Builder);
        }
    }
}
