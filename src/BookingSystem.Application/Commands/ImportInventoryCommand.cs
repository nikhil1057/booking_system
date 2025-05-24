using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingSystem.Application.Commands
{
    public record ImportInventoryCommand(IFormFile File) : IRequest<bool>;
}
