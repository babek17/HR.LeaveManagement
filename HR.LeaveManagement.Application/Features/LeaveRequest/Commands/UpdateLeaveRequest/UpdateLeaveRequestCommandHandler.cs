using System;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _appLogger;

    public UpdateLeaveRequestCommandHandler(IMapper mapper, IEmailSender emailSender,
        ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository,
        IAppLogger<UpdateLeaveRequestCommandHandler> appLogger)
    {
        _mapper = mapper;
        _emailSender = emailSender;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _appLogger = appLogger;
    }

    public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await GetAndValidateLeaveRequestAsync(request.Id);

        await ValidateAndMapLeaveRequest(request, leaveRequest);

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        await TrySendUpdateNotificationAsync(request);

        return Unit.Value;
    }

    private async Task<Domain.LeaveRequest> GetAndValidateLeaveRequestAsync(int id)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);

        if (leaveRequest == null)
            throw new NotFoundException(nameof(LeaveRequest), id);

        return leaveRequest;
    }

    private async Task ValidateAndMapLeaveRequest(UpdateLeaveRequestCommand request, Domain.LeaveRequest leaveRequest)
    {
        var validator = new UpdateLeaveRequestCommandValidator(_leaveRequestRepository, _leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
            throw new BadRequestException("Invalid Request Type", validationResult);

        _mapper.Map(request, leaveRequest);
    }

    private async Task TrySendUpdateNotificationAsync(UpdateLeaveRequestCommand request)
    {
        try
        {
            await SendUpdateNotificationAsync(request);
        }
        catch(Exception e)
        {
            _appLogger.LogWarning(e.Message);
            throw;
        }
    }

    private async Task SendUpdateNotificationAsync(UpdateLeaveRequestCommand request)
    {
        var email = new EmailMessage
        {
            To = string.Empty,
            Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                $"has been updated successfully.",
            Subject = "Leave Request Updated"
        };

        await _emailSender.SendEmail(email);
    }
}
