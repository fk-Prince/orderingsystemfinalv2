using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Components;
using OrderingSystem.Model;
using OrderingSystem.Repo.CashierMenuRepository;
using OrderingSystem.Services;
using OrderingSystem.util;

namespace OrderingSystem.CashierApp.Forms.Menu
{
    public partial class VariantMenuPopup : Form
    {

        private List<IngredientModel> ingredientSelected;
        private readonly List<MenuDetailModel> variantList;
        private readonly IngredientServices ingredientServices;

        public VariantMenuPopup(List<MenuDetailModel> variantList, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.variantList = variantList;
            this.ingredientServices = ingredientServices;
            ingredientSelected = new List<IngredientModel>();

            displayIngredient();
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
            string flavor = cmbFlavor.Text.Trim();
            string size = cmbSize.Text.Trim();
            if (flavor == "") flavor = "Regular";
            if (size == "") size = "Regular";
            flavor = flavor.Substring(0, 1).ToUpper() + flavor.Substring(1).ToLower();
            size = size.Substring(0, 1).ToUpper() + size.Substring(1).ToLower();

            bool exist = variantList.Any(ee =>
                 string.Equals(ee.FlavorName, flavor, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(ee.SizeName, size, StringComparison.OrdinalIgnoreCase));
            if (exist)
            {
                MessageBox.Show($"This pair of Size: {size} & Flavor: {flavor}, is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!TimeSpan.TryParse(estimatedTime.Text.Trim(), out TimeSpan ex))
            {
                MessageBox.Show("Invalid Time Format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ingredientSelected.Count <= 0)
            {
                MessageBox.Show("No Selected Ingredient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MenuDetailModel variant = MenuDetailModel.Builder()
                .WithPrice(double.Parse(menuPrice.Text.Trim()) / 1.12)
                .WithFlavorName(flavor)
                .WithEstimatedTime(TimeSpan.Parse(estimatedTime.Text.Trim()))
                .WithSizeName(size)
                .WithIngredients(ingredientSelected.ToList())
                .Build();
            variantList.Add(variant);

            MessageBox.Show("Successfully Added");
            ingredientSelected.Clear();
        }
        public List<MenuDetailModel> getVariants()
        {
            return variantList;
        }
        private void exit(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        private void NumberOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }
        private void VariantMenuPopup_Load(object sender, EventArgs e)
        {
            try
            {
                MenuService m = new MenuService(new MenuRepository());
                List<string> flavor = m.getSizeFlavor("flavor");
                List<string> size = m.getSizeFlavor("size");
                flavor.ForEach(f => cmbFlavor.Items.Add(f));
                size.ForEach(f => cmbSize.Items.Add(f));
                if (flavor.Count > 0) cmbFlavor.SelectedItem = "Regular";
                if (size.Count > 0) cmbSize.SelectedItem = "Regular";
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void displayIngredient()
        {
            if (mm.Controls.Count > 0)
            {
                mm.Controls.RemoveAt(0);
            }

            IngredientMenu im = new IngredientMenu(ingredientServices);
            im.getIngredient();
            im.initTable2();
            im.confirmButton.Visible = true;
            im.IngredientSelectedEvent += (s, e) =>
            {
                ingredientSelected = e;
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
                p.Text = "Price";
                lp.Location = new Point(p.Right, lp.Location.Y);
                return;
            }
            if (double.TryParse(menuPrice.Text.Trim(), out double d))
            {
                p.Text = $"Price After Tax | ( {(d / TaxHelper.TAX_F).ToString("N2")} ) - Sale Price ";
            }
            else
            {
                p.Text = "Price";
            }
            lp.Location = new Point(p.Right, lp.Location.Y);
        }
    }
}
