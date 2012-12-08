using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;

namespace DatabaseTest
{
    [TestClass]
    public class PaymentStatusTest
    {
        private Member SetupMemberWithPaymentStatus(string datefee, string feeamount)
        {
            Member m = Member.CreateMember(0, "John", "Doe", "", "", "", "", "", "");
            m.Address = Address.CreateAddress(0, "", "", "", "");
            m.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", String.Format("{0};{1}", datefee, feeamount));
            return m;
        }

        [TestMethod]
        public void TestPaymentStatusWithEmptyDateFee()
        {
            Member m = SetupMemberWithPaymentStatus("", "55,50");

            Assert.IsFalse(m.GetPaymentStatus().Paid);
            Assert.AreEqual(55.50, m.GetPaymentStatus().AmountLeft);
            Assert.AreEqual(DateTime.MinValue, m.GetPaymentStatus().PaymentDate);
        }

        [TestMethod]
        public void TestPaymentStatusWithCorrectPayment()
        {
            Member m = SetupMemberWithPaymentStatus("9.11.2009", "0,00");

            Assert.IsTrue(m.GetPaymentStatus().Paid);
            Assert.AreEqual(0.0, m.GetPaymentStatus().AmountLeft);
            Assert.AreEqual(DateTime.Parse("9.11.2009"), m.GetPaymentStatus().PaymentDate);
        }

        [TestMethod]
        public void TestPaymentStatusDoesNotThrowExceptionWhenPassingInvalidString()
        {
            PaymentStatus.Parse("");
        }
    }
}
