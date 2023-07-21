using Application.Payment.Dtos;

namespace Application.Payment
{
    public interface IStripePaymentService
    {
        Task<PaymentStateDto> PayWithCreditCard(string paymentIntention);
        Task<PaymentStateDto> PayWithDebitCard(string paymentIntention);
        Task<PaymentStateDto> PayWithBankTransfer(string paymentIntention);
    }
}