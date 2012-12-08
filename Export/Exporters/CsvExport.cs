using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.IO;
using Fonet;

namespace Export
{
    /// <summary>
    /// Exports the complete member list containing the name and living place of the member.
    /// </summary>
    public class CsvExport
    {
        public CsvExport()
        {

        }

        public void Export(IEnumerable<Member> members, string path)
        {

            members = members.Where(m =>
                {
                    return true;
                    //return m.MemberDetais.membergroup.Equals("x") ||
                    //       m.MemberDetais.membergroup.Equals("y") ||
                    //       m.MemberDetais.membergroup.Equals("z");
                });

            StringBuilder sb = new StringBuilder();
            foreach (var member in members)
            {
                foreach (var contact in member.Contact)
                    if (!String.IsNullOrEmpty(contact.email))
                        sb.AppendFormat("{0}\n", contact.email);
            }
            sb.Remove(sb.Length - 1, 1);

            File.WriteAllText(path, sb.ToString());
        }
    }
}
