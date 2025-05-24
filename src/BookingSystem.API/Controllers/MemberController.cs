using BookingSystem.Application.Commands;
using BookingSystem.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _mediator.Send(new GetMembersQuery());
            return Ok(members);
        }

        [HttpGet("{email}/bookings")]
        public async Task<IActionResult> GetBookings(string email)
        {
            var bookings = await _mediator.Send(new GetMemberBookingsQuery(email));
            return Ok(bookings);
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var result = await _mediator.Send(new ImportMembersCommand(file));

            if (result)
            {
                return Ok("Members imported successfully.");
            }
            else
            {
                return BadRequest("Failed to import members.");
            }
        }
    }
}
