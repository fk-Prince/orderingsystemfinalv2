using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication.Component
{
    public partial class Filter : Guna2Panel

    {
        private List<MyRadioButton2> catR;
        private List<CategoryModel> cat;
        public List<int> catSelected;
        public Filter(List<CategoryModel> cat, double maxPrice)
        {
            InitializeComponent();
            BackColor = Color.Transparent;
            FillColor = ColorTranslator.FromHtml("#DBEAFE");
            BorderThickness = 2;
            BorderRadius = 10;
            Margin = new Padding(0, 0, 0, 0);
            catSelected = new List<int>();
            catR = new List<MyRadioButton2>();
            this.cat = cat;

            tt.Maximum = ((int)Math.Ceiling(maxPrice));
            max.Text = ((int)Math.Ceiling(maxPrice)).ToString();
            tt.Value = ((int)Math.Ceiling(maxPrice));
            max.Text = tt.Value.ToString();

            displayCategory();
        }

        private void displayCategory()
        {
            flow.Controls.Clear();
            foreach (var i in cat)
            {
                MyRadioButton2 b = new MyRadioButton2(i.CategoryName);
                b.check += (s, e) =>
                {
                    if (e)
                        catSelected.Add(i.CategoryId);
                    else
                    {
                        catSelected.Remove(i.CategoryId);
                        b.radio.Checked = false;
                    }
                };
                catR.Add(b);
                flow.Controls.Add(b);
            }
        }

        private void tt_Scroll(object sender, ScrollEventArgs e)
        {
            max.Text = tt.Value.ToString();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int price = tt.Value;
            if (FindForm() is KioskLayout parentForm)
                parentForm.displayCategory(catSelected, price);
        }

        public void resetFilter()
        {
            catSelected.Clear();
            max.Text = tt.Maximum.ToString();
            catR.ForEach(ex => ex.radio.Checked = false);
            tt.Value = tt.Maximum;
        }

        private void fb_Click(object sender, EventArgs e)
        {
            resetFilter();
            if (FindForm() is KioskLayout parentForm)
                parentForm.displayCategory(new List<int>(), tt.Maximum);
        }
    }
}
