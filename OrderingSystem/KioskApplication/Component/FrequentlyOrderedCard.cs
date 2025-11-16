using System;
using System.Drawing;
using System.Windows.Forms;
using OrderingSystem.Model;
namespace OrderingSystem.KioskApplication.Components
{
    public partial class FrequentlyOrderedCard : UserControl
    {
        private MenuDetailModel menu;
        public event EventHandler<OrderItemModel> checkedMenu;
        public event EventHandler<OrderItemModel> unCheckedMenu;

        public FrequentlyOrderedCard(MenuDetailModel menu)
        {
            InitializeComponent();
            this.menu = menu;
            cardLayout();
            displayMenu();
            cardChecked();
        }

        private void cardChecked()
        {
            checkBox.Checked = false;
            checkBox.CheckedChanged += (s, e) =>
            {
                if (checkBox.Checked)
                {
                    pp.BorderColor = Color.FromArgb(94, 148, 255);
                    pp.BorderThickness = 2;
                    checkedMenu.Invoke(this, new OrderItemModel(1, menu));
                }
                else
                {
                    pp.BorderColor = Color.DarkGray;
                    pp.BorderThickness = 1;
                    unCheckedMenu.Invoke(this, new OrderItemModel(1, menu));
                    menu = null;
                }
            };
        }

        private void displayMenu()
        {
            menuName.Text = menu.MenuName;
            detail.Text = menu.SizeName;
            price.Text = "₱       + " + menu.getPriceAfterVatWithDiscount().ToString("N2");
            image.Image = menu.MenuImage;
        }
        private void cardLayout()
        {
            pp.BorderRadius = 5;
            pp.BorderColor = Color.DarkGray;
            pp.BorderThickness = 1;
            pp.FillColor = Color.FromArgb(244, 244, 244);
            pp.BackColor = Color.Transparent;
        }
    }
}
