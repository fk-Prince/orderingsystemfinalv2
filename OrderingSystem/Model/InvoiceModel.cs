using OrderingSystem.CashierApp.Payment;

namespace OrderingSystem.Model
{
    public class InvoiceModel
    {
        public string InvoiceId { get; }
        public OrderModel Order { get; }
        public StaffModel Staff { get; }
        public Payment payment { get; }
        public double totalAmount { get; }
        public InvoiceModel(string InvoiceId, OrderModel Order, StaffModel Staff, Payment payment, double totalAmount)
        {
            this.InvoiceId = InvoiceId;
            this.Order = Order;
            this.Staff = Staff;
            this.payment = payment;
            this.totalAmount = totalAmount;
        }
    }
}
