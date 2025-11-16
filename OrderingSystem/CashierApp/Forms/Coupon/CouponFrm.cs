using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OrderingSystem.CashierApp.Forms.FactoryForm;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Forms.Coupon
{
    public partial class CouponFrm : Form
    {
        private readonly CouponServices couponServices;
        private readonly TableLayout tableLayout;
        private DataView view;
        private readonly IForms f;

        public CouponFrm(IForms f, CouponServices couponServices)
        {
            InitializeComponent();
            this.f = f;
            this.couponServices = couponServices;

            loadForm(f.selectForm(tableLayout = new TableLayout(), "view-coupon"));
            tableLayout.FilterChanged += checkedChanged;
            tableLayout.ButtonClicked += addCouponPopup;
            displayCoupons();
        }
        private void addCouponPopup(object sender, EventArgs e)
        {
            PopupForm p = new PopupForm();
            p.buttonClicked += (ss, ee) =>
            {
                try
                {
                    DataView suc = couponServices.saveCoupon(p.t1.Text, p.dt2.Value, p.t3.Text, p.t4.Text, p.c5.Text, p.t6.Text);
                    if (suc != null)
                    {
                        printCoupon(suc);
                        displayCoupons();
                        MessageBox.Show("Successfully generate coupons.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        p.Hide();
                    }
                    else
                        MessageBox.Show("Failed to generate coupons.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidInput ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Internal Server Erorr.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            p.c5.Items.Add("Percentage");
            p.c5.Items.Add("Fixed");
            p.c5.SelectedIndex = 0;
            p.c5.DropDownStyle = ComboBoxStyle.DropDownList;
            p.c5.SelectedIndexChanged += (ss, ee) =>
            {
                string c = p.c5.Text;
                if (c.ToLower().Equals("fixed"))
                {
                    p.l1.Text = "Fixed Amount";
                    p.t6.Visible = true;
                    p.l6.Visible = true;
                }
                else if (c.ToLower().Equals("percentage"))
                {
                    p.l1.Text = "Rate %";
                    p.t6.Visible = false;
                    p.l6.Visible = false;
                }
            };

            DialogResult rs = f.selectForm(p, "coupon").ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                p.Hide();
            }
        }
        private void displayCoupons()
        {
            try
            {
                List<CouponModel> couponList = couponServices.getCoupons();
                DataTable table = new DataTable();
                table.Columns.Add("Coupon Code");
                table.Columns.Add("Description");
                table.Columns.Add("Amount / Rate %");
                table.Columns.Add("Type");
                table.Columns.Add("Until");
                table.Columns.Add("Status");

                couponList.ForEach(c =>
                    table.Rows.Add(c.CouponCode, c.Description, c.getCouponRate(), c.getType(),
                    c.ExpiryDate.ToString("yyyy/MM/dd"), c.Status)
                );

                view = new DataView(table);
                tableLayout.dataGrid.DataSource = view;


                if (SessionStaffData.Role == StaffModel.StaffRole.Cashier)
                    tableLayout.b1.Visible = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void checkedChanged(object sender, bool e)
        {
            if (tableLayout.title.Text.ToLower() == "coupon")
            {
                if (tableLayout.cb.Checked)
                    view.RowFilter = "[Status] = 'Used'";
                else
                    view.RowFilter = "[Status] = 'Not-Used'";
            }
        }
        private void loadForm(Form f)
        {
            if (mm.Controls.Count > 0) mm.Controls.Clear();
            if (SessionStaffData.Role == StaffModel.StaffRole.Cashier && f is TableLayout t) t.b1.Visible = false;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;

            mm.Controls.Add(f);
            mm.Tag = f;
            f.Show();
        }
        private void printCoupon(DataView view)
        {
            DataTable table = view.ToTable();
            using (var save = new SaveFileDialog { Filter = "PDF File|*.pdf", FileName = $"Coupons{" - " + DateTime.Now.ToString("yyyy-MM-dd")}" })
            {
                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (var doc = new Document(PageSize.A4, 10f, 10f, 20f, 20f))
                    {
                        PdfWriter.GetInstance(doc, new FileStream(save.FileName, FileMode.Create));
                        doc.Open();

                        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                        Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

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
                                dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
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

    }
}
