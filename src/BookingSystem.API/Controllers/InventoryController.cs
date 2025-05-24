using BookingSystem.Application.Commands;
using BookingSystem.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _mediator.Send(new GetInventoryItemsQuery());
            return Ok(items);
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var result = await _mediator.Send(new ImportInventoryCommand(file));

            if (result)
            {
                return Ok("Inventory items imported successfully.");
            }
            else
            {
                return BadRequest("Failed to import inventory items.");
            }
        }
    }
}
