using System;

namespace OrderingSystem.Model
{
    public enum CouponType
    {
        PERCENTAGE,
        FIXED
    }
    public class CouponModel
    {
        public string CouponCode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public double CouponRate { get; set; }
        public double CouponMin { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int NumberOfTimes { get; set; }
        public CouponType type { get; set; }

        public CouponModel(double couponRate, CouponType type)
        {
            this.CouponRate = couponRate;
            this.type = type;
        }
        public CouponModel(string couponCode, string status, double couponRate, DateTime expiryDate, string description, CouponType type, double min)
        {
            this.CouponCode = couponCode;
            this.Status = status;
            this.CouponRate = couponRate;
            this.ExpiryDate = expiryDate;
            this.Description = description;
            this.type = type;
            this.CouponMin = min;
        }

        public CouponModel(double couponRate, DateTime expiryDate, string description, int numberOfTimes, CouponType type, double min)
        {
            this.CouponRate = couponRate;
            this.ExpiryDate = expiryDate;
            this.Description = description;
            this.NumberOfTimes = numberOfTimes;
            this.type = type;
            this.CouponMin = min;
        }

        public CouponType getType()
        {
            return type;
        }
        public static CouponType getType(string type)
        {
            return type.ToUpper() == "FIXED" ? CouponType.FIXED : CouponType.PERCENTAGE;
        }
        public double getCouponTotal(double total)
        {
            return getType() == CouponType.FIXED ? CouponRate : total * CouponRate;
        }
        public double getCouponRate()
        {
            return getType() == CouponType.FIXED ? CouponRate : CouponRate * 100;
        }
    }
}
