using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Win32;
using Registry.ViewModel;
using Registry.Interfaces;

namespace Registry.Menues
{
#if DEBUG
    /// <summary>
    /// Provides developer tools.
    /// </summary>
    class DebugMenu
    {
        readonly IMainWindow _mainWindow;
        [ImportingConstructor]
        public DebugMenu(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        
        [Export("MainWindowMenuItem")]
        public MenuItem BuildImportExPortMenu
        {
            get
            {
                var importExportItem = new MenuItem { Header = "Debug" };

                importExportItem.Items.Add(new MenuItem
                {
                    Header = "Import sample database from XML",
                    Command = new RelayCommand(param => this.OnStartImport())
                });

                importExportItem.Items.Add(new MenuItem
                {
                    Header = "Display debug log",
                    Command = new RelayCommand(delegate { this.DisplayDebugLog(); })
                });

                return importExportItem;
            }
        }

        private void DisplayDebugLog()
        {
            _mainWindow.DisplayTab(ViewModel.DebugLogViewModel.Instance);
        }

        private void OnStartImport()
        {
            var dialog = new OpenFileDialog {Filter = "Database files|*.mdb|All files|*.*"};
            if (dialog.ShowDialog().Value)
            {
                var iwvm = new ImportWindowViewModel(_mainWindow.ActiveDatabase, new Import.SampleXmlImport())
                               {ImportFileName = dialog.FileName};

                var importWindow = new View.ImportWindow();
                iwvm.RequestClose += delegate { importWindow.Close(); };
                importWindow.DataContext = iwvm;
                importWindow.Show();

                iwvm.StartImport();
            }
        }

    }
#endif // DEBUG
}
