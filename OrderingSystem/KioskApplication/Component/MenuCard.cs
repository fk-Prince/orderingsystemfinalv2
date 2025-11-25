using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.KioskApplication.Cards
{
    public partial class MenuCard : UserControl
    {
        public readonly MenuDetailModel menu;
        private readonly KioskMenuServices kioskMenuServices;
        public event EventHandler<OrderItemModel> orderListEvent;

        public MenuCard(KioskMenuServices kioskMenuServices, MenuDetailModel menu)
        {
            InitializeComponent();
            this.kioskMenuServices = kioskMenuServices;
            this.menu = menu;

            displayMenu();
            cardLayout();
            handleClicked(pp);
            hoverEffects(pp);
        }

        public void outOfOrder(bool b)
        {
            ooo.Visible = b;
        }
        private void cardLayout()
        {
            dPrice.Text = menu.servingMenu.getPriceAfterVat() != menu.servingMenu.getPriceAfterVatWithDiscount()
                ? menu.servingMenu.getPriceAfterVatWithDiscount().ToString("C", new CultureInfo("en-PH"))
                : "0.00";
            dPrice.Visible = menu.servingMenu.getPriceAfterVatWithDiscount() != menu.servingMenu.getPriceAfterVat();
            v1.Visible = menu.servingMenu.getPriceAfterVatWithDiscount() != menu.servingMenu.getPriceAfterVat();
            v2.Visible = menu.servingMenu.getPriceAfterVatWithDiscount() != menu.servingMenu.getPriceAfterVat();

            sale.Visible = ooo.Visible || (menu.Discount != null && menu.Discount.DiscountId != 0);
            ooo.Visible = menu.servingMenu.LeftQuantity <= 0;

            pp.BorderColor = Color.FromArgb(220, 220, 220);
            pp.BorderThickness = 1;
            pp.BackColor = Color.Transparent;
            pp.FillColor = Color.FromArgb(241, 241, 241);
        }

        private void handleClicked(Control c)
        {
            c.Click += clicked;
            foreach (Control cc in c.Controls)
            {
                handleClicked(cc);
            }
        }

        private void clicked(object sender, EventArgs e)
        {
            if (menu.servingMenu.LeftQuantity > 0)
            {
                menu.servingMenu.LeftQuantity--;
                OrderItemModel m = new OrderItemModel(1, menu);
                orderListEvent?.Invoke(this, m);
            }
            else
                MessageBox.Show("No Servings Left", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void hoverEffects(Control c)
        {
            c.MouseEnter += (s, e) =>
            {
                pp.BorderRadius = 8;
                pp.BorderColor = Color.FromArgb(94, 148, 255);
                pp.BorderThickness = 2;
                pp.BackColor = Color.Transparent;
                pp.FillColor = Color.LightGray;
            };

            c.MouseLeave += (s, e) =>
            {
                pp.BorderRadius = 8;
                pp.BorderColor = Color.FromArgb(220, 220, 220);
                pp.BorderThickness = 1;
                pp.BackColor = Color.Transparent;
                pp.FillColor = Color.FromArgb(241, 241, 241);
            };

            foreach (Control cc in c.Controls)
                hoverEffects(cc);
        }
        private void displayMenu()
        {
            menuName.Text = menu.MenuName;
            price.Text = menu.servingMenu.getPriceAfterVat().ToString("C", new CultureInfo("en-PH"));
            image.Image = menu.MenuImage;
            description.Text = menu.MenuDescription;
        }
    }
}
