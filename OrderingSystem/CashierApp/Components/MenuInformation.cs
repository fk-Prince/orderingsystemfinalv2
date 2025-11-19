using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.Forms.Menu;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.CashierApp.Table;
using OrderingSystem.Model;
using OrderingSystem.Repository.Discount;
using OrderingSystem.Services;
using OrderingSystem.util;

namespace OrderingSystem.CashierApp.Components
{
    public partial class MenuInformation : Form
    {
        private readonly CategoryServices categoryServices;
        private readonly IngredientServices ingredientServices;
        private List<MenuDetailModel> variantList;
        private List<MenuDetailModel> included;
        private MenuService menuService;

        private readonly MenuDetailModel menu;
        private RegularTable regular;
        private PackageTable package;
        private bool isEditMode = false;
        private bool isPackaged = false;
        public event EventHandler menuUpdated;
        public MenuInformation(MenuDetailModel menu, MenuService menuService, CategoryServices categoryServices, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.menu = menu;
            this.menuService = menuService;
            this.categoryServices = categoryServices;
            this.ingredientServices = ingredientServices;
            displayMenuDetails();
            displayTable();
        }
        private void displayMenuDetails()
        {
            image.Image = menu.MenuImage;
            menuName.Text = menu.MenuName;
            description.Text = menu.MenuDescription;
            catTxt.Text = menu.CategoryName;
            category.Text = menu.CategoryName;
            cBox.SelectedIndex = menu.isAvailable ? 0 : 1;
            if (SessionStaffData.Role == StaffModel.StaffRole.Cashier)
            {
                b1.Visible = false;
                b2.Visible = false;
                b3.Visible = false;
                b4.Visible = false;
            }
            try
            {
                isAvailable();

                List<CategoryModel> c = categoryServices.getCategories();
                c.ForEach(ex => category.Items.Add(ex.CategoryName));

                cbd.DisplayMember = "DisplayText";
                List<DiscountModel> dm = new DiscountServices(new DiscountRepository()).getDiscountAvailable();

                dm.ForEach(d => d.DisplayText = $"{d.Rate * 100}% | {d.UntilDate:yyyy-MM-dd}");

                cbd.Items.Clear();
                dm.ForEach(ex => cbd.Items.Add(ex));

                if (menu.Discount != null)
                {
                    var match = dm.FirstOrDefault(ed => ed.DiscountId == menu.Discount.DiscountId);
                    if (match != null)
                        cbd.SelectedItem = match;
                }
                else
                {
                    cbd.SelectedIndex = -1;
                }
                prevStat = menu.isAvailable ? "Available" : "Not Available";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadForm(Form f)
        {
            if (mm.Controls.Count > 0)
            {
                mm.Controls.Clear();
            }

            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mm.Controls.Add(f);
            f.Show();
        }
        private void displayTable()
        {
            try
            {
                isPackaged = menuService.isMenuPackage(menu);
                if (isPackaged)
                {
                    b2.Visible = false;
                    b4.Visible = true;
                    included = menuService.getBundled(menu);
                    loadForm(package = new PackageTable(included));

                    lPrice.Visible = true;
                    price.Visible = true;
                    l2Price.Visible = true;
                    dprice.Visible = true;
                    double pricex = menuService.getBundlePrice(menu);
                    DiscountModel c = (DiscountModel)cbd?.SelectedItem;
                    price.Text = pricex.ToString("N2");
                    dprice.Text = (((pricex - ((c?.Rate ?? 0) * pricex)) * TaxHelper.TAX_F)).ToString("N2");
                }
                else
                {
                    b3.Visible = false;
                    variantList = menuService.getMenuDetail().FindAll(e => e.MenuId == menu.MenuId);
                    loadForm(regular = new RegularTable(variantList, ingredientServices));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal Server Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void price_TextChanged(object sender, EventArgs e)
        {
            if (isUpdate) return;
            isUpdate = true;
            DiscountModel c = (DiscountModel)cbd?.SelectedItem;
            if (double.TryParse(price.Text.Trim(), out double d))
            {
                dprice.Text = (((d - ((c?.Rate ?? 0) * d)) * TaxHelper.TAX_F)).ToString("N2");
            }
            isUpdate = false;
        }
        private bool isUpdate;
        private void dprice_TextChanged(object sender, EventArgs e)
        {
            if (isUpdate) return;
            isUpdate = true;
            DiscountModel c = (DiscountModel)cbd?.SelectedItem;
            if (double.TryParse(dprice.Text.Trim(), out double dp))
            {
                price.Text = (dp / ((1 - (c?.Rate ?? 0)) * TaxHelper.TAX_F)).ToString("N2");
            }
            isUpdate = false;
        }
        private void changeMode(object sender, EventArgs e)
        {
            isEditMode = !isEditMode;
            b1.Text = isEditMode ? "Save" : "Edit";
            if (isEditMode)
            {
                catTxt.Visible = false;
                category.Visible = true;
                Border(true);
            }
            else
            {
                confirmEdit();
                catTxt.Visible = true;
                Border(false);
            }
            cBox.Enabled = isEditMode;
            cbd.Enabled = isEditMode;
            category.Text = menu.CategoryName;
            catTxt.Text = menu.CategoryName;

        }
        private void confirmEdit()
        {
            try
            {
                DialogResult rs = MessageBox.Show("Do you want to proceed to update this menu?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.No)
                {
                    return;
                }

                string name = menuName.Text.Trim();
                string cat = category.Text.Trim();
                string desc = description.Text.Trim();
                string pricex = price.Text.Trim();



                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(cat) || (isPackaged && string.IsNullOrEmpty(pricex)))
                {
                    MessageBox.Show("Empty Field", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!isPriceValid(pricex) && isPackaged)
                {
                    MessageBox.Show("Invalid price", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] imagex = null;
                if (image.ImageLocation != null)
                    imagex = ImageHelper.GetImageFromFile(image.Image);

                bool suc = false;
                MenuDetailModel menus = null;
                string type = "";
                if (package != null)
                {
                    if (package.getMenus() == null) return;

                    var builder = MenuPackageModel.Builder()
                                     .WithMenuId(menu.MenuId)
                                     .WithMenuName(name)
                                     .isAvailable(cBox.Text == "Available")
                                     .WithMenuDescription(desc)
                                     .WithCategoryName(cat)
                                     .WithPrice(double.Parse(pricex))
                                     .WithPackageIncluded(package.getMenus());
                    if (cbd.SelectedIndex != -1) builder = builder.WithDiscount((DiscountModel)cbd.SelectedItem);
                    if (imagex != null) builder = builder.WithMenuImageByte(imagex);
                    menus = builder.Build();
                    type = "bundle";

                }
                else if (regular != null)
                {
                    if (regular.getMenus() == null) return;
                    var builder = MenuDetailModel.Builder()
                                 .WithMenuId(menu.MenuId)
                                 .WithMenuName(name)
                                 .isAvailable(cBox.Text == "Available")
                                 .WithMenuDescription(desc)
                                 .WithCategoryName(cat)
                                 .WithVariant(regular.getMenus());
                    if (imagex != null) builder = builder.WithMenuImageByte(imagex);
                    if (cbd.SelectedIndex != -1) builder = builder.WithDiscount((DiscountModel)cbd.SelectedItem);
                    menus = builder.Build();
                    type = "regular";
                }

                suc = menuService.updateMenu(menus, type);

                if (suc)
                {
                    MessageBox.Show("Updated Successfully", "Update Scucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    menuUpdated.Invoke(this, EventArgs.Empty);
                    regular?.refreshTable(menuService.getMenuDetail().FindAll(e => e.MenuId == menu.MenuId));
                }
                else MessageBox.Show("Failed to update", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Internal Server Error." + ex.Message);
            }
        }
        private void Border(bool x)
        {
            foreach (var xb in Controls)
            {
                if (xb is Guna2TextBox tb)
                {
                    tb.ReadOnly = !x;
                    tb.Enabled = x;
                    tb.BorderThickness = x ? 1 : 0;
                }
            }
        }
        private bool isPriceValid(string text)
        {
            return Regex.IsMatch(text, @"^(\d{1,3}(,\d{3})*|\d+)(\.\d{1,2})?$");
        }
        private void ImageButton(object sender, EventArgs e)
        {
            ofd.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string imagePath = ofd.FileName;
                image.ImageLocation = imagePath;
            }
            else
            {
                image.ImageLocation = null;
                image.Image = menu.MenuImage;
            }
        }
        private void newVariantButton(object sender, EventArgs e)
        {
            var cloneList = new List<MenuDetailModel>(variantList);
            VariantMenuPopup pop = new VariantMenuPopup(cloneList, ingredientServices);
            DialogResult rs = pop.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                cloneList = pop.getVariants();
                var difference = cloneList.Except(variantList).ToList();
                menuService.newMenuVariant(menu.MenuId, difference);
                variantList.AddRange(difference);
                displayTable();
            }
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            BundleMenuPopup bb = new BundleMenuPopup(menuService, included);
            DialogResult rs = bb.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                included = bb.getMenuSelected();
                bool suc = menuService.updateBundle(menu.MenuDetailId, included);
                if (suc)
                    MessageBox.Show("Updated Bundled", "Update Scucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Failed to update", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                displayTable();
                bb.Hide();
            }
        }
        private void b4_Click(object sender, EventArgs e)
        {
            IngredientMenu pop = new IngredientMenu(ingredientServices);
            pop.IngredientSelectedEvent += (ss, ee) =>
            {
                List<IngredientModel> ingredientSelected = ee;
                if (ingredientSelected.Count > 0)
                {

                    bool suc = ingredientServices.saveIngredientByMenu(menu.MenuId, ingredientSelected, "Package");
                    if (suc)
                    {
                        MessageBox.Show("Ingredient Updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pop.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update ingredient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            pop.confirmButton.Enabled = true;
            pop.getIngredientByMenu(menu);
            pop.initTable2();
            DialogResult rs = pop.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                pop.Hide();
            }
        }
        private void isAvailable()
        {
            if (cBox.Text == "Available" || !isEditMode)
            {
                BackColor = Color.White;
                if (package != null) package.BackColor = Color.White;
                if (regular != null) regular.BackColor = Color.White;
            }
            else
            {
                if (package != null) package.BackColor = Color.LightGray;
                if (regular != null) regular.BackColor = Color.LightGray;
                BackColor = Color.LightGray;
            }
        }

        private string prevStat;
        private void cBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBox.Text == prevStat)
                return;
            cBox.SelectedIndexChanged -= cBox_SelectedIndexChanged;
            DialogResult rs = MessageBox.Show($"Are you sure you want to {(cBox.Text != "Available" ? "disable" : "enable")} this menu?", "Confirm Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                cBox.SelectedIndex = (cBox.Text == "Available") ? 0 : 1;
            }
            else
                cBox.SelectedIndex = (cBox.Text == "Available") ? 1 : 0;
            isAvailable();
            prevStat = cBox.Text;
            cBox.SelectedIndexChanged += cBox_SelectedIndexChanged;
        }


    }
}
