using System;
using System.Drawing;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Forms.Staffs;

namespace OrderingSystem.CashierApp.Forms.FactoryForm
{
    public class FormFactory : IForms
    {

        // POPUPFORM
        public static Size defaultSize = new Size(425, 484);
        public static Size wideSize = new Size(820, 484);
        public Form selectForm(Form fx, string type)
        {
            if (fx is PopupForm f1 && type.ToLower() == "coupon")
            {
                f1.dt2.MinDate = DateTime.Now;
                f1.dt2.Visible = true;
                f1.t2.Visible = false;
                f1.l1.Text = "Rate %";
                f1.l2.Text = "Expiry Date";
                f1.l3.Text = "Number of Times";
                f1.l4.Text = "Description";
                f1.b1.Text = "Submit";
                f1.title.Text = "Generate Coupon";
                f1.dt2.MinDate = DateTime.Now;
            }
            else if (fx is PopupForm f2 && type.ToLower() == "add-ingredients")
            {
                f2.l1.Text = "Ingredient Name";
                f2.l2.Text = "Initial-Stock";
                f2.l3.Text = "Unit";
                f2.l4.Text = "Expiry-Date";
                f2.t3.Visible = false;
                f2.t4.Visible = false;
                f2.dt4.Visible = true;
                f2.dt4.MinDate = DateTime.Now;
                f2.c3.Visible = true;
                f2.b1.Text = "Submit";
                f2.title.Text = "Add Ingredients";
                f2.Size = wideSize;
                f2.title.Size = new Size(wideSize.Width, f2.title.Height);
                f2.x.Location = new Point(wideSize.Width - (f2.x.Width + 4), 4);
                f2.l6.Visible = true;
                f2.l5.Visible = true;
                f2.c5.Visible = true;
                f2.t6.Visible = true;
                f2.c5.Tag = "Optional";
                centerButton(f2);
            }
            else if (fx is PopupForm f3 && type.ToLower() == "restock-ingredients")
            {
                f3.t1.Visible = false;
                f3.t3.Visible = false;
                f3.t4.Visible = false;
                f3.l1.Text = "Ingredient Name";
                f3.l2.Text = "Quantity";
                f3.l4.Text = "Reason";
                f3.l3.Text = "Expiry-Date";
                f3.c1.Visible = true;
                f3.dt3.Visible = true;
                f3.dt3.MinDate = DateTime.Now.AddDays(1);
                f3.c4.Visible = true;
                f3.b1.Text = "Submit";
                f3.title.Text = "Restock Ingredients";
                f3.Size = wideSize;
                f3.title.Size = new Size(wideSize.Width, f3.title.Height);
                f3.x.Location = new Point(wideSize.Width - (f3.x.Width + 4), 4);
                f3.l6.Visible = true;
                f3.l5.Visible = true;
                f3.c5.Visible = true;
                f3.t6.Visible = true;
                f3.c5.Tag = "Optional";
                centerButton(f3);
            }
            else if (fx is PopupForm f4 && type.ToLower() == "deduct-ingredients")
            {
                f4.t2.Enabled = false;
                f4.t4.Visible = false;
                f4.c1.Visible = true;
                f4.t1.Visible = false;
                f4.c4.Visible = true;
                f4.l1.Text = "Ingredient Stock Id";
                f4.l2.Text = "Ingredient Name";
                f4.l3.Text = "Quantity to remove";
                f4.l4.Text = "Reason";
                f4.b1.Text = "Submit";
                f4.title.Text = "Deduct Ingredients";
                f4.t6.Tag = "Optional";
            }
            else if (fx is PopupForm f5 && type.ToLower() == "update-ingredients")
            {
                f5.title.Text = "Update Ingredient";
                f5.l1.Text = "Ingredient Name";
                f5.l2.Text = "Unit";
                f5.l3.Visible = false;
                f5.l4.Visible = false;
                f5.c2.Visible = true;
                f5.t2.Visible = false;
                f5.t3.Visible = false;
                f5.t4.Visible = false;
            }
            else if (fx is StaffInformation f6 && type.ToLower() == "add-staff")
            {
                f6.fb.Visible = false;
                f6.b1.Text = "Add Staff";
                f6.cRolePanel.Visible = false;
                f6.cRole.Visible = true;
                f6.ll.Visible = true;
                f6.fn.Visible = true;
                f6.firstName.Visible = true;
                f6.lastName.Visible = true;
                f6.fName.Visible = false;
                f6.ffn.Visible = false;
                f6.dt.Visible = true;
                f6.rr.Visible = true;
                f6.hired.Visible = false;
            }
            else if (fx is StaffInformation f7 && type.ToLower() == "view-staff")
            {
                f7.b1.Text = "Update";
            }
            else if (fx is TableLayout f8 && type.ToLower() == "category")
            {
                f8.cb.Visible = false;
                f8.title.Text = "Category";
            }
            else if (fx is TableLayout f9 && type.ToLower() == "view-coupon")
            {
                f9.b1.Text = "Add Coupon";
                f9.cb.Text = "View Used Coupon";
                f9.title.Text = "Coupon";
                f9.search.Visible = false;
            }
            else if (fx is PopupForm f10 && type.ToLower() == "add-discount")
            {
                f10.title.Text = "Add Discount";
                f10.l1.Text = "Discount Rate ( % )";
                f10.l2.Text = "Discount Available until";
                f10.t2.Visible = false;
                f10.t3.Visible = false;
                f10.t4.Visible = false;
                f10.l3.Visible = false;
                f10.l4.Visible = false;
                f10.dt2.Visible = true;
                f10.dt2.MinDate = DateTime.Now;
            }

            return fx;
        }

        public void centerButton(PopupForm f)
        {
            f.b1.Location = new Point((f.Width - f.b1.Width) / 2, f.b1.Location.Y);
        }
    }
}
