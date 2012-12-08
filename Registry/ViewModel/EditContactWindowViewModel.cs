using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.Windows.Input;
using System.Globalization;

namespace Registry.ViewModel
{
    /// <summary>
    /// Used to display a view for editing a single contact.
    /// </summary>
    class EditContactWindowViewModel : ClosableViewModel
    {
        private MembersContainer _container;
        private Contact _contact;
        private MemberViewModel _member;

        public string FirstName
        {
            get { return _contact.firstname; }
            set { _contact.firstname = value; OnPropertyChanged("FirstName"); }
        }
        public string LastName
        {
            get { return _contact.lastname; }
            set { _contact.lastname = value; OnPropertyChanged("LastName"); }
        }
        public string StreetAddress
        {
            get { return _contact.Address.address; }
            set { _contact.Address.address = value; OnPropertyChanged("StreetAddress"); }
        }
        public string PostalCode
        {
            get { return _contact.Address.postalcode; }
            set { _contact.Address.postalcode = value; OnPropertyChanged("PostalCode"); }
        }
        public string City
        {
            get { return _contact.Address.city; }
            set { _contact.Address.city = value; OnPropertyChanged("City"); }
        }
        public string Email
        {
            get { return _contact.email; }
            set { _contact.email = value; OnPropertyChanged("Email"); }
        }
        public string Phone
        {
            get { return _contact.phone; }
            set { _contact.phone = value; OnPropertyChanged("Phone"); }
        }
        public string Mobile
        {
            get { return _contact.mobile; }
            set { _contact.mobile = value; OnPropertyChanged("Mobile"); }
        }

        /// <summary>
        /// Construct new EditContactWindowViewModel.
        /// </summary>
        /// <param name="container">The member container</param>
        /// <param name="member">The member to which this contact belongs</param>
        /// <param name="contact">The contact to edit or null to create new</param>
        public EditContactWindowViewModel(MembersContainer container,
                                          MemberViewModel member,
                                          Contact contact)
        {
            _container = container;
            _contact = contact;
            _member = member;

            if (_contact == null)
                _contact = CreateEmptyContact(_member);
        }

        /// <summary>Create a placeholder contact for editing when adding new member.</summary>
        private Contact CreateEmptyContact(MemberViewModel member)
        {
            Contact contact = _container.ContactSet.CreateObject();
            contact.firstname = "";
            contact.lastname = "";
            contact.mobile = "";
            contact.phone = "";
            contact.email = "";
            contact.Address = _container.AddressSet.CreateObject();
            contact.Address.address = "";
            contact.Address.postalcode = "";
            contact.Address.city = "";
            contact.Address.country = "";
            contact.Member = member.InternalMember;
            _container.ContactSet.AddObject(contact);
            return contact;
        }

        public string GroupBoxHeader
        {
            get
            {
                return String.Format(CultureInfo.CurrentCulture,
                                     "Käyttäjän {0} yhteyshenkilö {1} {2}",
                                     _member.DisplayName,
                                     _contact.firstname, _contact.lastname);
            }
        }

        private void OnSaveAndClose()
        {
            _container.SaveChanges();
            CloseCommand.Execute(this);
        }

        public ICommand SaveAndCloseCommand
        {
            get { return new RelayCommand(param => this.OnSaveAndClose()); }
        }
    }
}
