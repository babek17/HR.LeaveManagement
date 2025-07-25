using System;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public class CancelLeaveRequestCommand : BaseLeaveRequest, IRequest<Unit>
{
    public int Id { get; set; }
}
