using System;
using System.Collections.Generic;
using System.Data;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Repository.Ingredients;

namespace OrderingSystem.Services
{
    public class IngredientServices
    {

        private readonly IIngredientRepository ingredientRepository;

        public IngredientServices(IIngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }

        public List<IngredientModel> getIngredients()
        {
            return ingredientRepository.getIngredients();
        }
        public DataView getIngredientsView()
        {
            return ingredientRepository.getIngredientsView();
        }
        public List<IngredientStockModel> getIngredientStock()
        {
            return ingredientRepository.getIngredientsStock();
        }

        public List<IngredientModel> getIngredientsOfMenu(MenuDetailModel variantDetail)
        {
            return ingredientRepository.getIngredientsOfMenu(variantDetail);
        }

        public bool saveIngredientByMenu(int menuId, List<IngredientModel> ingredientSelected, string type)
        {
            return ingredientRepository.saveIngredientByMenu(menuId, ingredientSelected, type);
        }

        public List<string> getReasons(string type)
        {
            return ingredientRepository.getInventoryReasons(type);
        }
        public bool removeExpiredIngredient()
        {
            return ingredientRepository.removeExpiredIngredient();
        }

        public bool validateDeductionIngredientStock(int stockId, int quantity, string reason, IngredientStockModel orig)
        {
            if (quantity <= 0)
                throw new InvalidInput("Invalid Quantity must be greater than zero.");

            if (quantity > orig.IngredientQuantity)
                throw new InvalidInput("Insufficient stock to deduct the requested quantity.");


            return ingredientRepository.deductIngredient(stockId, quantity, reason);
        }
        public bool validateRestockIngredient(int id, string quantity, DateTime value, string reason, string supplierName, string batchCost)
        {
            if (id == 0)
                throw new InvalidInput("No Selected Ingredient");

            if (!int.TryParse(quantity, out int qty))
                throw new InvalidInput("Quantity must be a valid integer.");

            if (qty <= 0)
                throw new InvalidInput("Invalid Quantity must be greater than zero.");

            if (string.IsNullOrWhiteSpace(supplierName))
                supplierName = "N/A";

            if (!double.TryParse(batchCost, out double cost))
                throw new InvalidInput("Cost must be a number.");

            if (cost <= 0)
                throw new InvalidInput("Cost must be must be greater than zero.");

            Supplier s = new Supplier(supplierName);
            IngredientStockModel os = IngredientStockModel.Builder()
                .WithIngredientStockId(id)
                .WithIngredientQTy(qty)
                .WithExpiryDate(value)
                .WithReason(reason)
                .WithSupplier(s)
                .WithBatchCost(cost)
                .Build();

            return ingredientRepository.restockIngredient(os);
        }
        public bool validateAddIngredients(string name, string quantity, string unit, DateTime expire, string supplierName, string batchCost)
        {
            if (ingredientRepository.isIngredientNameExists(name))
                throw new InvalidInput("Ingredient name already exists.");

            if (!int.TryParse(quantity, out int qty))
                throw new InvalidInput("Quantity must be a valid integer.");

            if (qty <= 0)
                throw new InvalidInput("Invalid Quantity must be greater than zero.");

            if (string.IsNullOrEmpty(supplierName))
                supplierName = "N/A";

            if (!double.TryParse(batchCost, out double cost))
                throw new InvalidInput("Cost must be a number.");

            if (cost <= 0)
                throw new InvalidInput("Cost must be must be greater than zero.");

            Supplier s = new Supplier(supplierName);
            IngredientStockModel os = IngredientStockModel.Builder()
                .WithSupplier(s)
                .WithBatchCost(cost)
                .WithExpiryDate(expire)
                .WithIngredientQTy(qty)
                .Build();

            IngredientModel om = IngredientModel.Builder()
                .WithIngredientUnit(unit)
                .WithIngredientName(name)
                .WithStock(new List<IngredientStockModel>() { os })
                .Build();


            return ingredientRepository.addIngredient(om);
        }
        public bool validateUpdateIngredient(int id, string name, string unit)
        {
            if (ingredientRepository.isIngredientNameExists(name, id))
                throw new InvalidInput("Ingredient name already exists.");

            return ingredientRepository.updateIngredient(id, name, unit);
        }
        public List<string> getSuppliers()
        {
            return ingredientRepository.getSuppliers();
        }
    }
}
