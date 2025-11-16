using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Components;
using OrderingSystem.CashierApp.Forms.Menu;
using OrderingSystem.Model;
using OrderingSystem.Repo.CashierMenuRepository;
using OrderingSystem.Repository.CategoryRepository;
using OrderingSystem.Repository.Ingredients;
using OrderingSystem.Services;
//using OrderingSystem.Repo.CashierMenuRepository;

namespace OrderingSystem.CashierApp.Forms
{
    public partial class MenuFrm : Form
    {

        private readonly IngredientServices ingredientServices;
        private readonly MenuService menuService;

        public MenuFrm()
        {
            InitializeComponent();
            menuService = new MenuService(new MenuRepository());
            ingredientServices = new IngredientServices(new IngredientRepository());

            displayMenu();
        }
        private void displayMenu()
        {
            try
            {
                flowMenu.Controls.Clear();
                List<MenuDetailModel> list = menuService.getMenus();
                foreach (var i in list)
                {
                    MenuCard m = new MenuCard(i);
                    m.Margin = new Padding(10, 10, 10, 10);
                    m.Tag = i;
                    flowMenu.Controls.Add(m);
                    hover(m, i);
                }
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message, "Menu Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Internal Server Error.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void hover(Control c, MenuDetailModel i)
        {
            c.Click += (s, e) =>
            {
                MenuInformation mi = new MenuInformation(i, menuService, new CategoryServices(new CategoryRepository()), ingredientServices);
                mi.menuUpdated += (ss, ee) => displayMenu();
                DialogResult rs = mi.ShowDialog(this);
                if (rs == DialogResult.OK)
                {
                    mi.Hide();
                }
            };

            foreach (Control cv in c.Controls)
            {
                hover(cv, i);
            }
        }
        public void showBundle()
        {
            MenuBundleFrm f = new MenuBundleFrm(menuService, ingredientServices);
            f.menuUpdate += (s, e) => displayMenu();
            DialogResult rs = f.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                f.Hide();
            }
        }
        public void showNewMenu()
        {
            NewMenu f = new NewMenu(menuService, ingredientServices);
            f.menuUpdate += (s, e) => displayMenu();
            DialogResult rs = f.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                f.Hide();
            }
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            debouncing.Stop();
            debouncing.Start();
        }

        private void debouncing_Tick(object sender, EventArgs e)
        {
            debouncing.Stop();
            string txtx = txt.Text.Trim().ToLower();

            foreach (Control control in flowMenu.Controls)
            {
                if (control is MenuCard card)
                {
                    MenuDetailModel cz = (MenuDetailModel)card?.Tag;
                    card.Visible = string.IsNullOrEmpty(txtx) ? true : cz.MenuName.ToLower().StartsWith(txtx);
                }
            }
        }
    }
}
