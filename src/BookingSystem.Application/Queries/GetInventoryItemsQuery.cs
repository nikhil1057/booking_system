using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Queries
{
    public record GetInventoryItemsQuery : IRequest<IEnumerable<InventoryItemDto>>;
}
