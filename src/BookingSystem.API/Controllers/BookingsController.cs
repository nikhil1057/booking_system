using BookingSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] BookRequest request)
        {
            var command = new CreateBookingCommand(request.MemberEmail, request.InventoryItemName);
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new { reference = result.BookingReference });
            }
            else
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelRequest request)
        {
            var command = new CancelBookingCommand(request.ReferenceNumber);
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new { message = "Booking cancelled successfully." });
            }
            else
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
        }
    }

    public class BookRequest
    {
        [Required, EmailAddress]
        public string MemberEmail { get; set; } = string.Empty;

        [Required]
        public string InventoryItemName { get; set; } = string.Empty;
    }

    public class CancelRequest
    {
        [Required]
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
