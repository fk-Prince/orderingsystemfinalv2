using OrderingSystem.CashierApp.Payment.Types;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;

namespace OrderingSystem.CashierApp.Payment
{
    public class PaymentFactory : IPaymentFactory
    {
        private OrderServices orderServices;
        public PaymentFactory(OrderServices orderServices)
        {
            this.orderServices = orderServices;
        }
        public IPayment paymentType(string type)
        {
            if (type.ToLower() == "cash")
                return new CashPayment(orderServices);
            else if (type.ToLower() == "g-cash" || type.ToLower() == "gcash")
                return new GCashPayment(orderServices);
            else if (type.ToLower() == "debitcard" || type.ToLower() == "debit-card")
                return new DebitCardPayment(orderServices);
            else
                throw new InvalidPayment("Payment Not Supported Yet.");
        }
    }
}
