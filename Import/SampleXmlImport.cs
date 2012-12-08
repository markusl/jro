using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Database;
using System.Globalization;

namespace Import
{
    /// <summary>
    /// Support for Blend-generated Sample XML import.
    /// </summary>
    [Export(typeof(IDatabaseImporter))]
    public class SampleXmlImport : DatabaseImporter
    {
        private XmlDocument _document;

        public SampleXmlImport()
        {
        }

        public override string Name
        {
            get { return "Sample Import"; }
        }

        /// <summary>
        /// Import member database from XML file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="adapter">Table adapter for member table</param>
        public override void Import(string path, MembersContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _document = new XmlDocument();
            _document.Load(path);
            try
            {
                //EmptyDatabase(container);
                OnProgressChanged(0.0);
                ImportOrganization(container);
                ImportMembers(container);
                container.SaveChanges();
                OnProgressChanged(1.0);
            }
            finally
            {

            }
        }

        private static void EmptyDatabase(MembersContainer container)
        {
            foreach(var m in container.MemberSet.AsEnumerable())
            {
                container.MemberSet.DeleteObject(m);
            }
            container.SaveChanges();
        }

        private static void ImportOrganization(MembersContainer container)
        {
            container.OrganizationSet.AddObject(Organization.CreateOrganization(0, "DistrictName", "District Name"));
            container.OrganizationSet.AddObject(Organization.CreateOrganization(0, "OrgName", "Sample Organization"));
            container.OrganizationSet.AddObject(Organization.CreateOrganization(0, "BankAccount", "110000-110000"));
        }

        private void ImportMembers(MembersContainer container)
        {
            XmlNodeList nodes = _document.DocumentElement.SelectNodes("*/*");

            int maxMembers = nodes.Count;
            int currentMember = 0;

            foreach (XmlNode node in nodes)
            {
                ConstructMemberFromXmlNode(node, container, currentMember);

                if (currentMember++ % 10 == 0)
                    OnProgressChanged((currentMember / (double)maxMembers) - 0.2);
            }
            Debug.Print("Import members done: " + maxMembers);
        }

        private static void ConstructMemberFromXmlNode(XmlNode node, MembersContainer container, int current)
        {
            string[] names = node.Attributes["name"].Value.Split(',');
            // address format "2345 Front St., Seattle, WA 12345"
            string[] address = node.Attributes["address"].Value.Split(',');
            string postalcode = address[2].Trim().Split(' ')[1];

            Member member = Member.CreateMember(0, "", "", "", "", "", "", "", "");
            member.Address = Address.CreateAddress(0, "", "", "", "");
            member.MemberDetais = MemberDetais.CreateMemberDetais(0, String.Format(CultureInfo.CurrentCulture, "{0}", current + 10000), "", "", "", "", "", "", "");
            container.MemberSet.AddObject(member);

            member.Address.address = address[0];
            member.Address.city = address[1];
            member.Address.postalcode = postalcode;
            member.Address.country = "";

            member.firstname = names[1].Trim();
            member.middlenames = "";
            member.lastname = names[0];
            member.email = "";
            member.phone = "";
            member.mobile = node.Attributes["mobile"].Value;
            member.birthdate = "12.1.2010";
            member.sex = "M";

            member.MemberDetais.membergroup = node.Attributes["department"].Value;
            member.MemberDetais.memberjob = "";
            member.MemberDetais.memberclass = "";
            member.MemberDetais.joindate = DateTime.Now.ToString();
            member.MemberDetais.changeddate = DateTime.Now.ToString();
            member.MemberDetais.exitdate = "";
            member.MemberDetais.paymentstatus = "";
        }
    }
}
