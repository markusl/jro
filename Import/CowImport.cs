using System;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Database;
using System.Globalization;

namespace Import
{
    /// <summary>
    /// Support for ClubOnWeb database import.
    /// </summary>
    /// <remarks>
    /// This class is made for importing ClubOnWeb membership database from
    /// Access 2000 (.mdb) format.
    /// 
    /// Since the format is not well documented, it has been kind of reverse-engineered; consider
    /// TestCowImport.cs as documentation of the database format.
    /// </remarks>
    [Export(typeof(IDatabaseImporter))]
    public class CowImport : DatabaseImporter
    {
        private IDbConnection _connection;

        /// <summary>
        /// Get the name of this importer.
        /// </summary>
        public override string Name
        {
            get { return "ClubOnWeb Import"; }
        }

        /// <summary>
        /// Import member database from CoW access database.
        /// </summary>
        /// <param name="path">The source (CoW) database</param>
        /// <param name="container">The target database</param>
        public override void Import(string path, MembersContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");


            string connectionString = String.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.JET.OLEDB.4.0;data source=\"{0}\"", path);
            _connection = new OleDbConnection(connectionString);
            _connection.Open();
            try
            {
                ImportOrganization(container);
                ImportMembers(container);
                HandleResignedMembers(container, _connection);

                container.SaveChanges(); // the important part! ;o
                OnProgressChanged(1.0);
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Checks if the member exists only in local database and marks as resigned if so.
        /// </summary>
        private static void HandleResignedMembers(MembersContainer container, IDbConnection connection)
        {
            foreach(var member in container.MemberSet.AsEnumerable())
            {
                if(member.IsMember()) // we don't care if non-members don't exist in source database
                {
                    if(!MemberExistsInDatabase(member, connection))
                    {
                        member.MemberDetais.membergroup = DBConstants.ResignedMember;
                        Debug.Print("Member resigned: {0} {1}", member.firstname, member.lastname);
                    }
                }
            }
        }

        private void ImportMembers(IDbCommand cmd, MembersContainer container)
        {
            int maxMembers = GetMemberCountInCow();
            int currentMember = 0;

            Debug.Print("Importing {0} members", maxMembers);

            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ImportMember(container, reader);

                if (currentMember++ % 10 == 0)
                    OnProgressChanged((currentMember / (double)maxMembers) - 0.2);
            }
            OnProgressChanged(0.8);

            Debug.Print("Import members done: " + maxMembers);
        }

        private void ImportMember(MembersContainer container, IDataReader reader)
        {
            string firstName = reader.Get<string>("NameFirst");
            string lastName = reader.Get<string>("NameFamily");
            Member member = container.MemberSet.AsEnumerable().FirstOrDefault(m => m.firstname.Equals(firstName) &&
                                                                                   m.lastname.Equals(lastName));

            // If the member doesn't exist yet
            if (member == null)
            {
                member = Member.CreateMember(0, "", "", "", "", "", "", "", "");
                member.Address = Address.CreateAddress(0, "", "", "", "");
                member.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");
                container.MemberSet.AddObject(member);
                Debug.Print("Importing a NEW member {0} {1}", firstName, lastName);
            }
            else
            {
                Debug.Print("Updating an existing member {0} {1}", firstName, lastName);
            }

            // Update or construct a member
            ConstructMemberAndUpdate(member, reader);
        }

        private int GetMemberCountInCow()
        {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Memb;";
            return (int)cmd.ExecuteScalar();
        }

        private static bool MemberExistsInDatabase(Member member, IDbConnection connection)
        {
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Memb.MembId FROM Memb WHERE Memb.NameFirst = @FirstName AND Memb.NameFamily = @LastName";
            cmd.Parameters.Add(new OleDbParameter("@FirstName", member.firstname));
            cmd.Parameters.Add(new OleDbParameter("@LastName", member.lastname));
            return cmd.ExecuteReader().Read();
        }

        private void ImportOrganization(MembersContainer container)
        {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Org;";
            IDataReader reader = cmd.ExecuteReader();
            reader.Read();

            container.OrganizationSet.AddObject(Organization.CreateOrganization(0, "DistrictName", reader.Get<string>("DistrictName")));
            container.OrganizationSet.AddObject(Organization.CreateOrganization(0, "OrgName", reader.Get<string>("OrgName")));
            container.OrganizationSet.AddObject(Organization.CreateOrganization(0, "BankAccount", reader.Get<string>("BankAccount")));
            Debug.Print("Imported organization name {0}/{1}", reader.Get<string>("OrgName"), reader.Get<string>("DistrictName"));
        }

