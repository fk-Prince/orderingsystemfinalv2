using System;
using System.Data;
using System.Windows.Forms;

namespace OrderingSystem.CashierApp.Forms.FactoryForm
{
    public partial class TableLayout : Form
    {
        public DataView view;
        public event EventHandler<bool> FilterChanged;
        public event EventHandler<string> SearchChanged;
        public event EventHandler ButtonClicked;

        public TableLayout()
        {
            InitializeComponent();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            ButtonClicked.Invoke(this, EventArgs.Empty);
        }
        private void search_TextChanged(object sender, EventArgs e)
        {
            SearchChanged?.Invoke(this, search.Text);
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            FilterChanged?.Invoke(this, cb.Checked);
        }
    }
}
