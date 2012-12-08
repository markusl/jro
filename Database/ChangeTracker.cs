using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Globalization;

namespace Database
{
    /// <summary>
    /// Purpose of this class is to do change-tracking for database objects.
    /// New changelog entries are created for added members and added contacts.
    /// </summary>
    class ChangeTracker
    {
        private MembersContainerImpl _database;
        private string _date;
        private string _time;
        private ICollection<Changelog> _pendingChanges = new List<Changelog>();

        /// <summary>
        /// Initialize new instance of this class and start change tracking.
        /// </summary>
        /// <param name="database"></param>
        public ChangeTracker(MembersContainerImpl database)
        {
            _database = database;
            _database.SavingChanges += _database_SavingChanges;
            _database.SavedChanges += new EventHandler(_database_SavedChanges);
        }

        void _database_SavingChanges(object sender, EventArgs e)
        {
            ObjectStateManager objectStateManager = _database.ObjectStateManager;

            _date = DateTime.Now.AsDate();
            _time = DateTime.Now.AsTime();

            IEnumerable<ObjectStateEntry> entries = objectStateManager.GetObjectStateEntries(EntityState.Added);

            AddChangelogEntriesForAddedObjects(entries.EntitiesOfType<Member>());
            AddChangelogEntriesForAddedObjects(entries.EntitiesOfType<Contact>());

            entries = objectStateManager.GetObjectStateEntries(EntityState.Modified);
            AddChangelogEntriesForResignedMembers(entries.EntitiesOfType<MemberDetais>(), _date, _time);
        }

        private void AddChangelogEntriesForResignedMembers(IEnumerable<ObjectStateEntry> entries, string date, string time)
        {
            foreach (var entry in entries)
            {
                MemberDetais t1 = entry.Entity as MemberDetais;

                if (ValueChangedTo(entry, "membergroup", DBConstants.ResignedMember) ||
                    ValueChangedTo(entry, "membergroup", DBConstants.InactiveMember))
                {
                    _database.ChangelogSet.AddObject(
                        new Changelog()
                        {
                            date = date,
                            time = time,
                            memberid = String.Format(CultureInfo.CurrentCulture, "{0}", t1.Member.Id),
                            action = DBConstants.NewNonMember,
                            oldvalue = GetOldValue(entry, "membergroup"),
                            newvalue = ""
                        });
                }
            }
        }

        private static bool ValueChangedTo(ObjectStateEntry entry, string field, string value)
        {
            var props = entry.GetModifiedProperties();
            bool modified = props.Where(prop => prop.Equals(field)).FirstOrDefault() != null;

            MemberDetais obj = entry.Entity as MemberDetais;
            return modified && obj.membergroup.Equals(value);
        }

        private static string GetOldValue(ObjectStateEntry entry, string field)
        {
            return entry.OriginalValues[field].ToString();
        }

        private void AddChangelogEntriesForAddedObjects(IEnumerable<ObjectStateEntry> entries)
        {
            foreach (var entry in entries)
            {
                // Objects are constructed with null id (0) before inserting into the database.
                // When inserting the object, the id is assigned by the database engine.
                // Here we add an event listener to get us notified when an id is assigned to the object, 
                // and insert change tracking events after that.
                (entry.Entity as EntityObject).PropertyChanged += entity_PropertyChanged;
            }
        }

        void entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
 	        if(e.PropertyName.Equals("Id"))
            {
                dynamic obj = sender;

                string action = (obj is Member ? DBConstants.NewMember : 
                                 obj is Contact ? DBConstants.NewContact :
                                 null);

                // add to pending changes list which is then inserted when SavedChanges occurs
                _pendingChanges.Add(
                    Changelog.CreateChangelog(0,
                        date: _date,
                        time: _time,
                        memberid: String.Format("{0}", obj.Id),
                        action: action,
                        oldvalue: "",
                        newvalue: ""
                    ));
            }
        }
        
        void _database_SavedChanges(object sender, EventArgs e)
        {
            foreach (var change in _pendingChanges)
            {
                _database.ChangelogSet.AddObject(change);
            }
            _database.SavedChanges -= _database_SavedChanges; // don't call recursively
            _database.SaveChanges();
            _database.SavedChanges += _database_SavedChanges;

            _pendingChanges = new List<Changelog>();
        }
    }

    public static class ObjectStateEntryExtension
    {
        public static IEnumerable<ObjectStateEntry> EntitiesOfType<T1>(this IEnumerable<ObjectStateEntry> entries)
        {
            return new List<ObjectStateEntry>(entries.Where(e => e.Entity is T1));
        }
    }
}
