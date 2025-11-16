using System;

namespace OrderingSystem.Model
{

    public class DiscountModel
    {
        public int DiscountId { get; protected set; }
        public double Rate { get; protected set; }
        public DateTime UntilDate { get; protected set; }
        public string DisplayText { get; set; }
        public DiscountModel(int discountId, double rate, DateTime UntilDate)
        {
            DiscountId = discountId;
            Rate = rate;
            this.UntilDate = UntilDate;
        }
        public DiscountModel(int discountId, double rate)
        {
            DiscountId = discountId;
            Rate = rate;
            this.UntilDate = UntilDate;
        }

    }
}
