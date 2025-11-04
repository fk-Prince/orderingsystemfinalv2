using System;
using System.Data;

namespace OrderingSystem.Repository.Reports
{
    public interface IReportRepository
    {
        DataView getIngredientTrackerView();
        DataView getIngredientExpiry();
        DataView getInventoryReports();
        DataView getIngredientsUsage();
        DataView getMenuPopularity();
        DataView getInvoice();
        DataView getSupplier();

        Tuple<string, string> getTransactions(DateTime now);
        Tuple<string, string> getOrders(DateTime now, string query);

        Tuple<string, string> getTotalOrders(DateTime now, string x);
    }
}
