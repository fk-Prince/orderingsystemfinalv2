using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.Forms.Order;
using OrderingSystem.CashierApp.Payment;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;
using OrderingSystem.Receipt;
using OrderingSystem.Repository;
using OrderingSystem.Repository.Order;

namespace OrderingSystem.CashierApp.Forms
{
    public partial class OrderFrm : Form
    {
        private DataTable table;
        private OrderModel om;
        private readonly OrderServices orderServices;
        private readonly IOrderRepository orderRepository;

        public OrderFrm()
        {
            InitializeComponent();
            orderRepository = new OrderRepository();
            orderServices = new OrderServices(orderRepository);
            initTable();
        }
        private void initTable()
        {
            table = new DataTable();
            table.Columns.Add("Order-ID");
            table.Columns.Add("Name");
            table.Columns.Add("Menu Order Status");
            table.Columns.Add("Price");
            table.Columns.Add("Quantity");
            table.Columns.Add("Total Amount", typeof(string));
            dataGrid.DataSource = table;
        }
        private void displayOrders(string orderId)
        {
            try
            {
                table.Rows.Clear();
                om = orderServices.getOrders(orderId);
                if (om.OrderItemList.Count > 0)
                    foreach (var order in om.OrderItemList)
                        table.Rows.Add(om.OrderId, order.PurchaseMenu.MenuName, order.Status, order.PurchaseMenu.getPriceAfterVatWithDiscount().ToString("N2"),
                            order.PurchaseQty, order.Status.ToLower() == "voided" ? "0.00" : order.getSubtotal().ToString("N2"));

                double withoutVat = om.OrderItemList.Sum(o => o.Status.ToLower() == "voided" ? 0.00 : o.PurchaseMenu.MenuPrice * o.PurchaseQty);
                double subtotald = om.GetGrossRevenue();
                double couponRated = om.GetCouponDiscount();
                double vatd = om.GetVATAmount();
                double totald = om.GetTotalWithVAT();

                subtotal.Text = "₱ " + subtotald.ToString("N2", new CultureInfo("en-PH"));
                coupon.Text = "₱ " + couponRated.ToString("N2", new CultureInfo("en-PH"));
                vat.Text = "₱ " + vatd.ToString("N2", new CultureInfo("en-PH"));
                wo.Text = "₱ " + withoutVat.ToString("N2", new CultureInfo("en-PH"));
                total.Text = "₱ " + totald.ToString("N2", new CultureInfo("en-PH"));
            }

            catch (Exception ex) when (ex is OrderInvalid || ex is OrderNotFound)
            {
                MessageBox.Show(ex.Message, "Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal Server Error" + ex.Message, "Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void reset(object sender, System.EventArgs e)
        {
            clear();
        }
        private void clear()
        {
            table.Clear();
            txt.Text = "";
            om = null;
            subtotal.Text = "0.00";
            coupon.Text = "0.00";
            vat.Text = "0.00";
            total.Text = "0.00";
            wo.Text = "0.00";
        }
        private void payment(object sender, System.EventArgs e)
        {
            try
            {
                if (om == null)
                {
                    MessageBox.Show("Enter an Order ID", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                bool suc = orderServices.isOrderAvailable(om.OrderId);

                if (om.OrderStatus.ToLower() == "voided")
                {
                    MessageBox.Show("Cannot Proceed with void Order", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (suc)
                {
                    PaymentMethod p = new PaymentMethod(orderServices);
                    p.setOrder(om);
                    DialogResult rs = p.ShowDialog(this);

                    if (rs == DialogResult.OK)
                    {
                        Tuple<TimeSpan, string, string> xd = orderServices.getTimeInvoiceWaiting(om.OrderId);
                        OrderReceipt or = new OrderReceipt(om);
                        or.Cash(p.Cash);
                        Payment.Payment pm = p.paymentT;
                        double fee = 0;
                        if (pm is IFeeCalculator f)
                            fee = f.feePercent;

                        InvoiceModel i = p.inv;
                        or.setInvoice(i);

                        or.receiptMessages("Wait for your Order", xd.Item1.ToString(@"hh\:mm\:ss"), xd.Item3);
                        or.print();
                        p.Hide();
                        clear();
                    }
                }
            }

            catch (Exception ex) when (ex is OrderInvalid || ex is OrderNotFound)
            {
                MessageBox.Show(ex.Message, "Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal Server Error" + ex.Message, "Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txt_MouseDown(object sender, MouseEventArgs e)
        {
            var txt = sender as Guna2TextBox;
            Rectangle iconBounds = new Rectangle(13, 19, 35, 35);

            if (iconBounds.Contains(e.Location))
            {
                displayOrders(txt.Text.Trim());
            }
        }
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                displayOrders(txt.Text.Trim());
                e.Handled = true;
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OrdersPopup pop = new OrdersPopup(orderServices);
            pop.selectedOrder += (ss, ee) => displayOrders(ee);
            DialogResult rs = pop.ShowDialog(this);
            if (rs == DialogResult.OK)
                pop.Hide();
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                bool suc = orderServices.adjustOrderingTime();
                if (suc)
                    MessageBox.Show("Successfully Adjusted the pending orders", "TIME", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Failed to adjust", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || om == null || om.OrderItemList == null || om.OrderItemList.Count == 0)
                return;

            int index = e.RowIndex;

            if (index >= 0 && index < om.OrderItemList.Count)
            {
                var selectedItem = om.OrderItemList[index];

                OrderDetail od = new OrderDetail(orderServices, selectedItem, om);
                DialogResult rs = od.ShowDialog(this);
                if (rs == DialogResult.OK)
                {
                    displayOrders(om.OrderId);
                    od.Hide();
                }
            }
        }
    }
}
