using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Export.Interfaces
{
    /// <summary>
    /// Interface for module that handles queuing and progress tracking of export jobs.
    /// </summary>
    public interface IExportService : INotifyPropertyChanged
    {
        /// <summary>
        /// Queue a job to be exported.
        /// </summary>
        /// <param name="export"></param>
        void QueueExportJob(IPdfExport export);

        /// <summary>
        /// Check whether the application is currently exporting something.
        /// </summary>
        bool ExportInProgress { get; set; }

        /// <summary>
        /// Get the percentage of export progress completed.
        /// </summary>
        int ExportProgress { get; set; }

        /// <summary>
        /// Get the name of document currently being exported, or null if none.
        /// </summary>
        string CurrentDocument { get; set; }

        /// <summary>
        /// Event that will be raised when all exports are completed.
        /// </summary>
        event EventHandler AllExportsCompleted;
    }
}
