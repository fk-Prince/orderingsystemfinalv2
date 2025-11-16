using System;
using System.Collections.Generic;
using System.Data;
using OrderingSystem.Model;

namespace OrderingSystem.Repository
{
    public interface IOrderRepository
    {
        bool getOrderAvailable(string order_id);
        bool isOrderExists(string order_id);
        OrderModel getOrders(string order_id);
        bool isOrderPaid(string order_id);
        bool saveNewOrder(OrderModel order);
        bool payOrder(InvoiceModel i);
        string getLastestOrderID();
        List<string> getAvailablePayments();

        Tuple<TimeSpan, string, string> getTimeInvoiceWaiting(string order_id);
        DataView getOrderView(int offSet);
        bool voidOrder(string orderId);
        bool adjustOrderingTime();
        double getFeePaymentMethod(string paymentName);
    }
}
