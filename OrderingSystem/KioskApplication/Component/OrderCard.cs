using System;
using System.Windows.Forms;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication.Component
{
    public partial class OrderCard : UserControl
    {

        public event EventHandler<OrderItemModel> AddQuantity;
        public event EventHandler<OrderItemModel> DeductQuantity;
        private OrderItemModel om;
        public OrderCard(OrderItemModel om)
        {
            InitializeComponent();
            this.om = om;
            refreshDetail();
        }

        private void addQuantity(object sender, EventArgs e)
        {
            AddQuantity?.Invoke(this, om);
        }


        private void deductQuantity(object sender, System.EventArgs e)
        {
            DeductQuantity?.Invoke(this, om);
        }

        public void refreshDetail()
        {
            image.Image = om.PurchaseMenu.MenuImage;
            name.Text = om.PurchaseMenu.MenuName;


            price.Text = om.PurchaseMenu.servingMenu.getPriceAfterVat().ToString("N2");
            dPrice.Text = om.PurchaseMenu.servingMenu.getPriceAfterVat() != om.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount()
                ? om.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount().ToString("N2") : "0.00";
            dPrice.Visible = om.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount() != om.PurchaseMenu.servingMenu.getPriceAfterVat();
            v1.Visible = om.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount() != om.PurchaseMenu.servingMenu.getPriceAfterVat();
            v2.Visible = om.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount() != om.PurchaseMenu.servingMenu.getPriceAfterVat();
            qty.Text = om.PurchaseQty.ToString();
            total.Text = om.getSubtotal().ToString("N2");
        }
    }
}
