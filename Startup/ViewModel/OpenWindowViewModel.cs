using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using Database;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Security;
using Registry.ViewModel;
using Registry;

namespace Startup.ViewModel
{
    class OpenWindowViewModel : ViewModelBase
    {
        private RecentItem _recentItem;

        public string Password { get; set; }
        public ObservableCollection<RecentItem> RecentItems { get; set; }
        public RecentItem RecentItem
        {
            get { return _recentItem; }
            set
            {
                if (_recentItem == value)
                    return;
                _recentItem = value;
                OnPropertyChanged("RecentItem");
            }
        }

        /// <summary>
        /// Raised when this view should be closed.
        /// </summary>
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        
        internal void Initialize()
        {
            HandleCommandLine(App.Args);
            PopulateRecentItems();
        }

        private void HandleCommandLine(string[] arguments)
        {
            if(arguments.Length > 1)
            {
                _recentItem = new RecentItem(new FileInfo(arguments[0]));
                this.Password = arguments[1];
                OpenDatabase();
                this.OnRequestClose();
            }
        }

        private void PopulateRecentItems()
        {
            List<RecentItem> items = new List<RecentItem>();
            RecentFiles.GetRecentFiles().ForEach(x => items.Add(new RecentItem(x)));
            if (items.Count > 0)
            {
                RecentItem = items[0];
            }

            RecentItems = new ObservableCollection<RecentItem>(items);
        }

        private void OpenDatabase()
        {
            try
            {
                DateTime startTime = DateTime.Now; // count the time used to open

                DatabaseHandler handler = new DatabaseHandler(_recentItem.File.FullName, Password);
                RecentFiles.AddNewFile(_recentItem.File.FullName);
                handler.Open();

                Registry.View.MainWindow mw = new Registry.View.MainWindow();
                MainWindowViewModel viewModel = new MainWindowViewModel(handler.Database, mw.Dispatcher, startTime);
                mw.DataContext = viewModel;
                mw.Show();
                OnRequestClose();
            }
            catch (ArgumentException ex)
            {
#if DEBUG
                MessageBox.Show(ex.ToString(), Properties.Resources.DatabaseCannotBeOpenedInvalidPassword);
#else
                MessageBox.Show(Properties.Resources.DatabaseCannotBeOpenedInvalidPassword, Properties.Resources.DatabaseCannotBeOpenedInvalidPassword);
#endif
            }
        }

        private void OnRequestBrowse()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                RecentItems.Insert(0, new RecentItem(new FileInfo(dialog.FileName)));
                RecentItem = RecentItems[0];
            }
        }

        private void OnCreateNewCommand()
        {
            new View.CreateDatabaseWindow().Show();
            OnRequestClose();
        }

        ICommand _openCommand;
        ICommand _cancelCommand;
        ICommand _createNewCommand;
        ICommand _browseCommand;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                    _openCommand = new RelayCommand(param => this.OpenDatabase());

                return _openCommand;
            }
        }

        public ICommand CreateNewCommand
        {
            get
            {
                if (_createNewCommand == null)
                    _createNewCommand = new RelayCommand(param => this.OnCreateNewCommand());

                return _createNewCommand;
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                    _browseCommand = new RelayCommand(param => this.OnRequestBrowse());

                return _browseCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(param => this.OnRequestClose());

                return _cancelCommand;
            }
        }
    }
}
