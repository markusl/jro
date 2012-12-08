using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Registry.ViewModel;

namespace Registry.Interfaces
{
    /// <summary>
    /// Defines the interface for member list view.
    /// </summary>
    public interface IMemberListView
    {
        /// <summary>
        /// Provides the list of menu items for selected members.
        /// </summary>
        ObservableCollection<Control> MenuOptions { get; }

        /// <summary>
        /// The list of currently selected items in member list view.
        /// </summary>
        ObservableCollection<MemberViewModel> SelectedItems { get; }

        /// <summary>
        /// The list of all items in this list view.
        /// </summary>
        ObservableCollection<MemberViewModel> AllItems { get; }
    }
}
