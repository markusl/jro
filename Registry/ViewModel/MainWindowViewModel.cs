using System;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

using Registry.Interfaces;
using Database;
using Registry.Utilities;
using System.Windows.Input;
using Export.Interfaces;

namespace Registry.ViewModel
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    public sealed class MainWindowViewModel : ViewModelBase, IMainWindow
    {
        private bool _exportInProgress = false;
        private int _exportProgress = 0;
        public MembersContainer ActiveDatabase { get; private set; }
        public Dispatcher Dispatcher { get; private set; }

        /// <summary>
        /// Returns the available TabPages to display.
        /// </summary>
        public ObservableCollection<ClosableViewModel> TabPages { get; private set; }

        /// <summary>
        /// Returns whether the application is currently exporting a document.
        /// </summary>
        public bool ExportInProgress {
            get { return _exportInProgress; }
            set { _exportInProgress = value; OnPropertyChanged("ExportInProgress"); }
        }

        /// <summary>
        /// Returns the status of export progress.
        /// </summary>
        public int ExportProgress {
            get { return _exportProgress; }
            set { _exportProgress = value; OnPropertyChanged("ExportProgress"); }
        }

        /// <summary>
        /// Get or set the application MainWindow title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Construct new application MainWindow.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="dispatcher"></param>
        /// <param name="startTime"></param>
        public MainWindowViewModel(MembersContainer container, Dispatcher dispatcher, DateTime startTime)
        {
            SynchronizedMemberList.RefreshFinished += (s, e) => dispatcher.BeginInvoke(new Action<DateTime>(Compose), startTime);
            SynchronizedMemberList.RefreshFinished += (s, e) => SetTitle();
            SynchronizedMemberList.Initialize(container);

            this.ActiveDatabase = container;
            this.Dispatcher = dispatcher;

            this.TabPages = new ObservableCollection<ClosableViewModel>();
            this.TabPages.CollectionChanged += this.OnTabPagesChanged;

            ShowDefaultView();
        }

        public void ShowDefaultView()
        {
            // Implement default view by showing list of all members
            DisplayFilteredTab(new ShowAllMembersFilter());
        }

        /// <summary>
        /// Purpose of this function is to load up list of importers we have.
        /// </summary>
        private void Compose(DateTime startTime)
        {
            if(this.MenuCommandsItems == null)
            {
                CompositionHelper.ComposeInAssemblyWithExported<Dispatcher, IMainWindow>(this, this.Dispatcher,
                                                                                         this, typeof(IExportService).Assembly);

                OnPropertyChanged("MenuCommandsItems");
                OnPropertyChanged("ExportService");
                TimeSpan ts = DateTime.Now - startTime;
                Debug.Print("Main window displayed and member list populated in {0}", ts.ToString());
                ExportService.AllExportsCompleted += AllExportsCompletedAction.ExportService_AllExportsCompleted;
            }
        }

        [ImportMany(typeof(IMemberViewFilter))]
        public List<IMemberViewFilter> ViewFilters { get; private set; }

        [ImportMany("MainWindowMenuItem")]
        public Collection<MenuItem> MenuCommandsItems { get; private set; }

        [Import(typeof(IExportService))]
        public IExportService ExportService { get; private set; }

        public void DisplayFilteredTab(IMemberViewFilter filter)
        {
            if (!SetActiveTab(filter.Name))
            {
                DisplayTab(new MemberListViewModel(filter.Name, this.ActiveDatabase,
                            this.Dispatcher, new MemberCollectionView(filter.Filter)));
                SetActiveTab(filter.Name);
            }
        }

        public void DisplayTab(ClosableViewModel vmb)
        {
            this.TabPages.Add(vmb);
        }

        private void AddAndShowTab(ClosableViewModel vmb)
        {
            this.TabPages.Add(vmb);
            SetActiveTab(vmb);
        }

        public bool SetActiveTab(string name)
        {
            ViewModelBase vmb = this.TabPages.FirstOrDefault(vm => vm.DisplayName.Equals(name));
            if (vmb != null)
            {
                SetActiveTab(vmb);
                return true;
            }
            return false;
        }

        public void SetActiveTab(ViewModelBase vmb)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.TabPages);
            collectionView.MoveCurrentTo(vmb);
        }

        private void SetTitle()
        {
            Organization org = this.ActiveDatabase.OrganizationSet.Where(o => o.key.Equals("OrgName")).FirstOrDefault();

            if (org != null)
                Title = String.Format("{0}:n jäsenrekisteri", org.value);
            else
                Title = "<Please import a database>";
            OnPropertyChanged("Title");
        }

        void OnTabPagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ClosableViewModel tab in e.NewItems)
                    tab.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ClosableViewModel tab in e.OldItems)
                    tab.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            ClosableViewModel tab = sender as ClosableViewModel;
            this.TabPages.Remove(tab);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            ActiveDatabase.Dispose();
        }
    }
}
