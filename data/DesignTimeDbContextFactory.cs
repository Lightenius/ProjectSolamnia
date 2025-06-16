using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectSolamnia;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectSolamniaDbContext>
{
    public ProjectSolamniaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectSolamniaDbContext>();
        optionsBuilder.UseSqlite("Data Source=projectsolamnia.db");

        return new ProjectSolamniaDbContext(optionsBuilder.Options);
    }
}
// bu kod migration yapabilmemizi sağlıyor