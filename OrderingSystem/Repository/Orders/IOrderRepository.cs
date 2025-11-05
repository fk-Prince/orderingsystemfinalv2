using System;
using System.Collections.Generic;
using System.Data;
using OrderingSystem.Model;

namespace OrderingSystem.Repository
{
    public interface IOrderRepository
    {
        bool getOrderAvailable(string order_id);
        bool getOrderExists(string order_id);
        OrderModel getOrders(string order_id);
        bool isOrderPayed(string order_id);
        bool saveNewOrder(OrderModel order);
        bool payOrder(OrderModel order, int staff_id, string payment_method);
        string getOrderId();
        List<string> getAvailablePayments();

        Tuple<TimeSpan, string> getTimeInvoiceWaiting(string order_id);
        DataView getOrderView(int offSet);
        bool voidOrder(string orderId);
    }
}
