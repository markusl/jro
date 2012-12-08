using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Registry.ViewModel;
using Database;
using Export.Interfaces;
using System.ComponentModel;

namespace Registry.Interfaces
{
    /// <summary>
    /// Defines the interface for Main window of the application.
    /// Main window is a tabbed document interface -like system.
    /// </summary>
    public interface IMainWindow : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Displays the default view of the application.
        /// </summary>
        void ShowDefaultView();

        /// <summary>
        /// Displays a tab in the user interface.
        /// </summary>
        /// <param name="vmb"></param>
        void DisplayTab(ClosableViewModel tab);

        /// <summary>
        /// Sets active tab to the specified view model.
        /// </summary>
        /// <param name="vmb"></param>
        void SetActiveTab(ViewModelBase tab);

        /// <summary>
        /// Display a list of members filtered by the given filter.
        /// </summary>
        /// <param name="filter"></param>
        void DisplayFilteredTab(IMemberViewFilter filter);

        /// <summary>
        /// Sets active tab by its name.
        /// </summary>
        /// <returns>True if tab was set to active, false if no tab exists with specified name</returns>
        bool SetActiveTab(string name);

        /// <summary>
        /// Get the currently active member database.
        /// </summary>
        MembersContainer ActiveDatabase { get; }

        /// <summary>
        /// The service that handles exports in this application.
        /// </summary>
        IExportService ExportService { get; }

        /// <summary>
        /// The application dispatcher.
        /// </summary>
        Dispatcher Dispatcher { get; }

        bool ExportInProgress { get; }
    }
}
