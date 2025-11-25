using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Components
{
    public partial class ServingTable : Form
    {
        private MenuDetailModel menu;
        private MenuService menuService;
        public ServingTable(MenuDetailModel menu, MenuService menuService)
        {
            InitializeComponent();
            this.menu = menu;
            this.menuService = menuService;
        }

        public void displayCurrentServings()
        {
            dataGrid.DataSource = null;
            DataTable table = new DataTable();
            table.Columns.Add("Serving Date");
            table.Columns.Add("Price Per Serve");
            table.Columns.Add("Serving Quantity");
            table.Columns.Add("Servings Left");
            table.Columns.Add("Prep. Time / Serve");
            table.Columns.Add("serving_id", typeof(int));
            dataGrid.DataSource = table;

            try
            {
                List<ServingsModel> sm = menuService.getServings(menu.MenuId);
                foreach (var i in sm)
                {
                    table.Rows.Add(i.date.ToString("yyyy-MM-dd"), i.getPrice().ToString("N2"), i.Quantity, i.LeftQuantity, i.PrepTime, i.ServingId);
                }

                if (!dataGrid.Columns.Contains("Cancel"))
                {
                    DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                    btnColumn.HeaderText = "Cancel";
                    btnColumn.Name = "Cancel";
                    btnColumn.Text = "Cancel";
                    btnColumn.UseColumnTextForButtonValue = true;
                    btnColumn.Width = 80;
                    dataGrid.Columns.Add(btnColumn);
                }

                if (dataGrid.Columns.Contains("serving_id"))
                {
                    dataGrid.Columns["serving_id"].Visible = false;
                }
                dataGrid.Columns["Cancel"].DisplayIndex = dataGrid.Columns.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && dataGrid.Columns[e.ColumnIndex].Name == "Cancel")
            {
                int servingId = (int)dataGrid.Rows[e.RowIndex].Cells["serving_id"].Value;
                var da = dataGrid.Rows[e.RowIndex].Cells["Serving Date"].Value;
                if (da != null && DateTime.TryParse(da.ToString(), out DateTime d))
                {
                    if (d.Date <= DateTime.Now.Date)
                    {
                        MessageBox.Show("Cannot cancel serving on the same day or past.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                var result = MessageBox.Show("Are you sure you want to cancel this serving?",
                    "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    bool suc = menuService.cancelServing(servingId);
                    if (suc)
                        displayCurrentServings();
                }
            }
        }
        public void displayHistoryServing()
        {
            dataGrid.DataSource = null;
            if (dataGrid.Columns.Contains("Cancel"))
            {
                dataGrid.Columns.Remove("Cancel");
            }
            string query = @"
                 WITH ingredient_costs AS (
                     SELECT 
                         si.serving_id,
                         SUM(ist.batch_cost) / SUM(mi.quantity) AS cost_per_serve
                     FROM serving_ingredients si
                     JOIN ingredient_stock ist ON ist.ingredient_stock_id = si.ingredient_stock_id
                     JOIN monitor_inventory mi ON mi.ingredient_stock_id = ist.ingredient_stock_id
                     WHERE mi.type = 'Add'
                     GROUP BY si.serving_id
                 ),
                 serving_sales AS (
                     SELECT 
                         serving_id,
                         SUM(quantity) AS used_quantity
                     FROM order_item
                     GROUP BY serving_id
                 )
                 SELECT 
                     DATE_FORMAT(serving_date, '%Y-%m-%d') AS 'Date Served',
                     ms.quantity  AS 'Total Servings',
                     IFNULL(ss.used_quantity, 0) AS 'Sold Servings',
                     (ms.quantity - IFNULL(ss.used_quantity, 0)) AS 'Spoiled Servings',
                     ms.price AS 'Price',
	                CASE 
                        WHEN ms.status = 'Cancelled' AND ms.cancelled_at < ms.serving_date THEN 0.00
                        ELSE  ROUND(ms.price * (ms.quantity - IFNULL(ss.used_quantity, 0)), 2)
                    END AS 'Lost Revenue',
                    CASE 
                        WHEN ms.status = 'Cancelled' AND ms.cancelled_at < ms.serving_date THEN 0.00
                        ELSE ROUND((ms.quantity - IFNULL(ss.used_quantity, 0)) * ic.cost_per_serve, 2)
                    END AS 'Ingredients Cost Lost',

                     CASE
                        WHEN ms.status = 'Cancelled' AND ms.cancelled_at < ms.serving_date THEN 'Cancelled (Early)'
                        ELSE ms.status
	                 END AS 'Status'
                 FROM menu_serving ms
                 JOIN menu m ON m.menu_id = ms.menu_id
                 LEFT JOIN serving_sales ss
                     ON ss.serving_id = ms.serving_id
                 LEFT JOIN ingredient_costs ic
                     ON ic.serving_id = ms.serving_id
                 WHERE ms.serving_date < CURDATE()
                 ORDER BY ms.serving_date DESC
                ";
            try
            {
                var db = DatabaseHandler.getInstance();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.getConnection()))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGrid.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
