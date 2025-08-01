using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequest;

public class LeaveRequestDto
{
    public string RequestingEmployeeId { get; set; } = string.Empty;
    public LeaveTypeDto LeaveType { get; set; }
    public DateTime DataRequested { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool? Approved { get; set; } 
}
