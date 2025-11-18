using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Components;
using OrderingSystem.CashierApp.Forms.FactoryForm;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Forms.Staffs
{
    public partial class StaffFrm : Form
    {
        private readonly StaffServices staffServices;
        private readonly IForms iForms;
        public StaffFrm(StaffServices staffServices)
        {
            InitializeComponent();
            iForms = new FormFactory();
            this.staffServices = staffServices;

            refreshList(null, EventArgs.Empty);
            if (SessionStaffData.Role == StaffModel.StaffRole.Cashier)
                b1.Visible = false;

        }

        private void refreshList(object sender, EventArgs e)
        {
            try
            {
                flowPanel.Controls.Clear();
                List<StaffModel> staffList = staffServices.getStaffs();
                foreach (var st in staffList)
                {
                    StaffCard s = new StaffCard(st, iForms, staffServices);
                    s.staffUpdated += refreshList;
                    s.Tag = st;
                    flowPanel.Controls.Add(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Staff", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            StaffInformation s = new StaffInformation(staffServices);
            s.staffUpdated += (ss, ee) => refreshList(this, EventArgs.Empty);
            DialogResult rs = iForms.selectForm(s, "add-staff").ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                s.Hide();
            }
        }

        private void debouncing_Tick(object sender, EventArgs e)
        {
            debouncing.Stop();
            string txt = search.Text.Trim().ToLower();

            foreach (var i in flowPanel.Controls)
            {
                if (i is StaffCard sc)
                {
                    StaffModel cz = (StaffModel)sc?.Tag;
                    sc.Visible = string.IsNullOrEmpty(txt) || cz.FirstName.ToLower().StartsWith(txt) || cz.LastName.ToLower().StartsWith(txt) || cz.StaffId.ToString().StartsWith(txt);
                }
            }
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            debouncing.Stop();
            debouncing.Start();
        }
    }
}
