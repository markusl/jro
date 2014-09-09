using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Diagnostics;
using System.Data.SQLite;

namespace Database
{
    /// <summary>
    /// Use partial class to make the class abstract.
    /// </summary>
    public abstract partial class MembersContainer
    {
    }

    /// <summary>
    /// Derived MembersContainer class which attach change tracking functionality.
    /// </summary>
    public class MembersContainerImpl  : MembersContainer
    {
        private readonly ChangeTracker _changeTracker;

        /// <summary>
        /// Occurs after changes have been saved.
        /// </summary>
        public event EventHandler SavedChanges;

        /// <summary>
        /// Construct new ObjectContext with an opened connection.
        /// </summary>
        /// <param name="connectionString">Connection string for creating EntityConnection</param>
        public MembersContainerImpl(string connectionString) : base(new EntityConnection(connectionString))
        {
            try
            {
                base.Connection.Open();
            }
            catch (SQLiteException sqle)
            {
                throw new ArgumentException(sqle.Message);
            }

            _changeTracker = new ChangeTracker(this);
        }

        public override int SaveChanges(SaveOptions options)
        {
            return SaveChanges(options, true);
        }

        public int SaveChanges(SaveOptions options, bool logChanges)
        {
            int result = base.SaveChanges(options);

            if(SavedChanges != null)
                SavedChanges(this, null);

            return result;
        }
    }
}
