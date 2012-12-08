using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shell;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Input;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Import;
using Database;

namespace Registry.ViewModel
{
    /// <summary>
    /// ViewModel class for database import window.
    /// </summary>
    public class ImportWindowViewModel : ClosableViewModel
    {
        private string _importFileName = "";
        private string _importLabel = "";
        private double _progressValue = 0.0;
        private MembersContainer _container;
        private TaskbarItemProgressState _progresState = TaskbarItemProgressState.Indeterminate;

        private IDatabaseImporter _importer;

        /// <summary>
        /// Get or set the filename for imported file.
        /// </summary>
        public string ImportFileName
        {
            get { return _importFileName; }
            set { _importFileName = value; OnPropertyChanged("ImportFileName"); }
        }

        public string ImportLabel
        {
            get { return _importLabel; }
            set { _importLabel = value; OnPropertyChanged("ImportLabel"); }
        }

        public double ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }
        public TaskbarItemProgressState ProgressState
        {
            get { return _progresState; }
            set
            {
                _progresState = value;
                OnPropertyChanged("ProgressState");
            }
        }

        public ImportWindowViewModel(MembersContainer targetDatabase, IDatabaseImporter importer)
        {
            _container = targetDatabase;
            _importer = importer;
        }

        private void SetLabel()
        {
            FileInfo fi = new FileInfo(ImportFileName);
            this.ImportLabel = String.Format("Tuodaan tietokantaa {0}...", fi.Name);
        }

        /// <summary>
        /// Start the import process in a non-UI thread.
        /// </summary>
        public void StartImport()
        {
            if (String.IsNullOrEmpty(this.ImportFileName))
                throw new ArgumentNullException();

            ThreadPool.QueueUserWorkItem(delegate
            {
                this.ProgressValue = 1.0;

                SetLabel();
            
                try
                {
                    _importer.Progress += update_progress;
                    _importer.Import(this.ImportFileName, _container);
                }
                catch (Exception e)
                {
                    this.ProgressState = TaskbarItemProgressState.Error;
                    this.ImportLabel = "Virhe tuonnissa: " + e.Message;
                    Debug.Write(e);
                }
            });
        }

        void update_progress(object o, TypedEventArgs<double> percent)
        {
            this.ProgressState = TaskbarItemProgressState.Normal;
            this.ProgressValue = percent.Value;

            FileInfo fi = new FileInfo(ImportFileName);
            if (this.ProgressValue >= 1.0)
                this.ImportLabel = String.Format("Tietokanta tuotu {0}...", fi.Name);
        }
    }
}
