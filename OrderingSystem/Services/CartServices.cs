using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Components;
using OrderingSystem.KioskApplication.Interface;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.KioskApplication.Services
{
    public class CartServices : ICalculateOrder
    {
        private KioskMenuServices menuServices;
        private FlowLayoutPanel flowCart;
        private List<OrderItemModel> orderList;
        public event EventHandler quantityChanged;

        private CouponModel coupon;
        public CartServices(KioskMenuServices menuServices, FlowLayoutPanel flowCart, List<OrderItemModel> orderList)
        {
            this.menuServices = menuServices;
            this.flowCart = flowCart;
            this.orderList = orderList;
        }
        public void addMenuToCart(OrderItemModel newOrders)
        {
            OrderItemModel existingOrder = getOrder(newOrders);
            if (existingOrder != null)
            {
                existingOrder.PurchaseQty++;
                existingOrder.PurchaseMenu.servingMenu.LeftQuantity--;

                foreach (var i in flowCart.Controls.OfType<CartCard>())
                {
                    i.displayPurchasedMenu();
                }
            }
            else
            {
                orderList.Add(newOrders);
                addNewCartCard(newOrders);
            }
            quantityChanged?.Invoke(this, EventArgs.Empty);
        }
        private void addNewCartCard(OrderItemModel menu)
        {
            CartCard cc = new CartCard(menu);
            cc.addQuantityEvent += addQuantity;
            cc.deductQuantityEvent += deductQuantity;
            cc.Margin = new Padding(10, 5, 5, 5);
            flowCart.Controls.Add(cc);
        }
        public void addQuantity(object sender, OrderItemModel e)
        {
            try
            {
                CartCard cc = sender as CartCard;
                OrderItemModel order = getOrder(e);
                int b = order.PurchaseMenu.servingMenu.LeftQuantity;
                if (b <= 0)
                    throw new MaxOrder("Unable to add more quantity.");

                order.PurchaseQty++;
                cc.displayPurchasedMenu();
                quantityChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (MaxOrder)
            {
                throw;
            }
        }
        public void deductQuantity(object sender, OrderItemModel e)
        {
            CartCard cc = sender as CartCard;
            OrderItemModel order = getOrder(e);
            order.PurchaseQty--;
            order.PurchaseMenu.servingMenu.LeftQuantity++;
            if (order.PurchaseQty <= 0)
            {
                orderList.Remove(order);
                flowCart.Controls.Remove(cc);
            }
            cc.displayPurchasedMenu();
            quantityChanged?.Invoke(this, EventArgs.Empty);
        }
        public OrderItemModel getOrder(OrderItemModel e)
        {
            return orderList.FirstOrDefault(ex => ex.PurchaseMenu.servingMenu.ServingId == e.PurchaseMenu.servingMenu.ServingId);
        }
        public double calculateSubtotal()
        {
            return orderList.Sum(e => e.getSubtotal());
        }
        public double calculateCoupon(CouponModel coupon)
        {
            if (coupon == null) return 0.00;
            this.coupon = coupon;
            double subtotal = calculateSubtotal();
            return coupon.getCouponTotal(subtotal);
        }
        public double calculateTotalAmount()
        {
            double subtotal = calculateSubtotal();
            double coupon = calculateCoupon(this.coupon);
            return Math.Max(0, (subtotal - coupon));
        }
    }
}