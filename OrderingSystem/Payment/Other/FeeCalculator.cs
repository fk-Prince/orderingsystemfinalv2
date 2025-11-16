namespace OrderingSystem.CashierApp.Payment
{
    public class FeeCalculator : IFeeCalculator
    {
        public double feePercent { get; }
        public FeeCalculator(double feePercent)
        {
            this.feePercent = feePercent;
        }
        public double calculateFee(double amount)
        {
            return amount * feePercent;
        }
        public double getTotalWithFee(double amount)
        {
            return amount + calculateFee(amount);
        }
    }
}
