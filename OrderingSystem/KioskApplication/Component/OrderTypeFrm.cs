using System;
using System.Windows.Forms;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication.Component
{
    public partial class OrderTypeFrm : Form
    {
        public event EventHandler<OrderModel.OrderType> OrderTypeChanged;
        public OrderTypeFrm()
        {
            InitializeComponent();
        }

        private void takeOut(object sender, EventArgs e)
        {
            OrderTypeChanged?.Invoke(this, OrderModel.OrderType.TAKE_OUT);
            DialogResult = DialogResult.OK;
        }

        private void dineIn(object sender, EventArgs e)
        {
            OrderTypeChanged?.Invoke(this, OrderModel.OrderType.DINE_IN);
            DialogResult = DialogResult.OK;
        }

        private void OrderTypeFrm_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }
    }
}
