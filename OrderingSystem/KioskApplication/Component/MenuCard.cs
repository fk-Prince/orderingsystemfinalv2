using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.KioskApplication.Cards
{
    public partial class MenuCard : UserControl
    {
        public readonly MenuModel menu;
        private readonly KioskMenuServices kioskMenuServices;
        public event EventHandler<List<OrderItemModel>> orderListEvent;

        public MenuCard(KioskMenuServices kioskMenuServices, MenuModel menu)
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
            dPrice.Text = menu.getPriceAfterVat() != menu.getPriceAfterVatWithDiscount()
                ? menu.getPriceAfterVatWithDiscount().ToString("C", new CultureInfo("en-PH"))
                : "0.00";
            dPrice.Visible = menu.getPriceAfterVatWithDiscount() != menu.getPriceAfterVat();
            v1.Visible = menu.getPriceAfterVatWithDiscount() != menu.getPriceAfterVat();
            v2.Visible = menu.getPriceAfterVatWithDiscount() != menu.getPriceAfterVat();

            sale.Visible = ooo.Visible || (menu.Discount != null && menu.Discount.DiscountId != 0);
            ooo.Visible = !(menu.MaxOrder <= 0);

            pp.BorderColor = Color.FromArgb(220, 220, 220);
            pp.BorderThickness = 1;
            pp.BackColor = Color.Transparent;
            pp.FillColor = Color.FromArgb(241, 241, 241);
        }
        private void menuClicked(object sender, EventArgs e)
        {
            PopupOption popup = new PopupOption(kioskMenuServices, menu);
            popup.orderListEvent += (s, args) => orderListEvent?.Invoke(this, args);
            DialogResult res = popup.ShowDialog(this);
            if (res == DialogResult.OK)
                popup.Hide();
        }
        private void handleClicked(Control c)
        {
            c.Click += menuClicked;
            foreach (Control cc in c.Controls)
                handleClicked(cc);
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
            price.Text = menu.getPriceAfterVat().ToString("C", new CultureInfo("en-PH"));
            image.Image = menu.MenuImage;
            description.Text = menu.MenuDescription;
        }
    }
}
