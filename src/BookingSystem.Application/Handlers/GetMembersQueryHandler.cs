using BookingSystem.Core.Interfaces;
using MediatR;
using BookingSystem.Application.Commands;
using BookingSystem.Core.Constants;
using BookingSystem.Core.Models;
using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Queries;

namespace BookingSystem.Application.Handlers
{
    public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, IEnumerable<MemberDto>>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public GetMembersQueryHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _memberRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemberDto>>(members);
        }
    }
}
