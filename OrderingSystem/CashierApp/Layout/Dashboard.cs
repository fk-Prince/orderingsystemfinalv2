using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.Repository.Reports;
using OrderingSystem.Services;

namespace OrderingSystem.CashierApp.Layout
{
    public partial class Dashboard : Form
    {
        private readonly ReportServices ir;
        public Dashboard()
        {
            InitializeComponent();
            ir = new ReportServices(new ReportRepository());

        }

        private void Dashboard_Load(object sender, System.EventArgs e)
        {
            fetchData();
        }
        public void fetchData()
        {

            try
            {
                Tuple<string, string> trans = ir.getTransactionByDate(date.Value);
                displayPercentage(transactionT, transactionP, transactionI, trans);

                Tuple<string, string> totalOrder = ir.getTotalOrderByType(date.Value, "");
                display(orderT, orderP, orderI, totalOrder);

                Tuple<string, string> totalComplate = ir.getTotalOrderByType(date.Value, "paid");
                display(comT, comP, comI, totalComplate);

                Tuple<string, string> totalCancelled = ir.getTotalOrderByType(date.Value, "cancelled");
                display(coT, coP, coI, totalCancelled);

                Tuple<string, string> totalPending = ir.getTotalOrderByType(date.Value, "pending");
                display(poT, poP, poI, totalPending);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displayPercentage(Label total, Label percentLabel, Guna2PictureBox picture, Tuple<string, string> trans)
        {
            if (trans == null) return;
            total.Text = trans.Item1;

            if (!double.TryParse(trans.Item2, out double percent))
            {
                percentLabel.Text = "0%";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
                return;
            }

            if (percent > 0)
            {
                percentLabel.Text = $"+{percent:N0}%";
                percentLabel.ForeColor = Color.Green;
                picture.Image = Properties.Resources.increase;
            }
            else if (percent < 0)
            {
                percentLabel.Text = $"{percent:N0}%";
                percentLabel.ForeColor = Color.Red;
                picture.Image = Properties.Resources.decrease;
            }
            else
            {
                percentLabel.Text = "0%";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
            }
        }
        private void display(Label total, Label percentLabel, Guna2PictureBox picture, Tuple<string, string> trans)
        {
            if (trans == null) return;
            total.Text = trans.Item1;

            if (!double.TryParse(trans.Item2, out double percent))
            {
                percentLabel.Text = "0%";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
                return;
            }

            if (percent > 0)
            {
                percentLabel.Text = $"+{percent:N0}";
                percentLabel.ForeColor = Color.Green;
                picture.Image = Properties.Resources.increase;
            }
            else if (percent < 0)
            {
                percentLabel.Text = $"{percent:N0}";
                percentLabel.ForeColor = Color.Red;
                picture.Image = Properties.Resources.decrease;
            }
            else
            {
                percentLabel.Text = "0%";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
            }
        }

        private void date_ValueChanged(object sender, EventArgs e)
        {
            fetchData();
        }
    }
}
