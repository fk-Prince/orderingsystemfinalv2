using System.Collections.Generic;

namespace OrderingSystem.Model
{
    public class IngredientModel
    {
        protected int ingredientId;
        protected int ingredientQuantity;
        protected int ingredientQuantity2;
        protected string ingredientName;
        protected string ingredientUnit;
        protected double ingredientCostPerunit;
        protected List<IngredientStockModel> os;


        public string IngredientName { get => ingredientName; }
        public int IngredientQuantity { get => ingredientQuantity; }
        public int IngredientQuantity2 { get => ingredientQuantity2; }
        public string IngredientUnit { get => ingredientUnit; }
        public double IngredientCostPerUnit { get => ingredientCostPerunit; }
        public List<IngredientStockModel> IngredientStockModel { get => os; }
        public int Ingredient_id { get => ingredientId; }

        public interface IIngredientModel
        {
            IngredientBuilder WithIngredientName(string ingredientName);
            IngredientBuilder WithIngredientID(int ingredient_id);
            IngredientBuilder WithInredeintQty(int ingredientQuantity);
            IngredientBuilder WithInredeintQty2(int ingredientQuantity);
            IngredientBuilder WithIngredientUnit(string ingredientUnit);
            IngredientBuilder WithIngredientCost(double dd);
            IngredientBuilder WithStock(List<IngredientStockModel> os);
            IngredientModel Build();
        }
        public static IngredientBuilder Builder() => new IngredientBuilder();

        public class IngredientBuilder : IIngredientModel
        {
            private IngredientModel ingredientModel = new IngredientModel();
            public IngredientBuilder WithIngredientName(string ingredientName)
            {
                ingredientModel.ingredientName = ingredientName;
                return this;
            }
            public IngredientBuilder WithIngredientID(int ingredient_id)
            {
                ingredientModel.ingredientId = ingredient_id;
                return this;
            }
            public IngredientModel Build()
            {
                return ingredientModel;
            }

            public IngredientBuilder WithIngredientUnit(string ingredientUnit)
            {
                ingredientModel.ingredientUnit = ingredientUnit;
                return this;
            }

            public IngredientBuilder WithInredeintQty(int ingredientQuantity)
            {
                ingredientModel.ingredientQuantity = ingredientQuantity;
                return this;
            }

            public IngredientBuilder WithStock(List<IngredientStockModel> os)
            {
                ingredientModel.os = os;
                return this;
            }

            public IngredientBuilder WithIngredientCost(double dd)
            {
                ingredientModel.ingredientCostPerunit = dd;
                return this;
            }

            public IngredientBuilder WithInredeintQty2(int ingredientQuantity)
            {
                ingredientModel.ingredientQuantity2 = ingredientQuantity;
                return this;
            }
        }
    }
}