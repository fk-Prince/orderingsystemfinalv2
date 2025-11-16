using System;
using System.Collections.Generic;
using System.Data;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Repository;

namespace OrderingSystem.KioskApplication.Services
{
    public class OrderServices
    {
        private readonly IOrderRepository orderRepository;
        public OrderServices(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public string getLastestOrderID()
        {
            return orderRepository.getLastestOrderID();
        }
        public bool confirmOrder(OrderModel order)
        {
            if (order == null)
                throw new OrderInvalid("Order Error.");


            if (order.OrderItemList.Count == 0)
                throw new OrderInvalid("No Items in the Cart.");




            return orderRepository.saveNewOrder(order);
        }
        public bool isOrderAvailable(string order_id)
        {
            bool payed = orderRepository.isOrderPaid(order_id);
            if (payed)
                throw new OrderInvalid("This order is already process.");

            bool isAvalable = orderRepository.getOrderAvailable(order_id);
            if (!isAvalable)
                throw new OrderInvalid("Order-ID expired.");


            return true;
        }
        public OrderModel getOrders(string order_id)
        {
            bool existsting = orderRepository.isOrderExists(order_id);
            if (!existsting)
                throw new OrderNotFound("Order-ID not Found.");

            return orderRepository.getOrders(order_id); ;
        }
        public bool payOrder(InvoiceModel i)
        {
            return orderRepository.payOrder(i);
        }
        public List<string> getAvailablePayments()
        {
            return orderRepository.getAvailablePayments();
        }
        public Tuple<TimeSpan, string, string> getTimeInvoiceWaiting(string order_id)
        {
            return orderRepository.getTimeInvoiceWaiting(order_id);
        }
        public DataView getOrders(int offSet)
        {
            return orderRepository.getOrderView(offSet);
        }
        public bool voidOrder(string orderId)
        {
            return orderRepository.voidOrder(orderId);
        }
        public bool adjustOrderingTime()
        {
            return orderRepository.adjustOrderingTime();
        }
        public double getFeePaymentMethod(string paymentName)
        {
            return orderRepository.getFeePaymentMethod(paymentName);
        }
    }
}
