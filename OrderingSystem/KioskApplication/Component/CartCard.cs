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
            cardLayout();
        }

        private void cardLayout()
        {
            //pp.BorderRadius = 5;
            //BorderColor = Color.LightGray;
            //BorderThickness = 1;
            //FillColor = Color.FromArgb(255, 255, 255);
            //BackColor = Color.Transparent;
        }

        public void displayPurchasedMenu()
        {

            menuName.Text = menu.PurchaseMenu.MenuName;
            price.Text = menu.PurchaseMenu.getPriceAfterVatWithDiscount().ToString("N2");

            string text = "";
            if (menu.PurchaseMenu is MenuPackageModel p) text = "Package";
            else if (menu.PurchaseMenu.SizeName.ToLower() == menu.PurchaseMenu.FlavorName.ToLower()) text = "Regular";
            else if (menu.PurchaseMenu.SizeName.ToLower() == "regular" || menu.PurchaseMenu.FlavorName.ToLower() == "regular") text = menu.PurchaseMenu.FlavorName.ToLower() == "regular" ? menu.PurchaseMenu.SizeName : menu.PurchaseMenu.FlavorName;
            else text = "Flavor: " + menu.PurchaseMenu.FlavorName + " - Size:" + menu.PurchaseMenu.SizeName;

            image.Image = menu.PurchaseMenu.MenuImage;
            detail.Text = text;
            qty.Text = menu.PurchaseQty.ToString();
            bb.Text = qty.Text;
            total.Text = (Math.Round(menu.PurchaseMenu.getPriceAfterVatWithDiscount(), 2) * menu.PurchaseQty).ToString("N2");
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
    }
}
