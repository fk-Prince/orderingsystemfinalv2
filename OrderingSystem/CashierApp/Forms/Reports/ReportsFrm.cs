using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Model;
using OrderingSystem.Repository.Ingredients;
using OrderingSystem.Services;
using Font = iTextSharp.text.Font;

namespace OrderingSystem.CashierApp.Forms
{
    public partial class ReportsFrm : Form
    {
        private DataView view;
        private readonly ReportServices inventoryServices;
        private string title;
        private string typePdf;
        public ReportsFrm(ReportServices inventoryServices)
        {
            InitializeComponent();
            this.inventoryServices = inventoryServices;
            dtTo.Value = DateTime.Now;
            cb.SelectedIndex = 0;
            cb_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void txt_TextChanged(object sender, System.EventArgs e)
        {
            db.Stop();
            db.Start();
        }
        private void filter()
        {
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (view == null) return;
            string s = cb.Text;

            switch (s)
            {
                case "Track Quantity In/Out":
                    typePdf = "Normal";
                    title = "Ingredient Report";
                    reportTrackQuantity();
                    break;
                case "Expiry Tracking":
                    typePdf = "Normal";
                    title = "Expiry Report";
                    reportExpirationIngredient();
                    break;
                case "Inventory Reports":
                    typePdf = "Normal";
                    title = "Inventory Report";
                    reportInventory();
                    break;
                case "Ingredient Usage":
                    typePdf = "Normal";
                    title = "Inventory Usage Report";
                    reportIngredientUsage();
                    break;
                case "Ingredient History":
                    typePdf = "Normal";
                    title = "Ingredient History";
                    reportIngredientHistory();
                    break;
                case "Menu Popular's":
                    typePdf = "Normal";
                    title = "Menu Popular Report";
                    reportMenuPopular();
                    break;
                case "Invoice Record":
                    typePdf = "Rotate";
                    title = "Invoice Report";
                    reportInvoice();
                    break;
                case "Suppliers":
                    typePdf = "Rotate";
                    title = "Suppliers";
                    reportSuppliers();
                    break;
            }
            ;



            dataGrid.Refresh();
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs ee)
        {
            if (cb.SelectedIndex == -1) return;
            txt.IconLeftClick -= txt_IconLeftClick;
            view = null;
            dataGrid.DataSource = null;
            p3.Visible = false;
            p1.Visible = false;
            p2.Visible = false;
            string s = cb.Text;
            dtTo.Value = DateTime.Now.AddDays(1);
            dt2.Value = DateTime.Now;
            dtFrom.Value = DateTime.Parse("2020-01-01");

            if (s == "Track Quantity In/Out")
            {
                txt.PlaceholderText = "Search Staff, Ingredient";
                view = inventoryServices.getTrackingIngredients();
            }
            else if (s == "Expiry Tracking")
            {
                txt.PlaceholderText = "Search Ingredient";
                view = inventoryServices.getIngredientExpiry();
            }
            else if (s == "Inventory Reports")
            {
                txt.PlaceholderText = "Search Staff, Ingredient";
                view = inventoryServices.getInventorySummaryReports();
            }
            else if (s == "Ingredient Usage")
            {
                txt.PlaceholderText = "Search Ingredient";
                view = inventoryServices.getIngredientsUsage();
            }
            else if (s == "Trace Ingredient Stock Order")
            {
                txt.IconLeftClick += txt_IconLeftClick;
                txt.PlaceholderText = "Search OrderID";
            }
            else if (s == "Ingredient History")
            {
                p1.Visible = true;
                p3.Visible = true;
                txt.PlaceholderText = "Search Ingredient";
                ic.Items.Clear();
                List<IngredientModel> ind = new IngredientServices(new IngredientRepository()).getIngredients2();
                ind.ForEach(e => ic.Items.Add(e.IngredientName));
                return;
            }
            else if (s == "Menu Popular's")
            {
                txt.PlaceholderText = "Search Menu";
                view = inventoryServices.getMenuPopularity();
            }
            else if (s == "Invoice Record")
            {
                txt.PlaceholderText = "Invoice Record";
                view = inventoryServices.getInvoice();
            }
            else if (s == "Suppliers")
            {
                txt.PlaceholderText = "Supplier name";
                view = inventoryServices.getSupplier();
            }

            dataGrid.DataSource = view;
            dataGrid.Refresh();
            filter();
        }


        private void dtTo_ValueChanged(object sender, System.EventArgs e)
        {
            filter();
        }
        private void dtFrom_ValueChanged(object sender, System.EventArgs e)
        {
            filter();
        }
        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            filter();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            db.Stop();
            filter();
        }
        private void ic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ic.SelectedIndex <= -1) return;
            view = inventoryServices.getIngredientHistory(ic.Text);
            dataGrid.DataSource = view;
            dataGrid.Refresh();
            filter();
        }
        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                row.Cells[e.ColumnIndex].Style.BackColor = ColorTranslator.FromHtml("#DBEAFE");
            }
        }
        private void dataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                row.Cells[e.ColumnIndex].Style.BackColor = ColorTranslator.FromHtml("#DBEAFE");
            }

        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (view == null)
            {
                MessageBox.Show("Nothing to print", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataTable table = view.ToTable();
            using (var save = new SaveFileDialog { Filter = "PDF File|*.pdf", FileName = $"{title + " - " + DateTime.Now.ToString("yyyy-MM-dd")}" })
            {
                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (var doc = new Document(typePdf == "Normal" ? PageSize.A4 : PageSize.A4.Rotate(), 10f, 10f, 20f, 20f))
                    {
                        PdfWriter.GetInstance(doc, new FileStream(save.FileName, FileMode.Create));
                        doc.Open();

                        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                        Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                        doc.Add(new Paragraph("Report: " + title, headerFont));
                        doc.Add(new Paragraph("Requested By: " + SessionStaffData.getFullName(), normalFont));
                        doc.Add(new Paragraph("Date: " + DateTime.Now.ToString("yyyy-MM-dd"), normalFont));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));

                        PdfPTable pdfTable = new PdfPTable(table.Columns.Count);
                        pdfTable.WidthPercentage = 100;

                        Font columnHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                        foreach (DataColumn column in table.Columns)
                        {
                            PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, columnHeaderFont));
                            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            headerCell.Padding = 6f;
                            headerCell.MinimumHeight = 20;
                            headerCell.BackgroundColor = new BaseColor(220, 220, 220);
                            headerCell.BorderWidth = 0.3f;
                            headerCell.BorderColor = new BaseColor(180, 180, 180);
                            pdfTable.AddCell(headerCell);
                        }

                        foreach (DataRow row in table.Rows)
                        {
                            foreach (var cell in row.ItemArray)
                            {
                                PdfPCell dataCell = new PdfPCell(new Phrase(cell?.ToString() ?? ""));
                                dataCell.Padding = 5f;
                                dataCell.MinimumHeight = 16f;
                                dataCell.BackgroundColor = new BaseColor(245, 245, 245);
                                dataCell.BorderWidth = 0.3f;
                                dataCell.BorderColor = new BaseColor(200, 200, 200);
                                pdfTable.AddCell(dataCell);
                            }
                        }

                        doc.Add(pdfTable);
                        doc.Close();
                    }
                    MessageBox.Show("Report saved to PDF", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // -- REPORTS
        private void reportTrackQuantity()
        {
            p1.Visible = true;
            p2.Visible = false;
            string staffFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Staff Incharge] LIKE '%{txt.Text}%'";
            string ingredientFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Ingredient Name] LIKE '%{txt.Text}%'";
            string dateFilter = $"[Date-Action] >= #{dtFrom.Value:yyyy-MM-dd}# AND [Date-Action] <= #{dtTo.Value:yyyy-MM-dd}#";
            string staff_ingredient_filter = string.Join(" OR ", new[] { staffFilter, ingredientFilter }.Where(f => !string.IsNullOrEmpty(f)));
            string finalFilter = string.IsNullOrEmpty(staff_ingredient_filter) ? dateFilter : $"({staff_ingredient_filter}) AND {dateFilter}";
            view.RowFilter = finalFilter;
        }
        private void txt_IconLeftClick(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txt.Text)) return;
            view = inventoryServices.getIngredientByOrder(txt.Text.Trim());
            dataGrid.DataSource = view;
            dataGrid.Refresh();
        }

        private void reportExpirationIngredient()
        {
            p1.Visible = true;
            p2.Visible = false;
            string ingredientFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Ingredient Name] LIKE '%{txt.Text}%'";
            string dateFilter = $"[Expiry Date] >= #{dtFrom.Value:yyyy-MM-dd}# AND [Expiry Date] <= #{dtTo.Value:yyyy-MM-dd}#";
            string finalFilter = string.Join(" OR ", new[] { ingredientFilter, dateFilter }.Where(f => !string.IsNullOrEmpty(f)));
            view.RowFilter = finalFilter;
        }
        private void reportIngredientHistory()
        {
            dataGrid.Columns[0].Width = 200;
            dataGrid.Columns[1].Width = 300;
            p1.Visible = true;
            p3.Visible = true;
            dt2.Value = DateTime.Now.AddDays(1);
            string dateFilter = $"[Date] >= #{dtFrom.Value:yyyy-MM-dd}# AND [Date] <= #{dtTo.Value:yyyy-MM-dd}#";
            view.RowFilter = dateFilter;
        }
        private void reportInventory()
        {
            p1.Visible = true;
            p2.Visible = false;
            string staffFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Staff Incharge] LIKE '%{txt.Text}%'";
            string ingredientFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Ingredient Name] LIKE '%{txt.Text}%'";
            string dateFilter = $"[IN - Recorded At] >= #{dtFrom.Value:yyyy-MM-dd}# AND [IN - Recorded At] <= #{dtTo.Value:yyyy-MM-dd}#";
            string staff_ingredient_filter = string.Join(" OR ", new[] { staffFilter, ingredientFilter }.Where(f => !string.IsNullOrEmpty(f)));
            string finalFilter = string.IsNullOrEmpty(staff_ingredient_filter) ? dateFilter : $"({staff_ingredient_filter}) AND {dateFilter}";
            view.RowFilter = finalFilter;
        }
        private void reportIngredientUsage()
        {
            dt2.CustomFormat = "yyyy";
            dataGrid.Columns[0].Width = 200;
            dataGrid.Columns[1].Width = 150;
            p1.Visible = false;
            p2.Visible = true;
            string ingredientFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Ingredient Name] LIKE '%{txt.Text}%'";
            if (string.IsNullOrEmpty(txt.Text))
            {
                string dateFilter = $"([Year] = '{dt2.Value.Year}' OR [Year] = 'Total' OR [Year] = 'Total per Year')";
                view.RowFilter = dateFilter;
            }
            else
            {
                string dateFilter = $"([Year] = '{dt2.Value.Year}' OR [Year] = 'Total')";
                string finalFilter = $"{ingredientFilter} AND {dateFilter}";
                view.RowFilter = finalFilter;
            }
        }
        private void reportInvoice()
        {
            p1.Visible = true;
            p2.Visible = false;

            string dateFilter = $"[Date] >= #{dtFrom.Value:yyyy-MM-dd}# AND [Date] <= #{dtTo.Value:yyyy-MM-dd}#";
            string invoiceFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Invoice ID] LIKE '%{txt.Text}%'";
            string finalFilter = string.Join(" AND ", new[] { invoiceFilter, dateFilter }.Where(f => !string.IsNullOrEmpty(f)));
            view.RowFilter = finalFilter;
            updateTotalRowsInvoice();
        }
        private void updateTotalRowsInvoice()
        {
            decimal grossRevenue = 0;
            decimal itemDiscount = 0;
            decimal couponDiscount = 0;
            decimal specialDiscount = 0;
            decimal totalDiscount = 0;
            decimal netRevenue = 0;
            decimal totalTax = 0;
            decimal netRevenueWithTax = 0;
            decimal totalCost = 0;
            decimal netProfit = 0;
            decimal fee = 0;
            DateTime? lastDate = null;

            foreach (DataRowView rowView in view)
            {
                DataRow row = rowView.Row;
                if (row["Invoice ID"].ToString() == "All")
                    continue;

                grossRevenue += decimal.Parse(row["Gross Revenue"].ToString());
                itemDiscount += decimal.Parse(row["Item Discount"].ToString());
                couponDiscount += decimal.Parse(row["Coupon Discount"].ToString());
                specialDiscount += decimal.Parse(row["Special Discount"].ToString());
                totalDiscount += decimal.Parse(row["Total Discount"].ToString());
                netRevenue += decimal.Parse(row["Net Revenue"].ToString());
                totalTax += decimal.Parse(row["Total Tax"].ToString());
                netRevenueWithTax += decimal.Parse(row["Net Revenue with Tax"].ToString());
                totalCost += decimal.Parse(row["Total Cost"].ToString());
                netProfit += decimal.Parse(row["Net Profit"].ToString());
                fee += decimal.Parse(row["Transaction Fee"].ToString());

                DateTime currentDate = DateTime.Parse(row["Date"].ToString());
                if (lastDate == null || currentDate > lastDate)
                    lastDate = currentDate;
            }

            decimal profitMargin = netRevenueWithTax > 0 ? (netProfit / netRevenueWithTax * 100) : 0;

            DataTable dt = view.Table;
            DataRow[] allRows = dt.Select("[Invoice ID] = 'All'");
            if (allRows.Length > 0)
            {
                DataRow allRow = allRows[0];
                allRow["Gross Revenue"] = grossRevenue;
                allRow["Item Discount"] = itemDiscount;
                allRow["Coupon Discount"] = couponDiscount;
                allRow["Special Discount"] = specialDiscount;
                allRow["Total Discount"] = totalDiscount;
                allRow["Net Revenue"] = netRevenue;
                allRow["Total Tax"] = totalTax;
                allRow["Net Revenue with Tax"] = netRevenueWithTax;
                allRow["Total Cost"] = totalCost;
                allRow["Net Profit"] = netProfit;
                allRow["Profit Margin %"] = profitMargin.ToString("N2");
                allRow["Transaction Fee"] = fee.ToString("N2");
                allRow["Date"] = lastDate ?? DateTime.Now;
            }
        }
        private void reportMenuPopular()
        {

            dt2.CustomFormat = "yyyy-MMMM";
            p1.Visible = false;
            p2.Visible = true;
            string dateFilter = $"[Year] = '{dt2.Value.Year}' AND [Month] = '{dt2.Value.ToString("MMMM")}'";
            string menuFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Menu Name] LIKE '%{txt.Text}%'";
            string finalFilter = string.Join(" AND ", new[] { menuFilter, dateFilter }.Where(f => !string.IsNullOrEmpty(f)));
            view.RowFilter = finalFilter;
        }
        private void reportSuppliers()
        {
            p1.Visible = true;
            p2.Visible = false;

            string dateFilter = $"[Recently Supplied] >= '{dtFrom.Value}' AND [Recently Supplied] <= '{dtTo.Value}'";
            string supplerFilter = string.IsNullOrEmpty(txt.Text) ? "" : $"[Supplier Name] LIKE '%{txt.Text}%'";
            string finalFilter = string.Join(" AND ", new[] { supplerFilter, dateFilter }.Where(f => !string.IsNullOrEmpty(f)));
            view.RowFilter = finalFilter;
        }

        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
