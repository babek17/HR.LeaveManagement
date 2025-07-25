using System;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequest;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries;

public class GetLeaveRequesListQuery : IRequest<List<LeaveRequestDto>>
{

}
