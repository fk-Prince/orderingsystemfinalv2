namespace OrderingSystem.Model
{
    public class OrderItemModel
    {

        public int OrderItemId { get; protected set; }
        public int PurchaseQty { get; set; }
        public MenuDetailModel PurchaseMenu { get; protected set; }


        public OrderItemModel(int orderItemId, int purchaseQty, MenuDetailModel purchaseMenu)
        {
            OrderItemId = orderItemId;
            PurchaseQty = purchaseQty;
            PurchaseMenu = purchaseMenu;
        }

        public OrderItemModel(int purchaseQty, MenuDetailModel purchaseMenu)
        {
            PurchaseQty = purchaseQty;
            PurchaseMenu = purchaseMenu;
        }
        public double getSubtotal()
        {
            return PurchaseMenu.getPriceAfterVatWithDiscount() * PurchaseQty;
        }
    }
}

