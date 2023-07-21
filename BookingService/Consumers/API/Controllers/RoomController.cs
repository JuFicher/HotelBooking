using Application;
using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Room.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<GuestsController> _logger;
        private readonly IRoomManager _roomManager;
        private readonly IMediator _mediator;

        public RoomController(
            ILogger<GuestsController> logger,
            IRoomManager roomManager,
            IMediator mediator)
        {
            _logger = logger;
            _roomManager = roomManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var request = new CreateRoomRequest
            {
                Data = room,
                UserName = "Teste da Silva"
            };

            var response = await _roomManager.CreateRoom(request);

            if (response.Success) return Created("", response.Data);

            else if (response.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(response);
            }
            else if (response.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA)
            {
                return BadRequest(response);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", response);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<RoomDto>> Get(int roomId)
        {
            var query = new GetRoomQuery
            {
                Id = roomId
            };

            var response = await _mediator.Send(query);

            if (response.Success) return Ok(response.Data);

            return NotFound(response);
        }
    }
}