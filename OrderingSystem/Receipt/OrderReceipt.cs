using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Payment;
using OrderingSystem.Model;

namespace OrderingSystem.Receipt
{
    public partial class OrderReceipt : Form
    {
        private Image image = Properties.Resources.finaldashicon;
        private readonly string orderId;
        private readonly List<OrderItemModel> menus;
        private readonly OrderModel om;
        private InvoiceModel invoice;

        private int y = 170;
        private int x = 10;
        private string message;
        private string type;
        private double cash;
        private string estimated_date;

        public OrderReceipt(OrderModel om)
        {
            InitializeComponent();
            this.orderId = om.OrderId;
            this.menus = om.OrderItemList;
            this.om = om;
        }

        public void setInvoice(InvoiceModel invoice)
        {
            this.invoice = invoice;
        }

        public void print()
        {
            int baseHeight = 700;
            int rowHeight = 50;
            int height = menus.Count > 1 ? Math.Max(rowHeight * menus.Count + baseHeight, baseHeight) : baseHeight;

            PaperSize customSize = new PaperSize("Custom", 400, height);
            printDocument.DefaultPageSettings.PaperSize = customSize;
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            printPreviewDialog.Document = printDocument;
            printPreviewDialog.WindowState = FormWindowState.Maximized;
            printPreviewDialog.ShowDialog();
        }

        public void receiptMessages(string message, string estimated_date, string type)
        {
            this.message = message;
            this.estimated_date = estimated_date;
            this.type = type;
        }

