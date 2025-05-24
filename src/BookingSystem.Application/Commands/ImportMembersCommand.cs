using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookingSystem.Application.Commands
{
    public record ImportMembersCommand(IFormFile File) : IRequest<bool>;
}
