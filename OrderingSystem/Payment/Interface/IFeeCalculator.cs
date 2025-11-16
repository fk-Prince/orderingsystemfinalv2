namespace OrderingSystem.CashierApp.Payment
{
    public interface IFeeCalculator
    {
        double feePercent { get; }
        double calculateFee(double amount);
        double getTotalWithFee(double amount);
    }
}
