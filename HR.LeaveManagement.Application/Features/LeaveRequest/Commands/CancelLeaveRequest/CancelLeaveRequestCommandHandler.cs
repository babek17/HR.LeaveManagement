using System;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<CancelLeaveRequestCommandHandler> _appLogger;

    public CancelLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository,
        IEmailSender emailSender,
        IAppLogger<CancelLeaveRequestCommandHandler> appLogger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _emailSender = emailSender;
        _appLogger = appLogger;
    }

    public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await CheckIfLeaveRequestExists(request);

        leaveRequest.Cancelled = true;

        await TrySendCreateNotificationsAsync(request);

        return Unit.Value;
    }

    private async Task<Domain.LeaveRequest> CheckIfLeaveRequestExists(CancelLeaveRequestCommand request)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

        if (leaveRequest == null)
            throw new NotFoundException(nameof(LeaveRequest), request.Id);

        return leaveRequest;
    }

     private async Task TrySendCreateNotificationsAsync(CancelLeaveRequestCommand request)
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

    private async Task SendCreateNotificationsAsync(CancelLeaveRequestCommand request)
    {
        var email = new EmailMessage
        {
            To = string.Empty,
            Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
            $"has been cancelled successfully.",
            Subject = "Leave Request Deleted"
        };

        await _emailSender.SendEmail(email);
    }
}
