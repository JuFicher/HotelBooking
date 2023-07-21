using Application.Booking.Responses;
using MediatR;
using Domain.Booking.Ports;
using Application.Booking.DTO;

namespace Application.Booking.Queries
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        public GetBookingQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.Get(request.Id);

            var bookingDto = BookingDto.MapToDto(booking);

            return new BookingResponse
            {
                Success = true,
                Data = bookingDto
            };
        }
    }
}