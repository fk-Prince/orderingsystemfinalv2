using System;
using System.Collections.Generic;
using OrderingSystem.util;

namespace OrderingSystem.Model
{
    public class ServingsModel
    {
        public int ServingId;
        public int Quantity;
        public double Price;
        public int LeftQuantity;
        public TimeSpan PrepTime;
        public DateTime date;
        public List<IngredientModel> ingList;
        public DiscountModel Discount;

        public double getPrice()
        {
            return Price + (Price * 0.12);
        }
        public double getPriceAfterVat()
        {
            return TaxHelper.VatCalulator(Price);
        }
        public double getPriceAfterVatWithDiscount()
        {
            return TaxHelper.VatCalulator(Price - (Price * (Discount == null ? 0 : Discount.Rate)));
        }

        public double getPriceAfterDiscount()
        {
            return Math.Round(Price - (Price * Discount?.Rate ?? 0), 2);
        }
        public interface IServing
        {
            ServingsBuilder withServingId(int id);
            ServingsBuilder withQuantity(int id);
            ServingsBuilder withLeftQuantity(int id);
            ServingsBuilder withPrice(double id);
            ServingsBuilder withPrepTime(TimeSpan id);
            ServingsBuilder withServingDate(DateTime id);
            ServingsBuilder withDiscount(DiscountModel id);
            ServingsBuilder withIngredientModel(List<IngredientModel> ingList);
            ServingsModel Build();
        }

        public static ServingsBuilder Build() => new ServingsBuilder();

        public class ServingsBuilder : IServing
        {
            private ServingsModel s;
            public ServingsBuilder()
            {
                s = new ServingsModel();
            }

            public ServingsModel Build()
            {
                return s;
            }

            public ServingsBuilder withDiscount(DiscountModel id)
            {
                s.Discount = id;
                return this;
            }

            public ServingsBuilder withIngredientModel(List<IngredientModel> ingList)
            {
                this.s.ingList = ingList;
                return this;
            }

            public ServingsBuilder withLeftQuantity(int id)
            {
                s.LeftQuantity = id;
                return this;
            }

            public ServingsBuilder withPrepTime(TimeSpan id)
            {
                s.PrepTime = id;
                return this;
            }

            public ServingsBuilder withPrice(double id)
            {
                s.Price = id;
                return this;
            }

            public ServingsBuilder withQuantity(int id)
            {
                s.Quantity = id;
                return this;
            }

            public ServingsBuilder withServingDate(DateTime id)
            {
                s.date = id;
                return this;
            }

            public ServingsBuilder withServingId(int id)
            {
                s.ServingId = id;
                return this;
            }
        }
    }
}
