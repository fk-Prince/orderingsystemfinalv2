using System;
using System.Collections.Generic;
using System.Drawing;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication.Components
{
    public partial class SizeLayout : System.Windows.Forms.UserControl
    {
        public event EventHandler<MenuDetailModel> SizeSelected;
        private List<MenuDetailModel> menuDetails;
        private List<MyRadioButton> radioSamples;
        private MenuDetailModel selectedFlavor;
        public SizeLayout(MenuDetailModel selectedFlavor, List<MenuDetailModel> menuDetails)
        {
            InitializeComponent();
            this.selectedFlavor = selectedFlavor;
            this.menuDetails = menuDetails;
            displaySizes();
        }

        public void displaySizes()
        {
            int y = 40;
            bool isFirst = true;
            radioSamples = new List<MyRadioButton>();
            foreach (var m in menuDetails)
            {
                double p = 0;
                string priceText = "";
                string displayPrice = "";

                if (selectedFlavor != null)
                {
                    if (selectedFlavor.MenuDetailId == m.MenuDetailId)
                    {
                        displayPrice = "Free";
                    }
                    else
                    {
                        p = m.getPriceAfterVatWithDiscount() - menuDetails[0].getPriceAfterVatWithDiscount();
                        priceText = p.ToString("N2");
                        if (isFirst)
                        {
                            displayPrice = isFirst && m.getPriceAfterVatWithDiscount() <= menuDetails[0].getPriceAfterVatWithDiscount() ? "Free" : priceText;
                        }
                        else
                        {
                            displayPrice = !isFirst && m.getPriceAfterVatWithDiscount() <= menuDetails[0].getPriceAfterVatWithDiscount() ? "Free" : priceText;
                        }
                    }
                }
                else
                {
                    p = m.getPriceAfterVatWithDiscount() - menuDetails[0].getPriceAfterVatWithDiscount();
                    priceText = p.ToString("N2");
                    if (isFirst)
                    {
                        displayPrice = isFirst && m.getPriceAfterVatWithDiscount() <= menuDetails[0].getPriceAfterVatWithDiscount() ? "Free" : priceText;
                    }
                    else
                    {
                        displayPrice = !isFirst && m.getPriceAfterVatWithDiscount() <= menuDetails[0].getPriceAfterVatWithDiscount() ? "Free" : priceText;
                    }
                }
                MyRadioButton rs = new MyRadioButton(m.SizeName, displayPrice, m);
                rs.Location = new Point(50, titleOption.Bottom + y);
                rs.RadioCheckedChanged += (s, e) =>
                {

                    foreach (var r in radioSamples)
                    {
                        if (r != rs)
                        {
                            r.Radio().Checked = false;
                        }
                    }
                    SizeSelected?.Invoke(this, m);
                };
                pp.Controls.Add(rs);
                radioSamples.Add(rs);
                y += 50;
                this.Height += 40;
                pp.Height += 40;
                isFirst = false;
            }

        }
        public void setTitleOption(string text, string menu)
        {
            titleOption.Text = text;
            menuName.Text = menu;
        }
        public void setSubTitle(string text)
        {
            subtitle.Text = text;
        }
        public void defaultSelection()
        {
            if (menuDetails.Count > 0 && radioSamples.Count > 0)
            {
                int x = 0;
                foreach (var i in menuDetails)
                {
                    if (i.MaxOrder >= 0)
                    {
                        radioSamples[x].Radio().Checked = true;
                        SizeSelected?.Invoke(this, menuDetails[x]);
                    }
                    else
                    {
                        x++;
                        SizeSelected?.Invoke(this, null);
                    }
                }
            }
        }
    }
}
