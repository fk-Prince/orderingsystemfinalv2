using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment
{
    public class GCashPayment : BasePayment
    {
        public override string PaymentName => "G-Cash";
        public GCashPayment(OrderServices orderServices) : base(orderServices)
        {
        }
        public override bool processPayment(OrderModel order)
        {
            validateOrder(order);
            return payOrder(order);
        }
    }
}
