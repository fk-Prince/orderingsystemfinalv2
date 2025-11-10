using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.KioskApplication.Components;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.KioskApplication.Layouts
{
    public partial class PackageLayout : UserControl
    {
        private string titleOption;
        private string subTitle;
        private SizeLayout sc;
        private MenuModel selectedFlavor;
        private MenuModel selectedSize;

        private MenuModel selectedMenu;
        public MenuModel SelectedMenuDetail => selectedMenu;

        private readonly MenuModel menuDetail;
        private readonly List<MenuModel> menuDetails;
        public PackageLayout(KioskMenuServices kioskMenuServices, MenuModel menuDetail)
        {
            InitializeComponent();
            this.menuDetail = menuDetail;
            try
            {
                menuDetails = kioskMenuServices.getDetailsByPackage(menuDetail);
                displayFlavor(menuDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void displayFlavor(MenuModel menuDetail)
        {
            string t = "Select your menu.";
            titleOption = "Option A";
            subTitle = $"Select Flavor of your choice.";
            try
            {
                var x = menuDetails
                   .GroupBy(ex => ex.FlavorName)
                   .Select(group => group.First())
                   .ToList();


                if (menuDetail is MenuPackageModel mp)
                {
                    if (mp.isFixed)
                        x = menuDetails.Take(1).ToList();
                }

                FlavorLayout fl = new FlavorLayout(x);
                fl.Margin = new Padding(0);
                fl.pp.BorderThickness = 0;
                fl.pp.BorderRadius = 0;
                fl.FlavorSelected += flavorSelected;
                fl.setTitle(titleOption, menuDetail.MenuName);
                fl.setSubTitle(subTitle);
                fl.defaultSelection();
                flowPanel.Controls.Add(fl);
                titleOption = "Option B";
                selectedFlavor = x[0];

                if (x.Count > 1)
                {
                    adjustHeight();
                    filterSizeByFlavor(menuDetails, menuDetail.MenuId, menuDetail.FlavorName);
                }
                else
                    fl.setSubTitle(t);
            }
            catch (Exception)
            {
                Console.WriteLine("Error on PackageLayout displayFlavor");
                throw;
            }
        }
        private void filterSizeByFlavor(List<MenuModel> menuDetails, int menuid, string flavor)
        {
            List<MenuModel> l = string.IsNullOrWhiteSpace(flavor) ? menuDetails.FindAll(x => menuid == x.MenuId) : menuDetails.FindAll(x => menuid == x.MenuId && x.FlavorName == flavor);
            displaySize(l);
        }
        private void flavorSelected(object sender, MenuModel e)
        {
            selectedFlavor = e;

            if (sc != null)
            {
                if (flowPanel.Controls.Contains(sc))
                {
                    flowPanel.Controls.Remove(sc);
                }
                sc.Dispose();
                sc = null;
                resetHeight();
            }
            if (e != null)
                filterSizeByFlavor(menuDetails, menuDetail.MenuId, menuDetail.FlavorName);
        }
        private void displaySize(List<MenuModel> menuList)
        {

            if (sc != null)
            {
                if (flowPanel.Controls.Contains(sc))
                {
                    flowPanel.Controls.Remove(sc);
                }
                sc.Dispose();
                sc = null;
            }
            var x = menuList
                  .GroupBy(ex => ex.SizeName)
                  .Select(group => group.First())
                  .ToList();

            if (menuDetail is MenuPackageModel mp)
            {
                if (mp.isFixed)
                {
                    x = menuDetails.Take(1).ToList();
                }
            }


            if (x.Count > 1)
            {
                subTitle = $"Select Size of your choice.";
                sc = new SizeLayout(selectedFlavor, x);
                sc.Margin = new Padding(0);
                sc.pp.BorderThickness = 0;
                sc.pp.BorderRadius = 0;
                sc.setTitleOption(titleOption, menuList[0].MenuName);
                sc.setSubTitle(subTitle);
                sc.SizeSelected += (s, e) =>
                {
                    selectedSize = e;
                    string flavorName = selectedFlavor?.FlavorName ?? "";
                    string sizeName = selectedSize?.SizeName ?? "";
                    if (flavorName == "" && sizeName == "") return;
                    getSelectedMenu();
                };
                flowPanel.Controls.Add(sc);
                sc.defaultSelection();
                adjustHeight();
            }
            else
            {
                selectedSize = x[0];
                getSelectedMenu();
            }
        }
        private void getSelectedMenu()
        {
            string flavorName = selectedFlavor?.FlavorName ?? "";
            string sizeName = selectedSize?.SizeName ?? "";
            selectedMenu = menuDetails.Find(e => (e.MenuDetailId == selectedFlavor.MenuDetailId || e.MenuDetailId == selectedSize.MenuDetailId) && e.FlavorName == flavorName && e.SizeName == sizeName);
        }
        private void adjustHeight()
        {
            int totalHeight = 0;
            foreach (Control control in flowPanel.Controls)
            {
                totalHeight += control.Height + control.Margin.Top;
            }

            flowPanel.Height = totalHeight;
            Height = totalHeight + 20;

            PerformLayout();
            Parent?.PerformLayout();
        }
        private void resetHeight()
        {
            int heigt = 0;
            foreach (Control c in flowPanel.Controls)
            {
                if (c is FlavorLayout)
                {
                    heigt += c.Height + c.Margin.Top;
                }
            }

            flowPanel.Height = heigt;
            Height = heigt + 20;

            PerformLayout();
            Parent?.PerformLayout();
        }
    }
}
