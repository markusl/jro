using System;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Registry.Utilities;
using Registry.ViewModel;
using Registry.Interfaces;

namespace Registry.Menues
{
    /// <summary>
    /// Provides a menu for displaying application log view.
    /// </summary>
    class ShowLogMenu
    {
        IMainWindow _mainWindow;
        [ImportingConstructor]
        public ShowLogMenu(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        [Export("MainWindowMenuItem")]
        private MenuItem BuildMemberByGroupViewsMenu
        {
            get
            {
                return new MenuItem()
                {
                    Header = "Loki",
                    ToolTip = "Avaa muutoshistoria",
                    Command = new RelayCommand(delegate
                    {
                        if (!_mainWindow.SetActiveTab(ChangelogViewModel.Name))
                        {
                            _mainWindow.DisplayTab(new ChangelogViewModel(_mainWindow));
                            _mainWindow.SetActiveTab(ChangelogViewModel.Name);
                        }
                    })
                };
            }
        }
    }
}
