using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace gWorks.Hiring.Data;

internal class SchoolDbContext : DbContext, IDbContext
{
    public SchoolDbContext(DbContextOptions options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    DbSet<T> IDbContext.GetDbSet<T>() => Set<T>();
}
