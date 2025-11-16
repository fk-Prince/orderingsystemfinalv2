using System.Windows.Forms;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.Forms.Menu
{
    public partial class MenuCard : UserControl
    {

        public MenuCard(MenuDetailModel menu)
        {
            InitializeComponent();
            cardLayout();

            menuName.Text = menu.MenuName;
            menuDescription.Text = menu.MenuDescription;
            image.Image = menu.MenuImage;
        }

        private void cardLayout()
        {
            ////BorderRadius = 10;
            ////BorderThickness = 1;
            ////BackColor = Color.Transparent;
            ////FillColor = ColorTranslator.FromHtml("#DBEAFE");
            ////BorderColor = ColorTranslator.FromHtml("#DBEAFE");

            ////ShadowDecoration.Enabled = true;
            ////ShadowDecoration.BorderRadius = BorderRadius;
            ////ShadowDecoration.Color = Color.FromArgb(60, 60, 60, 60);
            ////ShadowDecoration.Depth = 10;
            ////ShadowDecoration.Shadow = new Padding(4, 4, 4, 4);

            //hoverEffects(this);
        }

        private void hoverEffects(Control c)
        {
            c.Cursor = Cursors.Hand;

            foreach (Control cc in c.Controls)
            {
                hoverEffects(cc);
            }
        }
    }
}
