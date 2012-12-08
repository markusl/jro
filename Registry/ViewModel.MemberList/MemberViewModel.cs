using Database;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Registry.ViewModel
{
    /// <summary>
    /// UI-wrapper for Database.Member.
    /// </summary>
    public class MemberViewModel : ViewModelBase, IEquatable<MemberViewModel>
    {
        readonly Member _member;
        private Lazy<string> _lazyAge;
        private Lazy<string> _lazyDisplayName;
        
        /// <summary>
        /// Get the approximate age of the member, in years.
        /// </summary>
        public double Age
        {
            get { TimeSpan ts = DateTime.Now - DateTime.Parse(_member.birthdate);
                  return ts.TotalDays / 365; }
        }

        /// <summary>
        /// Age with one decimal.
        /// </summary>
        public string FormattedAge
        {
            get { return _lazyAge.Value; }
        }

        public string Phone
        {
            get { return _member.mobile.Length != 0 ? _member.mobile : _member.phone; }
        }

        public string Email
        {
            get { return _member.email; }
        }

        /// <summary>
        /// Get or set the group of the member.
        /// </summary>
        public string MemberGroup
        {
            get { return _member.MemberDetais.membergroup; }
            set { _member.MemberDetais.membergroup = value; }
        }

        /// <summary>
        /// Street City Postal code
        /// </summary>
        public string Address
        {
            get { return String.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", _member.Address.address,
                    _member.Address.city, _member.Address.postalcode); }
        }

        /// <summary>
        /// Check if membership fee is paid.
        /// </summary>
        public PaymentStatus Payment
        {
            get { return _member.GetPaymentStatus(); }
        }

        /// <summary>
        /// Gets whether this is an actual member.
        /// </summary>
        public bool IsMember { get { return _member.IsMember(); } }

        /// <summary>
        /// Gets whether this member is resigned.
        /// </summary>
        public bool IsResigned { get { return this.MemberGroup.Equals(DBConstants.ResignedMember); } }

        /// <summary>
        /// Gets whether this member is marked as inactive.
        /// </summary>
        public bool IsInactive { get { return this.MemberGroup.Equals(DBConstants.InactiveMember); } }

        private MemberViewModel() { }
        public MemberViewModel(Member member)
        {
            if (member == null || member.MemberDetais == null || member.Address == null || member.Contact == null)
                throw new ArgumentNullException("member");

            _member = member;
            _lazyAge = new Lazy<string>(() => String.Format(CultureInfo.CurrentCulture, "{0:00} vuotta", Age));
            _lazyDisplayName = new Lazy<string>(() => FormatDisplayName(_member.firstname, _member.lastname));
        }

        public override string DisplayName { get { return _lazyDisplayName.Value; } protected set { } }

        public string Group
        {
            get { return _member.MemberDetais.membergroup; }
        }

        internal IEnumerable<Contact> GetRelatedContacts()
        {
            return _member.Contact.AsEnumerable();
        }

        internal Member InternalMember { get { return _member; } }

        public static string FormatDisplayName(string firstname, string lastname)
        {
            return firstname + " " + lastname;
        }

        public bool Equals(MemberViewModel other)
        {
            return other._member.MemberDetais.memberno.Equals(this._member.MemberDetais.memberno);
        }

        public bool Equals(Member other)
        {
            return other.MemberDetais.memberno.Equals(this._member.MemberDetais.memberno);
        }
    }
}
