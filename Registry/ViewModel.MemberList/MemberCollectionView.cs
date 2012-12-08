using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace Registry.ViewModel
{
    /// <summary>
    /// The CollectionViewSource wrapper to be used with member list.
    /// Provides grouping and sorting and filtering for the display.
    /// </summary>
    public class MemberCollectionView
    {
        private CollectionViewSource CollectionViewSource { get; set; }

        /// <summary>
        /// Get the constructed CollectionView to be used in the UI.
        /// </summary>
        public ICollectionView CollectionView { get; private set; }

        /// <summary>
        /// Get the list of filters that filter the current view of members.
        /// </summary>
        public ICollection<Predicate<MemberViewModel>> Filters { get; private set; }

        /// <summary>
        /// Construct new CollectionViewSource for the member list.
        /// </summary>
        /// <param name="restrictor">Optional filter do display subset of members in the view.</param>
        public MemberCollectionView(Predicate<MemberViewModel> restrictor = null)
        {
            CollectionViewSource = new CollectionViewSource();
            CollectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            CollectionViewSource.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Descending));
            CollectionViewSource.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
            CollectionViewSource.Source = SynchronizedMemberList.Members;

            Filters = new List<Predicate<MemberViewModel>>();
            CollectionView = CollectionViewSource.View;
            CollectionView.Filter = ApplyActiveFilters;
            DefineFilters(restrictor);
            CollectionView.Refresh();
        }

        private void DefineFilters(Predicate<MemberViewModel> restrictor)
        {
            if (restrictor != null)
                Filters.Add(restrictor);
        }

        private bool ApplyActiveFilters(object member)
        {
            return Filters.All(f => f.Invoke((MemberViewModel)member));
        }
    }
}
