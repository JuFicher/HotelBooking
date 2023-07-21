using Application.Payment.Responses;

namespace Application.Payment
{
    public interface IMercadoPagoPaymentService
    {
        Task<PaymentResponse> PayWithCreditCard(string paymentIntention);
        Task<PaymentResponse> PayWithDebitCard(string paymentIntention);
        Task<PaymentResponse> PayWithBankTransfer(string paymentIntention);
    }
}