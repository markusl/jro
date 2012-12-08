using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registry.ViewModel;
using System.IO;
using Database;
using Import;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.Specialized;
using UnitTests;

namespace RegistryTest
{
    [TestClass]
    public class SynchronizedMemberListTest : TestUsingSampleData
    {
        private void InitializeMembersListWithEvent(AutoResetEvent evt)
        {
            SynchronizedMemberList.RefreshFinished += delegate { evt.Set(); };
            SynchronizedMemberList.Initialize(_handler.Database);

            // wait until the list is fully constructed
            DispatcherUtil.DoEventsUntil(evt);

            evt.Reset();
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestInitializeFillsTheMembersList()
        {
            base.Initialize();
            AutoResetEvent evt = new AutoResetEvent(false);
            InitializeMembersListWithEvent(evt);

            Assert.AreEqual(_numberOfMembersInTestDatabase, SynchronizedMemberList.Members.Count);
        }
        
        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestNewMembersAreAddedToTheMembersList()
        {
            base.Initialize();
            AutoResetEvent evt = new AutoResetEvent(false);
            InitializeMembersListWithEvent(evt);

            Assert.AreEqual(_numberOfMembersInTestDatabase, SynchronizedMemberList.Members.Count);

            // add a new member
            Member m = Member.CreateMember(0, "a", "b", "", "", "", "", "", "");
            m.Address = Address.CreateAddress(0, "", "", "", "");
            m.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");

            _handler.Database.MemberSet.AddObject(m);
            _handler.Database.SaveChanges();          // signal the change

            // wait until we get the RefreshFinished event again
            DispatcherUtil.DoEventsUntil(evt);

            // member count should now be up by one
            Assert.AreEqual(_numberOfMembersInTestDatabase + 1, SynchronizedMemberList.Members.Count,
                            "Invalid number of members");
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMemberGroupModifyReAddsMemberToList()
        {
            base.Initialize();
            // initialize the list normally
            AutoResetEvent evt = new AutoResetEvent(false);
            InitializeMembersListWithEvent(evt);

            bool memberAdded = false, memberRemoved = false;
            SynchronizedMemberList.Members.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add) { Assert.IsFalse(memberAdded); memberAdded = true; }
                else if (e.Action == NotifyCollectionChangedAction.Remove) { Assert.IsFalse(memberRemoved); memberRemoved = true; }
            };

            // modify the member
            Member member = _handler.Database.MemberSet.First();
            member.MemberDetais.membergroup = "new_group_for_member";
            _handler.Database.SaveChanges();          // signal the change

            // wait until we get the RefreshFinished event again
            DispatcherUtil.DoEventsUntil(evt);

            // member count should be the same
            Assert.AreEqual(_numberOfMembersInTestDatabase, SynchronizedMemberList.Members.Count);
            // member should have been re-added to get UI refreshed
            Assert.IsTrue(memberAdded);
            Assert.IsTrue(memberRemoved); 
        }
    }
}
