using System.Collections.Generic;

namespace OrderingSystem.Model
{
    public class IngredientModel
    {
        protected int ingredientId;
        protected int ingredientQuantity;
        protected string ingredientName;
        protected string ingredientUnit;
        protected List<IngredientStockModel> os;


        public string IngredientName { get => ingredientName; }
        public int IngredientQuantity { get => ingredientQuantity; }
        public string IngredientUnit { get => ingredientUnit; }
        public List<IngredientStockModel> IngredientStockModel { get => os; }
        public int Ingredient_id { get => ingredientId; }

        public interface IIngredientModel
        {
            IngredientBuilder WithIngredientName(string ingredientName);
            IngredientBuilder WithIngredientID(int ingredient_id);
            IngredientBuilder WithInredeintQty(int ingredientQuantity);
            IngredientBuilder WithIngredientUnit(string ingredientUnit);
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
        }
    }
}