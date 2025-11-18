using System.Windows.Forms;
using OrderingSystem.CashierApp.Layout;

namespace OrderingSystem.KioskApplication.Forms
{
    public partial class Dashboard : Form
    {
        private string fullText = "\"Your go-to spot for quick meals, great flavors, and an atmosphere worth coming back to.”-";
        private int charIndex = 0;
        public Dashboard()
        {
            InitializeComponent();
            typingTimer.Start();
        }

        private void dinein(object sender, System.EventArgs e)
        {
            KioskLayout k = new KioskLayout();
            k.setType("Dine-in");
            loadForm(k);
        }

        private void takeout(object sender, System.EventArgs e)
        {
            KioskLayout k = new KioskLayout();
            k.setType("Take-out");
            loadForm(k);
        }

        public void loadForm(Form f)
        {
            if (mm.Controls.Count > 0) mm.Controls.Clear();

            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mm.Controls.Add(f);
            f.Show();
        }
        public void reset()
        {
            if (mm.Controls.Count > 0) mm.Controls.Clear();
            mm.Controls.Add(bb);
        }



        private void typingTimer_Tick(object sender, System.EventArgs e)
        {
            if (charIndex < fullText.Length)
            {
                Typing.Text += fullText[charIndex];
                charIndex++;
                Typing.Left = (mm.Width - Typing.Width) / 2;

            }
            else
            {
                typingTimer.Stop();
            }
        }

        private void mm_SizeChanged(object sender, System.EventArgs e)
        {
            Typing.Left = (mm.Width - Typing.Width) / 2;

        }

        private void label1_Click(object sender, System.EventArgs e)
        {
            Hide();
            LoginLayout k = new LoginLayout();
            k.Show();
        }

        private void guna2Button1_Click(object sender, System.EventArgs e)
        {
            KioskLayout k = new KioskLayout();
            k.setType("");
            loadForm(k);
        }
    }
}
