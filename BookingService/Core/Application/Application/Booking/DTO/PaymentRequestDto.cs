namespace Application.Booking.DTO
{
    public enum SupportedPaymentProviders
    {
        PagSeguro = 1,
        Paypal = 2,
        Stripe = 3,
        MercadoPago = 4
    }

    public enum SupportedPaymentMethods
    {
        DebitCard = 1,
        CreditCard = 2,
        BankTransfer = 3
    }

    public class PaymentRequestDto
    {
        public int BookingId { get; set; }
        public string paymentIntention { get; set; }
        public SupportedPaymentProviders SelectedPaymentProvider { get; set; }
        public SupportedPaymentMethods SelectedPaymentMethod { get; set; }
    }
}