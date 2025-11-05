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
        public string getNewOrderId()
        {
            return orderRepository.getOrderId();
        }
        public bool confirmOrder(OrderModel order)
        {
            if (order == null)
                throw new OrderInvalid("Order Error.");


            if (order.OrderItemList.Count == 0)
                throw new OrderInvalid("No Items in the Cart.");




            return orderRepository.saveNewOrder(order);
        }
        public OrderModel getAllOrders(string order_id)
        {
            bool existsting = orderRepository.getOrderExists(order_id);
            if (!existsting)
            {
                throw new OrderNotFound("Order-ID not Found.");
            }
            bool isAvalable = orderRepository.getOrderAvailable(order_id);
            if (!isAvalable)
            {
                throw new OrderInvalid("Order-ID expired.");
            }

            bool payed = orderRepository.isOrderPayed(order_id);
            if (payed)
            {
                throw new OrderInvalid("This order is already process.");
            }
            return orderRepository.getOrders(order_id); ;
        }
        public bool payOrder(OrderModel order, int staff_id, string payment_method)
        {
            return orderRepository.payOrder(order, staff_id, payment_method);
        }

        public List<string> getAvailablePayments()
        {
            return orderRepository.getAvailablePayments();
        }

        public Tuple<TimeSpan, string> getTimeInvoiceWaiting(string order_id)
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
    }
}
