using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public UpdateLeaveAllocationCommandValidator(ILeaveAllocationRepository leaveAllocationRepository,
        ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveAllocationRepository = leaveAllocationRepository;
        _leaveTypeRepository = leaveTypeRepository;

        RuleFor(p => p.Id).NotNull()
            .MustAsync(LeaveAllocationMustExist)
            .WithMessage("{PropertyName} must be present.");
        
        RuleFor(p=>p.NumberOfDays)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        
        RuleFor(p=>p.LeaveTypeId)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.")
            .MustAsync(LeaveTypeMustExist)
            .WithMessage("{PropertyName} does not exist.");
        
        RuleFor(p=>p.Period)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be after {ComparisonValue}.");
    }
    
    private async Task<bool> LeaveAllocationMustExist(int leaveAllocationId, CancellationToken token)
    {
        var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(leaveAllocationId);
        return leaveAllocation is not null;
    }
    
    private async Task<bool> LeaveTypeMustExist(int id, CancellationToken arg2)
    {
        var leaveType = await _leaveTypeRepository.GetByIdAsync(id);
        return leaveType is not null;
    }
    
}