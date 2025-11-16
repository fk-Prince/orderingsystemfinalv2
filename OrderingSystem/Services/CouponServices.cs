

using System;
using System.Collections.Generic;
using System.Data;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Repository;
using OrderingSystem.Repository.Coupon;

namespace OrderingSystem.Services
{
    public class CouponServices
    {
        private ICouponRepository couponRepository;
        public CouponServices()
        {
            couponRepository = new CouponRepository();
        }
        public DataView saveCoupon(string rate, DateTime dateTime, string numberofTimes, string description, string type, string minC)
        {
            try
            {
                if (!double.TryParse(rate, out double dRate))
                {
                    throw new InvalidInput("Invalid rate.");
                }

                if (!Enum.TryParse(type?.Trim(), true, out CouponType couponType))
                    throw new InvalidInput("Invalid coupon type.");

                if (couponType == CouponType.PERCENTAGE)
                {
                    if (dRate < 0 || dRate > 100)
                    {
                        throw new InvalidInput("Rate must be greater than 0 and less than 100.");
                    }
                    dRate = dRate / 100;
                }

                double min = 0;

                if (couponType == CouponType.FIXED)
                {
                    if (!double.TryParse(minC, out min))
                        throw new InvalidInput("Invalid min.");

                    if (min < 0)
                        throw new InvalidInput("Rate must be greater than 0 ");

                    if (min < dRate)
                        throw new InvalidInput("The Fixed amount cannot be less than Minimun Amount");
                }
                else
                    min = 0;

                if (dateTime <= DateTime.Now)
                    throw new InvalidInput("Date should be greater today");

                if (!int.TryParse(numberofTimes, out int times) || times <= 0)
                    throw new InvalidInput("Number of times must be a positive whole number.");

                CouponModel cc = new CouponModel(dRate, dateTime, description, times, CouponModel.getType(type), min);
                return couponRepository.generateCoupon(cc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CouponModel> getCoupons()
        {
            return couponRepository.getAllCoupon();
        }
    }
}
