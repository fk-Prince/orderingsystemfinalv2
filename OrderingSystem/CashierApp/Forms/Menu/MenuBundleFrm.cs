using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.CashierApp.Components;
using OrderingSystem.Model;
using OrderingSystem.Properties;
using OrderingSystem.Repository.CategoryRepository;
using OrderingSystem.Services;
using OrderingSystem.util;
namespace OrderingSystem.CashierApp.Forms.Menu
{
    public partial class MenuBundleFrm : Form
    {
        private List<MenuDetailModel> inclded = new List<MenuDetailModel>();
        private List<IngredientModel> ingredientSelected = new List<IngredientModel>();
        private readonly IngredientServices ingredientServices;
        private readonly MenuService menuService;
        public event EventHandler menuUpdate;
        public MenuBundleFrm(MenuService menuService, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.menuService = menuService;
            this.ingredientServices = ingredientServices;
        }


        private void newBundleEvent(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(menuName.Text.Trim()) ||
                string.IsNullOrEmpty(menuDescription.Text.Trim()) ||
                string.IsNullOrEmpty(menuPrice.Text.Trim()) ||
                string.IsNullOrEmpty(cmbCat.Text.Trim()) ||
                string.IsNullOrWhiteSpace(estimatedTime.Text.Trim()))
                {
                    MessageBox.Show("Please fill all * fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (inclded.Count <= 1)
                {
                    MessageBox.Show("It should atleast contain 2 Existing Menu.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!isPriceValid(menuPrice.Text.Trim()))
                {
                    MessageBox.Show("Invalid price .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!TimeSpan.TryParse(estimatedTime.Text.Trim(), out TimeSpan est))
                {
                    MessageBox.Show("Invalid estimated time format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string name = menuName.Text.Trim();

                if (menuService.isMenuNameExist(name))
                {
                    MessageBox.Show("Menu Name already exists, try different.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string desc = menuDescription.Text.Trim();
                double price = double.Parse(menuPrice.Text.Trim());
                string cat = cmbCat.Text.Trim();
                if (pictureBox.Image == null) pictureBox.Image = Resources.placeholder;
                byte[] image = ImageHelper.GetImageFromFile(pictureBox.Image);


                MenuPackageModel md = MenuPackageModel.Builder()
                    .WithMenuName(name)
                    .WithMenuDescription(desc)
                    .WithPrice(price / 1.12)
                    .WithEstimatedTime(est)
                    .WithMenuImageByte(image)
                    .WithIngredients(ingredientSelected)
                    .WithPackageIncluded(inclded)
                    .WithCategoryName(cat)
                    .Build();

                if (menuService.saveMenu(md, "Bundle"))
                {
                    MessageBox.Show("New menu created successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    menuUpdate?.Invoke(this, EventArgs.Empty);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Failed to create new menu.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (MySqlException)
            {
                MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool isPriceValid(string text)
        {
            return Regex.IsMatch(menuPrice.Text.Trim(), @"^(\d{1,3}(,\d{3})*|\d+)(\.\d{1,2})?$");
        }
        private void ingredientListButton(object sender, EventArgs eb)
        {

            IngredientMenu p = new IngredientMenu(ingredientServices);
            p.getIngredient();
            p.initTable2();
            p.ingredientSelector(ingredientSelected);
            p.closeButton.Visible = true;
            p.IngredientSelectedEvent += (s, e) =>
            {
                ingredientSelected = e;
                if (e.Count > 0) MessageBox.Show("Added");
            };
            DialogResult rs = p.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                ingredientButton.Text = ingredientSelected.Count > 0 ? "View selected ingredients" : "Click to choose ingredients";
                p.Hide();
            }
        }
        private void menuListButton(object sender, System.EventArgs e)
        {
            BundleMenuPopup p = new BundleMenuPopup(menuService, inclded);
            DialogResult rs = p.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                inclded = p.getMenuSelected();
                menuButton.Text = inclded.Count > 0 ? "View included menu" : "Click to choose bundle menu";
                p.Hide();
            }
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void MenuBundleFrm_Load(object sender, EventArgs e)
        {
            ICategoryRepository categoryRepository = new CategoryRepository();
            List<CategoryModel> cat = categoryRepository.getCategoriesByMenu();
            cat.ForEach(ex => cmbCat.Items.Add(ex.CategoryName));
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            chooseImage();
        }
        private void chooseImage()
        {
            ofd.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string imagePath = ofd.FileName;
                pictureBox.ImageLocation = imagePath;
            }
            else
            {
                pictureBox.ImageLocation = null;
                pictureBox.Image = null;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            chooseImage();
        }

        private void menuPrice_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(menuPrice.Text))
            {
                l2.Text = "Price";
                lp.Location = new Point(l2.Right + 2, lp.Location.Y);
                return;
            }
            if (double.TryParse(menuPrice.Text.Trim(), out double d))
            {
                l2.Text = $"Price After Tax | (  {(d / TaxHelper.TAX_F).ToString("N2")} Sale Price";
            }
            else
            {
                l2.Text = "Price";
            }
            lp.Location = new Point(l2.Right + 2, lp.Location.Y);
        }
    }
}
