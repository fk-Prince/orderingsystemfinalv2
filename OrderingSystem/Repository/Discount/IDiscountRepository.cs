using System;
using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Discount
{
    public interface IDiscountRepository
    {

        List<DiscountModel> getDiscount();
        bool saveDiscount(double rate, DateTime date);
    }
}
