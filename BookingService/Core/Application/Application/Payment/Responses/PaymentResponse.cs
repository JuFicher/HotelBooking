using Application.Payment.Dtos;

namespace Application.Payment.Responses
{
    public class PaymentResponse : Response
    {
        public PaymentStateDto Data { get; set; }
    }
}