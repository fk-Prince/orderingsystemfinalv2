using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.Forms.Category;
using OrderingSystem.CashierApp.Forms.Coupon;
using OrderingSystem.CashierApp.Forms.FactoryForm;
using OrderingSystem.CashierApp.Forms.Ingredient;
using OrderingSystem.CashierApp.Forms.Menu;
using OrderingSystem.CashierApp.Forms.Staffs;
using OrderingSystem.CashierApp.Layout;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Model;
using OrderingSystem.Repository.CategoryRepository;
using OrderingSystem.Repository.Reports;
using OrderingSystem.Services;
using Dashboard = OrderingSystem.CashierApp.Layout.Dashboard;

namespace OrderingSystem.CashierApp.Forms
{
    public partial class CashierLayout : Form
    {
        private IngredientPanel ingredientPanel;
        private IngredientFrm instance;
        private MenuFrm menuIntance;
        private Guna2Button lastClicked;

        private StaffServices staffService;
        private readonly IForms iForms;
        public CashierLayout(StaffServices staffService)
        {
            InitializeComponent();
            dts.Start();
            iForms = new FormFactory();
            this.staffService = staffService;
            ingredientPanel = new IngredientPanel(iForms);

            addB();

            lastClicked = b1;
            lastClicked.ForeColor = Color.FromArgb(34, 34, 34);
            lastClicked.BorderThickness = 1;
            lastClicked.BorderColor = Color.Gray;
            lastClicked.FillColor = Color.LightGray;
            lastClicked.BorderRadius = 5;

            drop.Items.Add("Role - " + SessionStaffData.Role);
            drop.Items.Add("Switch User");
            drop.Items.Add("Sign-out");

            if (SessionStaffData.Role == StaffModel.StaffRole.Cashier) b7.Visible = false;


            loadForm(new Dashboard());
            displayStaffDetails();
        }

        private void displayStaffDetails()
        {
            if (SessionStaffData.Role == StaffModel.StaffRole.Cashier)
            {
                nm.Visible = false;
                ri.Visible = false;
                ai.Visible = false;
                ri.Visible = false;
                md.Visible = false;
                b3.Visible = false;
                b4.Visible = false;
                b5.Visible = false;
                b6.Visible = false;
                b7.Visible = false;
                b8.Visible = false;
                b9.Visible = false;
                b10.Visible = false;
            }
            image.Image = SessionStaffData.Image;
            name.Text = SessionStaffData.getFullName();
            role.Text = SessionStaffData.Role.ToString();
        }
        public void loadForm(Form f)
        {
            if (mm.Tag is Form ff && ff.Name == f.Name) return;
            if (mm.Controls.Count > 0) mm.Controls.Clear();
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mm.Controls.Add(f);
            mm.Tag = f;
            f.Show();
        }
        private void showSubPanel(Panel panel)
        {
            if (panel.Visible == false && SessionStaffData.Role == StaffModel.StaffRole.Manager)
            {
                hideSubPanel();
                panel.Visible = true;
            }
            else
            {
                panel.Visible = false;
            }
        }
        private void hideSubPanel()
        {
            if (b4.Visible == true && SessionStaffData.Role == StaffModel.StaffRole.Manager) b4.Visible = false;
            if (b6.Visible == true && SessionStaffData.Role == StaffModel.StaffRole.Manager) b6.Visible = false;
        }
        private void viewOrder(object sender, System.EventArgs e)
        {
            loadForm(new OrderFrm());
            hideSubPanel();
        }
        private void showMenu(object sender, System.EventArgs e)
        {
            loadForm(menuIntance = new MenuFrm());
            showSubPanel(b4);
        }
        private void newMenu(object sender, System.EventArgs e)
        {
            if (menuIntance == null) return;
            menuIntance.showNewMenu();

        }


