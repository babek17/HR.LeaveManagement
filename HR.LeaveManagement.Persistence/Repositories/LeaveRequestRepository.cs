using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestRepository(HrDatabaseContext context) : base(context)
    {
    }

    public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
    {
        var leaveRequest = await Context.LeaveRequests
            .Include(q => q.LeaveType)
            .FirstOrDefaultAsync(q=> q.Id == id);
        return leaveRequest;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
    {
        var leaveRequests = await Context.LeaveRequests
            .Include(q => q.LeaveType)
            .ToListAsync();
        return leaveRequests;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
    {
        var leaveRequests = await Context.LeaveRequests
            .Where(q=>q.RequestingEmployeeId==userId)
            .Include(q => q.LeaveType)
            .ToListAsync();
        return leaveRequests;
    }
}