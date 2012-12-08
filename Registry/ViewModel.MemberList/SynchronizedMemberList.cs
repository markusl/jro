using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.Objects;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ThreadSafeCollection;
using Registry.Utilities;
using Database;

namespace Registry.ViewModel
{
    /// <summary>
    /// This class is responsible for managing the list of MemberViewModel's and keeping that
    /// list in sync with the database of members.
    /// </summary>
    public static class SynchronizedMemberList
    {
        /// <summary>
        /// The thread safe collection of member objects.
        /// </summary>
        public static ThreadSafeObservableCollection<MemberViewModel> Members { get; private set; }

        /// <summary>
        /// Occurs when Members list has been refreshed.
        /// </summary>
        public static event EventHandler RefreshFinished;

        /// <summary>
        /// Asynchronously construct initial list of members from database.
        /// Usually done at program startup.
        /// </summary>
        /// <param name="container"></param>
        public static void Initialize(MembersContainer container)
        {
            Members = new ThreadSafeObservableCollection<MemberViewModel>();

            // do asynchronous update on the list
            new Task(delegate { PopulateMembersCollectionTask(container); }).Start();

            container.SavingChanges += (s, o) => RefreshMembersListTask(container.ObjectStateManager);
        }

        public static void InitializeNonAsync(MembersContainer container)
        {
            Members = new ThreadSafeObservableCollection<MemberViewModel>();

            PopulateMembersCollectionTask(container);

            container.SavingChanges += (s, o) => RefreshMembersListTask(container.ObjectStateManager);
        }

        private static void PopulateMembersCollectionTask(MembersContainer container)
        {
            try
            {
                var members = container.MemberSet.AsEnumerable().
                                                              Select(me => new MemberViewModel(me));

                // updating the screen looks better if done group by group
                members = members.OrderBy(me => me.MemberGroup);
                foreach (var member in members)
                {
                    Members.Add(member);
                }

                if (RefreshFinished != null)
                    RefreshFinished(null, null);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                Debug.Print(e.StackTrace);
            }
        }

        /// <summary>
        /// Keep the member list synchronized with the one that is saved to database.
        /// </summary>
        /// <param name="objectStateManager"></param>
        private static void RefreshMembersListTask(ObjectStateManager objectStateManager)
        {
            using(new DebugTimer("Refresh members list"))
            {
                IEnumerable<ObjectStateEntry> addedEntries = objectStateManager.GetObjectStateEntries(System.Data.EntityState.Added);
                IEnumerable<ObjectStateEntry> deletedEntries = objectStateManager.GetObjectStateEntries(System.Data.EntityState.Deleted);
                IEnumerable<ObjectStateEntry> modifiedEntries = objectStateManager.GetObjectStateEntries(System.Data.EntityState.Modified);

                HandleAddedMembers(addedEntries);
                HandleDeletedMembers(deletedEntries);
                HandleModifiedMembers(modifiedEntries);
                Debug.Print("Got {0} added, {1} deleted and {2} modified members", addedEntries.Count(), deletedEntries.Count(), modifiedEntries.Count());
            }

            if (RefreshFinished != null)
                RefreshFinished(null, null);
        }

        /// <summary>
        /// WPF ListView workaround: Changing group-property of an object doesn't
        /// update grouping on the screen. To work around that remove the item and re-add
        /// to make the item to appear in the correct group.
        /// </summary>
        private static void HandleModifiedMembers(IEnumerable<ObjectStateEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.Entity is MemberDetais)
                {
                    Member member = (entry.Entity as MemberDetais).Member;
                    string propModified = entry.GetModifiedProperties().Where(p => p.Equals("membergroup")).FirstOrDefault();

                    if (propModified != null)
                    {
                        MemberViewModel mvm = Members.FindSame(member);
                        Members.Remove(mvm);
                        Members.Add(mvm);
                    }
                }
            }
        }

        private static void HandleDeletedMembers(IEnumerable<ObjectStateEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.Entity is Member)
                {
                    Member member = entry.Entity as Member;
                    MemberViewModel mvm = Members.FindSame(member);
                    bool found = Members.Remove(mvm);

                    if(!found)
                        Debug.Print("Member was not found in the list: {0} {1}", member.firstname, member.lastname);
                }
            }
        }

        private static void HandleAddedMembers(IEnumerable<ObjectStateEntry> entries)
         {
             foreach (var entry in entries)
             {
                 if (entry.Entity is Member)
                 {
                     Members.Add(new MemberViewModel(entry.Entity as Member));
                 }
             }
        }

        /// <summary>
        /// Extension for finding a MemberViewModel from collection by Member.
        /// </summary>
        public static MemberViewModel FindSame(this ObservableCollection<MemberViewModel> collection, Member member)
        {
            return collection.Where(m => m.Equals(member)).FirstOrDefault();
        }
    }
}
