using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Database;
using System.Data;
using System.Reflection;
using System.Diagnostics;

namespace Registry.ViewModel
{
    /// <summary>
    /// UI-wrapper for Changelog.
    /// </summary>
    public class ChangelogRowViewModel : ViewModelBase
    {
        private Changelog _row;
        private DateTime _dateTime;
        private Lazy<string> _cachedDescription = null;
        private MembersContainer _container;

        public ChangelogRowViewModel(Changelog row, MembersContainer container)
        {
            _row = row;
            _container = container;
            _dateTime = DateTime.Parse(String.Format("{0} {1}", _row.date, _row.time));
            _cachedDescription = new Lazy<string>(() => GetItemDescription(row));
        }

        /// <summary>
        /// Get the datetime of this change event.
        /// </summary>
        public DateTime ChangeTime { get { return _dateTime; } }

        /// <summary>
        /// Get the date part of this change event.
        /// </summary>
        public DateTime ChangeDate { get { return new DateTime(_dateTime.Year, _dateTime.Month, _dateTime.Day); } }

        public string Date
        {
            get { return _dateTime.AsDate(); }
        }

        public override string DisplayName
        {
            get { return _dateTime.Date.ToString(); }
        }

        public string Time
        {
            get { return _dateTime.AsTime(); }
        }
        
        public string Description
        {
            get {
                return _cachedDescription.Value;
            }
        }

        private string GetItemDescription(Changelog item)
        {
            Member member = GetMember(item);
            Contact contact = GetContact(item);

            if (member == null && contact == null)
                return String.Format("Käyttäjä <{0}> puuttuu {1}", item.memberid, item.action);

            return GetDescriptionFor(item, member, contact);
        }

        private static string GetDescriptionFor(Changelog item, Member member, Contact contact)
        {
            switch (item.action)
            {
                case DBConstants.NewContact:
                    return String.Format("Lisätty uusi yhteystieto jäsenelle {0} {1} ({2})",
                                         member.firstname, member.lastname, member.MemberDetais.membergroup);
                case DBConstants.DeleteContact:
                    return String.Format("Yhteystieto poistettu jäseneltä {0} {1}",
                                          member.firstname, member.lastname);
                case DBConstants.EditContact:
                    return String.Format("Yhteystiedolta {0} {1} ({2} {3}) muutettu {4}",
                                        contact.firstname, contact.lastname,
                                        member.firstname, member.lastname,
                                        item.oldvalue);
                case DBConstants.NewMember:
                    return String.Format("Lisätty uusi jäsen {0} {1} ({2})",
                                        member.firstname, member.lastname, member.MemberDetais.membergroup);
                case DBConstants.EditMember:
                    return String.Format("Muokattu jäsentä {0} {1} ({2}) tietoa {3}",
                                        member.firstname, member.lastname, member.MemberDetais.membergroup, item.oldvalue);
                case DBConstants.NewNonMember:
                    return String.Format("Jäsen eronnut {0} {1} ({2}) syy: {3}",
                                        member.firstname, member.lastname, item.oldvalue, member.MemberDetais.membergroup);
                default:
                    Debug.Assert(false, "Should not be reached");
                    return "<Empty>";
            }
        }

        private Member GetMember(Changelog item)
        {
            IEnumerable<Member> rows = _container.MemberSet.AsEnumerable().
                                            Where(m => m.Id == long.Parse(item.memberid));
            if (rows.Count() == 0)
                return null;

            return rows.First();
        }

        /// <summary>
        /// Get the contact that is related to the change item.
        /// </summary>
        private Contact GetContact(Changelog item)
        {
            if (!item.action.Equals(DBConstants.EditContact))
                return null;

            var log = _container.ContactSet.AsEnumerable();
            IEnumerable<Contact> rows = log.Where(c => c.Member.Id == long.Parse(item.memberid) &&
                                                       c.Id == long.Parse(item.newvalue));

            return rows.FirstOrDefault();
        }
    }
}
