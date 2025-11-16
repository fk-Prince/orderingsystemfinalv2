using System;
using System.Windows.Forms;
using OrderingSystem.CashierApp.Forms.FactoryForm;
using OrderingSystem.Exceptions;
using OrderingSystem.Repository.Discount;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Forms.Menu
{
    public class MenuDiscountPanel
    {

        private readonly IForms iForms;
        public MenuDiscountPanel(IForms iForms)
        {
            this.iForms = iForms;
        }

        public void AddDiscountPopup(Form f)
        {
            PopupForm p = new PopupForm();
            p.buttonClicked += (ss, ee) =>
            {
                try
                {
                    bool suc = new DiscountServices(new DiscountRepository()).saveDiscount(p.t1.Text, p.dt2.Value);
                    if (suc)
                    {
                        MessageBox.Show("Successfully added", "Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        p.Hide();
                    }
                    else
                        MessageBox.Show("Failed to create discount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidInput ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Internal Server Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            DialogResult rs = iForms.selectForm(p, "add-discount").ShowDialog(f);
            if (rs == DialogResult.OK)
            {
                p.Hide();
            }
        }
    }
}
