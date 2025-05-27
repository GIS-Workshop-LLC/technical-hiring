using Microsoft.EntityFrameworkCore;

namespace gWorks.Hiring.Data;

internal class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly IDbContext _dbContext;

    public Repository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private DbSet<TEntity> DbSet => _dbContext.GetDbSet<TEntity>();

    public IQueryable<TEntity> Query => DbSet;

    public TEntity? GetById(params object?[] ids)
    {
        return DbSet.Find(ids);
    }

    public ValueTask<TEntity?> GetByIdAsync(params object?[] ids)
    {
        return DbSet.FindAsync(ids);
    }

    public void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);
    public Task AddRangeAsync(IEnumerable<TEntity> entities) => DbSet.AddRangeAsync(entities);

    public void Add(params TEntity[] entities) => DbSet.AddRange(entities);
    public Task AddAsync(params TEntity[] entities) => DbSet.AddRangeAsync(entities);

    public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

}