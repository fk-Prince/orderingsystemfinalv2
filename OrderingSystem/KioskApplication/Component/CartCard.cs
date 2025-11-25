using System;
using OrderingSystem.Model;
using UserControl = System.Windows.Forms.UserControl;
namespace OrderingSystem.KioskApplication.Components
{
    public partial class CartCard : UserControl
    {
        public OrderItemModel menu;
        public event EventHandler<OrderItemModel> addQuantityEvent;
        public event EventHandler<OrderItemModel> deductQuantityEvent;
        public CartCard(OrderItemModel menu)
        {
            InitializeComponent();
            this.menu = menu;
            displayPurchasedMenu();
        }

        public void displayPurchasedMenu()
        {
            menuName.Text = menu.PurchaseMenu.MenuName;
            price.Text = menu.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount().ToString("N2");
            image.Image = menu.PurchaseMenu.MenuImage;
            qty.Text = menu.PurchaseQty.ToString();
            bb.Text = qty.Text;
            total.Text = (Math.Round(menu.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount(), 2) * menu.PurchaseQty).ToString("N2");
        }

        private void addQuantity(object sender, System.EventArgs e)
        {
            addQuantityEvent.Invoke(this, menu);
            displayPurchasedMenu();
        }

        private void deductQuantity(object sender, System.EventArgs e)
        {
            deductQuantityEvent.Invoke(this, menu);
            displayPurchasedMenu();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string text = bb.Text.Trim();
            if (!int.TryParse(text, out int qty) || qty < 0)
                return;

            int maxAvailable = menu.PurchaseMenu.servingMenu.Quantity;
            int oldQty = menu.PurchaseQty;

            if (qty > maxAvailable)
                qty = maxAvailable;

            menu.PurchaseQty = qty;
            menu.PurchaseMenu.servingMenu.LeftQuantity = maxAvailable - qty;
            displayPurchasedMenu();
        }

        private void bb_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(bb.Text.Trim(), out int qty) || qty <= 0)
            {
                menu.PurchaseQty = 0;
                menu.PurchaseMenu.servingMenu.LeftQuantity = menu.PurchaseMenu.servingMenu.Quantity;

                var parentPanel = this.Parent as System.Windows.Forms.FlowLayoutPanel;
                if (parentPanel != null)
                    parentPanel.Controls.Remove(this);

                deductQuantityEvent?.Invoke(this, menu);
                return;
            }

            int maxAvailable = menu.PurchaseMenu.servingMenu.Quantity;
            menu.PurchaseMenu.servingMenu.LeftQuantity = maxAvailable - menu.PurchaseQty;
            displayPurchasedMenu();
        }
    }
}
