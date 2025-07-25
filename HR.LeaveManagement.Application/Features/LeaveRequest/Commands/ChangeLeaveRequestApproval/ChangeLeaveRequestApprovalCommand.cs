using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;

public class ChangeLeaveRequestApprovalCommand : BaseLeaveRequest, IRequest<Unit>
{
    public int Id { get; set; }
    public bool Approved { get; set; }
}
