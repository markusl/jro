using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registry.Menues;
using Registry.Interfaces;
using Registry.ViewModel;
using Database;
using System.Windows.Controls;
using UnitTests;

namespace RegistryTest
{
    class MemberListViewFake : IMemberListView
    {
        private ObservableCollection<System.Windows.Controls.Control> _opts = new ObservableCollection<System.Windows.Controls.Control>();
        private ObservableCollection<MemberViewModel> items = new ObservableCollection<MemberViewModel>();

        public ObservableCollection<MemberViewModel> SelectedItems
        {
            get { return items; }
        }

        public ObservableCollection<MemberViewModel> AllItems
        {
            get { return SynchronizedMemberList.Members; }
        }

        public ObservableCollection<System.Windows.Controls.Control> MenuOptions
        {
            get { return _opts; }
            set { _opts = value; }
        }
    }

    [TestClass]
    public class MemberListViewMenuTest : TestUsingSampleData
    {
        MemberListViewFake fake;
        MemberListViewMenu menu;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            fake = new MemberListViewFake();
            menu = new MemberListViewMenu(fake, _handler.Database);
            SynchronizedMemberList.Initialize(_handler.Database);

            AutoResetEvent evt = new AutoResetEvent(false);
            SynchronizedMemberList.RefreshFinished += delegate { evt.Set(); };

            // wait until the member list is constructed
            DispatcherUtil.DoEventsUntil(evt);
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMenuOptions_IsEmptyWhenNoMembersAreSelected()
        {
            Assert.AreEqual(0, fake.MenuOptions.Count);
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMenuOptions_CreatesMenuItemsWhenSelectingAnItem()
        {
            AutoResetEvent evt = new AutoResetEvent(false);

            fake.MenuOptions.CollectionChanged += delegate { evt.Set(); };
            fake.SelectedItems.Add(fake.AllItems.First());

            DispatcherUtil.DoEventsUntil(evt);

            Assert.AreEqual(6, fake.MenuOptions.Count);
            Assert.AreEqual(3, (fake.MenuOptions[0] as MenuItem).Items.Count);
        }

        private Contact ConstructAndAddNewContact()
        {
            Contact c = Contact.CreateContact(0, "New", "Contact", "", "", "");
            c.Address = Address.CreateAddress(0, "", "", "", "");
            _handler.Database.ContactSet.AddObject(c);
            return c;
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestMenuOptions_RebuildsWhenMemberContainerChanges()
        {
            // The member object we are interested in, just use the first one
            Member selectedMember = _handler.Database.MemberSet.First();

            // select the member by the ViewModel
            fake.SelectedItems.Add(fake.AllItems.FindSame(selectedMember));

            // set up a collection changed event to check that menu is changed
            AutoResetEvent evt = new AutoResetEvent(false);
            fake.MenuOptions.CollectionChanged += delegate { evt.Set(); };

            // change member settings, add new contact and sync database
            selectedMember.Contact.Add(ConstructAndAddNewContact());
            _handler.Database.SaveChanges();

            // build new menu asynchronously
            DispatcherUtil.DoEventsUntil(evt);

            Assert.AreEqual(6, fake.MenuOptions.Count);
            Assert.AreEqual(4, (fake.MenuOptions[0] as MenuItem).Items.Count);
            Assert.AreEqual("New Contact", ((fake.MenuOptions[0] as MenuItem).Items[2] as MenuItem).Header);
        }
    }
}
