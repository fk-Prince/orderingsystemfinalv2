using System;
using System.Collections.Generic;
using OrderingSystem.Model;
using OrderingSystem.Repo.CashierMenuRepository;

namespace OrderingSystem.Services
{
    public class MenuService
    {
        private IMenuRepository menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }
        public List<string> getSizeFlavor(string type)
        {
            List<string> s = null;
            if (type.ToLower() == "size")

                s = menuRepository.getSize();
            else
                s = menuRepository.getFlavor();

            return s;
        }
        public bool saveMenu(MenuDetailModel md, string type)
        {
            if (type.ToLower() == "regular")
                return menuRepository.createRegularMenu(md);
            else if (md is MenuPackageModel mp && type.ToLower() == "bundle")
                return menuRepository.createBundleMenu(mp);
            else
                throw new NotSupportedException("Not Supported.");
        }
        public bool updateMenu(MenuDetailModel menu, string type)
        {
            if (menu is MenuPackageModel mp && type.ToLower() == "bundle")
                return menuRepository.updatePackageMenu(mp);
            else if (type.ToLower() == "regular")
                return menuRepository.updateRegularMenu(menu);
            else
                throw new NotSupportedException("Not Supported.");
        }
        public bool isMenuNameExist(string name)
        {
            return menuRepository.isMenuNameExist(name);
        }
        public List<MenuDetailModel> getMenus()
        {
            return menuRepository.getMenu();
        }
        public List<MenuDetailModel> getMenuDetail()
        {
            return menuRepository.getMenuDetail();
        }
        public List<MenuDetailModel> getBundled(MenuDetailModel menu)
        {
            return menuRepository.getBundled(menu);
        }
        public bool isMenuPackage(MenuDetailModel menu)
        {
            return menuRepository.isMenuPackage(menu);
        }
        public double getBundlePrice(MenuDetailModel menu)
        {
            return menuRepository.getBundlePrice(menu);
        }
        public bool newMenuVariant(int id, List<MenuDetailModel> m)
        {
            return menuRepository.newMenuVariant(id, m);
        }
        public bool updateBundle(int id, List<MenuDetailModel> included)
        {
            return menuRepository.updateBundle(id, included);
        }
    }
}
