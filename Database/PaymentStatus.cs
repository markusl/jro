using System;

namespace Database
{
    /// <summary>
    /// Holds the information of payment status of the member, eg. whether the member has paid the membership fee.
    /// </summary>
    public class PaymentStatus
    {
        readonly DateTime _paymentDate = DateTime.MinValue;
        readonly double _amountLeft = 0.0;

        /// <summary>
        /// Check if the member has paid the membership fee.
        /// </summary>
        public bool Paid { get; private set; }

        /// <summary>
        /// How much there is left to pay.
        /// </summary>
        public double AmountLeft { get { return _amountLeft; }
        }

        /// <summary>
        /// The date when member paid the fee, or if in the future, the due date of the bill.
        /// </summary>
        public DateTime PaymentDate { get { return _paymentDate; }
        }

        private PaymentStatus(string paymentStatus)
        {
            Paid = false;

            string[] values = paymentStatus.Split(';');
            if (!String.IsNullOrEmpty(values[0]))
                DateTime.TryParse(values[0], out _paymentDate);

            if (values.Length > 1)
                Double.TryParse(values[1], out _amountLeft);

            if (_paymentDate != DateTime.MinValue && _amountLeft <= 0.0)
                Paid = true;
        }

        /// <summary>
        /// Parse from database string. Format is: paymentdate;fee, where payment data
        /// is a string that can be parsed with DateTime.Parse and fee is a double value
        /// containing the sum that the member must pay.
        /// </summary>
        public static PaymentStatus Parse(string paymentStatus)
        {
            return new PaymentStatus(paymentStatus);
        }
    }
}
