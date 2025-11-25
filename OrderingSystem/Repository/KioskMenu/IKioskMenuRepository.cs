using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.Repository
{
    public interface IKioskMenuRepository
    {
        List<MenuDetailModel> getMenu();
    }
}
