using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OrderingSystem.Model
{
    public class OrderModel
    {
        public enum OrderType
        {
            DINE_IN,
            TAKE_OUT
        }

        private string order_id;

        public List<OrderItemModel> OrderItemList { get; set; }
        public CouponModel Coupon { get; set; }
        public string OrderId { get => order_id; }
        public OrderType Type { get; set; }
        public string OrderId1 { get; }
        public CouponModel CouponSelected { get; }
        public OrderModel(string orderId, List<OrderItemModel> orderList, CouponModel couponSelected, OrderType orderType)
        {
            order_id = orderId;
            OrderItemList = orderList;
            CouponSelected = couponSelected;
            Type = orderType;
        }
        public OrderModel(string orderId, CouponModel couponModel, List<OrderItemModel> oim)
        {
            order_id = orderId;
            CouponSelected = couponModel;
            OrderItemList = oim;
        }

        public static OrderType getOrderType(string t)
        {
            return t.ToUpper().Replace("-", "_") == "DINE_IN" ? OrderType.DINE_IN : OrderType.TAKE_OUT;
        }
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

    }
}