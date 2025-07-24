using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;

    public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, 
        ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
    {
        _leaveAllocationRepository = leaveAllocationRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
    }
    
    public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationCommandValidator(_leaveAllocationRepository, _leaveTypeRepository);
        var validationResult = validator.Validate(request);
        
        if(validationResult.Errors.Any())
            throw new BadRequestException("Invalid Leave Allocation Request.", validationResult);

        var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
        
        //optional conditional, validator already checks this
        if(leaveAllocation is null)
            throw new NotFoundException(nameof(LeaveAllocation), request.Id);
        
        _mapper.Map(request, leaveAllocation);
        await _leaveAllocationRepository.UpdateAsync(leaveAllocation);
        return Unit.Value;
    }
}