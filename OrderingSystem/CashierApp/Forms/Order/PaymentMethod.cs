using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Payment;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Forms.Order
{
    public partial class PaymentMethod : Form
    {
        private OrderModel om;
        private OrderServices orderServices;

        public double Cash;
        public PaymentMethod(OrderServices orderServices)
        {
            InitializeComponent();
            this.orderServices = orderServices;

            List<string> x = orderServices.getAvailablePayments();
            x.ForEach(e => cb.Items.Add(e));
            if (x.Count > 0)
                cb.SelectedIndex = 0;

            if (x.Contains("Cash"))
                cb.SelectedIndex = x.IndexOf("Cash");


            cb_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void calculateChange(double cash)
        {
            if (double.TryParse(this.total.Text, out double total))
            {
                if (cash < total)
                {
                    t2.Text = "0.00";
                }
                else
                {
                    t2.Text = (cash - total).ToString("N2");
                }
            }
            else
            {
                t2.Text = "0.00";
            }
        }
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cb.SelectedIndex == -1)
                    throw new InvalidPayment("No Payment is Selected");

                IPaymentFactoryType factory = new PaymentFactory(orderServices);
                IPayment payment = factory.paymentType(cb.SelectedItem.ToString());

                if (!isCashValid(t1.Text.ToString().Trim()) && t1.Visible)
                    throw new InvalidPayment("Cash amount is invalid.");

                payment.calculateFee(om.OrderItemList.Sum(ex => ex.getTotal()));

                double cashAmount = cb.SelectedItem.ToString() == "Cash" ? double.Parse(t1.Text) : 0;
                bool suc = payment.processPayment(om, cashAmount);
                if (suc)
                {
                    MessageBox.Show("Successfull Payment", "Payment Method", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Cash = payment.getCash();
                }
                else
                    MessageBox.Show("Failed to Proceed Payment", "Payment Method", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Payment Method", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isCashValid(string v)
        {
            return double.TryParse(v, out double value) && value >= 0;
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb.SelectedIndex == -1) return;

            if (cb.SelectedItem.ToString() == "Cash")
            {
                t1.Visible = true;
                l1.Visible = true;
                t2.Visible = true;
                l2.Visible = true;
            }
            else
            {
                t1.Visible = false;
                l1.Visible = false;
                t2.Visible = false;
                l2.Visible = false;
            }
        }



        public void setOrder(OrderModel om)
        {
            this.om = om;
            total.Text = om.OrderItemList.Sum(e => e.getSubtotal()).ToString("N2");
        }

        private void t1_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(t1.Text, out double p))
            {
                calculateChange(p);
            }
        }
    }
}
