using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Linq;
using Microsoft.Win32;
using System.Collections.Generic;
using Database;
using Registry.ViewModel;
using Registry.Interfaces;

namespace Registry.Menues
{
    /// <summary>
    /// Provides the default Import and Export menu for Main window
    /// </summary>
    class ExportMenu
    {
        readonly IMainWindow _mainWindow;
        [ImportingConstructor]
        public ExportMenu(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// The getter to use from XAML.
        /// </summary>
        [Export("MainWindowMenuItem")]
        public MenuItem BuildImportExPortMenu
        {
            get
            {
                var importExportItem = new MenuItem { Header = "Tuo ja vie" };

                importExportItem.Items.Add(new MenuItem
                {
                    Header = "Tuo ClubOnWeb-tietokanta",
                    Command = new RelayCommand(param => this.OnStartImport())
                });

                importExportItem.Items.Add(new MenuItem
                {
                    Header = "Vie yhdistyksen jäsenluettelo",
                    Command = new RelayCommand(delegate { Exporters.ExportMemberList(_mainWindow); })
                });
                importExportItem.Items.Add(new MenuItem
                {
                    Header = "Vie osoitetiedot",
                    Command = new RelayCommand(delegate { Exporters.ExportMemberAddress(_mainWindow); })
                });
#if DEBUG
                importExportItem.Items.Add(new MenuItem
                {
                    Header = "Vie CSV",
                    Command = new RelayCommand(delegate { Exporters.ExportCustom(_mainWindow); })
                });
#endif
                importExportItem.Items.Add(BuildExportMemberGroupsMenu());

                return importExportItem;
            }
        }

        private MenuItem BuildExportMemberGroupsMenu()
        {
            MembersContainer container = _mainWindow.ActiveDatabase;
            var exportMemberGroupsMenu = new MenuItem { Header = "Vie ryhmän jäsenluettelo" };

            IEnumerable<Member> members = container.MemberSet.AsEnumerable();
            foreach (var group in members.Select(member => member.MemberDetais.membergroup).Distinct())
            {
                string localGroup = group;
                exportMemberGroupsMenu.Items.Add(
                    new MenuItem
                    {
                        Header = localGroup,
                        Command = new RelayCommand(delegate { Exporters.ExportSingleMemberGroupList(localGroup, _mainWindow); })
                    });
            }
            exportMemberGroupsMenu.Items.Add(new MenuItem
            {
                Header = "Kaikki ryhmät",
                Command = new RelayCommand(delegate { Exporters.ExportAllMemberGroupLists(_mainWindow); })
            });
            return exportMemberGroupsMenu;
        }

        private void OnStartImport()
        {
            var dialog = new OpenFileDialog {Filter = "Database files|*.mdb|All files|*.*"};
            if (dialog.ShowDialog().Value)
            {
                var iwvm = new ImportWindowViewModel(_mainWindow.ActiveDatabase, new Import.CowImport())
                               {ImportFileName = dialog.FileName};

                var importWindow = new View.ImportWindow();
                iwvm.RequestClose += delegate { importWindow.Close(); };
                importWindow.DataContext = iwvm;
                importWindow.Show();

                iwvm.StartImport();
            }
        }
    }
}
