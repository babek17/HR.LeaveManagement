using System;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;

public class LeaveRequestDetailsDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string RequestingEmployeeId { get; set; } = string.Empty;
    public LeaveTypeDto LeaveType { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime DataRequested { get; set; }
    public string RequestComments { get; set; } = string.Empty;
    public DateTime? DateActioned { get; set; }
    public bool? Approved { get; set; }
    public bool Cancelled { get; set; } 
}