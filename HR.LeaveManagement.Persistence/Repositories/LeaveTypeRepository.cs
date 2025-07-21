using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(HrDatabaseContext context) : base(context)
    {
    }

    public async Task<bool> IsLeaveTypeUnique(string name)
    {
        return await Context.LeaveTypes.AnyAsync(p=>p.Name==name);
    }

    public async Task<string?> GetNameByIdAsync(int id)
    {
        var leaveType = await Context.LeaveTypes.FindAsync(id);
        if (leaveType == null)
        {
            return null;
        }

        return leaveType.Name;
    }
}