using System;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Component;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;
using OrderingSystem.Receipt;

namespace OrderingSystem.KioskApplication.Forms
{
    public partial class OrderLayout : Form
    {
        private OrderModel om;
        private OrderServices orderServices;
        public event EventHandler successfulPayment;
        public event EventHandler<OrderItemModel> AddQuantity;
        public event EventHandler<OrderItemModel> DeductQuantity;
        public bool isBrowsing;
        public OrderLayout(OrderModel om, OrderServices orderServices)
        {
            InitializeComponent();
            this.om = om;
            this.orderServices = orderServices;
            total.Text = "₱  " + om.GetTotalWithVAT().ToString("N2");
            foreach (var i in om.OrderItemList)
            {
                OrderCard oc = new OrderCard(i);
                oc.Margin = new Padding(50, 0, 0, 10);
                oc.AddQuantity += (s, e) =>
                {
                    AddQuantity?.Invoke(this, i);
                    total.Text = "₱  " + om.GetTotalWithVAT().ToString("N2");
                };
                oc.DeductQuantity += (s, e) =>
                {
                    DeductQuantity?.Invoke(this, i);
                    if (i.PurchaseQty == 0)
                    {
                        flow.Controls.Remove(oc);
                        om.OrderItemList.Remove(i);
                    }
                    total.Text = "₱  " + om.GetTotalWithVAT().ToString("N2");
                };
                flow.Controls.Add(oc);
            }
        }
        public void refersh()
        {
            foreach (var cc in flow.Controls.OfType<OrderCard>())
            {
                cc.refreshDetail();
            }
        }
        private void guna2Button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                OrderTypeFrm x = new OrderTypeFrm();
                x.OrderTypeChanged += (ss, ee) => om.Type = ee;
                if (!isBrowsing)
                {
                    bool suc = orderServices.confirmOrder(om);
                    if (suc)
                    {
                        OrderReceipt or = new OrderReceipt(om);
                        or.receiptMessages("Proceed to the cashier \n    Within 30 minutes",
                            DateTime.Now.AddMinutes(30).ToString("hh:mm:ss tt"),
                            om.Type.ToString());
                        or.print();
                        successfulPayment?.Invoke(this, EventArgs.Empty);
                        DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    DialogResult rs = x.ShowDialog(this);
                    if (rs == DialogResult.OK)
                    {
                        bool suc = orderServices.confirmOrder(om);
                        if (suc)
                        {
                            OrderReceipt or = new OrderReceipt(om);
                            or.receiptMessages("Proceed to the cashier \n    Within 30 minutes",
                                DateTime.Now.AddMinutes(30).ToString("hh:mm:ss tt"),
                                om.Type.ToString());
                            or.print();
                            successfulPayment?.Invoke(this, EventArgs.Empty);
                            DialogResult = DialogResult.OK;
                        }
                    }
                }
                x.Hide();
            }
            catch (OrderInvalid ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
