using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment.Types
{
    public class DebitCardPayment : Payment, IFeeCalculator
    {
        private readonly IFeeCalculator fee;
        public override string PaymentName => "Debit-Card";

        public double feePercent { get; }
        public DebitCardPayment(OrderServices orderServices) : base()
        {
            feePercent = orderServices.getFeePaymentMethod(PaymentName);
            fee = new FeeCalculator(orderServices.getFeePaymentMethod(PaymentName));
        }

        public override InvoiceModel processPayment(OrderModel order)
        {
            validateOrder(order);
            return finalizeOrder(order, feePercent);
        }

        public double calculateFee(double amount)
        {
            return fee.calculateFee(amount);
        }

        public double getTotalWithFee(double amount)
        {
            return fee.getTotalWithFee(amount);
        }
    }
}
