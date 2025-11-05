using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Forms.Staffs
{
    public partial class StaffInformation : Form
    {
        private bool isEditMode = false;
        private readonly StaffServices staffServices;
        private StaffModel viewingStaff;
        public event EventHandler staffUpdated;
        public StaffInformation(StaffServices staffServices)
        {
            InitializeComponent();
            this.staffServices = staffServices;
            if (SessionStaffData.Role.ToLower() == "cashier")
                fb.Visible = false;

        }
        private void allowType(bool isEditMode)
        {
            foreach (var xb in Controls)
            {
                if (xb is Guna2TextBox tb)
                {
                    if (tb.Name == "hired") continue;

                    tb.ReadOnly = !isEditMode;
                    tb.Enabled = isEditMode;
                    tb.BorderThickness = isEditMode ? 1 : 0;
                    tb.FillColor = ColorTranslator.FromHtml("#EFF6FF");


                    if (isEditMode && (tb.Name == "firstName" || tb.Name == "lastName"))
                    {
                        ll.Visible = true;
                        fn.Visible = true;
                        firstName.Visible = true;
                        lastName.Visible = true;
                        fName.Visible = false;
                        ffn.Visible = false;
                        cRolePanel.Visible = false;
                        cRole.Visible = true;
                    }
                    else if (!isEditMode && (tb.Name == "firstName" || tb.Name == "lastName"))
                    {
                        ll.Visible = false;
                        fn.Visible = false;
                        firstName.Visible = false;
                        lastName.Visible = false;
                        fName.Visible = true;
                        ffn.Visible = true;
                        cRolePanel.Visible = true;
                        cRole.Visible = false;
                    }
                }
            }
            cRole.SelectedItem = viewingStaff.Role.Substring(0, 1).ToUpper() + viewingStaff.Role.Substring(1).ToLower();

        }
        public void displayStaff(StaffModel viewingStaff)
        {
            this.viewingStaff = viewingStaff;
            role.Text = viewingStaff.Role;
            fName.Text = viewingStaff.FirstName.Substring(0, 1).ToUpper() + viewingStaff.FirstName.Substring(1).ToLower() + "  " + viewingStaff.LastName.Substring(0, 1).ToUpper() + viewingStaff.LastName.Substring(1).ToLower();
            image.Image = viewingStaff.Image;
            phone.Text = string.IsNullOrWhiteSpace(viewingStaff.PhoneNumber) ? "N/A" : viewingStaff.PhoneNumber;
            hired.Text = viewingStaff.HiredDate.ToString("yyyy, MMMM dd");
            username.Text = viewingStaff.Username;
            password.Text = "passwordplaceholder";
            firstName.Text = viewingStaff.FirstName;
            lastName.Text = viewingStaff.LastName;
            cRole.SelectedItem = viewingStaff.Role.Substring(0, 1).ToUpper() + viewingStaff.Role.Substring(1).ToLower();


            if (viewingStaff.Status.ToLower() == "inactive")
            {
                fb.Text = "Fired";
                fb.Click -= b1_Click;
                fb.Enabled = false;
            }
            if (viewingStaff.Image != null) image.BorderStyle = BorderStyle.None;

            allowType(isEditMode);
        }
        private void guna2PictureBox1_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        private void StaffInformation_Load(object sender, System.EventArgs e)
        {
            cRole.Items.Add("Cashier");
            cRole.Items.Add("Manager");

            cRole.SelectedIndex = 0;
        }
        private void b1_Click(object sender, System.EventArgs e)
        {
            if (b1.Text == "Add Staff") addStaffLogic();
            if (b1.Text == "Update" || b1.Text == "Save") updateStaffLogic();
        }
        private void updateStaffLogic()
        {
            try
            {
                isEditMode = !isEditMode;
                if (isEditMode) b1.Text = "Save";
                else b1.Text = "Update";
                allowType(isEditMode);

                if (!isEditMode)
                {
                    StaffModel ss = StaffModel.Builder()
                         .WithStaffId(viewingStaff.StaffId)
                         .WithRole(cRole.Text)
                         .WithFirstName(firstName.Text)
                         .WithLastName(lastName.Text)
                         .WithPhoneNumber(phone.Text == "N/A" ? "" : phone.Text)
                         .WithUsername(username.Text)
                         .WithPassword(password.Text == "passwordplaceholder" ? "" : password.Text)
                         .WithImage(image.Image)
                         .Build();
                    bool success = staffServices.isInputValidated(ss);
                    if (!success) return;
                    DialogResult rs = MessageBox.Show("Are sure you want to update?", "Information Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rs == DialogResult.Yes)
                    {
                        bool upSuc = staffServices.updateStaff(ss);
                        if (upSuc)
                        {
                            fName.Text = firstName.Text.Substring(0, 1).ToUpper() + firstName.Text.Substring(1).ToLower() + "  " + lastName.Text.Substring(0, 1).ToUpper() + lastName.Text.Substring(1).ToLower();
                            MessageBox.Show("Successfully updated", "Information Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            staffUpdated.Invoke(this, EventArgs.Empty);
                        }

                    }
                }
            }
            catch (InvalidInput ex)
            {
                MessageBox.Show(ex.Message, "Information Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void addStaffLogic()
        {
            try
            {
                foreach (var c in this.Controls)
                {
                    if (c is Guna2TextBox tb)
                    {
                        if (tb.Visible && string.IsNullOrEmpty(tb.Text) && tb.Name != "phone")
                        {
                            throw new InvalidInput("Fill * the fields.");
                        }

                        if (cRole.SelectedIndex == -1)
                        {
                            throw new InvalidInput("Fill * the fields.");
                        }
                    }
                }
                StaffModel ss = StaffModel.Builder()
                         .WithRole(cRole.Text)
                         .WithFirstName(firstName.Text)
                         .WithLastName(lastName.Text)
                         .WithPhoneNumber(phone.Text)
                         .WithUsername(username.Text)
                         .WithPassword(password.Text)
                         .WithImage(image.Image)
                         .WithHiredDate(dt.Value)
                         .Build();
                bool succ = staffServices.addStaff(ss);
                if (succ)
                {
                    MessageBox.Show("Successfully Added", "New Staff", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    staffUpdated.Invoke(this, EventArgs.Empty);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (InvalidAction ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidInput ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void fireButton(object sender, System.EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Are sure you want to fire this staff?", "Information Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                bool upSuc = staffServices.fireStaff(viewingStaff.StaffId);
                if (upSuc)
                {
                    fb.Text = "Fired";
                    fb.Click -= b1_Click;
                    staffUpdated.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Successfully fired.", "Information Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void image_Click(object sender, EventArgs e)
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
                image.Image = viewingStaff.Image;
                if (viewingStaff.Image != null) image.BorderStyle = BorderStyle.None;
            }
        }
    }
}
