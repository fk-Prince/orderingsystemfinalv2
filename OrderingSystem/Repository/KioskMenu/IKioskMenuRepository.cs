using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.Repository
{
    public interface IKioskMenuRepository
    {
        List<MenuDetailModel> getMenu();
        List<MenuDetailModel> getDetails(MenuDetailModel menu);
        List<MenuDetailModel> getDetailsByPackage(MenuDetailModel menu);
        List<MenuDetailModel> getFrequentlyOrderedTogether(MenuDetailModel menu);
        List<MenuDetailModel> getIncludedMenu(MenuDetailModel menu);
        bool isMenuPackage(MenuDetailModel menu);
        int getMaxOrderRealTime(int menuDetailId, List<OrderItemModel> orderList);
        int getMaxOrderRealTime2(int menu_id, List<OrderItemModel> orderList);
        double getNewPackagePrice(int menuid, List<MenuDetailModel> selectedMenus);
    }
}
