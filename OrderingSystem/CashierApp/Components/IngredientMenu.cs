using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Components
{
    public partial class IngredientMenu : Form
    {
        private DataTable table;
        private readonly IngredientServices ingredientServices;
        private DataView view;
        private List<IngredientModel> ingredientList;
        private List<IngredientModel> ingredientSelected;
        public event EventHandler<List<IngredientModel>> IngredientSelectedEvent;
        public IngredientMenu(IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.ingredientServices = ingredientServices;
            ingredientSelected = new List<IngredientModel>();
        }


        public void getIngredient(DateTime date)
        {
            try
            {
                if (table != null)
                    table.Rows.Clear();
                ingredientList = ingredientServices.getIngredients(date);
                initTable1();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal Server Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void initTable1()
        {
            table = new DataTable();
            table.Columns.Add("Select", typeof(bool));
            table.Columns.Add("Ingredient Name");
            table.Columns.Add("Total Quantity");
            table.Columns.Add("Quantity to be Used");
            table.Columns.Add("Unit");
            table.Columns.Add("Cost per Unit");

            ingredientList.ForEach(e => table.Rows.Add(false, e.IngredientName, e.IngredientQuantity, "", e.IngredientUnit, e.IngredientCostPerUnit.ToString("N2")));
            view = new DataView(table);

            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.DataPropertyName = "Select";

            dataGrid.DataSource = view;
            dataGrid.Enabled = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            view.RowFilter = "Quantity >= 0";
            dataGrid.Enabled = true;
            table.Rows.Clear();
            table.Columns.Clear();
        }
        private void dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {


            if (dataGrid.Columns[e.ColumnIndex].Name == "Quantity to be Used")
            {
                string input = e.FormattedValue?.ToString().Trim();

                if (string.IsNullOrEmpty(input))
                    return;

                string pattern = @"^(?:\d{1,3}(?:,\d{3})*|\d+)?(?:\.\d+)?$";

                if (!Regex.IsMatch(input, pattern))
                {
                    MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                if (!decimal.TryParse(input, out decimal qty))
                {
                    MessageBox.Show("Invalid number format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                var a = dataGrid.Rows[e.RowIndex].Cells["Total Quantity"].Value;

                if (a != null && decimal.TryParse(a.ToString(), out decimal avQty))
                {
                    qty = qty * this.qty;
                }
            }
        }
        private int qty = 0;
        public void setServing(int qty)
        {
            this.qty = qty;
        }
        private void search_TextChanged(object sender, EventArgs e)
        {
            string tx = search.Text.Trim().Replace("'", "''");
            view.RowFilter = string.IsNullOrEmpty(tx) ? "" : $"[Ingredient Name] LIKE '%{tx}%'";
        }
        private void submit(object sender, EventArgs e)
        {
            if (qty == 0)
            {
                MessageBox.Show("Please set the serving quantity first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ingredientSelected.Clear();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                bool check = false;

                if (row.Cells[3].Value != null && row.Cells[0].Value != null && bool.TryParse(row.Cells[0].Value.ToString(), out bool val))
                {
                    check = val;
                }

                if (check)
                {
                    if (int.TryParse(row.Cells["Quantity to be Used"].Value?.ToString(), out int quantity) && quantity > 0)
                    {
                        var a = row.Cells["Total Quantity"].Value;
                        if (quantity * this.qty > int.Parse(a.ToString()))
                        {
                            MessageBox.Show("Exceed the available quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string ingredientName = row.Cells["Ingredient Name"].Value?.ToString();

                        var selectedIngredient = ingredientList.Find(x => x.IngredientName == ingredientName);
                        if (selectedIngredient != null)
                        {
                            IngredientModel ingredient = IngredientModel.Builder()
                                .WithIngredientID(selectedIngredient.Ingredient_id)
                                .WithIngredientName(ingredientName)
                                .WithInredeintQty(quantity)
                                .Build();
                            ingredientSelected.Add(ingredient);
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is an empty quantity field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    string ingredientName = row.Cells["Ingredient Name"].Value?.ToString();
                    var selectedIngredient = ingredientList.Find(x => x.IngredientName == ingredientName);
                    if (selectedIngredient != null)
                    {
                        ingredientSelected.Remove(selectedIngredient);
                    }
                }
            }
            IngredientSelectedEvent?.Invoke(this, ingredientSelected);
        }
        public void reset()
        {
            ingredientSelected.Clear();
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

    }
}
