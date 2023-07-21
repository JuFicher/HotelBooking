using Application.Ports;
using Application.Guest.Responses;
using Microsoft.AspNetCore.Mvc;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Application;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestsController : ControllerBase
    {   
        private readonly ILogger<GuestsController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestsController(
            ILogger<GuestsController> logger,
            IGuestManager guestManager)
        {
            _logger = logger;
            _guestManager = guestManager;
        }        

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest
            };
            
            var response = await _guestManager.CreateGuest(request);

            if (response.Success) return Created("", response.Data);

            if (response.ErrorCode == ErrorCodes.NOT_FOUND)
            {
                return NotFound(response);
            }
            else if (response.ErrorCode == ErrorCodes.INVALID_DOCUMENT_ID)
            {
                return BadRequest(response);
            }
            else if (response.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(response);
            }
            else if (response.ErrorCode == ErrorCodes.INVALID_EMAIL)
            {
                return BadRequest(response);
            }
            else if (response.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA)
            {
                return BadRequest(response);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", response);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<GuestDTO>> Get(int guestId)
        {
            var response = await _guestManager.GetGuest(guestId);

            if (response.Success) return Created("", response.Data);

            return NotFound(response);
        }
    }
}