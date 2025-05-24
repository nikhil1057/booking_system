using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Queries
{
    public record GetMembersQuery : IRequest<IEnumerable<MemberDto>>;
}
