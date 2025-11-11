using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OrderingSystem.Model
{
    public class OrderModel
    {
        private string order_id;
        public List<OrderItemModel> OrderItemList { get; set; }
        public CouponModel Coupon { get; set; }
        public string OrderId { get => order_id; }
        public string OrderType { get; set; }


        public double GetGrossRevenue()
        {
            return OrderItemList.Sum(item => item.getSubtotal());
        }
        public double GetCouponDiscount()
        {
            if (Coupon == null) return 0;

            if (Coupon.getType() == CouponType.PERCENTAGE)
                return GetGrossRevenue() * Coupon.CouponRate;
            else if (Coupon.getType() == CouponType.FIXED)
                return Coupon.CouponRate;
            return 0;
        }
        public double GetTotalWithVAT()
        {
            return GetGrossRevenue() - GetCouponDiscount();
        }
        public double GetVATAmount()
        {
            double totalWithVAT = GetTotalWithVAT();
            double totalWithoutVAT = totalWithVAT / 1.12;
            return Math.Max(0, totalWithVAT - totalWithoutVAT);
        }
        public double GetAmountWithoutVAT()
        {
            return GetTotalWithVAT() / 1.12;
        }

        public string JsonOrderList()
        {
            return JsonConvert.SerializeObject(OrderItemList);
        }

        public static OrderBuilder Builder() => new OrderBuilder();

        public class OrderBuilder
        {
            private OrderModel _order;

            public OrderBuilder()
            {
                _order = new OrderModel();
            }

            public OrderBuilder WithOrderId(string c)
            {
                _order.order_id = c;
                return this;
            }
            public OrderBuilder WithOrderType(string c)
            {
                _order.OrderType = c;
                return this;
            }

            public OrderBuilder WithOrderItemList(List<OrderItemModel> c)
            {
                _order.OrderItemList = c;
                return this;
            }

            public OrderBuilder WithCoupon(CouponModel c)
            {
                _order.Coupon = c;
                return this;
            }

            public OrderModel Build()
            {
                return _order;
            }
        }
    }
}