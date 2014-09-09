using System;

namespace Database
{
    public static class DBConstants
    {
        /// <summary>
        /// Related contact record was inserted.
        /// </summary>
        public const string NewContact = "newcontact";
        /// <summary>
        /// Related contact record was removed.
        /// </summary>
        public const string DeleteContact = "delcontact";
        /// <summary>
        /// Related contact record was edited.
        /// </summary>
        public const string EditContact = "editcontact";

        public const string NewMember = "newmember";
        public const string EditMember = "editmember";

        public const string DeleteNonMember = "delnonmember";
        public const string NewNonMember = "newnonmember";

        /// <summary>
        /// Empty (null) date as it gets imported from Access database
        /// </summary>
        public const string AccessNullDate = "1.1.0001 0:00:00";
        /// <summary>
        /// Used to mark a member that is not an active member of the organization.
        /// </summary>
        public const string InactiveMember = "<Inactive>";
        /// <summary>
        /// Used to mark a member who has left the organization.
        /// </summary>
        public const string ResignedMember = "<Resigned>";

        /// <summary>
        /// Check if membergroup field value is "special", eg. system changed or user changed.
        /// </summary>
        public static bool IsSpecialGroup(string group)
        {
            if (group == null) throw new ArgumentNullException("group");

            return group.Equals(ResignedMember) || group.Equals(InactiveMember);
        }
    }
}