        private void viewIngredient(object sender, System.EventArgs e)
        {
            showSubPanel(b6);
            loadForm(instance = new IngredientFrm());
        }
        private void viewRestockIngredient(object sender, System.EventArgs e)
        {
            ingredientPanel.IngredientUpdated += (ss, ee) => instance.updateTable();
            ingredientPanel.PopupRestockIngredient(this);
        }
        private void viewAddIngredients(object sender, System.EventArgs e)
        {
            ingredientPanel.IngredientUpdated += (ss, ee) => instance.updateTable();
            ingredientPanel.PopupAddIngredient(this);
        }
        private void viewDeductIngredient(object sender, System.EventArgs e)
        {
            ingredientPanel.IngredientUpdated += (ss, ee) => instance.updateTable();
            ingredientPanel.PopupDeductIngredient(this);
        }

        private void primaryButtonClickedSide(object sender, MouseEventArgs e)
        {
            Guna2Button b = sender as Guna2Button;
            if (lastClicked != b)
            {
                b.ForeColor = Color.FromArgb(34, 34, 34);
                b.BorderThickness = 1;
                b.BorderColor = Color.Gray;
                b.FillColor = Color.LightGray;
                b.BorderRadius = 5;
                lastClicked.ForeColor = Color.Gray;
                lastClicked.BorderRadius = 0;
                lastClicked.BorderThickness = 0;
                lastClicked.FillColor = Color.White;
                lastClicked = b;
            }
        }
        private void viewInventory(object sender, System.EventArgs e)
        {
            hideSubPanel();
            loadForm(new ReportsFrm(new ReportServices(new ReportRepository())));
        }
        private void viewStaff(object sender, System.EventArgs e)
        {
            hideSubPanel();
            loadForm(new StaffFrm(staffService));
        }
        private void signoutUser(object sender, System.EventArgs e)
        {
            Hide();
            LoginLayout ll = new LoginLayout();
            ll.Show();
        }
        private void switchUser(object sender, System.EventArgs e)
        {
            LoginLayout ll = new LoginLayout();
            ll.isPopup = true;
            DialogResult rs = ll.ShowDialog(this);
            if (rs == DialogResult.OK)
            {

                if (ll.isLogin)
                {
                    Hide();
                }
                ll.Hide();
            }
        }
        private void viewCoupon(object sender, System.EventArgs e)
        {
            CouponFrm c = new CouponFrm(iForms, new CouponServices());
            loadForm(c);
        }
        private void guna2Button2_Click(object sender, System.EventArgs e)
        {
            CategoryFrm c = new CategoryFrm(new CategoryServices(new CategoryRepository()));
            loadForm(c);
        }

        private void dts_Tick(object sender, System.EventArgs e)
        {
            time.Text = DateTime.Now.ToString("hh:mm:ss tt");
            date.Text = DateTime.Now.ToString("yyyy, MMMM dd - dddd");
        }

        private void menuDiscount(object sender, EventArgs e)
        {
            MenuDiscountPanel md = new MenuDiscountPanel(new FormFactory());
            md.AddDiscountPopup(this);
        }



        bool d = false;
        private void image_Click(object sender, EventArgs e)
        {
            d = !d;
            drop.DroppedDown = d;
        }

        private void drop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drop.SelectedIndex <= -1) return;

            if (drop.SelectedIndex == 1)
                switchUser(this, e);
            else if (drop.SelectedIndex == 2)
                signoutUser(this, e);
        }

        public void addB()
        {
            Control[] buttons = { b1, b2, b3, b4, b5, b6, b7, b8, b9, b10 };

            for (int i = buttons.Length - 1; i >= 0; i--)
            {
                Control btn = buttons[i];
                btn.Dock = DockStyle.Top;
                flows.Controls.Add(btn);
            }
        }

        private void dashboard(object sender, EventArgs e)
        {
            loadForm(new Dashboard());
            hideSubPanel();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Hide();
            LoginLayout ll = new LoginLayout();
            ll.Show();
        }
    }
}
