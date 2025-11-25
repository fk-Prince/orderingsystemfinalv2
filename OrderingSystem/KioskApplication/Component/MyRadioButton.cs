using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication.Components
{
    public partial class MyRadioButton : UserControl
    {
        public event EventHandler<MenuDetailModel> RadioCheckedChanged;
        public MyRadioButton(string namex, string xprice, MenuDetailModel m)
        {
            InitializeComponent();

            price.Text = xprice == "Free" ? xprice : "+   " + xprice;
            name.Text = namex;

            //if (m.MaxOrder <= 0)
            //{
            //    ooo.Visible = true;
            //    radioButton.Enabled = false;
            //}
            //else
            //{
            //    ooo.Visible = false;
            //    radioButton.Enabled = true;

            //}

            radioButton.CheckedChanged += (s, e) =>
            {
                //if (m.MaxOrder <= 0)
                //{
                //    if (radioButton.Checked)
                //    {
                //        radioButton.Checked = false;
                //        RadioCheckedChanged?.Invoke(this, null);
                //    }
                //}
                //else
                //{
                //    if (radioButton.Checked)
                //    {
                //        RadioCheckedChanged?.Invoke(this, m);
                //    }
                //}
            };
        }

        public Guna2CustomRadioButton Radio()
        {
            return radioButton;
        }
    }
}
