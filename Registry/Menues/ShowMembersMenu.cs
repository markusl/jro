using System;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Database;
using Registry.ViewModel;
using Registry.Interfaces;
using Registry.Utilities;

namespace Registry.Menues
{
    /// <summary>
    /// Provides the menu for Main window for showing different groups of members
    /// </summary>
    class ShowMembersMenu
    {
        IMainWindow _mainWindow;
        [ImportingConstructor]
        public ShowMembersMenu(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            // Gather list of implemented filters in this assembly.
            CompositionHelper.ComposeInExecutingAssembly(this);
        }

        [ImportMany(typeof(IMemberViewFilter))]
        private List<IMemberViewFilter> Filters { get; set; }

        [Export("MainWindowMenuItem")]
        private MenuItem BuildMemberByGroupViewsMenu
        {
            get
            {
                MenuItem showMembersMenu = new MenuItem() { Header = "Näytä jäsenet" };
                BuildMenuForListingMembers(showMembersMenu);
                BuildMenuForListingMembersByGroup(showMembersMenu);

                return showMembersMenu;
            }
        }

        private void BuildMenuForListingMembers(MenuItem showMembersMenu)
        {
            foreach (var f in this.Filters)
            {
                var filter = f;
                showMembersMenu.Items.Add(new MenuItem()
                {
                    Header = filter.Name,
                    Command = new RelayCommand(delegate
                    {
                        _mainWindow.DisplayFilteredTab(filter);
                    })
                });
            }
        }

        class FilterByGroup : IMemberViewFilter
        {
            public string Group { get; set; }
            public int Priority { get { return 11; } }
            public string Name { get { return Group; } }

            public Predicate<MemberViewModel> Filter {
                get
                {
                    return delegate(MemberViewModel member) { return member.MemberGroup.Equals(Group); };
                }
            }
        }

        private void BuildMenuForListingMembersByGroup(MenuItem showMembersMenu)
        {
            MenuItem item = new MenuItem() { Header = "Näytä ryhmä" };

            IEnumerable<string> groups = _mainWindow.ActiveDatabase.MemberSet.AsEnumerable().
                                            Select(member => member.MemberDetais.membergroup).Distinct();
            foreach (var g in groups)
            {
                var group = g;
                item.Items.Add(new MenuItem()
                {
                    Header = group,
                    Command = new RelayCommand(delegate
                    {
                        _mainWindow.DisplayFilteredTab(new FilterByGroup() { Group = group });
                    })
                });
            }

            showMembersMenu.Items.Add(item);
        }
    }
}
