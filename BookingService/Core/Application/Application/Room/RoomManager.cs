using Application.Room.Responses;
using Application.Room.DTO;
using Application.Room.Requests;
using Application.Room.Ports;
using Domain.Room.Exceptions;
using Domain.Room.Ports;
using Domain.Users;

namespace Application.Room
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        public RoomManager(
            IRoomRepository roomRepository,
            IUserRepository userRepository)
        { 
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }

        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                var user = await _userRepository.Get(request.UserName);

                if (!user.Roles.Contains("manager"))
                {
                    return new RoomResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.ROOM_INVALID_PERMISSION,
                        Message = "User dos not have permission to perform this action"
                    };
                }
                var room = RoomDto.MapToEntity(request.Data);

                await room.Save(_roomRepository);
                request.Data.Id = room.Id;

                return new RoomResponse
                {
                    Success = true,
                    Data = request.Data,
                };
            }
            catch (InvalidRoomDataException)
            {

                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidRoomPriceException)
            {

                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULD_NOT_STORE_DATA,
                    Message = "The room price is incorrect"
                };
            }
            catch (Exception)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<RoomResponse> GetRoom(int roomId)
        {
            var room = await _roomRepository.Get(roomId);

            if (room == null)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_NOT_FOUND,
                    Message = "Could not find a Room with the given Id"
                };
            }

            return new RoomResponse
            {
                Data = RoomDto.MapToDto(room),
                Success = true,
            };
        }
    }
}