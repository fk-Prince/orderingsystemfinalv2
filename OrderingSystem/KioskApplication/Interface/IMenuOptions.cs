using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApplication.Services
{
    public interface IMenuOptions
    {
        void displayMenuOptions(MenuDetailModel menu);
        List<OrderItemModel> confirmOrder();
    }
}
