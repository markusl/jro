using System;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;
using Database;
using Registry.Utilities;
using Registry.Interfaces;
using Registry.ViewModel;

namespace Registry.Menues
{
    /// <summary>
    /// Provides the context menu for members list view.
    /// </summary>
    public class MemberListViewMenu : IDisposable
    {
        private IMemberListView _view;
        private MembersContainer _container;
        private Dispatcher _dispatcher;
        private EventDelayer<bool> _delayMenuRebuild;

        /// <summary>
        /// Construct new menu builder with the view and member container.
        /// </summary>
        public MemberListViewMenu(IMemberListView view, MembersContainer container)
        {
            _container = container;
            _view = view;
            _dispatcher = Dispatcher.CurrentDispatcher;

            // menu needs to be rebuilt when selected items or the members in the container are changed
            _delayMenuRebuild = new EventDelayer<bool>(delegate { RebuildMenu(); return true; }, _dispatcher);
            _container.SavingChanges += delegate { _delayMenuRebuild.DelayEvent(); };
            _view.SelectedItems.CollectionChanged += delegate { _delayMenuRebuild.DelayEvent(); };
        }

        private void RebuildMenu()
        {
            using (new Utilities.DebugTimer("Rebuild context menu"))
            {
                _view.MenuOptions.Clear();
                BuildMenuItems(_view.MenuOptions);
                Debug.Print("Menu items set {0}..", _view.MenuOptions.Count);
            }
        }

        private void BuildMenuItems(ObservableCollection<Control> menu)
        {
            if (_view.SelectedItems.Count > 0)
            {
                GetMenuItemsForSelectedMembers(menu);
            }
            else
            {
                menu.Add(new MenuItem() { Header = "Valitse jäsen" });
            }
        }

        private void GetMenuItemsForSelectedMembers(ObservableCollection<Control> menu)
        {
            foreach (MemberViewModel member in _view.SelectedItems)
            {
                menu.Add(BuildMenuForMember(member));
            }

            menu.Add(new Separator());

            bool allMembers = _view.SelectedItems.All(m => m.IsMember);
            bool allNotMembers = _view.SelectedItems.All(m => !m.IsMember);
            if (allMembers)
                foreach (var action in BuildActionsForActiveMembers())
                    menu.Add(action);
            else if (allNotMembers)
                menu.Add(BuildActionsForNonActiveMembers());
        }

        private ICollection<Control> BuildActionsForActiveMembers()
        {
            ICollection<Control> list = new List<Control>();
            list.Add(new MenuItem()
            {
                Header = "Siirrä epäaktiivisiin",
                Command = new RelayCommand(delegate { MoveToGroup(_view.SelectedItems, DBConstants.InactiveMember); })
            });
            list.Add(new MenuItem()
            {
                Header = "Siirrä poistettuihin",
                Command = new RelayCommand(delegate { MoveToGroup(_view.SelectedItems, DBConstants.ResignedMember); })

            });
            list.Add(new Separator());
            list.Add(new MenuItem()
            {
                Header = "Kopio sähköpostiosoitteet",
                Command = new RelayCommand(delegate { CopyMemberAndContactEmailsToClipBoard(_view.SelectedItems); })

            });
            return list;
        }

        private MenuItem BuildActionsForNonActiveMembers()
        {
            MenuItem moveToGroupItem = new MenuItem()
            {
                Header = "Siirrä ryhmään"
            };
            foreach(var g in _container.MemberDetaisSet.AsEnumerable().Select(m => m.membergroup).Distinct())
            {
                var group = g;
                moveToGroupItem.Items.Add(new MenuItem()
                                {
                                    Header = group,
                                    Command = new RelayCommand((a) => MoveToGroup(_view.SelectedItems, group))
                                });
            }

            return moveToGroupItem;
        }

        private static IEnumerable<string> GetEmails(ICollection<MemberViewModel> members)
        {
            List<string> emails = new List<string>();
            foreach (var member in members)
            {
                var contacts = member.GetRelatedContacts().Where(c => !String.IsNullOrEmpty(c.email));
                emails.AddRange(contacts.Select(x => x.email));
            }
            emails.AddRange(members.Where(m => !String.IsNullOrEmpty(m.Email)).Select(c => c.Email));

            return emails.Distinct();
        }

        private void CopyMemberAndContactEmailsToClipBoard(ICollection<MemberViewModel> members)
        {
            IEnumerable<string> emails = GetEmails(members);
            string emailString = String.Join(", ", emails);
            Clipboard.SetText(emailString);
            MessageBox.Show(emailString, "Copied to clipboard");
        }

        private void MoveToGroup(ICollection<MemberViewModel> members, string group)
        {
            foreach (var member in members)
            {
                member.MemberGroup = group;
            }
            _container.SaveChanges();
        }

        private MenuItem BuildMenuForMember(MemberViewModel member)
        {
            MenuItem mi = new MenuItem() { Header = member.DisplayName };
            mi.Items.Add(new MenuItem() { Header = "Yhteystiedot", Padding = new Thickness(0.1) });
            mi.Items.Add(new Separator());

            foreach (var item in member.GetRelatedContacts())
            {
                Contact contact = item;
                mi.Items.Add(new MenuItem()
                {
                    Header = String.Format("{0} {1}", contact.firstname, contact.lastname),
                    Command = new RelayCommand(delegate { ShowEditContactWindow(contact, member); })
                });
            }
            mi.Items.Add(new MenuItem()
            {
                Header = "Lisää uusi",
                Command = new RelayCommand(delegate { ShowEditContactWindow(null, member); })
            });
            return mi;
        }

        private void ShowEditContactWindow(Contact contact, MemberViewModel member)
        {
            EditContactWindowViewModel ecwvm = new EditContactWindowViewModel(_container, member, contact);
            View.EditContactWindow window = new View.EditContactWindow();
            ecwvm.RequestClose += delegate { window.Close(); };
            window.DataContext = ecwvm;
            window.Show();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _delayMenuRebuild.Dispose();
        }
    }
}
