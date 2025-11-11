using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Model;
using OrderingSystem.Repo.CashierMenuRepository;
using OrderingSystem.Services;
using OrderingSystem.util;
using ComboBox = System.Windows.Forms.ComboBox;

namespace OrderingSystem.CashierApp.Components
{
    public partial class RegularTable : Form
    {
        private List<MenuModel> variants;
        private DataTable table;
        private readonly IngredientServices ingredientServices;
        private bool isUpdating = false;

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
            table.Columns.Add("Price After Discount");
            table.Columns.Add("Price After Discount / Tax");
            variants.ForEach(v => table.Rows.Add(v.FlavorName, v.SizeName, v.EstimatedTime, v.MenuPrice, v.getPriceAfterDiscount().ToString("N2"), v.getPriceAfterVatWithDiscount().ToString("N2")));
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
            priceTaxColumn.Name = "Price After Discount";
            priceTaxColumn.HeaderText = "Price After Discount";
            priceTaxColumn.DataPropertyName = "Price After Discount";
            dataGrid.Columns.Add(priceTaxColumn);

            DataGridViewTextBoxColumn priceTaxColmn = new DataGridViewTextBoxColumn();
            priceTaxColmn.Name = "Price After Discount / Tax";
            priceTaxColmn.HeaderText = "Price After Discount / Tax";
            priceTaxColmn.DataPropertyName = "Price After Discount / Tax";
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
            dataGrid.Columns[6].Width = 100;
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
                                MessageBox.Show("Ingredient Updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (SessionStaffData.Role.ToLower() == "cashier")
                    {
                        im.confirmButton.Visible = false;
                        im.updateButton.Visible = false;
                    }
                    DialogResult rs = im.ShowDialog(this);
                    if (rs == DialogResult.OK)
                    {
                        im.Hide();
                    }
                }
            };
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
                table.Columns.Add("Price After Discount");
                table.Columns.Add("Price After Discount / Tax");
            }

            foreach (var v in variants)
            {
                table.Rows.Add(
                    v.FlavorName,
                    v.SizeName,
                    v.EstimatedTime,
                    v.MenuPrice,
                    v.getPriceAfterDiscount().ToString("N2"),
                    v.getPriceAfterVatWithDiscount().ToString("N2")
                );
            }

            dataGrid.DataSource = null;
            dataGrid.DataSource = table;
        }
        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isUpdating) return;
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGrid.Rows[e.RowIndex];
            if (dataGrid.Columns[e.ColumnIndex].Name == "Price")
            {
                string priceS = row.Cells["Price"].Value?.ToString();
                if (double.TryParse(priceS, out double price))
                {
                    var variant = variants.Count > e.RowIndex ? variants[e.RowIndex] : null;
                    var discount = variant?.Discount?.Rate ?? 0;
                    var priceWithTax = price * TaxHelper.TAX_F;
                    var discountedPrice = price * (1 - discount);
                    var discountedPriceWithTax = discountedPrice * TaxHelper.TAX_F;

                    isUpdating = true;
                    row.Cells["Price After Discount"].Value = discountedPrice.ToString("N2");
                    row.Cells["Price After Discount / Tax"].Value = discountedPriceWithTax.ToString("N2");
                    isUpdating = false;
                }
                else
                {
                    row.Cells["Price After Discount"].Value = "";
                    row.Cells["Price After Discount / Tax"].Value = "";
                }
            }
            else if (dataGrid.Columns[e.ColumnIndex].Name == "Price After Discount / Tax")
            {
                string priceAfterTaxS = row.Cells["Price After Discount / Tax"].Value?.ToString();
                if (double.TryParse(priceAfterTaxS, out double priceTax))
                {
                    var variant = variants.Count > e.RowIndex ? variants[e.RowIndex] : null;
                    var discount = variant?.Discount?.Rate ?? 0;
                    var origPrice = priceTax / ((1 - discount) * (TaxHelper.TAX_F));
                    var discountedPrice = origPrice * (1 - discount);
                    isUpdating = true;
                    row.Cells["Price"].Value = origPrice.ToString("N2");
                    row.Cells["Price After Discount"].Value = discountedPrice.ToString("N2");
                    isUpdating = false;
                }
                else
                {
                    row.Cells["Price"].Value = "";
                    row.Cells["Price After Discount"].Value = "";
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
