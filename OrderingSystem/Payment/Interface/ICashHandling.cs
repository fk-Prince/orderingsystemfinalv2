namespace OrderingSystem.CashierApp.Payment
{
    public interface ICashHandling
    {
        void setCashReceieved(double amount);
        double calculateChage(double total);
    }
}
