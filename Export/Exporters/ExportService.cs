using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.ComponentModel.Composition;
using Export.Interfaces;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Export.Exporters
{
    /// <summary>
    /// Implement a service that handles queuing and progress tracking of export jobs.
    /// </summary>
    /// <remarks>Exports are done as asynchronous tasks but</remarks>
    [Export(typeof(IExportService))]
    public class ExportService : IExportService
    {
        private Dispatcher _dispatcher;
        private long _jobInProgress;
        private long _completedJobs;
        private long _totalJobs;
        private BlockingCollection<IPdfExport> _jobBuffer = new BlockingCollection<IPdfExport>();

        /// <summary>
        /// Construct new export service with a dispatcher.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to use in progress reporting.</param>
        [ImportingConstructor]
        public ExportService(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            new Task(ExportLoop).Start();
        }

        private void ExportLoop()
        {
            while(true)
            {
                IPdfExport export;
                if (!_jobBuffer.TryTake(out export, 30))
                    continue;

                this.ExportInProgress = true;

                Debug.Print("Exporting {0}...", export.OutputPath);

                Action updateCurrentDocument = () => this.CurrentDocument = export.OutputPath;
                _dispatcher.Invoke(updateCurrentDocument);
                export.Export();
                Interlocked.Increment(ref _completedJobs);

                long jobsLeft = Interlocked.Decrement(ref _jobInProgress);
                if (jobsLeft == 0 && AllExportsCompleted != null)
                {
                    AllExportsCompleted(this, null);
                    _completedJobs = 0;
                    _totalJobs = 0;
                    this.CurrentDocument = null;
                    this.ExportInProgress = false;
                }
            }
        }

        public void QueueExportJob(IPdfExport export)
        {
            AddProgressReporting(ref export);
            Interlocked.Increment(ref _jobInProgress);
            Interlocked.Increment(ref _totalJobs);
            _jobBuffer.Add(export);
        }

        /// <summary>
        /// Takes care of reporting the 
        /// </summary>
        /// <param name="export"></param>
        private void AddProgressReporting(ref IPdfExport export)
        {
            export.ProgressChanged += delegate(object o, ProgressChangedEventArgs pcea)
            {
                Action updateProgressPercentage = () => this.ExportProgress = pcea.ProgressPercentage;
                _dispatcher.Invoke(updateProgressPercentage);
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _exportInProgress;
        public bool ExportInProgress
        {
            get { return _exportInProgress; }
            set { _exportInProgress = value; OnPropertyChanged("ExportInProgress"); }
        }

        private int _exportProgress = 0;
        public int ExportProgress
        {
            get {
                double currentProgress = ((double)_exportProgress / _totalJobs);
                double jobsCompleted = (((double)_completedJobs / _totalJobs)*100);
                return (int)(currentProgress + jobsCompleted);
            }
            set { _exportProgress = value; OnPropertyChanged("ExportProgress"); }
        }

        private string _currentDocument;
        public string CurrentDocument
        {
            get
            {
                return _currentDocument;
            }
            set
            {
                _currentDocument = value;
                OnPropertyChanged("CurrentDocument");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler AllExportsCompleted;
    }
}
