using System;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Payment
{
    public class CashPayment : IPayment
    {
        private double amount;
        private double cashReceived;
        private readonly OrderServices orderServices;
        public string PaymentName => "Cash";

        public CashPayment(OrderServices orderServices)
        {
            this.orderServices = orderServices;
        }
        public double calculateFee(double amount)
        {
            return this.amount = amount;
        }

        public bool processPayment(OrderModel order, double cashReceived)
        {
            if (SessionStaffData.StaffData == null)
                throw new ArgumentNullException("Staff information is required.");

            if (string.IsNullOrWhiteSpace(order.OrderId))
                throw new ArgumentException("Invalid order ID.");

            if (cashReceived <= 0)
                throw new InvalidPayment("Cash amount must be greater than zero.");

            if (amount > cashReceived)
                throw new InsuffiecientAmount("The cash amount is insufficient to process the payment.");

            this.cashReceived = cashReceived;
            return orderServices.payOrder(order, SessionStaffData.StaffId, PaymentName);
        }

        public double getCash()
        {
            return cashReceived;
        }
    }
}
