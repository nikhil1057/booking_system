using BookingSystem.Core.Interfaces;
using MediatR;
using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Queries;

namespace BookingSystem.Application.Handlers
{
    public class GetInventoryItemsQueryHandler : IRequestHandler<GetInventoryItemsQuery, IEnumerable<InventoryItemDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public GetInventoryItemsQueryHandler(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryItemDto>> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _inventoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(items);
        }
    }
}
