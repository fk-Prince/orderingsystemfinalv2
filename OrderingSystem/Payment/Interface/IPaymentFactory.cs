namespace OrderingSystem.CashierApp.Payment
{
    public interface IPaymentFactory
    {
        IPayment paymentType(string type);
    }
}
