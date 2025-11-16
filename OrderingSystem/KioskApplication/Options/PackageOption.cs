using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OrderingSystem.Exceptions;
using OrderingSystem.KioskApplication.Layouts;
using OrderingSystem.KioskApplication.Services;
using OrderingSystem.Model;
using OrderingSystem.Services;

namespace OrderingSystem.KioskApplication.Options
{
    public class PackageOption : IMenuOptions, ISelectedFrequentlyOrdered
    {

        private readonly KioskMenuServices kioskMenuServices;
        private readonly FlowLayoutPanel flowPanel;
        private readonly FrequentlyOrderedOption frequentlyOrderedOption;
        private readonly List<PackageLayout> orderList;

        private MenuDetailModel menu;
        public PackageOption(KioskMenuServices kioskMenuServices, FlowLayoutPanel flowPanel)
        {
            this.kioskMenuServices = kioskMenuServices;
            this.flowPanel = flowPanel;
            orderList = new List<PackageLayout>();
            frequentlyOrderedOption = new FrequentlyOrderedOption(kioskMenuServices, flowPanel);
        }
        public void displayMenuOptions(MenuDetailModel menu)
        {
            try
            {
                this.menu = menu;
                List<MenuDetailModel> menuList = kioskMenuServices.getIncludedMenu(menu);
                foreach (MenuPackageModel item in menuList)
                {
                    var pakage = new PackageLayout(kioskMenuServices, item);
                    pakage.Margin = new Padding(20, 20, 0, 20);
                    flowPanel.Controls.Add(pakage);
                    orderList.Add(pakage);
                }

                frequentlyOrderedOption.displayFrequentlyOrdered(menu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrderItemModel> getFrequentlyOrdered()
        {
            return frequentlyOrderedOption?.getFrequentlyOrdered();
        }

        public List<OrderItemModel> confirmOrder()
        {
            try
            {
                if (orderList.Any(pg => pg.SelectedMenuDetail == null || pg.SelectedMenuDetail.MaxOrder <= 0))
                    throw new OutOfOrder("Currently this menu is unavailable.");

                var includedMenu = orderList.Select(pg => pg.SelectedMenuDetail).ToList();
                double newPrice = kioskMenuServices.getNewPackagePrice(menu.MenuDetailId, includedMenu);


                var packageBundle = MenuPackageModel.Builder()
                    .WithMenuDetailId(menu.MenuDetailId)
                    .WithMenuName(menu.MenuName)
                    .WithPrice(newPrice)
                    .WithMenuImage(menu.MenuImage)
                    .WithMenuId(menu.MenuId)
                    .WithPackageIncluded(includedMenu)
                    .WithDiscount(menu.Discount)
                    .WithEstimatedTime(menu.EstimatedTime)
                    .Build();

                var om = new OrderItemModel(1, packageBundle);

                return new List<OrderItemModel> { om };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
