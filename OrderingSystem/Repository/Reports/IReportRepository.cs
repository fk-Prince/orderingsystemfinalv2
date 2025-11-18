using System;
using System.Collections.Generic;
using System.Data;

namespace OrderingSystem.Repository.Reports
{
    public interface IReportRepository
    {
        DataView getIngredientTrackerView();
        DataView getIngredientExpiry();
        DataView getInventoryReports();
        DataView getIngredientsUsage();
        DataView getIngredientHistory(string ingredientName);
        DataView getMenuPopularity();
        DataView getInvoice();
        DataView getSupplier();
        DataView getOrderStock(string v);

        Tuple<string, string> getTransactions(DateTime now);
        Tuple<string, string> getOrders(DateTime now, string query);
        Tuple<string, string> getTotalOrders(DateTime now, string x);
        List<Tuple<DateTime, int, int, int>> getOrderMonthly();
    }
}
