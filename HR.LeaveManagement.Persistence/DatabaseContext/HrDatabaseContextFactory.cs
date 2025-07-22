using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HR.LeaveManagement.Persistence.DatabaseContext;

public class HrDatabaseContextFactory : IDesignTimeDbContextFactory<HrDatabaseContext>
{
    public HrDatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HrDatabaseContext>();

        // Configure your connection string here
        optionsBuilder.UseSqlServer("Server=localhost;Database=HrLeaveManagementDB;User Id=sa;Password=reallyStrongPwd123;TrustServerCertificate=True;");

        return new HrDatabaseContext(optionsBuilder.Options);
    }
}