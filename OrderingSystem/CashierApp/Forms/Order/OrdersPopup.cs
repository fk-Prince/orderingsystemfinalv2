using System;
using System.Data;
using System.Windows.Forms;
using OrderingSystem.KioskApplication.Services;

namespace OrderingSystem.CashierApp.Forms.Order
{
    public partial class OrdersPopup : Form
    {
        private int offSet = 0;
        private OrderServices orderServices;
        public OrdersPopup(OrderServices orderServices)
        {
            InitializeComponent();
            this.orderServices = orderServices;
            fetchData();
        }
        private DataView view;
        private void fetchData()
        {
            try
            {
                view = orderServices.getOrders(offSet);
                dataGridView1.DataSource = view;
                addViewButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void next(object sender, EventArgs e)
        {
            offSet += 50;
            fetchData();
        }

        private void prev(object sender, EventArgs e)
        {
            if (offSet != 0)
            {
                offSet -= 50;
                fetchData();
            }
        }
        private void addViewButton()
        {
            if (dataGridView1.Columns["Void Order"] == null)
            {
                DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
                viewButton.HeaderText = "Action";
                viewButton.Name = "Void Order";
                viewButton.Text = "Void Order";
                viewButton.UseColumnTextForButtonValue = true;
                viewButton.Width = 70;
                dataGridView1.Columns.Add(viewButton);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            view.Sort = guna2CheckBox1.Checked ? "[Available Until] DESC" : "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Void Order")
            {
                string orderId = dataGridView1.Rows[e.RowIndex].Cells["Order ID"].Value.ToString();
                string status = dataGridView1.Rows[e.RowIndex].Cells["Status"].Value.ToString();

                if (!status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Only orders with status 'Pending' can be voided.\nCurrent status: {status}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to void order #{orderId}?",
                    "Confirm Void",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        bool suc = orderServices.voidOrder(orderId);

                        if (suc)
                        {
                            MessageBox.Show("Order has been voided successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            fetchData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error voiding order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
