using Application.Room.DTO;

namespace Application.Room.Requests
{
    public class CreateRoomRequest
    {
        public RoomDto Data { get; set; }
        public string UserName { get; set; }        
    }
}