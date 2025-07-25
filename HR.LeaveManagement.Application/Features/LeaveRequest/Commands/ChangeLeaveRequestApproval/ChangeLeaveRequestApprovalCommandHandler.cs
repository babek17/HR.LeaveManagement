using System;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;

public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _appLogger;

    public ChangeLeaveRequestApprovalCommandHandler(
        IMapper mapper,
        IEmailSender emailSender,
        ILeaveRequestRepository leaveRequestRepository,
        ILeaveTypeRepository leaveTypeRepository,
        IAppLogger<CreateLeaveRequestCommandHandler> appLogger)
    {
        _mapper = mapper;
        _emailSender = emailSender;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _appLogger = appLogger;
    }
    public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await GetLeaveRequestAsync(request);

        leaveRequest.Approved = request.Approved;

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        await TrySendCreateNotificationsAsync(request);

        return Unit.Value;
    }

    private async Task<Domain.LeaveRequest> GetLeaveRequestAsync(ChangeLeaveRequestApprovalCommand request)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

        if (leaveRequest == null)
            throw new NotFoundException(nameof(leaveRequest), request.Id);

        return leaveRequest;
    }

    private async Task TrySendCreateNotificationsAsync(ChangeLeaveRequestApprovalCommand request)
    {
        try
        {
            await SendCreateNotificationsAsync(request);
        }
        catch (Exception e)
        {
            _appLogger.LogWarning(e.Message);
            throw;
        }
    }

    private async Task SendCreateNotificationsAsync(ChangeLeaveRequestApprovalCommand request)
    {
        var email = new EmailMessage
        {
            To = string.Empty,
            Body = $"The approval status for your leave request for {request.StartDate:D} to {request.EndDate:D} " +
            $"has been updated.", 
            Subject = "Leave Request Approval Status Updated"
        };

        await _emailSender.SendEmail(email);
    }
}
