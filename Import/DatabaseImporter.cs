using System;
using Database;

namespace Import
{
    /// <summary>
    /// Base class for database importers.
    /// </summary>
    public abstract class DatabaseImporter : IDatabaseImporter
    {
        public event EventHandler<TypedEventArgs<double>> Progress;
        public abstract string Name { get; }

        /// <summary>
        /// Implement to do the import and update 
        /// </summary>
        public abstract void Import(string path, MembersContainer container);

        /// <summary>
        /// Fire progress changed event to notify about import progress.
        /// </summary>
        /// <param name="percent">0.0 - 1.0</param>
        protected void OnProgressChanged(double percent)
        {
            if(Progress != null)
                Progress(this, new TypedEventArgs<double>(percent));
        }
    }
}
