using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.ComponentModel;

namespace Export
{
    /// <summary>
    /// Interface for implementing PDF exporters.
    /// </summary>
    public interface IPdfExport
    {
        /// <summary>
        /// Name (description) of this exporter
        /// </summary>
        string Name { get; }

        string DocumentTitle { get; set; }
        string DocumentHeader { get; set; }
        string DocumentDate { get; set; }
        string OrgName { get; set; }

        /// <summary>
        /// The list of members to include in this export.
        /// </summary>
        IEnumerable<Member> Members { get; set; }

        /// <summary>
        /// Path to the output file.
        /// </summary>
        string OutputPath { get; set; }

        /// <summary>
        /// Export the job.
        /// </summary>
        void Export();

        /// <summary>
        /// Name (description) of this exporter
        /// </summary>
        string ToString();

        /// <summary>
        /// Happens when progress happens in rendering the output.
        /// </summary>
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        /// Happens when the export is completed.
        /// </summary>
        event EventHandler ExportCompleted;
    }
}
