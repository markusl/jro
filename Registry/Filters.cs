using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using Database;
using Registry.ViewModel;
using Registry.Interfaces;

namespace Registry
{
    [Export(typeof(IMemberViewFilter))]
    class ShowAllMembersFilter : IMemberViewFilter
    {
        public int Priority { get { return 1; } }
        public string Name { get { return "Näytä kaikki"; } }
        public Predicate<MemberViewModel> Filter
        {
            get
            {
                return delegate(MemberViewModel member) { return member.IsMember; };
            }
        }
    }

    [Export(typeof(IMemberViewFilter))]
    class ShowPaidMembersFilter : IMemberViewFilter
    {
        public int Priority { get { return 5; } }
        public string Name { get { return "Maksaneet"; } }
        public Predicate<MemberViewModel> Filter
        {
            get
            {
                return delegate(MemberViewModel member)
                {
                    return member.IsMember && member.Payment.Paid;
                };
            }
        }
    }

    [Export(typeof(IMemberViewFilter))]
    class ShowNonPaidMembersFilter : IMemberViewFilter
    {
        public int Priority { get { return 6; } }
        public string Name { get { return "Maksamattomat"; } }
        public Predicate<MemberViewModel> Filter
        {
            get
            {
                return delegate(MemberViewModel member)
                {
                    return member.IsMember && !member.Payment.Paid;
                };
            }
        }
    }

    [Export(typeof(IMemberViewFilter))]
    class ShowResignedMembersFilter : IMemberViewFilter
    {
        public int Priority { get { return 9; } }
        public string Name { get { return "Eronneet (polusta)"; } }
        public Predicate<MemberViewModel> Filter
        {
            get
            {
                return delegate(MemberViewModel member) { return member.IsResigned; };
            }
        }
    }

    [Export(typeof(IMemberViewFilter))]
    class ShowInactiveMembersFilter : IMemberViewFilter
    {
        public int Priority { get { return 10; } }
        public string Name { get { return "Epäaktiiviset"; } }
        public Predicate<MemberViewModel> Filter
        {
            get
            {
                return delegate(MemberViewModel member) { return member.IsInactive; };
            }
        }
    }

    [Export(typeof(IMemberViewFilter))]
    class ShowNonBilledMembersFilter : IMemberViewFilter
    {
        public int Priority { get { return 10; } }
        public string Name { get { return "Uudet jäsenet"; } }
        public Predicate<MemberViewModel> Filter
        {
            get
            {
                return delegate(MemberViewModel member)
                {
                    return member.IsMember && member.Payment.PaymentDate == DateTime.MinValue;
                };
            }
        }
    }
}
