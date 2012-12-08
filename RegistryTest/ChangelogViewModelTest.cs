using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registry.ViewModel;
using System.Windows.Threading;
using Registry.Interfaces;
using Export.Interfaces;
using Database;
using UnitTests;

namespace RegistryTest
{
    class FakeMainWindow : IMainWindow
    {
        Database.MembersContainer _database;
        public FakeMainWindow(Database.MembersContainer database)
        {
            _database = database;
        }
        public void ShowDefaultView()
        {
            throw new NotImplementedException();
        }

        public void DisplayTab(ClosableViewModel tab)
        {
            throw new NotImplementedException();
        }

        public void SetActiveTab(ViewModelBase tab)
        {
            throw new NotImplementedException();
        }

        public void DisplayFilteredTab(IMemberViewFilter filter)
        {
            throw new NotImplementedException();
        }

        public bool SetActiveTab(string name)
        {
            throw new NotImplementedException();
        }

        public Database.MembersContainer ActiveDatabase
        {
            get { return _database; }
        }

        public IExportService ExportService
        {
            get { throw new NotImplementedException(); }
        }

        public Dispatcher Dispatcher
        {
            get { return Dispatcher.CurrentDispatcher; }
        }

        public bool ExportInProgress
        {
            get { throw new NotImplementedException(); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            //PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {};
            PropertyChanged(null, null);
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class ChangelogViewModelTest : TestUsingSampleData
    {
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
        public void TestImportingSampleData_HasChangelogHistory()
        {
            IMainWindow mw = new FakeMainWindow(base._handler.Database);
            ChangelogViewModel cvm = new ChangelogViewModel(mw);

            DispatcherUtil.DoEvents();
            Assert.AreEqual(1, cvm.AllChangesCollection.Groups.Count);
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestChangelog_DeletingMembers_CreatesNewDateGroup()
        {
            foreach (var entry in _handler.Database.ChangelogSet.AsEnumerable())
                _handler.Database.ChangelogSet.DeleteObject(entry);

            IMainWindow mw = new FakeMainWindow(base._handler.Database);
            ChangelogViewModel cvm = new ChangelogViewModel(mw);
            Assert.AreEqual(0, cvm.AllChangesCollection.Groups.Count);

            foreach (var member in _handler.Database.MemberSet.Take(6))
            {
                member.MemberDetais.membergroup = DBConstants.ResignedMember;
            }
            _handler.Database.SaveChanges();

            DispatcherUtil.DoEvents();
            Assert.AreEqual(1, cvm.AllChangesCollection.Groups.Count);
        }

        [TestMethod]
        [DeploymentItem(_deploymentItem)]
        public void TestChangelog_DeletingMembers_ChangesTheId()
        {
            long firstId = _handler.Database.MemberSet.First().Id;
            foreach (var member in _handler.Database.MemberSet.Take(1))
            {
                member.MemberDetais.membergroup = DBConstants.ResignedMember;
            }
            _handler.Database.SaveChanges();
            Assert.AreEqual(firstId, _handler.Database.MemberSet.First().Id);
        }
    }
}
