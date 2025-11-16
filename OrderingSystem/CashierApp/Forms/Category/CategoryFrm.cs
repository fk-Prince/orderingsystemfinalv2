using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Components;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Forms.Category
{
    public partial class CategoryFrm : Form
    {

        private CategoryServices categoryServices;

        public CategoryFrm(CategoryServices categoryServices)
        {
            InitializeComponent();
            this.categoryServices = categoryServices;
            b1.Visible = SessionStaffData.Role != StaffModel.StaffRole.Cashier;
            loadCategory();
        }

        private void loadCategory()
        {
            try
            {
                flow.Controls.Clear();
                List<CategoryModel> categories = categoryServices.getCategories();
                foreach (var i in categories)
                {
                    CategoryCard c = new CategoryCard(categoryServices, i);
                    c.isAuthorized();
                    c.Tag = i;
                    c.SuccessAction += (s, e) => loadCategory();
                    c.Margin = new Padding(5);
                    flow.Controls.Add(c);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addNewCategory(object sender, EventArgs eb)
        {
            CategoryPopup cat = new CategoryPopup("Add New Category");
            cat.ButtonClicked += (s, e) =>
            {
                CategoryPopup cc = s as CategoryPopup;
                try
                {
                    bool succ = categoryServices.createCategory(cc.name.Text, cc.image.Image);
                    if (succ)
                    {
                        MessageBox.Show("Category Successfully Created", "Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cat.DialogResult = DialogResult.OK;
                        loadCategory();
                    }
                    else MessageBox.Show("Category Failed to Create", "Category", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex) when (ex is InvalidInput || ex is DuplicateException)
                {
                    MessageBox.Show(ex.Message, "Category Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Internal Server Error", "Category", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            DialogResult rs = cat.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                cat.Hide();
            }
        }
        private void search_TextChanged(object sender, EventArgs e)
        {
            debouncing.Stop();
            debouncing.Start();
        }
        private void debouncing_Tick(object sender, EventArgs e)
        {
            debouncing.Stop();
            string txt = search.Text.Trim().ToLower();

            foreach (Control control in flow.Controls)
            {
                if (control is CategoryCard card)
                {
                    CategoryModel cz = (CategoryModel)card?.Tag;
                    card.Visible = string.IsNullOrEmpty(txt) ? true : cz.CategoryName.ToLower().StartsWith(txt);
                }
            }
        }
    }
}
