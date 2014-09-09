using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fonet;
using Database;
using System.Diagnostics;
using System.ComponentModel;

namespace Export
{
    /// <summary>
    /// Pdf exporter that implements the common functionality for exporting a pdf.
    /// </summary>
    public abstract class PdfExport : IPdfExport
    {
        public IEnumerable<Member> Members { get; set; }
        public string OutputPath { get; set; }
        public string Name { get; protected set; }
        public string DocumentTitle { get; set; }
        public string DocumentHeader { get; set; }
        public string OrgName { get; set; }
        public string DocumentDate { get; set; }
        public abstract int PagesPer100Members { get; }
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler ExportCompleted;

        private static string CreateTempFile(string contents)
        {
            string tempFile = Path.GetTempFileName();

            File.WriteAllText(tempFile, contents);

            return tempFile;
        }

        protected abstract StringBuilder BuildDocument(IEnumerable<Member> members);

        public void Export()
        {
            StringBuilder document = BuildDocument(this.Members);
            RenderDocument(document);
        }

        protected void OnUpdateProgress(int progress)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new ProgressChangedEventArgs(progress, this));
        }

        protected void RenderDocument(StringBuilder document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            try {
                var driver = new FonetDriver
                                 {
                                     Options =
                                         new Fonet.Render.Pdf.PdfRendererOptions
                                             {Author = OrgName, Title = DocumentTitle}
                                 };
                driver.OnInfo += driver_OnInfo;
                string tempFile = CreateTempFile(document.ToString());
                driver.Render(tempFile, OutputPath);
                File.Delete(tempFile);
            }
            catch(Exception e)
            {
                Debug.Print(e.Message);
            }

            OnUpdateProgress(100);
            if (ExportCompleted != null)
                ExportCompleted(this, null);
        }

        void driver_OnInfo(object driver, FonetEventArgs e)
        {
            string message = e.GetMessage();
            int progess = -1;
            if (message.Equals("Building formatting object tree"))
                progess = 5;
            else if(message.StartsWith("[") && message.EndsWith("]"))
                progess = GetProgressFromPage(message);

            if (progess != -1)
                OnUpdateProgress(progess);
        }

        private int GetProgressFromPage(string message)
        {
            int currentPage = Int32.Parse(message.Substring(1, message.Length - 2));
            double expectedPages = Math.Max((this.Members.Count() / 100), 1) * PagesPer100Members;
            int pageProgress = (int)((currentPage / expectedPages) * 100);
            return Math.Min(Math.Max(7, pageProgress), 100);
        }
    }
}
