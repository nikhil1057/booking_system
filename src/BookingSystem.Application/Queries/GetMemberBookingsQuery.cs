using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Queries
{

    public record GetMemberBookingsQuery(string Email) : IRequest<IEnumerable<BookingDto>>;
}
