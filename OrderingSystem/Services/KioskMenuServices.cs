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
    }
}
