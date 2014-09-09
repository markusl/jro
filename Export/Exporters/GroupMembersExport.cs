using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.Diagnostics;
using System.Globalization;

namespace Export
{
    /// <summary>
    /// This class is used to export member lists for member groups
    /// </summary>
    public class GroupMembersExport : PdfExport
    {
        public override int PagesPer100Members { get { return 1; } }
        public GroupMembersExport()
        {
            Name = "Ryhmän jäsenlista (kaikki tiedot)";
            DocumentTitle = "OTE JÄSENREKISTERISTÄ";
        }

        public override string ToString() { return Name; }

        protected override StringBuilder BuildDocument(IEnumerable<Member> members)
        {
            var sb = new StringBuilder();

            Properties.Export export = global::Export.Properties.Export.Default;

            sb.AppendFormat(export.FoDocumentHeader, OrgName, "", DocumentTitle, DocumentDate, "");

            IEnumerable<string> groups = members.Select(member => member.MemberDetais.membergroup).Distinct();

            int counter = 0;
            foreach (string group in groups)
            {
                sb.AppendFormat(export.FoBlockBold, "RYHMÄN JÄSENLISTA - " + group);

                sb.AppendFormat(export.Fo4ColumnTableStart, "Jäsen", "Yhteystiedot", "Huoltaja 1", "Huoltaja 2");

                IEnumerable<Database.Member> groupMembers = members.
                                        Where(member => member.MemberDetais.membergroup.Equals(group)).
                                        OrderBy(member => member.lastname);

                foreach (Database.Member row in groupMembers)
                {
                    counter = AppendMember(sb, export, counter, row);
                }

                sb.Append(export.FoTableEnd);
            }
            sb.AppendFormat(export.FoBlockBold, String.Format("{0} jäsentä listattu", counter));
            sb.Append(export.FoDocumentEnd);

            return sb;
        }

        private static int AppendMember(StringBuilder sb, Properties.Export export, int counter, Database.Member member)
        {
            StringBuilder col1Builder = new StringBuilder();
            col1Builder.AppendFormat(export.FoFirstBoldBlock, member.lastname + " " + member.firstname);
            col1Builder.AppendFormat(export.FoBlock, DateTime.Parse(member.birthdate, CultureInfo.CurrentCulture).AsDate());
            col1Builder.AppendFormat(export.FoBlock, member.mobile);
            col1Builder.AppendFormat(export.FoBlock, member.email);

            StringBuilder col2Builder = new StringBuilder();
            col2Builder.AppendFormat(export.FoFirstBlock, member.Address.address);
            col2Builder.AppendFormat(export.FoBlock, member.Address.postalcode + " " + member.Address.city);

            StringBuilder col3Builder = new StringBuilder();
            StringBuilder col4Builder = new StringBuilder();

            IEnumerable<Contact> contacts = member.Contact.AsEnumerable();
            if (contacts.Count() > 0)
                col3Builder = BuildContactColumn(export, contacts.ElementAt(0));
            if (contacts.Count() > 1)
                col4Builder = BuildContactColumn(export, contacts.ElementAt(1));

            string foTableRow = ++counter % 2 == 0 ? export.Fo4ColumnTableOddRow : export.Fo4ColumnTableEvenRow;
            sb.AppendFormat(foTableRow, col1Builder, col2Builder,
                                        col3Builder, col4Builder);
            return counter;
        }

        private static StringBuilder BuildContactColumn(Properties.Export export, Contact contact)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(export.FoFirstBlock, contact.lastname + " " + contact.firstname);
            AppendBlockOr(export, sb, contact.mobile, "Matkapuhelin puuttuu");
            AppendBlockOr(export, sb, contact.email, "Sähköposti puuttuu");
            if (contact.Address != null)
            {
                sb.AppendFormat(export.FoBlock, contact.Address.address);
                sb.AppendFormat(export.FoBlock, contact.Address.postalcode + " " + contact.Address.city);
            }
            else Debug.Print("Got null address for contact {0} {1}", contact.firstname, contact.lastname);
            sb.AppendFormat(export.FoBlock, contact.phone);

            return sb;
        }

        private static void AppendBlockOr(Properties.Export export, StringBuilder sb, string value, string missing)
        {
            if (String.IsNullOrEmpty(value.Trim()))
            {
                sb.AppendFormat(export.FoBlockItalic, missing);
            }
            else
            {
                sb.AppendFormat(export.FoBlock, value);
            }
        }
    }
}
