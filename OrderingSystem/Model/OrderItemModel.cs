using System;

namespace OrderingSystem.Model
{
    public class OrderItemModel
    {

        public int OrderItemId { get; protected set; }
        public int PurchaseQty { get; set; }
        public MenuDetailModel PurchaseMenu { get; protected set; }
        public string Status { get; set; }

        public OrderItemModel(int purchaseQty, MenuDetailModel purchaseMenu)
        {
            PurchaseQty = purchaseQty;
            PurchaseMenu = purchaseMenu;
        }
        public OrderItemModel(int id, int purchaseQty, MenuDetailModel purchaseMenu)
        {
            OrderItemId = id;
            PurchaseQty = purchaseQty;
            PurchaseMenu = purchaseMenu;
        }

        public double getSubtotal()
        {
            return (Math.Round(PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount(), 2) * PurchaseQty);
        }
    }
}

