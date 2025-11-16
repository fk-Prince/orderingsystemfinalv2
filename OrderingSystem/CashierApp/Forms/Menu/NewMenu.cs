using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private List<MenuDetailModel> variantList;
        private DataTable table;
        private MenuService menuService;
        private readonly IngredientServices ingredientServices;
        public event EventHandler menuUpdate;
        public NewMenu(MenuService menuService, IngredientServices ingredientServices)
        {
            InitializeComponent();
            this.menuService = menuService;
            this.ingredientServices = ingredientServices;
            variantList = new List<MenuDetailModel>();
            initTable();
        }
        private void initTable()
        {
            table = new DataTable();
            table.Columns.Add("Flavor", typeof(string));
            table.Columns.Add("Size", typeof(string));
            table.Columns.Add("Prep-Time", typeof(TimeSpan));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Ingredients", typeof(string));
            table.Columns.Add("Action", typeof(Image));

            dataGrid.DataSource = table;
            dataGrid.AutoGenerateColumns = true;

            DataGridViewImageColumn del = new DataGridViewImageColumn();
            del.Image = Resources.exit2;
            del.Name = "Action";
            del.HeaderText = "Action";
            del.Width = 20;
            del.ImageLayout = DataGridViewImageCellLayout.Normal;
            del.DataPropertyName = "Action";


            dataGrid.Columns[5].Width = 50;

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
        private void confirmNewMenu(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(menuName.Text.Trim()) ||
                    string.IsNullOrEmpty(menuDescription.Text.Trim()) ||
                    string.IsNullOrEmpty(cmbCat.Text.Trim()))
                {
                    throw new InvalidInput("Please fill all * fields.");
                }

                if (variantList.Count <= 0)
                {
                    throw new InvalidInput("No Selected Ingredient");
                }

                string name = menuName.Text.Trim();

                if (menuService.isMenuNameExist(name))
                {
                    throw new InvalidInput("Menu Name already exists, try different.");
                }

                string desc = menuDescription.Text.Trim();
                string cat = cmbCat.Text.Trim();
                if (pictureBox.Image == null) pictureBox.Image = Resources.placeholder;
                byte[] image = ImageHelper.GetImageFromFile(pictureBox.Image);


                MenuDetailModel md = MenuDetailModel.Builder()
                    .WithMenuName(name)
                    .WithVariant(variantList)
                    .WithMenuDescription(desc)
                    .WithMenuImageByte(image)
                    .WithCategoryName(cat)
                    .Build();

                bool success = menuService.saveMenu(md, "regular");
                if (success)
                {
                    menuUpdate.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("New menu created successfully.", "Menu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Internal Server Error.");
            }
        }
        private void VariantPopupButton(object sender, System.EventArgs e)
        {

            VariantMenuPopup p = new VariantMenuPopup(variantList, ingredientServices);
            DialogResult rs = p.ShowDialog(this);

            if (rs == DialogResult.OK)
            {
                variantList = p.getVariants();
                table.Rows.Clear();
                if (variantList.Count > 0)
                {
                    variantButton.Text = "Add Another Variant";

                    foreach (var x in variantList)
                    {
                        string s = string.Join(", ", x.MenuIngredients.Select(xx => xx.IngredientName));
                        table.Rows.Add(x.FlavorName, x.SizeName, x.EstimatedTime, x.MenuPrice, s);
                    }
                }
                else
                {
                    variantButton.Text = "Add Variant";
                }
            }
        }
        private void exit(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }
        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGrid.Columns[e.ColumnIndex] is DataGridViewImageColumn)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete this variant / details?", "Confirm", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dataGrid.Rows.RemoveAt(e.RowIndex);
                    variantList.RemoveAt(e.RowIndex);
                    variantButton.Text = variantList.Count > 0 ? "Add Another Variant" : "Add Variant";
                }
            }
        }
    }
}
