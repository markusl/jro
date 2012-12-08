using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database
{
    /// <summary>
    /// Extensions for Member.
    /// </summary>
    public static class MemberExtension
    {
        /// <summary>
        /// Check if the member is an actual member of the organization.
        /// Means that this method returns false for inactive and resigned members.
        /// </summary>
        public static bool IsMember(this Member member)
        {
            if (member == null) throw new ArgumentNullException("member");

            return !member.MemberDetais.membergroup.Equals(DBConstants.InactiveMember) &&
                   !member.MemberDetais.membergroup.Equals(DBConstants.ResignedMember);
        }

        /// <summary>
        /// Get the payment information of the member
        /// </summary>
        public static PaymentStatus GetPaymentStatus(this Member member)
        {
            if (member == null) throw new ArgumentNullException("member");

            return PaymentStatus.Parse(member.MemberDetais.paymentstatus);
        }
    }
}
