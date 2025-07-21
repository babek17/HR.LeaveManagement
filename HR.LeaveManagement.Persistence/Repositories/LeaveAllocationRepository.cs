using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
    {
    }

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
    {
        var leaveallocation = await Context.LeaveAllocations
            .Include(l => l.LeaveType)
            .FirstOrDefaultAsync(q=>q.Id==id);
        return leaveallocation;
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        return await Context.LeaveAllocations
            .Include(l => l.LeaveType)
            .ToListAsync();
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
    {
        return await Context.LeaveAllocations
            .Where(l=>l.EmployeeId==userId)
            .Include(l => l.LeaveType)
            .ToListAsync();
    }

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
    { 
        return await Context.LeaveAllocations.AnyAsync(q=>q.EmployeeId==userId && q.LeaveTypeId == leaveTypeId && q.Period == period);
    }

    public async Task AddAllocations(List<LeaveAllocation> allocations)
    {
        await Context.AddRangeAsync(allocations);
        await Context.SaveChangesAsync();
    }

    public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
    {
        var leaveAllocation = await Context.LeaveAllocations.FirstOrDefaultAsync(q=>q.EmployeeId==userId && q.LeaveTypeId == leaveTypeId);
        return leaveAllocation;
    }
}