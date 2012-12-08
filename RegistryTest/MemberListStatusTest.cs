using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registry.Interfaces;
using Registry.Utilities;
using Registry.ViewModel;

namespace RegistryTest
{
    [TestClass]
    public class MemberListStatusTest : TestUsingSampleData
    {
        MemberListViewFake _memberList = new MemberListViewFake();

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _memberList.AllItems = new ObservableCollection<MemberViewModel>();
            _memberList.SelectedItems = new ObservableCollection<MemberViewModel>();
            foreach (var m in _handler.Database.MemberSet.AsEnumerable())
                _memberList.AllItems.Add(new MemberViewModel(m));
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void StatusText_ContainsNumberOfItemsInDatabase()
        {
            CompositionHelper.ComposeInExecutingAssemblyWithExported<IMemberListView>(_memberList);
            Assert.IsTrue(_memberList.StatusText.Status.Contains(String.Format("{0}", _numberOfMembersInTestDatabase)));
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void StatusText_ContainsNumberOfSelectedItems()
        {
            const int Number = 8;
            foreach (var m in _memberList.AllItems.AsEnumerable().Take(Number))
            {
                _memberList.SelectedItems.Add(m);
            }
            CompositionHelper.ComposeInExecutingAssemblyWithExported<IMemberListView>(_memberList);
            Assert.IsTrue(_memberList.StatusText.Status.Contains(String.Format("{0}", Number)));
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void StatusText_ContainsZeroIfNoMembers()
        {
            _memberList.AllItems.Clear();
            CompositionHelper.ComposeInExecutingAssemblyWithExported<IMemberListView>(_memberList);
            Assert.IsTrue(_memberList.StatusText.Status.Contains("0 "));
            Assert.IsTrue(_memberList.StatusText.Status.Contains("0,0"));
        }

        /// <summary>
        /// Dummy member list view to hold the needed collections
        /// and to import the status text provider.
        /// </summary>
        class MemberListViewFake : IMemberListView
        {
            /// <summary>
            /// The imported status text provider to test.
            /// </summary>
            [Import(typeof(IMemberListStatus))]
            public IMemberListStatus StatusText { get; private set; }

            public ObservableCollection<Control> MenuOptions
            {
                get { throw new NotImplementedException(); }
            }

            public ObservableCollection<MemberViewModel> SelectedItems
            {
                get;
                set;
            }

            public ObservableCollection<MemberViewModel> AllItems
            {
                get;
                set;
            }
        }
    }
}
