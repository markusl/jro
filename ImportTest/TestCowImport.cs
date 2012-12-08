using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.Data.EntityClient;
using System.Data.Objects;
using System.IO;
using Import;

namespace ImportTest
{
    [TestClass]
    public class TestCowImport
    {
        private DatabaseHandler _handler;
        private string _testDatabaseName = Path.GetTempFileName();
        private const string _01_importDatabase = "01_test_import_database.mdb";
        private const string _02_updateDatabase = "02_test_update_database.mdb";
        private const string _03_importPaymentStatus = "03_test_import_payment_status.mdb";

        [TestInitialize]
        public void Initialize()
        {
            _handler = new DatabaseHandler(_testDatabaseName, "password");
            _handler.Open();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _handler.CloseDatabase();
            File.Delete(_testDatabaseName);
        }

        private void InitialImportDatabase()
        {
            // Import the initial database
            IDatabaseImporter import = new CowImport();
            import.Import(_01_importDatabase, _handler.Database);

            Assert.AreEqual(3, _handler.Database.MemberSet.AsEnumerable().Count());
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        public void TestInitialDatabaseImport_ImportsThreeMembers()
        {
            InitialImportDatabase();
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        public void TestInitialDatabaseImport_ImportSameDatabaseTwice()
        {
            InitialImportDatabase();
            InitialImportDatabase();
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        public void TestInitialDatabaseImport_CreatesChangelogEntriesForNewContacts()
        {
            IDatabaseImporter import = new CowImport();
            import.Import(_01_importDatabase, _handler.Database);

            Assert.AreEqual(3, _handler.Database.ChangelogSet.Count());
            Assert.AreEqual(3, _handler.Database.ChangelogSet.AsEnumerable().
                                Where(c => c.action.Equals(DBConstants.NewMember)).Count());
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        [DeploymentItem(_02_updateDatabase)]
        public void TestDatabaseUpdate_MovesDeletedUsersToNonMemberTable()
        {
            InitialImportDatabase();

            // Import updated database, where a member is removed
            IDatabaseImporter import = new CowImport();
            import.Import(_02_updateDatabase, _handler.Database);

            // one changelog entry should have been created and
            // one member should now be a resigned member
            Assert.AreEqual(1, _handler.Database.ChangelogSet.AsEnumerable().
                                Where(c => c.action.Equals(DBConstants.NewNonMember)).Count());
            Assert.AreEqual(1, _handler.Database.MemberSet.AsEnumerable().Where(m => !m.IsMember()).Count());
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        [DeploymentItem(_02_updateDatabase)]
        public void TestDatabaseUpdate_AddsNewMembers()
        {
            InitialImportDatabase();

            // Import updated database, where a member is removed and a new one is added
            IDatabaseImporter import = new CowImport();
            import.Import(_02_updateDatabase, _handler.Database);

            // membercount should be the same since one new member is added
            Assert.AreEqual(3, _handler.Database.MemberSet.AsEnumerable().Where(m => m.IsMember()).Count());
            Assert.AreEqual(4, _handler.Database.ChangelogSet.AsEnumerable().
                                Where(c => c.action.Equals(DBConstants.NewMember)).Count());
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        public void TestDatabaseUpdate_IgnoresInactiveMembers()
        {
            InitialImportDatabase();

            // Mark all three members as inactive
            foreach (var member in _handler.Database.MemberSet.AsEnumerable())
            {
                member.MemberDetais.membergroup = DBConstants.InactiveMember;
            }
            _handler.Database.SaveChanges();

            // re-import the database
            InitialImportDatabase();

            // members should still be inactive
            foreach (var member in _handler.Database.MemberSet.AsEnumerable())
            {
                Assert.AreEqual(DBConstants.InactiveMember, member.MemberDetais.membergroup);
            }
        }

        [TestMethod]
        [DeploymentItem(_01_importDatabase)]
        [DeploymentItem(_03_importPaymentStatus)]
        public void TestDatabaseUpdate_PaymentStatus()
        {
            InitialImportDatabase();

            IDatabaseImporter import = new CowImport();
            import.Import(_03_importPaymentStatus, _handler.Database);
        }
    }
}