        public void Cash(double cash)
        {
            this.cash = cash;
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(image, 235, 20, 140, 140);
            e.Graphics.DrawString("PandaBite", new Font("Segui UI", 23, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline), Brushes.Black, 15, 25);
            e.Graphics.DrawString("506 J.P. Laurel Ave,", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, 15, 65);
            e.Graphics.DrawString("Poblacion District, Davao City", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, 15, 80);
            if (invoice != null)
            {
                e.Graphics.DrawString("Cashier Name.: ", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, 15, y);
                e.Graphics.DrawString(invoice.Staff.getFullName(), new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline), Brushes.Black, 110, y);
                y += 20;
                e.Graphics.DrawString("Invoice ID.: ", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, 15, y);
                e.Graphics.DrawString(invoice.InvoiceId, new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline), Brushes.Black, 90, y);
                y += 20;
                e.Graphics.DrawString("Order No.: ", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, 15, y);
                e.Graphics.DrawString(invoice.Order.OrderId.ToString(), new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline | FontStyle.Bold), Brushes.Black, 90, y);
            }
            else
            {
                y += 20;
                e.Graphics.DrawString("Order No.: ", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, 15, y);
                e.Graphics.DrawString(orderId.ToString(), new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline | FontStyle.Bold), Brushes.Black, 90, y);
            }

            if (!string.IsNullOrEmpty(type))
            {
                SizeF size2 = e.Graphics.MeasureString(type, new Font("Segoe UI", 9, FontStyle.Regular));
                e.Graphics.DrawString(type.Replace("_", "-").ToUpper(), new Font("Sans-serif", 9), Brushes.Black, e.PageBounds.Width - size2.Width - 10, y);
            }
            y += 50;

            e.Graphics.DrawLine(Pens.Black, x + 25, y, x + 25, y + 20);
            e.Graphics.DrawString("Qty", new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline), Brushes.Black, x, y);
            x += 30;
            e.Graphics.DrawString("Menu Name", new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline), Brushes.Black, x, y);
            x += 290;
            e.Graphics.DrawLine(Pens.Black, x - 10, y, x - 10, y + 20);
            e.Graphics.DrawString("Price", new Font("Segui UI", 9, FontStyle.Regular | FontStyle.Underline), Brushes.Black, x, y);
            x = 10;
            y += 20;


            Brush brush = Brushes.Black;
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            Font mainFont = new Font("Segui UI", 9);
            int line = y + 30;
            for (int i = 0; i < menus.Count; i++)
            {
                OrderItemModel a = menus[i];
                e.Graphics.DrawString(a.PurchaseQty.ToString(), mainFont, Brushes.Black, x, y);
                e.Graphics.DrawLine(Pens.Black, x + 25, y - 30, x + 25, line);
                e.Graphics.DrawLine(Pens.Black, x + 310, y - 30, x + 310, line);
                x += 30;
                e.Graphics.DrawString(a.PurchaseMenu.MenuName, mainFont, Brushes.Black, x, y);
                y += 15;
                x += 30;
                if (!(a.PurchaseMenu is MenuPackageModel))
                {
                    string va = a.PurchaseMenu.SizeName?.ToLower().Trim() == a.PurchaseMenu.FlavorName?.ToLower().Trim() ? "( " + a.PurchaseMenu.SizeName + " )" : "( " + a.PurchaseMenu.SizeName + " - " + a.PurchaseMenu.FlavorName + " )";
                    e.Graphics.DrawString(va, mainFont, Brushes.Black, x, y);
                }
                x += 260;
                e.Graphics.DrawString(a.getSubtotal().ToString("N2"), mainFont, Brushes.Black, x, y);
                x = 10;
                y += 35;
                line = y + 50;

            }

            y += 20;
            SizeF size1;
            double subtotal = om.GetGrossRevenue();
            double couponDiscount = om.GetCouponDiscount();
            double totalWithVAT = om.GetTotalWithVAT();
            double vatAmount = om.GetVATAmount();
            double amountWithoutVAT = om.GetAmountWithoutVAT();

            int y1 = y;

            e.Graphics.DrawString("Subtotal", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
            y += 20;

            if (couponDiscount > 0)
            {
                e.Graphics.DrawString("Coupon Discount", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
                y += 20;
            }

            e.Graphics.DrawString("Amount Due", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
            y += 20;
            e.Graphics.DrawString("Less: VAT (12%)", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
            y += 20;
            e.Graphics.DrawString("VATable Sales", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
            y += 20;

            if (invoice != null && invoice.payment != null && invoice.payment is IFeeCalculator f)
            {
                e.Graphics.DrawString($"Payment Method: {invoice.payment.PaymentName} {f.feePercent * 100}%", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
                y += 20;
            }
            if (invoice != null && invoice.payment != null && invoice.specialDiscount.ToLower() != "regular")
            {
                e.Graphics.DrawString($"SC:PWD Discount", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x, y);
                y += 20;
            }


            e.Graphics.DrawString("Total Amount Due", new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x, y);

            if (cash != 0)
            {
                y += 20;
                e.Graphics.DrawString("Cash", new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x, y);
                y += 20;
                e.Graphics.DrawString("Change", new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x, y);
            }

            x += 360;

            size1 = graphics.MeasureString(subtotal.ToString("N2"), mainFont);
            e.Graphics.DrawString(subtotal.ToString("N2"), mainFont, Brushes.Black, x - size1.Width, y1);
            y1 += 20;

            if (couponDiscount > 0)
            {
                size1 = graphics.MeasureString(couponDiscount.ToString("N2"), mainFont);
                e.Graphics.DrawString("(" + couponDiscount.ToString("N2") + ")", mainFont, Brushes.Black, x - size1.Width, y1);
                y1 += 20;
            }

            size1 = graphics.MeasureString(totalWithVAT.ToString("N2"), mainFont);
            e.Graphics.DrawString(totalWithVAT.ToString("N2"), mainFont, Brushes.Black, x - size1.Width, y1);
            y1 += 20;

            size1 = graphics.MeasureString(vatAmount.ToString("N2"), mainFont);
            e.Graphics.DrawString("(" + vatAmount.ToString("N2") + ")", mainFont, Brushes.Black, x - size1.Width, y1);
            y1 += 20;

            size1 = graphics.MeasureString(amountWithoutVAT.ToString("N2"), mainFont);
            e.Graphics.DrawString(amountWithoutVAT.ToString("N2"), mainFont, Brushes.Black, x - size1.Width, y1);
            y1 += 20;

            if (invoice != null && invoice.payment != null && invoice.payment is IFeeCalculator ff)
            {
                double fee = totalWithVAT * ff.feePercent;
                totalWithVAT += fee;
                e.Graphics.DrawString(fee.ToString("N2"), new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x - size1.Width, y1);
                y1 += 20;
            }
            if (invoice != null && invoice.payment != null && invoice.specialDiscount.ToLower() != "regular")
            {
                e.Graphics.DrawString($"( {(invoice.Order.getTotalDiscount() * 0.20).ToString("N2")} )", new Font("Segui UI", 9, FontStyle.Regular), Brushes.Black, x - size1.Width, y1);
                y1 += 20;

                size1 = graphics.MeasureString((invoice.Order.getTotalDiscount() - (om.getTotalDiscount() * 0.20)).ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular));
                e.Graphics.DrawString((invoice.Order.getTotalDiscount() - (om.getTotalDiscount() * 0.20)).ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x - size1.Width, y1);
                y1 += 20;
            }
            else
            {
                size1 = graphics.MeasureString(totalWithVAT.ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular));
                e.Graphics.DrawString(totalWithVAT.ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x - size1.Width, y1);
                y1 += 20;
            }

            if (cash != 0)
            {
                size1 = graphics.MeasureString(cash.ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular));
                e.Graphics.DrawString(cash.ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x - size1.Width, y1);

                y1 += 20;
                if (invoice != null && invoice.payment != null && invoice.specialDiscount.ToLower() != "regular")
                {
                    size1 = graphics.MeasureString((cash - (invoice.Order.getTotalDiscount() - (om.getTotalDiscount() * 0.20))).ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular));
                    e.Graphics.DrawString((cash - (invoice.Order.getTotalDiscount() - (om.getTotalDiscount() * 0.20))).ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x - size1.Width, y1);
                }
                else
                {
                    size1 = graphics.MeasureString((cash - totalWithVAT).ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular));
                    e.Graphics.DrawString((cash - totalWithVAT).ToString("N2"), new Font("Segui UI", 10, FontStyle.Bold | FontStyle.Regular), Brushes.Black, x - size1.Width, y1);
                }
            }


            int bx = 5;
            for (int b = 0; b < 55; b++)
            {
                e.Graphics.DrawString("-", new Font("Sans-serif", 9), Brushes.Black, bx + 2, y + 40);
                bx += 7;
            }

            y += 70;

            e.Graphics.DrawString(message, new Font("Segui UI", 15, FontStyle.Bold), Brushes.Black, 95, y);

            if (!string.IsNullOrWhiteSpace(estimated_date))
            {
                y += 60;
                size1 = e.Graphics.MeasureString(estimated_date, new Font("Segoe UI", 15, FontStyle.Regular));
                float x = (400 - size1.Width) / 2;
                e.Graphics.DrawString(estimated_date, new Font("Segui UI", 15, FontStyle.Regular), Brushes.Black, x, y);
            }


        }
    }
}