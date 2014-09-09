using System;
using Database;

namespace Import
{
    /// <summary>
    /// Event arguments for specified type.
    /// </summary>
    /// <typeparam name="TArgType"></typeparam>
    public class TypedEventArgs<TArgType> : EventArgs
    {
        private readonly TArgType m_t;
        public TypedEventArgs(TArgType value)
        {
            m_t = value;
        }

        public TArgType Value { get { return m_t; } }
    }

    /// <summary>
    /// Interface for database-importers.
    /// </summary>
    public interface IDatabaseImporter
    {
        /// <summary>
        /// The progress event handler that receives the updated progress status.
        /// </summary>
        event EventHandler<TypedEventArgs<double>> Progress;

        /// <summary>
        /// Start the import progress
        /// </summary>
        void Import(string path, MembersContainer container);

        /// <summary>
        /// Get the name of this importer.
        /// </summary>
        string Name { get; }
    }
}
