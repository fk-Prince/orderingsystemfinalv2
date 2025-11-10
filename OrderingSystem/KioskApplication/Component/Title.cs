using System.Drawing;
using System.Windows.Forms;

namespace OrderingSystem.KioskApplication.Component
{
    public partial class Title : UserControl
    {
        public Title(string title, Image img)
        {
            InitializeComponent();
            tt.Text = title;
            //AutoRoundedCorners = true;
            //BorderColor = ColorTranslator.FromHtml("#EFF6FF");
            //BorderColor = Color.FromArgb(9, 119, 206);
            //BackColor = Color.Transparent;
            //FillColor = ColorTranslator.FromHtml("#689FF9");
            im.Image = img;
            //BorderThickness = 1;
        }


    }
}
