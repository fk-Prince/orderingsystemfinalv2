using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Table
{
    public partial class PackageTable : Form
    {
        private DataTable table;
        private List<MenuDetailModel> included;
        private List<MenuDetailModel> menuS;

        public List<MenuDetailModel> getMenus()
        {
            menuS = new List<MenuDetailModel>();

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (row.IsNewRow) continue;

                string menuName = row.Cells["Menu Name"]?.Value?.ToString();
                string quantityStr = row.Cells["Quantity"]?.Value?.ToString();
                object fixedObj = row.Cells["Fixed ✓"]?.Value;

                if (string.IsNullOrWhiteSpace(menuName)) continue;

                if (!int.TryParse(quantityStr, out int quantity))
                {
                    MessageBox.Show($"Invalid quantity for '{menuName}'", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                bool isFixed = fixedObj != null && Convert.ToBoolean(fixedObj);

                var selectedMenu = included.Find(x => x.MenuName == menuName);
                if (selectedMenu == null) continue;

                MenuPackageModel p = MenuPackageModel.Builder()
                    .WithMenuDetailId(selectedMenu.MenuDetailId)
                    .WithPackageId((selectedMenu as MenuPackageModel)?.PackageId ?? 0)
                    .WithMenuName(menuName)
                    .WithQuantity(quantity)
                    .isFixed(isFixed)
                    .Build();
                menuS.Add(p);
            }

            return menuS;
        }
        public PackageTable(List<MenuDetailModel> included)
        {
            InitializeComponent();
            this.included = included;
            displayPackage();
        }

        public void displayPackage()
        {
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            table = new DataTable();
            table.Columns.Add("Menu Name");
            table.Columns.Add("Quantity");
            table.Columns.Add("Fixed ✓", typeof(bool));
            dataGrid.DataSource = table;

            DataGridViewCheckBoxColumn fx = new DataGridViewCheckBoxColumn();
            fx.DataPropertyName = "Fixed ✓";
            fx.HeaderText = "Fixed ✓";

            foreach (var m in included)
            {
                if (m is MenuPackageModel v)
                {
                    table.Rows.Add(v.MenuName, v.Quantity, v.isFixed);
                }
            }
        }
    }
}
