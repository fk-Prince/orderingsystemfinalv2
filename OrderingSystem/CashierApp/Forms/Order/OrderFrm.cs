using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.Forms.Order;
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
        string orderId;
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
            //table.Columns.Add("Note");
            //table.Columns.Add("Note Approval", typeof(bool));
            table.Columns.Add("Price");
            table.Columns.Add("Quantity");
            table.Columns.Add("Total Amount", typeof(string));
            dataGrid.DataSource = table;
            //dataGrid.Columns["Note Approval"].Width = 70;
            //DataGridViewCheckBoxColumn fx = new DataGridViewCheckBoxColumn();
            //fx.DataPropertyName = "Note Approval";
            //fx.HeaderText = "Note Approval";
            //dataGrid.CellValueChanged += (s, e) =>
            //{
            //    if (e.ColumnIndex == dataGrid.Columns["Note Approval"].Index && e.RowIndex >= 0)
            //    {
            //        bool approved = (bool)dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            //        om.OrderItemList[e.RowIndex].NoteApproved = approved;
            //    }
            //};
        }

        private void displayOrders()
        {
            try
            {
                orderId = txt.Text.Trim();
                om = orderServices.getAllOrders(orderId);
                if (om.OrderItemList.Count > 0)
                    foreach (var order in om.OrderItemList)
                        table.Rows.Add(om.OrderId, order.PurchaseMenu.MenuName, order.PurchaseMenu.getPriceAfterVatWithDiscount().ToString("N2"), order.PurchaseQty, order.getSubtotal().ToString("N2"));

                double subtotald = om.OrderItemList.Sum(o => o.getSubtotal());
                double couponRated = subtotald * (om.Coupon == null ? 0 : om.Coupon.CouponRate);
                double vatd = om.OrderItemList.Sum(o => (subtotald - couponRated) * 0.12);
                double withoutVat = om.OrderItemList.Sum(o => o.PurchaseMenu.MenuPrice * o.PurchaseQty);
                double totald = (subtotald - couponRated);

                double rated = om.Coupon.CouponRate * 100;
                subtotal.Text = "₱ " + subtotald.ToString("N2", new CultureInfo("en-PH"));
                coupon.Text = "₱ " + couponRated.ToString("N2", new CultureInfo("en-PH"));
                rate.Text = rated != 0 ? rated.ToString() + "%" : "";
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
            subtotal.Text = "0.00";
            coupon.Text = "0.00";
            vat.Text = "0.00";
            total.Text = "0.00";
            wo.Text = "0.00";
        }
        private void cashPayment(object sender, System.EventArgs e)
        {
            PaymentMethod p = new PaymentMethod(orderServices);
            p.setOrder(om);
            DialogResult rs = p.ShowDialog(this);

            if (rs == DialogResult.OK)
            {
                Tuple<TimeSpan, string> xd = orderServices.getTimeInvoiceWaiting(om.OrderId);
                OrderReceipt or = new OrderReceipt(om);
                or.Cash(p.Cash);
                or.Message("Wait for your Order", xd.Item1.ToString(@"hh\:mm\:ss"), xd.Item2);
                or.print();
                p.Hide();
                clear();
            }
        }

        private void txt_MouseDown(object sender, MouseEventArgs e)
        {
            var txt = sender as Guna2TextBox;
            Rectangle iconBounds = new Rectangle(13, 19, 35, 35);

            if (iconBounds.Contains(e.Location))
            {
                displayOrders();
            }
        }
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                displayOrders();
                e.Handled = true;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OrdersPopup pop = new OrdersPopup(orderServices);
            DialogResult rs = pop.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                pop.Hide();
            }
        }
    }
}
