using Application.Booking.DTO;
using Application.Booking.Responses;
using Application.Payment.Responses;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(BookingDto booking);
        Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto);
        Task<BookingResponse> GetBooking(int id);        
    }
}