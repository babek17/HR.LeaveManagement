using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Query.GetLeaveAllocationDetails;

public class GetLeaveAllocationDetailQuery : IRequest<LeaveAllocationDetailsDto>
{
    public int Id { get; set; }
}