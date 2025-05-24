using BookingSystem.Core.Interfaces;
using MediatR;
using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Queries;

namespace BookingSystem.Application.Handlers
{
    public class GetMemberBookingsQueryHandler : IRequestHandler<GetMemberBookingsQuery, IEnumerable<BookingDto>>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetMemberBookingsQueryHandler(
            IMemberRepository memberRepository,
            IBookingRepository bookingRepository,
            IMapper mapper)
        {
            _memberRepository = memberRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingDto>> Handle(GetMemberBookingsQuery request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByEmailAsync(request.Email);
            if (member == null)
            {
                return Enumerable.Empty<BookingDto>();
            }

            var bookings = await _bookingRepository.GetByMemberIdAsync(member.Id);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
    }
}
