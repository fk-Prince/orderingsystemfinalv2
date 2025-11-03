using System.Data;
using OrderingSystem.Repository.Reports;

namespace OrderingSystem.Services
{
    public class ReportServices
    {
        private IInventoryReportsRepository inventoryReportsRepository;
        public ReportServices(IInventoryReportsRepository inventoryReportsRepository)
        {
            this.inventoryReportsRepository = inventoryReportsRepository;

        }
        public DataView getTrackingIngredients()
        {
            return inventoryReportsRepository.getIngredientTrackerView();
        }
        public DataView getIngredientExpiry()
        {
            return inventoryReportsRepository.getIngredientExpiry();
        }
        public DataView getInventorySummaryReports()
        {
            return inventoryReportsRepository.getInventoryReports();
        }
        public DataView getIngredientsUsage()
        {
            return inventoryReportsRepository.getIngredientsUsage();
        }

        public DataView getMenuPopularity()
        {
            return inventoryReportsRepository.getMenuPopularity();
        }
        public DataView getInvoice()
        {
            return inventoryReportsRepository.getInvoice();
        }

        public DataView getSupplier()
        {
            return inventoryReportsRepository.getSupplier();
        }
    }
}
