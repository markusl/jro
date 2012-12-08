using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Linq;
using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
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
        IMainWindow _mainWindow;
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
                MenuItem importExportItem = new MenuItem() { Header = "Tuo ja vie" };

                importExportItem.Items.Add(new MenuItem()
                {
                    Header = "Tuo ClubOnWeb-tietokanta",
                    Command = new RelayCommand(param => this.OnStartImport())
                });

                importExportItem.Items.Add(new MenuItem()
                {
                    Header = "Vie yhdistyksen jäsenluettelo",
                    Command = new RelayCommand(delegate { Exporters.ExportMemberList(_mainWindow); })
                });
                importExportItem.Items.Add(new MenuItem()
                {
                    Header = "Vie osoitetiedot",
                    Command = new RelayCommand(delegate { Exporters.ExportMemberAddress(_mainWindow); })
                });
#if DEBUG
                importExportItem.Items.Add(new MenuItem()
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
            MenuItem exportMemberGroupsMenu = new MenuItem() { Header = "Vie ryhmän jäsenluettelo" };

            IEnumerable<Member> members = container.MemberSet.AsEnumerable();
            foreach (var group in members.Select(member => member.MemberDetais.membergroup).Distinct())
            {
                string localGroup = group;
                exportMemberGroupsMenu.Items.Add(
                    new MenuItem()
                    {
                        Header = localGroup,
                        Command = new RelayCommand(delegate { Exporters.ExportSingleMemberGroupList(localGroup, _mainWindow); })
                    });
            }
            exportMemberGroupsMenu.Items.Add(new MenuItem()
            {
                Header = "Kaikki ryhmät",
                Command = new RelayCommand(delegate { Exporters.ExportAllMemberGroupLists(_mainWindow); })
            });
            return exportMemberGroupsMenu;
        }

        private void OnStartImport()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Database files|*.mdb|All files|*.*";
            if (dialog.ShowDialog().Value)
            {
                ImportWindowViewModel iwvm = new ImportWindowViewModel(_mainWindow.ActiveDatabase, new Import.CowImport());
                iwvm.ImportFileName = dialog.FileName;

                View.ImportWindow importWindow = new View.ImportWindow();
                iwvm.RequestClose += delegate { importWindow.Close(); };
                importWindow.DataContext = iwvm;
                importWindow.Show();

                iwvm.StartImport();
            }
        }
    }
}
