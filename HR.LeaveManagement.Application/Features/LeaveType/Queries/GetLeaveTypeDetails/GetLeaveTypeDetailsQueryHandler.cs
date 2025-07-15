using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQueryHandler:  IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;

    public GetLeaveTypeDetailsQueryHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
    }


    public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        //Query the database
        var leaveTypeDetails = await _leaveTypeRepository.GetByIdAsync(request.Id);
        //Validate that record exists
        if (leaveTypeDetails == null)
        {
            throw new NotFoundException(nameof(LeaveType), request.Id);
        }
        //Convert data objects to DTO objects
        var data =  _mapper.Map<LeaveTypeDetailsDto>(leaveTypeDetails);
        //return DTO objects
        return data;
    }
}