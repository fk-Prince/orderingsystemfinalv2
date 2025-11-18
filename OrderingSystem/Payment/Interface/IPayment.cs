using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment
{
    public interface IPayment
    {
        string PaymentName { get; }
        InvoiceModel processPayment(OrderModel order);
    }
}
