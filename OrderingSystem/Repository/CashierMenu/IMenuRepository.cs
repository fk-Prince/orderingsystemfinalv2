using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.Repo.CashierMenuRepository
{
    public interface IMenuRepository
    {
        bool isMenuNameExist(string name);
        bool updateRegularMenu(MenuDetailModel update);
        bool updatePackageMenu(MenuPackageModel update);
        bool isMenuPackage(MenuDetailModel m);
        bool createRegularMenu(MenuDetailModel md);
        bool createBundleMenu(MenuPackageModel md);
        bool newMenuVariant(int menuId, List<MenuDetailModel> m);
        List<string> getFlavor();
        List<MenuDetailModel> getMenu();
        List<MenuDetailModel> getBundled(MenuDetailModel menu);
        List<MenuDetailModel> getMenuDetail();
        List<string> getSize();
        double getBundlePrice(MenuDetailModel menu);

        bool updateBundle(int id, List<MenuDetailModel> included);
    }
}
