using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

public class GetLeaveTypesQueryHandler : IRequestHandler<GetLeaveTypesQuery, List<LeaveTypeDto>>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    
    public GetLeaveTypesQueryHandler(IMapper mapper,  ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
    }
    
    public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        //Query the database
        var leaveTypes = await _leaveTypeRepository.GetAllAsync();
        //Convert data objects to DTO object
        var data = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
        //return list of DTO objects
        return data;
    }
}