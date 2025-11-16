using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment
{
    public class CashPayment : BasePayment, ICashHandling
    {
        private double cashReceived;
        public override string PaymentName => "Cash";

        public CashPayment(OrderServices orderServices)
           : base(orderServices)
        {
        }
        public virtual void validateCashAmount(double cashReceived, double totalAmount)
        {
            if (cashReceived <= 0)
                throw new InvalidPayment("Cash amount must be greater than zero.");

            if (totalAmount > cashReceived)
                throw new InsuffiecientAmount("The cash amount is insufficient to process the payment.");
        }

        public override bool processPayment(OrderModel order)
        {
            validateOrder(order);
            double totalAmount = order.GetTotalWithVAT();
            validateCashAmount(cashReceived, totalAmount);

            return payOrder(order);
        }
        public void setCashReceieved(double cashReceived)
        {
            this.cashReceived = cashReceived;
        }
        public double calculateChage(double total)
        {
            return cashReceived - total;
        }
    }
}
