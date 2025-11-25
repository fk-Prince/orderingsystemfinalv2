using System;
using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.Repo.CashierMenuRepository
{
    public interface IMenuRepository
    {
        List<MenuDetailModel> getMenu();
        bool isMenuNameExist(string name);
        bool createServing(int id, ServingsModel servings);
        bool isServingDateExistsing(int id, DateTime date);
        bool saveMenu(MenuDetailModel md);
        bool saveMenuWithServing(MenuDetailModel md);
        bool cancelServing(int id);
        List<ServingsModel> getServings(int id);
    }
}
