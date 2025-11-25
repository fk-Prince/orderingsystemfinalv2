using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Components;
using OrderingSystem.Model;
using OrderingSystem.Services;
using OrderingSystem.util;

namespace OrderingSystem.CashierApp.Forms.Menu
{
    public partial class NewServingsFrm : Form
    {

        private List<IngredientModel> ingredientServings;
        private readonly IngredientServices ingredientServices;
        private IngredientMenu im;
        public event EventHandler<ServingsModel> served;
        public NewServingsFrm(IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.ingredientServices = ingredientServices;
            ingredientServings = new List<IngredientModel>();
            dtServed.MinDate = DateTime.Now;
            displayIngredient();
        }
        private void dtServed_ValueChanged(object sender, EventArgs e)
        {
            if (im == null) return;
            im.getIngredient(dtServed.Value);
        }
        private bool isPriceValid(string text)
        {
            return Regex.IsMatch(menuPrice.Text.Trim(), @"^(\d{1,3}(,\d{3})*|\d+)(\.\d{1,2})?$");
        }
        private void confirmButton()
        {
            if (!isPriceValid(menuPrice.Text.Trim()))
            {
                MessageBox.Show("Invalid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ingredientServings.Count <= 0)
            {
                MessageBox.Show("No Selected Ingredient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ServingsModel s = ServingsModel.Build()
                .withPrice(double.Parse(menuPrice.Text.Trim()) / 1.12)
                .withServingDate(dtServed.Value)
                .withQuantity(int.Parse(qty.Text.Trim()))
                .withPrepTime(TimeSpan.Parse(prep.Text.Trim()))
                .withIngredientModel(ingredientServings)
                .Build();
            served?.Invoke(this, s);
            DialogResult = DialogResult.OK;
            ingredientServings.Clear();
        }
        private void exit(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        public void displayIngredient()
        {
            if (mm.Controls.Count > 0)
            {
                mm.Controls.RemoveAt(0);
            }

            im = new IngredientMenu(ingredientServices);
            im.xx.Visible = false;
            im.getIngredient(dtServed.Value);
            im.confirmButton.Visible = true;
            im.IngredientSelectedEvent += (s, e) =>
            {
                ingredientServings = e;
                confirmButton();
                im.reset();
            };
            im.TopLevel = false;
            im.Dock = DockStyle.Fill;
            mm.Controls.Add(im);
            im.Show();

        }
        private void menuPrice_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(menuPrice.Text))
            {
                p.Text = "Price per Servings";
                return;
            }
            if (double.TryParse(menuPrice.Text.Trim(), out double d))
            {
                p.Text = $"Price After Tax | ( {(d / TaxHelper.TAX_F).ToString("N2")} ) Sale Price";
            }
            else
            {
                p.Text = "Price per Servings";
            }
        }
        private void menuPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void qty_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(qty.Text.Trim(), out int q) && q > 0)
            {
                im.setServing(q);
            }
        }
    }
}
