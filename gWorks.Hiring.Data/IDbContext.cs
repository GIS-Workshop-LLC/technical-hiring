using Microsoft.EntityFrameworkCore;

namespace gWorks.Hiring.Data;

public interface IDbContext : IDisposable
{
    DbSet<T> GetDbSet<T>() where T : class;

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
