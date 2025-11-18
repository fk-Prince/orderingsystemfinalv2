using System;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment
{
    public abstract class Payment : IPayment
    {
        public abstract string PaymentName { get; }

        public virtual InvoiceModel processPayment(OrderModel order)
        {
            validateOrder(order);
            return finalizeOrder(order);
        }
        public virtual void validateOrder(OrderModel order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (string.IsNullOrWhiteSpace(order.OrderId))
                throw new ArgumentException("Invalid order ID.");
        }
        public virtual InvoiceModel finalizeOrder(OrderModel order, double fee = 0)
        {
            InvoiceModel i = new InvoiceModel(order.OrderId, order, SessionStaffData.StaffData, this, order.GetTotalWithVAT() + (1 * fee));
            return i;
        }
    }
}
