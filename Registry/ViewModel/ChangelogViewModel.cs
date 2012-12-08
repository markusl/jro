using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Database;
using Registry.Interfaces;
using System.Windows.Data;

namespace Registry.ViewModel
{
    /// <summary>
    /// Represents a changelog view for the UI.
    /// </summary>
    public class ChangelogViewModel : ClosableViewModel, IDisposable
    {
        public static readonly string Name = Properties.Resource.ChangeLog;
        private ICollection<CheckableDataItem> _dataFilters = new List<CheckableDataItem>();
        private ObservableCollection<ChangelogRowViewModel> _allChanges = new ObservableCollection<ChangelogRowViewModel>();
        ICollectionView _allChangesCollectionView;
        private IMainWindow _mainWindow;

        /// <summary>
        /// Display this amount of changes immediately when opening the changelog view.
        /// Displaying the rest of the entries is then queued to increase responsiveness of the UI.
        /// </summary>
        private const int _numberOfChangesToDisplayImmediately = 20;

        /// <summary>
        /// Construct new changelog view, constructs the viewmodels for individual changelog entries.
        /// </summary>
        /// <param name="mainWindow"></param>
        public ChangelogViewModel(IMainWindow mainWindow)
        {
            base.DisplayName = Name;
            _mainWindow = mainWindow;

            BuildDataFilters();
            CreateCollectionViewSource();

            _mainWindow.Dispatcher.BeginInvoke(new Action(() => ListChanges()), DispatcherPriority.Background);
        }

        private void CreateCollectionViewSource()
        {
            CollectionViewSource collectionViewSource = new CollectionViewSource();
            collectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("ChangeDate"));
            collectionViewSource.SortDescriptions.Add(new SortDescription("ChangeDate", ListSortDirection.Descending));
            collectionViewSource.Source = _allChanges;
            _allChangesCollectionView = collectionViewSource.View;
            _allChangesCollectionView.Refresh();
        }

        /// <summary>
        /// Get the list of togglable data filters that affect what is shown in the list.
        /// </summary>
        public ICollection<CheckableDataItem> ShownItems { get { return _dataFilters; } }

        /// <summary>
        /// Returns a collection of all the ChangelogRowViewModel objects.
        /// </summary>
        public ObservableCollection<ChangelogRowViewModel> AllChanges
        {
            get { return _allChanges; }
            private set
            {
                Dispose();
                _allChanges = value;
                OnPropertyChanged("AllChanges");
            }
        }

        public ICollectionView AllChangesCollection
        {
            get { return _allChangesCollectionView; }
        }

        private void BuildDataFilters()
        {
            _dataFilters.Add(new CheckableDataItem(Properties.Resource.NewMembersCheck, DBConstants.NewMember, true));
            _dataFilters.Add(new CheckableDataItem(Properties.Resource.NewContactsCheck, DBConstants.NewContact, true));
            _dataFilters.Add(new CheckableDataItem(Properties.Resource.RemovedContactsCheck, DBConstants.DeleteContact, true));
            _dataFilters.Add(new CheckableDataItem(Properties.Resource.RemovedMembersCheck, DBConstants.NewNonMember, true));
            _dataFilters.Add(new CheckableDataItem(Properties.Resource.ChangesToInfoCheck, DBConstants.EditMember, false));
            _dataFilters.Add(new CheckableDataItem(Properties.Resource.ChangesToContactsCheck, DBConstants.EditContact, false));

            foreach (var filter in _dataFilters)
                filter.PropertyChanged += delegate { ListChanges(); };
        }

        private void ListChanges()
        {
            IEnumerable<string> fields = _dataFilters.Where(filter => filter.IsChecked).Select(filter => filter.DatabaseField);
            var allChanges = _mainWindow.ActiveDatabase.ChangelogSet.AsEnumerable();
            IOrderedEnumerable<ChangelogRowViewModel> changes = allChanges.
                                                                    Where(r => fields.Contains(r.action)).
                                                                    Select(change => new ChangelogRowViewModel(change, _mainWindow.ActiveDatabase)).
                                                                    OrderByDescending(change => change.ChangeTime);

            this.AllChanges.Clear();
            foreach (var change in changes.Take(_numberOfChangesToDisplayImmediately))
                this.AllChanges.Add(change);
            _mainWindow.Dispatcher.BeginInvoke(new Action(() => AppendRestOfTheChanges(changes.Skip(_numberOfChangesToDisplayImmediately))), DispatcherPriority.Background);
        }

        private void AppendRestOfTheChanges(IEnumerable<ChangelogRowViewModel> changes)
        {
            foreach(var change in changes)
            {
                this.AllChanges.Add(change);
            }
        }

        public void Dispose()
        {
            this.AllChanges.Clear();
        }
    }
}
