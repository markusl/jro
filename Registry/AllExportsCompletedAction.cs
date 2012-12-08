using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Export.Interfaces;
using System.Diagnostics;
using System.IO;

namespace Registry
{
    /// <summary>
    /// The default action to launch when all exports are completed.
    /// </summary>
    static class AllExportsCompletedAction
    {
        /// <summary>
        /// Starts the Explorer program displaying the output directory with latest exported file selected.
        /// </summary>
        public static void ExportService_AllExportsCompleted(object sender, EventArgs e)
        {
            IExportService service = sender as IExportService;
            FileInfo fi = new FileInfo(service.CurrentDocument);
            using(Process process = new Process())
            {
                process.StartInfo.FileName = "explorer.exe";
                process.StartInfo.WorkingDirectory = fi.DirectoryName;
                process.StartInfo.Arguments = "/select," + fi.Name;
                process.Start();
            }
        }
    }
}
