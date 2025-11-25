using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.Forms.Menu;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Repository.Discount;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Components
{
    public partial class MenuInformation : Form
    {
        private readonly CategoryServices categoryServices;
        private readonly IngredientServices ingredientServices;
        private MenuService menuService;
        private ServingTable se;

        private readonly MenuDetailModel menu;
        private bool isEditMode = false;
        public event EventHandler menuUpdated;
        public MenuInformation(MenuDetailModel menu, MenuService menuService, CategoryServices categoryServices, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.menu = menu;
            this.menuService = menuService;
            this.categoryServices = categoryServices;
            this.ingredientServices = ingredientServices;
            displayMenuDetails();
            loadForm(se = new ServingTable(menu, menuService));
        }
        private void displayMenuDetails()
        {
            image.Image = menu.MenuImage;
            menuName.Text = menu.MenuName;
            description.Text = menu.MenuDescription;
            catTxt.Text = menu.CategoryName;
            category.Text = menu.CategoryName;
            cBox.SelectedIndex = menu.isAvailable ? 0 : 1;

            try
            {
                isAvailable();

                List<CategoryModel> c = categoryServices.getCategories();
                c.ForEach(ex => category.Items.Add(ex.CategoryName));

                List<DiscountModel> dm = new DiscountServices(new DiscountRepository()).getDiscountAvailable();

                dm.ForEach(d => d.DisplayText = $"{d.Rate * 100}% | {d.UntilDate:yyyy-MM-dd}");
                prevStat = menu.isAvailable ? "Available" : "Not Available";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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



                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(cat))
                {
                    MessageBox.Show("Empty Field", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] imagex = null;
                if (image.ImageLocation != null)
                    imagex = ImageHelper.GetImageFromFile(image.Image);


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
        private void closeButton_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void isAvailable()
        {
            if (cBox.Text == "Available" || !isEditMode)
            {
                BackColor = Color.White;

            }
            else
            {
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
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            NewServingsFrm pop = new NewServingsFrm(ingredientServices);
            pop.served += (ss, ee) =>
            {
                try
                {
                    if (menuService.isServingDateExistsing(menu.MenuId, ee.date))
                        throw new InvalidInput("This menu on this date is already have servings");

                    bool succ = menuService.saveNewServing(menu.MenuId, ee);
                    if (succ)
                        MessageBox.Show("Successfully Added", "New Servings Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (InvalidInput ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception)
                {
                    MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            DialogResult rs = pop.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                se.displayCurrentServings();
                pop.Hide();
            }
        }

        public void loadForm(Form f)
        {
            if (mm.Controls.Count > 0) mm.Controls.Clear();
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mm.Controls.Add(f);
            f.Show();
            se.displayCurrentServings();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            se.displayCurrentServings();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            se.displayHistoryServing();
        }
    }
}