        private void ImportMembers(MembersContainer container)
        {
            IDbCommand cmd = _connection.CreateCommand();
            
            cmd.CommandText = @"SELECT 
                                       Department.DepName, Department.SortOrder, Memb.MembId,
                                       Memb.MembNo, Memb.DateOfBirth, Memb.Sex, Memb.NameFamily,
                                       Memb.NameFirst, Memb.Address1, Memb.Address2, Memb.Address3,
                                       Memb.PostalCode1, Memb.PostalCode2, Memb.City, Memb.Country,
                                       Memb.CountryCode, Memb.MembType, Memb.NoMagazine,
                                       Memb.DateChange, Memb.EnterDate, Memb.DateFee, Memb.FeeAmount
                                FROM Memb
                                LEFT JOIN Department ON
                                          (Memb.OrgId = Department.OrgId) AND
                                          (Memb.MembId = Department.MembId);";

            try
            {
                ImportMembers(cmd, container);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        private void ConstructMemberAndUpdate(Member member, IDataRecord reader)
        {
            member.Address.address = reader.Get<string>("Address2");
            member.Address.city = reader.Get<string>("City");
            member.Address.postalcode = reader.Get<string>("PostalCode1");
            member.Address.country = "";

            int membid = reader.Get<int>("MembId");
            member.firstname = reader.Get<string>("NameFirst");
            member.middlenames = "";
            member.lastname = reader.Get<string>("NameFamily");
            member.email = GetEmail(membid, _connection);
            member.phone = GetPhoneNumber(membid, _connection);
            member.mobile = GetMobileNumber(membid, _connection);
            member.birthdate = reader.Get<DateTime>("DateOfBirth").ToString();
            member.sex = reader.Get<string>("Sex");

            // keep resigned/inactive members unchanged
            if(!DBConstants.IsSpecialGroup(member.MemberDetais.membergroup))
                member.MemberDetais.membergroup = reader.Get<string>("DepName");
            member.MemberDetais.memberjob = "";
            member.MemberDetais.memberno = reader.Get<string>("MembNo");
            member.MemberDetais.memberclass = "";
            member.MemberDetais.joindate = reader.Get<DateTime>("EnterDate").ToString();
            member.MemberDetais.changeddate = reader.Get<DateTime>("DateChange").ToString();
            member.MemberDetais.exitdate = "";
            member.MemberDetais.paymentstatus = GetPaymentStatus(reader);
        }

        private static string GetPaymentStatus(IDataRecord reader)
        {
            return String.Format(CultureInfo.CurrentCulture,
                                    "{0};{1}", reader.Get<DateTime>("DateFee"),
                                            reader.Get<Single>("FeeAmount"));
        }

        private static string GetEmail(long membid, IDbConnection conn)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT LocalNo FROM IT_information WHERE MembId = @MembId AND Descr = 'Sähköposti'";
            ((OleDbParameterCollection)cmd.Parameters).AddWithValue("@MembId", membid);
            return String.Format(CultureInfo.CurrentCulture, "{0}", cmd.ExecuteScalar());
        }

        private static string GetMobileNumber(long membid, IDbConnection conn)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT AreaNo,LocalNo FROM IT_information WHERE MembId = @MembId AND Descr = 'Matkapuhelin'";
            ((OleDbParameterCollection)cmd.Parameters).AddWithValue("@MembId", membid);
            IDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                return String.Format(CultureInfo.CurrentCulture, "{0} {1}", reader["AreaNo"], reader["LocalNo"]);
            return "";
        }

        private static string GetPhoneNumber(long membid, IDbConnection conn)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT AreaNo,LocalNo FROM IT_information WHERE MembId = @MembId AND Descr = 'Puhelin'";
            ((OleDbParameterCollection)cmd.Parameters).AddWithValue("@MembId", membid);
            IDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                return String.Format(CultureInfo.CurrentCulture, "{0} {1}", reader["AreaNo"], reader["LocalNo"]);
            return "";
        }
    }
}
