using MediatR;
using Application.Booking.DTO;
using Application.Booking.Responses;

namespace Application.Booking.Commands
{
    public class CreateBookingCommand : IRequest<BookingResponse>
    {
        public BookingDto BookingDto { get; set; }
    }
}