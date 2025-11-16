using System;

namespace OrderingSystem.Model
{
    public class IngredientStockModel
    {
        public int IngredientStockId { get; protected set; }
        public string IngredientName { get; protected set; }
        public int IngredientQuantity { get; protected set; }
        public string Reason { get; protected set; }
        public Supplier Supplier { get; protected set; }
        public double BatchCost { get; protected set; }
        public DateTime ExpiryDate { get; protected set; }

        public interface IIngredientStockModel
        {
            IngredientStockBuilder WithIngredientName(string ingredientName);
            IngredientStockBuilder WithIngredientStockId(int id);
            IngredientStockBuilder WithIngredientQTy(int ingredientQuantity);
            IngredientStockBuilder WithReason(string r);
            IngredientStockBuilder WithSupplier(Supplier id);
            IngredientStockBuilder WithBatchCost(double b);
            IngredientStockBuilder WithExpiryDate(DateTime b);
            IngredientStockModel Build();
        }

        public static IngredientStockBuilder Builder() => new IngredientStockBuilder();


        public class IngredientStockBuilder : IIngredientStockModel
        {
            private IngredientStockModel ingredientStock = new IngredientStockModel();
            public IngredientStockBuilder WithIngredientName(string ingredientName)
            {
                ingredientStock.IngredientName = ingredientName;
                return this;
            }
            public IngredientStockModel Build()
            {
                return ingredientStock;
            }
            public IngredientStockBuilder WithIngredientQTy(int ingredientQuantity)
            {
                ingredientStock.IngredientQuantity = ingredientQuantity;
                return this;
            }
            public IngredientStockBuilder WithIngredientStockId(int id)
            {
                ingredientStock.IngredientStockId = id;
                return this;
            }

            public IngredientStockBuilder WithReason(string r)
            {
                ingredientStock.Reason = r;
                return this;
            }

            public IngredientStockBuilder WithSupplier(Supplier id)
            {
                ingredientStock.Supplier = id;
                return this;
            }

            public IngredientStockBuilder WithBatchCost(double b)
            {
                ingredientStock.BatchCost = b;
                return this;
            }

            public IngredientStockBuilder WithExpiryDate(DateTime b)
            {
                ingredientStock.ExpiryDate = b;
                return this;
            }
        }

    }
}
