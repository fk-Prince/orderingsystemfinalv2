using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.Forms;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Repository.Staff;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Layout
{
    public partial class LoginLayout : Form
    {
        private bool isShowing;
        public bool isPopup = false;
        public bool isLogin = false;
        public LoginLayout()
        {
            InitializeComponent();
        }



        private void exit(object sender, EventArgs e)
        {
            if (!isPopup)
            {
                Environment.Exit(0);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Text) || string.IsNullOrWhiteSpace(pass.Text))
                {
                    throw new IncorrectCredentials("Incorrect Username or Password.");
                }
                StaffServices staffService = new StaffServices(new StaffRepository());
                StaffModel loginStaff = staffService.loginStaff(user.Text.Trim(), pass.Text.Trim());
                if (loginStaff != null)
                {
                    SessionStaffData.setSessionData(loginStaff);
                    MessageBox.Show("Successfully Login", "Authorized", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    isLogin = true;
                    CashierLayout ca = new CashierLayout(staffService);
                    Hide();
                    ca.Show();
                }
                else
                    throw new IncorrectCredentials("Incorrect Username or Password.");
            }
            catch (IncorrectCredentials ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Hide();
            KioskApplication.Forms.Dashboard d = new KioskApplication.Forms.Dashboard();
            d.Show();
        }
        private void pass_MouseDown(object sender, MouseEventArgs e)
        {
            var txt = sender as Guna2TextBox;
            Rectangle iconBounds = new Rectangle(pass.Width - 25 - pass.Padding.Right, (pass.Height - 20) / 2, 20, 20);

            if (iconBounds.Contains(e.Location))
            {
                if (!isShowing) pass.IconRight = Properties.Resources.eyeclosed;
                else pass.IconRight = Properties.Resources.eye;
                pass.UseSystemPasswordChar = !pass.UseSystemPasswordChar;
                isShowing = !isShowing;
            }
        }
    }
}
