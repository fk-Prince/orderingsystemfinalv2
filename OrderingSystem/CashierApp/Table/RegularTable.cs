using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OrderingSystem.Model;
using OrderingSystem.Repo.CashierMenuRepository;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Components
{
    public partial class RegularTable : Form
    {
        private List<MenuModel> variants;
        private DataTable table;
        private readonly IngredientServices ingredientServices;

        public RegularTable(List<MenuModel> variants, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.variants = variants;
            this.ingredientServices = ingredientServices;
            displayMenu();
        }

        private void displayMenu()
        {

            table = new DataTable();
            table.Columns.Add("Flavor");
            table.Columns.Add("Size");
            table.Columns.Add("Prep Estimated Time");
            table.Columns.Add("Price");
            table.Columns.Add("Price After Tax");
            table.Columns.Add("Price After Tax / Discount");
            variants.ForEach(v => table.Rows.Add(v.FlavorName, v.SizeName, v.EstimatedTime, v.MenuPrice, v.getPriceAfterVat().ToString("N2"), v.getPriceAfterVatWithDiscount().ToString("N2")));
            dataGrid.AutoGenerateColumns = false;
            dataGrid.DataSource = table;

            dataGrid.Columns.Clear();


            List<string> flavors = new MenuRepository().getFlavor();
            List<string> sizes = new MenuRepository().getSize();
            DataGridViewComboBoxColumn flavorCombo = new DataGridViewComboBoxColumn();
            flavorCombo.Name = "Flavor";
            flavorCombo.HeaderText = "Flavor";
            flavorCombo.DataSource = flavors;
            flavorCombo.DataPropertyName = "Flavor";
            flavorCombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            dataGrid.Columns.Add(flavorCombo);


            DataGridViewComboBoxColumn sizeCombo = new DataGridViewComboBoxColumn();
            sizeCombo.Name = "Size";
            sizeCombo.HeaderText = "Size";
            sizeCombo.DataSource = sizes;
            sizeCombo.DataPropertyName = "Size";
            sizeCombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            dataGrid.Columns.Add(sizeCombo);


            DataGridViewTextBoxColumn timeColumn = new DataGridViewTextBoxColumn();
            timeColumn.Name = "Prep Estimated Time";
            timeColumn.HeaderText = "Prep Estimated Time";
            timeColumn.DataPropertyName = "Prep Estimated Time";
            dataGrid.Columns.Add(timeColumn);


            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn();
            priceColumn.Name = "Price";
            priceColumn.HeaderText = "Price";
            priceColumn.DataPropertyName = "Price";
            dataGrid.Columns.Add(priceColumn);

            DataGridViewTextBoxColumn priceTaxColumn = new DataGridViewTextBoxColumn();
            priceTaxColumn.Name = "Price After Tax";
            priceTaxColumn.HeaderText = "Price After Tax";
            priceTaxColumn.DataPropertyName = "Price After Tax";
            dataGrid.Columns.Add(priceTaxColumn);

            DataGridViewTextBoxColumn priceTaxColmn = new DataGridViewTextBoxColumn();
            priceTaxColmn.Name = "Price After Tax / Discount";
            priceTaxColmn.HeaderText = "Price After Tax / Discount";
            priceTaxColmn.DataPropertyName = "Price After Tax / Discount";
            dataGrid.Columns.Add(priceTaxColmn);


            DataGridViewButtonColumn ingredientsButtonColumn = new DataGridViewButtonColumn();
            ingredientsButtonColumn.Name = "ViewIngredients";
            ingredientsButtonColumn.HeaderText = "Ingredients";
            ingredientsButtonColumn.Text = "View Ingredients";
            ingredientsButtonColumn.UseColumnTextForButtonValue = true;
            dataGrid.Columns.Add(ingredientsButtonColumn);

            dataGrid.EditingControlShowing += (s, e) =>
            {
                if (e.Control is ComboBox cb)
                {
                    cb.DropDownStyle = ComboBoxStyle.DropDown;
                    cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
            };

            dataGrid.CellContentClick += (s, e) =>
            {
                if (e.ColumnIndex == dataGrid.Columns["ViewIngredients"].Index && e.RowIndex >= 0)
                {
                    var variantDetail = variants[e.RowIndex];
                    IngredientMenu im = new IngredientMenu(ingredientServices);
                    im.IngredientSelectedEvent += (ss, ee) =>
                    {
                        List<IngredientModel> ingredientSelected = ee;
                        if (ingredientSelected.Count > 0)
                        {
                            bool suc = ingredientServices.saveIngredientByMenu(variantDetail.MenuDetailId, ingredientSelected, "Regular");
                            if (suc)
                            {
                                MessageBox.Show("Ingredient Updated.");
                                im.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Ingredient Failed to Update", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };
                    im.getIngredientByMenu(variantDetail);
                    im.initTable1();
                    im.hideNotSelected();
                    im.updateButton.Visible = true;
                    im.confirmButton.Visible = true;
                    DialogResult rs = im.ShowDialog(this);
                    if (rs == DialogResult.OK)
                    {
                        im.Hide();
                    }
                }
            };
        }

        private void dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (dataGrid.Columns[e.ColumnIndex].Name == "Price")
            //{
            //    string input = e.FormattedValue?.ToString().Trim();

            //    if (string.IsNullOrEmpty(input))
            //        return;

            //    string pattern = @"^(?:\d{1,3}(?:,\d{3})*|\d+)?(?:\.\d+)?$";

            //    if (!Regex.IsMatch(input, pattern))
            //    {
            //        MessageBox.Show("Invalid Input", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        e.Cancel = true;
            //    }
            //}
        }

        public List<MenuModel> getMenus()
        {

            List<MenuModel> menus = new List<MenuModel>();
            string patternNumber = @"^(?:\d{1,3}(?:,\d{3})*|\d+)?(?:\.\d+)?$";
            string patternText = @"^[a-zA-Z]+$";

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (row.IsNewRow) continue;

                string price = row.Cells["Price"].Value?.ToString().Trim();
                string prepTimeText = row.Cells["Prep Estimated Time"].Value?.ToString().Trim();
                string sizeText = row.Cells["Size"].Value?.ToString().Trim();
                string flavorText = row.Cells["Flavor"].Value?.ToString().Trim();


                if (string.IsNullOrEmpty(price) || !Regex.IsMatch(price, patternNumber))
                {
                    MessageBox.Show("Invalid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (string.IsNullOrEmpty(prepTimeText) && !TimeSpan.TryParse(prepTimeText, out _))
                {
                    MessageBox.Show("Invalid Preparation Time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }


                if (string.IsNullOrEmpty(sizeText) && !Regex.IsMatch(sizeText, patternText))
                {
                    MessageBox.Show("Invalid Size", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }


                if (string.IsNullOrEmpty(flavorText) && !Regex.IsMatch(flavorText, patternText))
                {
                    MessageBox.Show("Invalid Flavor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                var menu = MenuModel.Builder()
                    .WithMenuDetailId(variants[row.Index].MenuDetailId)
                    .WithFlavorName(flavorText)
                    .WithSizeName(sizeText)
                    .WithEstimatedTime(TimeSpan.Parse(prepTimeText))
                    .WithPrice(double.Parse(price))
                    .Build();

                menus.Add(menu);
            }

            return menus;
        }


        public List<MenuModel> getValues()
        {
            for (int i = 0; i < variants.Count; i++)
            {
                var row = table.Rows[i];

                string flavor = row[0]?.ToString();
                string size = row[1]?.ToString();
                TimeSpan time = TimeSpan.Parse(row[2]?.ToString());
                double price = double.Parse(row[3]?.ToString());

                variants[i] = MenuModel.Builder()
                    .WithMenuDetailId(variants[i].MenuDetailId)
                    .WithFlavorName(flavor)
                    .WithSizeName(size)
                    .WithEstimatedTime(time)
                    .WithPrice(price)
                    .Build();
            }

            return variants;
        }

        public void refreshTable(List<MenuModel> variants)
        {
            this.variants = variants;

            if (table != null)
            {
                table.Rows.Clear();
            }
            else
            {
                table = new DataTable();
                table.Columns.Add("Flavor");
                table.Columns.Add("Size");
                table.Columns.Add("Prep Estimated Time");
                table.Columns.Add("Price");
                table.Columns.Add("Price After Tax / Discount");
            }

            foreach (var v in variants)
            {
                table.Rows.Add(
                    v.FlavorName,
                    v.SizeName,
                    v.EstimatedTime,
                    v.MenuPrice,
                    v.getPriceAfterVatWithDiscount().ToString("N2")
                );
            }

            dataGrid.DataSource = null;
            dataGrid.DataSource = table;
        }

        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGrid.Rows[e.RowIndex];

            if (dataGrid.Columns[e.ColumnIndex].Name == "Price")
            {
                string priceText = row.Cells["Price"].Value?.ToString();
                if (double.TryParse(priceText, out double price))
                {
                    double vat = price * 0.12;
                    double priceAfterTax = price + vat;

                    var variant = variants.Count > e.RowIndex ? variants[e.RowIndex] : null;
                    double discount = variant?.Discount?.Rate ?? 0;

                    double priceAfterDiscount = priceAfterTax - discount;

                    row.Cells["Price After Tax"].Value = priceAfterTax.ToString("N2");
                    row.Cells["Price After Tax / Discount"].Value = priceAfterDiscount.ToString("N2");
                }
                else
                {
                    row.Cells["Price After Tax"].Value = "";
                    row.Cells["Price After Tax / Discount"].Value = "";
                }
            }
        }

        private void dataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGrid.IsCurrentCellDirty)
                dataGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
