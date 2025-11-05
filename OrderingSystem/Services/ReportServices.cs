using System;
using System.Collections.Generic;
using System.Data;
using OrderingSystem.Repository.Reports;

namespace OrderingSystem.Services
{
    public class ReportServices
    {
        private IReportRepository inventoryReportsRepository;
        public ReportServices(IReportRepository inventoryReportsRepository)
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

        public Tuple<string, string> getTransactionByDate(DateTime date)
        {

            return inventoryReportsRepository.getTransactions(date);
        }
        public Tuple<string, string> getOrders(DateTime date, string type)
        {
            return inventoryReportsRepository.getOrders(date, type);
        }

        public Tuple<string, string> getTotalOrderByType(DateTime date, string type)
        {
            return inventoryReportsRepository.getTotalOrders(date, type);
        }

        public List<Tuple<DateTime, int, int, int>> getOrderTotal()
        {




            return inventoryReportsRepository.getOrderMonthly();
        }
    }
}
