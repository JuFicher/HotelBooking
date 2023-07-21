using Domain.Enums;
using Entities = Domain.Entities;

namespace Application.Booking.DTO
{
    public class BookingDto
    {
        public BookingDto()
        { 
            this.PlacedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        private Status Status { get; set; }

        public static Entities.Booking MapToEntity(BookingDto bookingDto)
        {
            return new Entities.Booking
            {
                Id = bookingDto.Id,
                PlacedAt = bookingDto.PlacedAt,
                Start = bookingDto.Start,
                End = bookingDto.End,   
                Room = new Entities.Room { Id = bookingDto.RoomId },  
                Guest = new Entities.Guest { Id = bookingDto.GuestId }                           
            };
        }

        public static BookingDto MapToDto(Entities.Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                PlacedAt = booking.PlacedAt,
                Start = booking.Start,
                End = booking.End,
                RoomId = booking.Room.Id,
                GuestId = booking.Guest.Id,
                Status = booking.Status,
            };
        }
    }
}