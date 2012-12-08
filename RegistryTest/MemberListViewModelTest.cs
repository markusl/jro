using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registry.ViewModel;
using System.Windows.Threading;
using System.Windows.Data;

namespace RegistryTest
{
    [TestClass]
    public class MemberListViewModelTest : TestUsingSampleData
    {
        MemberListViewModel _memberListView;
        Dispatcher _dispatcher;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            SynchronizedMemberList.InitializeNonAsync(_handler.Database);
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMembersList_CanBeInitializedWithNullFilter()
        {
            _memberListView = new MemberListViewModel("Default", _handler.Database, _dispatcher, new MemberCollectionView());
            Assert.AreEqual(85, _memberListView.AllItems.Count);
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMembersList_ContainsDefinedGroups()
        {
            _memberListView = new MemberListViewModel("Default", _handler.Database, _dispatcher, new MemberCollectionView());
            Assert.AreEqual(21, _memberListView.Collection.Groups.Count);
        }

        private int CountMembersInGroups()
        {
            int members = 0;
            foreach (CollectionViewGroup group in _memberListView.Collection.Groups)
            {
                members += group.ItemCount;
            }
            return members;
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMembersList_ContainsCustomFilteredMembers()
        {
            Predicate<MemberViewModel> nameFilter = delegate(MemberViewModel member) { return member.DisplayName.Contains("Miles"); };
            _memberListView = new MemberListViewModel("Default", _handler.Database, _dispatcher, new MemberCollectionView(nameFilter));
            Assert.AreEqual(2, _memberListView.Collection.Groups.Count);
            Assert.AreEqual(2, CountMembersInGroups());
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMembersList_ContainsMembersFilteredByString()
        {
            _memberListView = new MemberListViewModel("Default", _handler.Database, _dispatcher, new MemberCollectionView());
            _memberListView.FilterString = "miles";
            Assert.AreEqual(2, _memberListView.Collection.Groups.Count);
            Assert.AreEqual(2, CountMembersInGroups());
        }
    }
}
