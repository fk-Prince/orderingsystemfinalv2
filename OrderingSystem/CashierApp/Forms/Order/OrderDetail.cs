using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Layout;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;
using OrderingSystem.Repository;
using OrderingSystem.Services;

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
            flavor.Text = om.PurchaseMenu.FlavorName;
            size.Text = om.PurchaseMenu.SizeName;
            qtyToAdd.Value = om.PurchaseQty;
            price.Text = om.PurchaseMenu.getPriceAfterVatWithDiscount().ToString("N2", new CultureInfo("en-PH"));
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
                qtyToAdd.Visible = false;
            }

            if (omm.OrderStatus.ToLower() == "pending")
            {
                qtyToAdd.Visible = false;
                if (om.Status.ToLower() == "voided")
                {
                    v.Visible = true;
                    b.Visible = false;
                }
                else
                {
                    qtyToAdd.Visible = true;
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
        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                List<OrderItemModel> o = new List<OrderItemModel>();
                o.AddRange(omm.OrderItemList); KioskMenuServices kk = new KioskMenuServices(new KioskMenuRepository(o));
                int b = kk.getMaxOrderRealTime(om.PurchaseMenu.MenuDetailId, o);
                if (b <= 0) throw new MaxOrder("Unable to add more quantity.");
                or.addQuantityOrderItem(om, (int)qtyToAdd.Value);
                qty.Text = qtyToAdd.Value.ToString();
                om.PurchaseQty = (int)qtyToAdd.Value;
                total.Text = om.getSubtotal().ToString("N2", new CultureInfo("en-PH"));
            }
            catch (MaxOrder ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                qtyToAdd.Value = (int)qtyToAdd.Value - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
