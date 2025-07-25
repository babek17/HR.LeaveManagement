using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _appLogger;

    public CreateLeaveRequestCommandHandler(
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

    public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await ValidateAndMapLeaveRequestAsync(request);

        await _leaveRequestRepository.CreateAsync(leaveRequest);

        await TrySendCreateNotificationsAsync(request);

        return Unit.Value;
    }

    private async Task<Domain.LeaveRequest> ValidateAndMapLeaveRequestAsync(CreateLeaveRequestCommand request)
    {
        var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
        var validatonResult = await validator.ValidateAsync(request);

        if (validatonResult.Errors.Any())
            throw new BadRequestException("Invalid Leave Request", validatonResult);

        var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
        return leaveRequest;
    }

    private async Task TrySendCreateNotificationsAsync(CreateLeaveRequestCommand request)
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

    private async Task SendCreateNotificationsAsync(CreateLeaveRequestCommand request)
    {
        var email = new EmailMessage
        {
            To = string.Empty,
            Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
            $"has been submitted successfully.", 
            Subject = "Leave Request Created"
        };

        await _emailSender.SendEmail(email);
    }
}