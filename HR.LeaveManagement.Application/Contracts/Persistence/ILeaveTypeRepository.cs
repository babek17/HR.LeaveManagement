using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence;

public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
{
    Task<bool> IsLeaveTypeUnique(string name);
    Task<String?> GetNameByIdAsync(int id);
}