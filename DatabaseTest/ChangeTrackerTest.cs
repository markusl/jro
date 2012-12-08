using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;

namespace DatabaseTest
{
    [TestClass]
    public class ChangeTrackerTest : DatabaseTestBase
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

        private static Member CreateJohnDoe()
        {
            Member m = Member.CreateMember(0, "John", "Doe", "", "", "", "", "", "");
            m.Address = Address.CreateAddress(0, "", "", "", "");
            m.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");
            return m;
        }

        [TestMethod]
        public void TestAddingContactsCreatesChangelogEntries()
        {
            _container.MemberSet.AddObject(CreateJohnDoe());
            _container.MemberSet.AddObject(CreateJohnDoe());
            _container.SaveChanges();

            Assert.AreEqual(2, _container.ChangelogSet.AsEnumerable().
                                    Where(entry => entry.action.Equals(DBConstants.NewMember)).Count());

            // ensure that ids are save so that they can actually be referenced
            Changelog logentry = _container.ChangelogSet.AsEnumerable().Where(entry => entry.action.Equals(DBConstants.NewMember)).First();
            Member member = _container.MemberSet.AsEnumerable().Where(mem => mem.Id == long.Parse(logentry.memberid)).FirstOrDefault();
            Assert.IsNotNull(member);
        }

        public static void TestChangingMemberGroupToSystemValueSavesOldValue(MembersContainer container, string systemValue)
        {
            const string group = "TheGroup";
            Member m = CreateJohnDoe();
            m.MemberDetais.membergroup = group;
            container.MemberSet.AddObject(m);
            container.SaveChanges();

            m.MemberDetais.membergroup = systemValue;
            container.SaveChanges();

            IEnumerable<Changelog> entries = container.ChangelogSet.AsEnumerable().
                                    Where(entry => entry.action.Equals(DBConstants.NewNonMember));
            Assert.AreEqual(1, entries.Count());
            Changelog log = entries.First();
            Assert.AreEqual(group, log.oldvalue);
        }

        [TestMethod]
        public void TestMakingResignedMemberCreatesLogEntryWithPreviousGroup()
        {
            TestChangingMemberGroupToSystemValueSavesOldValue(_container, DBConstants.ResignedMember);
        }

        [TestMethod]
        public void TestMakingInactiveMemberCreatesLogEntryWithPreviousGroup()
        {
            TestChangingMemberGroupToSystemValueSavesOldValue(_container, DBConstants.InactiveMember);
        }
    }
}
