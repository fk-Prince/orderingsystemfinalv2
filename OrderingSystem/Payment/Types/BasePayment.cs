using System;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment
{
    public abstract class BasePayment : IPayment
    {
        public abstract string PaymentName { get; }
        private OrderServices orderServices;
        public BasePayment(OrderServices orderServices)
        {
            this.orderServices = orderServices;
        }
        public virtual bool processPayment(OrderModel order)
        {
            validateOrder(order);
            return payOrder(order);
        }
        public virtual void validateOrder(OrderModel order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (string.IsNullOrWhiteSpace(order.OrderId))
                throw new ArgumentException("Invalid order ID.");
        }
        public virtual bool payOrder(OrderModel order, double fee = 0)
        {
            InvoiceModel i = new InvoiceModel(order.OrderId, order, SessionStaffData.StaffData, this, order.GetTotalWithVAT() + (1 * fee));
            return orderServices.payOrder(i);
        }
    }
}
