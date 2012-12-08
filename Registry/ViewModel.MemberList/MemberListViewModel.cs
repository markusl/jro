using System;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
using Registry.Menues;
using Registry.Utilities;
using Registry.Interfaces;
using Database;

namespace Registry.ViewModel
{
    /// <summary>
    /// Used to display grouped, sorted and filtered list of all members in registry.
    /// UI wrapper for SynchronizedMemberList.Members.
    /// </summary>
    public class MemberListViewModel : ClosableViewModel, IMemberListView
    {
        private string _filterString;

        /// <summary>
        /// Get or set status text shown in the UI status bar.
        /// </summary>
        [Import(typeof(IMemberListStatus))]
        public IMemberListStatus StatusText { get; private set; }

        /// <summary>
        /// The collection to which the UI ListView is bound to.
        /// </summary>
        public ICollectionView Collection { get; private set; }

        /// <summary>
        /// Get or set textual filter that restricts the currently visible set of members.
        /// </summary>
        public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; OnPropertyChanged("FilterString"); this.Collection.Refresh(); }
        }

        private Predicate<MemberViewModel> _textFilterPredicate;

        /// <summary>
        /// Construct new MemberListViewModel.
        /// </summary>
        /// <param name="name">Name of the view. To be displayed in the user interface</param>
        /// <param name="container">The member container</param>
        /// <param name="restrictor">The filter to restrict members shown in this view.</param>
        public MemberListViewModel(string name, MembersContainer container,
                                   Dispatcher dispatcher,
                                   MemberCollectionView mcv)
        {
            base.DisplayName = name;
            this.Collection = mcv.CollectionView;
            this.SelectedItems = new ObservableCollection<MemberViewModel>();
            this.MenuOptions = new ObservableCollection<Control>();

            new MemberListViewMenu(this, container);
            this.SelectedItems.CollectionChanged += delegate { this.OnPropertyChanged("StatusText"); };

            // Gather list of implemented filters in this assembly.
            CompositionHelper.ComposeInExecutingAssemblyWithExported<IMemberListView>(this);
            DefineFilters(mcv);
        }

        private void DefineFilters(MemberCollectionView mcv)
        {
            _textFilterPredicate = (MemberViewModel member) =>
            {
                return String.IsNullOrEmpty(FilterString) ? true :
                        member.DisplayName.ToLower().Contains(FilterString.ToLower());
            };
            mcv.Filters.Add(_textFilterPredicate);
        }

        public ObservableCollection<Control> MenuOptions { get; private set; }

        public ObservableCollection<MemberViewModel> SelectedItems { get; private set; }

        public ObservableCollection<MemberViewModel> AllItems
        {
            get { return SynchronizedMemberList.Members; }
        }
    }
}
