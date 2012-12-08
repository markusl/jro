using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.Data.EntityClient;
using System.Data.Objects;
using System.IO;

namespace DatabaseTest
{
    [TestClass]
    public class TestDatabase : DatabaseTestBase
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
        }

        [TestMethod]
        public void TestCreatedDatabaseContainsNoMembers()
        {
            Assert.AreEqual(0, _container.MemberSet.Count());
        }

        [TestMethod]
        public void TestDatabaseCanBeOpenedAfterClosing()
        {
            _handler.CloseDatabase();
            _handler.Open();
            _container = _handler.Database;
            Assert.AreEqual(0, _container.MemberSet.Count());
        }

        [TestMethod]
        public void TestOpeningExistingDatabaseCreatesBakFile()
        {
            _handler.CloseDatabase();
            _handler.Open();
            _container = _handler.Database;

            Assert.IsTrue(File.Exists(_testDatabaseName + ".bak"),
                          "Bak-file should be created when db is opened");
        }

        [TestMethod]
        public void TestAddMemberToDatabase()
        {
            Member m = Member.CreateMember(0, "", "", "", "", "", "", "", "");
            m.Address = Address.CreateAddress(0, "", "", "", "");
            m.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");

            _container.MemberSet.AddObject(m);
            _container.SaveChanges();

            _handler.CloseDatabase();
            _handler.Open();
            _container = _handler.Database;

            Assert.AreEqual(1, _container.MemberSet.Count());
            Assert.AreEqual(1, _container.AddressSet.Count());
            Assert.AreEqual(1, _container.MemberDetaisSet.Count());
        }

        [TestMethod]
        public void TestAddedMembersGetDifferentIds()
        {
            Member m = Member.CreateMember(0, "a", "", "", "", "", "", "", "");
            m.Address = Address.CreateAddress(0, "", "", "", "");
            m.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");

            Member m2 = Member.CreateMember(0, "b", "", "", "", "", "", "", "");
            m2.Address = Address.CreateAddress(0, "", "", "", "");
            m2.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");

            _container.MemberSet.AddObject(m);
            _container.MemberSet.AddObject(m2);
            _container.SaveChanges();

            Assert.AreEqual(2, _container.MemberSet.Count());

            Assert.AreNotEqual(
                _container.MemberSet.AsEnumerable().Where(me => me.firstname.Equals("a")).First().Id,
                _container.MemberSet.AsEnumerable().Where(me => me.firstname.Equals("b")).First().Id);
        }

        [TestMethod]
        public void TestAddingContact()
        {
            Member m = Member.CreateMember(0, "a", "", "", "", "", "", "", "");
            m.Address = Address.CreateAddress(0, "", "", "", "");
            m.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");
            _container.MemberSet.AddObject(m);

            Contact c = Contact.CreateContact(0, "", "", "", "", "");
            c.Address = Address.CreateAddress(0, "", "", "", "");
            m.Contact.Add(c);
            _container.SaveChanges();

            Assert.AreEqual(1, _container.MemberSet.AsEnumerable().
                                    Where(me => me.firstname.Equals("a")).First().Contact.Count);
        }
    }
}
