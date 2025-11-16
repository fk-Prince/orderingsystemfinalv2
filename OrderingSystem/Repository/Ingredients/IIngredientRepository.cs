using System.Collections.Generic;
using System.Data;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Ingredients
{
    public interface IIngredientRepository
    {
        bool saveIngredientByMenu(int menudetail_id, List<IngredientModel> menu, string type);
        bool addIngredient(IngredientModel os);
        List<IngredientModel> getIngredientsOfMenu(MenuDetailModel menu);
        List<IngredientModel> getIngredients();
        List<IngredientStockModel> getIngredientsStock();
        DataView getIngredientsView();
        bool isIngredientNameExists(string name, int id = 0);
        bool removeExpiredIngredient();
        bool deductIngredient(int id, int quantity, string reason);
        bool restockIngredient(IngredientStockModel os);
        bool updateIngredient(int id, string name, string unit);
        List<string> getInventoryReasons(string type);
        List<string> getSuppliers();
    }
}
