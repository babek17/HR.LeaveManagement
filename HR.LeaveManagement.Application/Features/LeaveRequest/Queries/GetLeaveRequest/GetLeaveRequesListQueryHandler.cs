using System;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequest;

public class GetLeaveRequesListQueryHandler : IRequestHandler<GetLeaveRequesListQuery, List<LeaveRequestDto>>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IMapper _mapper;

    public GetLeaveRequesListQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _mapper = mapper;
    }

    public async Task<List<LeaveRequestDto>> Handle(GetLeaveRequesListQuery request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
        var requests = _mapper.Map<List<LeaveRequestDto>>(leaveRequest);
        return requests;
    }
}
