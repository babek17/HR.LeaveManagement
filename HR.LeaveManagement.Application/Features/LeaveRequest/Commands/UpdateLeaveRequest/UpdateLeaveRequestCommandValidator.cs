using System;
using System.Data;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;

    public UpdateLeaveRequestCommandValidator(ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;

        Include(new BaseLeaveRequestValidator(_leaveTypeRepository));

        RuleFor(p => p.Id)
            .NotNull()
            .MustAsync(LeaveRequestMustExist)
            .WithMessage("{PropertyName} must be present");
    }

    private async Task<bool> LeaveRequestMustExist(int id, CancellationToken token)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
        return leaveRequest != null;
    }
}
