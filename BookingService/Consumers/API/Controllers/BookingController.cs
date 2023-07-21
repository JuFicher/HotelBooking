using Microsoft.AspNetCore.Mvc;
using Application.Booking.Ports;
using Application.Booking.DTO;
using Application;
using Application.Payment.Responses;
using Application.Booking.Responses;
using MediatR;
using Application.Booking.Commands;
using Application.Booking.Queries;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {   
        private readonly IBookingManager _bookingManager;
        private readonly ILogger<BookingController> _logger;
        private readonly IMediator _mediator;
        public BookingController(
            IBookingManager bookingManager,
            ILogger<BookingController> logger,
            IMediator mediator)
        {
            _bookingManager = bookingManager;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{booking}/Pay")]
        public async Task<ActionResult<PaymentResponse>> Pay(
            PaymentRequestDto paymentRequestDto, int bookingId)
        {
            paymentRequestDto.BookingId = bookingId;
            var response = await _bookingManager.PayForABooking(paymentRequestDto);

            if (response.Success) return Ok(response.Data);

            return BadRequest(response);
        } 

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Post(BookingDto booking)
        {
            var command = new CreateBookingCommand
            {
                BookingDto = booking,
            };
            var response = await _mediator.Send(command);

            if(response.Success) return Created("", response.Data);

            else if (response.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(response);
            }
            else if (response.ErrorCode == ErrorCodes.BOOKING_COULD_NOT_STORE_DATA)
            {
                return BadRequest(response);
            }
            else if (response.ErrorCode == ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED)
            {
                return BadRequest(response);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", response);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<BookingDto>> Get(int id)
        {
            var query = new GetBookingQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);

            if (response.Success) return Created("", response.Data);

            _logger.LogError("Could not process the request", response);
            return BadRequest(response);
        }
    }    
}