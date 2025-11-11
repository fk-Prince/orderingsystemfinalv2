using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Forms.FactoryForm;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.Repository.Ingredients;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Forms
{
    public partial class IngredientFrm : Form
    {
        private DataView view;
        private IngredientServices ingredientServices;
        public IngredientFrm()
        {
            InitializeComponent();
            ingredientServices = new IngredientServices(new IngredientRepository());
            updateTable();

            if (SessionStaffData.Role.ToLower() == "cashier")
            {
                bb.Visible = false;
                dataGrid.CellClick -= dataGrid_CellClick;
            }
        }

        public void updateTable()
        {
            check.Checked = false;
            txt.Text = "";
            view = ingredientServices.getIngredientsView();
            dataGrid.DataSource = view;
        }
        private void textChanged(object sender, System.EventArgs e)
        {
            loadIngredientData();
        }
        public void loadIngredientData()
        {
            string ingredientQuery = string.IsNullOrEmpty(txt.Text) ? "" : $"[Ingredient Name] LIKE '%{txt.Text}%'";
            string expiryFilter = "";
            if (check.Checked)
                expiryFilter = $"[Expiry Date] <= #{DateTime.Now:yyyy-MM-dd}#";
            else
                expiryFilter = $"[Expiry Date] > #{DateTime.Now:yyyy-MM-dd}#";

            string finalFilter = string.Join(" OR ", new[] { ingredientQuery, expiryFilter }.Where(f => !string.IsNullOrEmpty(f)));
            view.RowFilter = finalFilter;
        }
        private void checkboxChanged(object sender, System.EventArgs e)
        {
            loadIngredientData();
        }
        private void removeAllExpiredClicked(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Do you want to remove expired ingredient?", "Deduct", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                try
                {
                    bool suc = ingredientServices.removeExpiredIngredient();
                    if (suc)
                    {
                        MessageBox.Show("Successful Removed", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updateTable();
                    }
                    else MessageBox.Show("Failed to remove expired ingredient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView grid = sender as DataGridView;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            int id = int.Parse(row.Cells["Stock ID"].Value.ToString());
            string name = row.Cells["Ingredient Name"].Value?.ToString();
            string unit = row.Cells["Unit"].Value?.ToString();

            PopupForm p = new PopupForm();
            p.t1.Text = name;
            p.c2.Items.Clear();
            p.c2.Items.Add("Piece");
            p.c2.Items.Add("Kg");
            p.c2.SelectedItem = unit;
            p.buttonClicked += (ss, ee) =>
            {
                try
                {
                    bool suc = ingredientServices.validateUpdateIngredient(id, p.t1.Text, p.c2.Text);
                    if (suc)
                    {
                        MessageBox.Show("Successful Updated", "Restock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updateTable();
                        p.Hide();
                    }
                    else
                        MessageBox.Show("Failed to update ingredient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidInput ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            DialogResult rs = new FormFactory().selectForm(p, "update-ingredients").ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                p.Hide();
            }

        }
    }
}
