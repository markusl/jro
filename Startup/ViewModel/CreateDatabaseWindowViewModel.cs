using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Security;
using System.ComponentModel;
using Microsoft.Win32;
using Database;
using Registry.ViewModel;
using Registry;
using System.Globalization;

namespace Startup.ViewModel
{
    class CreateDatabaseWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        public DatabaseCreationInfo DatabaseInfo { get; set; }

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

        public CreateDatabaseWindowViewModel()
        {
            this.DatabaseInfo = new DatabaseCreationInfo();
        }

        private void CreateDatabase()
        {
            DatabaseHandler handler = DoCreateDatabase();
            if (handler != null)
            {
                OpenCreatedDatabase(handler);
                OnRequestClose();
            }
        }

        private static void OpenCreatedDatabase(DatabaseHandler handler)
        {
            Registry.View.MainWindow mw = new Registry.View.MainWindow();
            MainWindowViewModel viewModel = new MainWindowViewModel(handler.Database, mw.Dispatcher, DateTime.Now);
            mw.DataContext = viewModel;
            mw.Show();
        }

        private DatabaseHandler DoCreateDatabase()
        {
            try
            {
                if (File.Exists(DatabaseInfo.FullPathToDatabase))
                    throw new System.IO.IOException(String.Format(CultureInfo.CurrentCulture,
                                                                 Properties.Resources.AFileWithThisNameAlreadyExistsOnDisk,
                                                                 DatabaseInfo.FullPathToDatabase));

                DatabaseHandler handler = new DatabaseHandler(DatabaseInfo.FullPathToDatabase, DatabaseInfo.DatabasePassword);
                handler.Open();

                RecentFiles.AddNewFile(DatabaseInfo.FullPathToDatabase);
                return handler;
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
            return null;
        }

        private void OnRequestBrowse()
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.Description = Properties.Resources.ChooseDirectoryForRegistry;

            if (dialog.ShowDialog() == true)
            {
                DatabaseInfo.DatabasePath = dialog.SelectedPath;
                OnPropertyChanged("DatabaseInfo");
            }
        }

        ICommand _cancelCommand;
        ICommand _createNewCommand;
        ICommand _browseCommand;

        public ICommand CreateNewCommand
        {
            get
            {
                if (_createNewCommand == null)
                    _createNewCommand = new RelayCommand(param => this.CreateDatabase());

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
                    _cancelCommand = new RelayCommand(param => this.Cancel());

                return _cancelCommand;
            }
        }

        private void Cancel()
        {
            // if canceled, return back to login window
            new View.OpenWindow().Show();
            this.OnRequestClose();
        }

        public string Error
        {
            get { return DatabaseInfo.Error; }
        }

        public string this[string columnName]
        {
            get { return DatabaseInfo[columnName]; }
        }
    }
}
