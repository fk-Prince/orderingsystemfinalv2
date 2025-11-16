using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OrderingSystem.Model;
using OrderingSystem.Services;


namespace OrderingSystem.CashierApp.Components
{
    public partial class BundleMenuPopup : Form
    {

        private DataTable table;
        private DataView view;
        private List<MenuDetailModel> menuSelected;
        private List<MenuDetailModel> menuList;
        public BundleMenuPopup(MenuService menuService, List<MenuDetailModel> menuSelected)
        {
            InitializeComponent();
            this.menuSelected = menuSelected;
            menuList = menuService.getMenuDetail();
            initTable();
        }

        private void initTable()
        {
            table = new DataTable();
            table.Columns.Add("Select", typeof(bool));
            table.Columns.Add("Fixed ✓", typeof(bool));
            table.Columns.Add("Quantity");
            table.Columns.Add("Menu Name");
            table.Columns.Add("Size");
            table.Columns.Add("Flavor");
            table.Columns.Add("Price");


            menuList.ForEach(e => table.Rows.Add(false, false, null, e.MenuName, e.SizeName, e.FlavorName, e.getPriceAfterDiscount().ToString("N2")));



            view = new DataView(table);
            dataGrid.DataSource = view;

            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.DataPropertyName = "Select";

            DataGridViewCheckBoxColumn fx = new DataGridViewCheckBoxColumn();
            fx.DataPropertyName = "Fixed ✓";

            dataGrid.Columns[0].Width = 48;
            dataGrid.Columns[1].Width = 48;
            dataGrid.Columns[2].Width = 48;
            dataGrid.Columns[3].Width = 140;
            dataGrid.Columns[4].Width = 60;
            dataGrid.Columns[5].Width = 60;
            dataGrid.Columns[6].Width = 60;

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                string menuName = row.Cells["Menu Name"].Value?.ToString();
                string flavor = row.Cells["Flavor"].Value?.ToString();
                string size = row.Cells["Size"].Value?.ToString();
                bool fxs = bool.Parse(row.Cells["Fixed ✓"].Value?.ToString());

                var selectedMenu = menuSelected.Find(x => x.MenuName == menuName && x.FlavorName == flavor && x.SizeName == size);

                if (selectedMenu != null)
                {
                    if (selectedMenu is MenuPackageModel p)
                    {
                        row.Cells["Select"].Value = true;
                        row.Cells["Fixed ✓"].Value = p.isFixed;
                        row.Cells["Quantity"].Value = p.Quantity.ToString();
                    }
                }
            }
            dataGrid.DataBindingComplete += dataGrid_DataBindingComplete;
        }

        private void dataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            filteR();
        }

        private void filteR()
        {
            HashSet<string> selected = new HashSet<string>();

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                bool isChecked = row.Cells["Select"].Value != null && bool.TryParse(row.Cells["Select"].Value.ToString(), out var val) && val;

                if (isChecked)
                {
                    string menuName = row.Cells["Menu Name"].Value?.ToString();
                    if (!string.IsNullOrEmpty(menuName))
                    {
                        selected.Add(menuName);
                    }
                }
            }

            foreach (DataGridViewRow row in dataGrid.Rows)
                row.Visible = true;

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                bool isChecked = row.Cells["Select"].Value != null && bool.TryParse(row.Cells["Select"].Value.ToString(), out var val) && val;
                string menuName = row.Cells["Menu Name"].Value?.ToString();

                if (!isChecked && selected.Contains(menuName))
                {
                    row.Visible = false;
                }
            }

        }

        private void confirmButton(object sender, EventArgs e)
        {
            menuSelected.Clear();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                bool isChecked = false;
                if (row.Cells[0].Value != null && bool.TryParse(row.Cells[0].Value.ToString(), out bool isC))
                {
                    isChecked = isC;
                }

                if (isChecked)
                {
                    if (!int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int r))
                    {
                        MessageBox.Show("No quantity");
                        return;
                    }
                    string menuName = row.Cells["Menu Name"].Value?.ToString();
                    string flavor = row.Cells["Flavor"].Value?.ToString();
                    string size = row.Cells["Size"].Value?.ToString();
                    bool fx = bool.Parse(row.Cells["Fixed ✓"].Value.ToString());
                    int q = int.Parse(row.Cells["Quantity"].Value?.ToString());

                    MenuDetailModel originalMenu = menuList.Find(x =>
                           x.MenuName == menuName &&
                           x.FlavorName == flavor &&
                           x.SizeName == size);

                    // Find if this was already selected (has a PackageId)
                    MenuPackageModel existingPackage = menuSelected.Find(x =>
                        x.MenuName == menuName &&
                        x.FlavorName == flavor &&
                        x.SizeName == size) as MenuPackageModel;
                    MenuPackageModel p = MenuPackageModel.Builder()
                        .WithMenuDetailId(originalMenu.MenuDetailId)
                        .WithPackageId(existingPackage?.PackageId ?? 0)
                        .WithMenuName(menuName)
                        .WithSizeName(size)
                        .WithFlavorName(flavor)
                        .WithQuantity(q)
                        .isFixed(fx)
                        .Build();
                    menuSelected.Add(p);

                }
            }
            DialogResult = DialogResult.OK;
            //xx?.Invoke(this, EventArgs.Empty);
        }

        public List<MenuDetailModel> getMenuSelected()
        {
            return menuSelected;
        }

        private void FormClosing1(object sender, FormClosingEventArgs e)
        {
        }

        private void dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGrid.Columns[e.ColumnIndex].Name == "Quantity")
            {
                string txt = e.FormattedValue?.ToString().Trim();

                if (string.IsNullOrEmpty(txt))
                    return;

                string reegx = @"^(?:[1-9]\d*|\d+)(?:\.\d+)?$";

                if (!Regex.IsMatch(txt, reegx))
                {
                    MessageBox.Show("Invalid Input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            filteR();
        }


        private void dataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGrid.IsCurrentCellDirty && dataGrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                dataGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }


        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string tx = s.Text.Trim().Replace("'", "''");
            view.RowFilter = string.IsNullOrEmpty(tx) ? "" : $"[Menu Name] LIKE '%{tx}%'";
        }
    }
}
