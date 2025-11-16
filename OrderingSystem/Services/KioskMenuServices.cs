using System.Collections.Generic;
using OrderingSystem.Model;
using OrderingSystem.Repository;

namespace OrderingSystem.Services
{
    public class KioskMenuServices
    {
        private IKioskMenuRepository menuRepository;
        public KioskMenuServices(IKioskMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        public List<MenuDetailModel> getMenu()
        {
            return menuRepository.getMenu();
        }
        public int getMaxOrderRealTime(int menuDetailId, List<OrderItemModel> orderList)
        {
            return menuRepository.getMaxOrderRealTime(menuDetailId, orderList);
        }
        public List<MenuDetailModel> getDetails(MenuDetailModel menu)
        {
            return menuRepository.getDetails(menu);
        }
        public bool isMenuPackage(MenuDetailModel menu)
        {
            return menuRepository.isMenuPackage(menu);
        }
        public List<MenuDetailModel> getFrequentlyOrderedTogether(MenuDetailModel menus)
        {
            return menuRepository.getFrequentlyOrderedTogether(menus);
        }
        public List<MenuDetailModel> getIncludedMenu(MenuDetailModel menu)
        {
            return menuRepository.getIncludedMenu(menu);
        }
        public double getNewPackagePrice(int menuDetailId, List<MenuDetailModel> includedMenu)
        {
            return menuRepository.getNewPackagePrice(menuDetailId, includedMenu);
        }
        public List<MenuDetailModel> getDetailsByPackage(MenuDetailModel menuDetail)
        {
            List<MenuDetailModel> l = menuRepository.getDetailsByPackage(menuDetail);

            List<MenuDetailModel> newList = new List<MenuDetailModel>();

            foreach (var i in l)
            {
                var menuPackage = MenuPackageModel.Builder()
                                    .WithMenuId(i.MenuId)
                                    .WithMenuDetailId(i.MenuDetailId)
                                    .WithPrice(i.MenuPrice)
                                    .WithMaxOrder(i.MaxOrder)
                                    .WithFlavorName(i.FlavorName)
                                    .WithDiscount(menuDetail.Discount)
                                    .WithSizeName(i.SizeName)
                                    .Build();

                newList.Add(menuPackage);
            }

            return newList;
        }
    }
}
