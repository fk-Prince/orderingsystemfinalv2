using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Properties;
using OrderingSystem.Repository.CategoryRepository;
using OrderingSystem.Services;
using NotSupportedException = OrderingSystem.Exceptions.NotSupportedException;

namespace OrderingSystem.CashierApp.Forms.Menu
{
    public partial class NewMenu : Form
    {

        private MenuService menuService;
        private readonly IngredientServices ingredientServices;
        public event EventHandler menuUpdate;
        private List<ServingsModel> servingMenu;
        public NewMenu(MenuService menuService, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.menuService = menuService;
            this.ingredientServices = ingredientServices;
            servingMenu = new List<ServingsModel>();
        }
        private void NewMenu_Load(object sender, System.EventArgs e)
        {
            try
            {
                List<CategoryModel> cat = new CategoryServices(new CategoryRepository()).getCategories();
                cat.ForEach(c => cmbCat.Items.Add(c.CategoryName));
            }
            catch (Exception)
            {
                MessageBox.Show("Server Error", "Category Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ImageButton1(object sender, System.EventArgs e)
        {
            chooseImage();
        }
        private void ImageButton2(object sender, System.EventArgs e)
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
        private void LetterOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }


        private void exit(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(menuName.Text.Trim()) ||
                    string.IsNullOrEmpty(menuDescription.Text.Trim()) ||
                    string.IsNullOrEmpty(cmbCat.Text.Trim()))
                    throw new InvalidInput("Please fill all * fields.");

                string name = menuName.Text.Trim();

                if (menuService.isMenuNameExist(name))
                    throw new InvalidInput("Menu Name already exists, try different.");

                DialogResult rs = MessageBox.Show("Do you want to add a serving?", "New Menu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    NewServingsFrm im = new NewServingsFrm(ingredientServices);
                    im.served += (ss, ee) =>
                    {
                        addNewMenu(true, ee);
                    };
                    DialogResult rs1 = im.ShowDialog(this);
                    if (rs1 == DialogResult.Yes)
                    {
                        im.Hide();
                    }
                }
                else
                {
                    addNewMenu(false);
                }
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is InvalidInput)
            {
                MessageBox.Show(ex.Message, "Menu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addNewMenu(bool x, ServingsModel s = null)
        {
            try
            {
                string name = menuName.Text.Trim();
                string desc = menuDescription.Text.Trim();
                string cat = cmbCat.Text.Trim();
                if (pictureBox.Image == null) pictureBox.Image = Resources.placeholder;
                byte[] image = ImageHelper.GetImageFromFile(pictureBox.Image);


                MenuDetailModel md = MenuDetailModel.Builder()
                    .WithMenuName(name)
                    .WithMenuDescription(desc)
                    .WithMenuImageByte(image)
                    .WithCategoryName(cat)
                    .withServing(s)
                    .Build();

                if (menuService.saveNewMenu(md, x))
                {
                    MessageBox.Show("New menu created successfully.", "Menu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show("Failed to create new menu.", "Menu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is InvalidInput)
            {
                MessageBox.Show(ex.Message, "Menu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
