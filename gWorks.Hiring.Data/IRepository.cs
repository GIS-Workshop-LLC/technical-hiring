namespace gWorks.Hiring.Data;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Query { get; }

    TEntity? GetById(params object?[] ids);

    ValueTask<TEntity?> GetByIdAsync(params object?[] ids);

    void AddRange(IEnumerable<TEntity> entities);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Add(params TEntity[] entities);

    Task AddAsync(params TEntity[] entities);

    void RemoveRange(IEnumerable<TEntity> entities);
}
