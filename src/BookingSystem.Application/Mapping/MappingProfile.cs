using BookingSystem.Application.DTOs;
using BookingSystem.Core.Models;
using AutoMapper;

namespace BookingSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, MemberDto>()
                .ForMember(dest => dest.ActiveBookingsCount,
                    opt => opt.MapFrom(src => src.ActiveBookings.Count));

            CreateMap<InventoryItem, InventoryItemDto>();

            CreateMap<Booking, BookingDto>();
        }
    }
}
