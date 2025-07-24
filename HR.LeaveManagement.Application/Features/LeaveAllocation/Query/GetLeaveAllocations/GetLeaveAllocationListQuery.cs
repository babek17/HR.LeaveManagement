using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Query.GetLeaveAllocations;

public class GetLeaveAllocationListQuery : IRequest<List<LeaveAllocationDto>>
{
}