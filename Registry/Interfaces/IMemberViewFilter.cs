using System;
using Registry.ViewModel;

namespace Registry.Interfaces
{
    /// <summary>
    /// Defines an interface that is used to define an filter to filter list of members in a view.
    /// </summary>
    public interface IMemberViewFilter
    {
        /// <summary>
        /// Priority of the filter. Used to sort the filters in user interface. 1 being the highest priority.
        /// </summary>
        int Priority { get; }
        /// <summary>
        /// Name of the filter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The filter predicate. Called once for each MemberViewModel to check if it should be displayed in the view.
        /// </summary>
        Predicate<MemberViewModel> Filter { get; }
    }
}
