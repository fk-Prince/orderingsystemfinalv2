using System.Data;

namespace OrderingSystem.Repository.Reports
{
    public interface IInventoryReportsRepository
    {
        DataView getIngredientTrackerView();
        DataView getIngredientExpiry();
        DataView getInventoryReports();
        DataView getIngredientsUsage();
        DataView getMenuPopularity();
        DataView getInvoice();
        DataView getSupplier();
    }
}
