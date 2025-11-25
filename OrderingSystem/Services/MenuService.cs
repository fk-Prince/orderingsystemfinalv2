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

        public bool isServingDateExistsing(int id, DateTime date)
        {
            return menuRepository.isServingDateExistsing(id, date);
        }
        public bool isMenuNameExist(string name)
        {
            return menuRepository.isMenuNameExist(name);
        }
        public List<MenuDetailModel> getMenus()
        {
            return menuRepository.getMenu();
        }
        public bool saveNewServing(int id, ServingsModel ee)
        {
            return menuRepository.createServing(id, ee);
        }

        public bool saveNewMenu(MenuDetailModel md, bool x)
        {
            if (x)
                return menuRepository.saveMenuWithServing(md);
            else
                return menuRepository.saveMenu(md);
        }

        public List<ServingsModel> getServings(int id)
        {
            return menuRepository.getServings(id);
        }

        public bool cancelServing(int servingId)
        {
            return menuRepository.cancelServing(servingId);
        }
    }
}
