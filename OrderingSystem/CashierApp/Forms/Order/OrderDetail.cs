using System;
using System.Globalization;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Layout;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Forms.Order
{
    public partial class OrderDetail : Form
    {
        private OrderServices or;
        private OrderItemModel om;
        private OrderModel omm;
        public OrderDetail(OrderServices or, OrderItemModel om, OrderModel omm)
        {
            InitializeComponent();
            this.or = or;
            this.om = om;
            this.omm = omm;
            image.Image = om.PurchaseMenu.MenuImage;
            name.Text = om.PurchaseMenu.MenuName;
            price.Text = om.PurchaseMenu.servingMenu.getPriceAfterVatWithDiscount().ToString("N2", new CultureInfo("en-PH"));
            qty.Text = om.PurchaseQty.ToString();
            total.Text = om.getSubtotal().ToString("N2", new CultureInfo("en-PH"));
            hide();
        }
        private void hide()
        {
            if (om.Status.ToLower() == "voided")
            {
                v.Visible = true;
            }
            else
            {
                b.Visible = false;
                b1.Visible = false;
                b2.Visible = false;
            }

            if (omm.OrderStatus.ToLower() == "pending")
            {
                b1.Visible = false;
                b2.Visible = false;
                if (om.Status.ToLower() == "voided")
                {
                    v.Visible = true;
                    b.Visible = false;
                }
                else
                {
                    b1.Visible = true;
                    b2.Visible = true;
                    b.Visible = true;
                }
            }
        }
        private void guna2PictureBox1_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        private void guna2Button1_Click(object sender, System.EventArgs e)
        {
            bool sucsc = false;
            if (SessionStaffData.Role != StaffModel.StaffRole.Manager)
            {
                ManagerLogin ml = new ManagerLogin();
                DialogResult rs1 = ml.ShowDialog(this);
                if (rs1 == DialogResult.OK)
                {
                    sucsc = true;
                }
            }
            else
            {
                sucsc = true;
            }
            if (sucsc)
            {
                DialogResult rs = MessageBox.Show("Are you sure you want to void this Menu Order it cannot be undone.", "Void Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    try
                    {
                        bool suc = or.voidOrderItem(om.OrderItemId.ToString());
                        if (suc)
                        {
                            hide();
                            DialogResult = DialogResult.OK;
                            MessageBox.Show("Successfully voided the Order Item", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Unable to void Order", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                int b = om.PurchaseMenu.servingMenu.LeftQuantity;
                if (b <= 0)
                    throw new MaxOrder("Unable to add more quantity.");

                om.PurchaseMenu.servingMenu.LeftQuantity -= 1;
                or.addQuantityOrderItem(om, om.PurchaseQty += 1);
                qty.Text = om.PurchaseQty.ToString();
                total.Text = om.getSubtotal().ToString("N2", new CultureInfo("en-PH"));
            }
            catch (MaxOrder ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                if (om.PurchaseQty <= 1) return;
                om.PurchaseQty -= 1;
                qty.Text = om.PurchaseQty.ToString();
                or.addQuantityOrderItem(om, om.PurchaseQty);
                total.Text = om.getSubtotal().ToString("N2", new CultureInfo("en-PH"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
