namespace gWorks.Hiring.Data;

public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Gets a query on a table referenced by '<typeparamref name="TEntity"/>'
    /// </summary>
    IQueryable<TEntity> Query { get; }

    /// <summary>
    /// Gets an entity referenced by the primary key(s)
    /// </summary>
    /// <param name="ids">primary  key(s)</param>
    /// <returns>The record if it exists, otherwise null</returns>
    TEntity? GetById(params object?[] ids);

    /// <summary>
    /// Gets an entity referenced by the primary key(s)
    /// </summary>
    /// <param name="ids">primary  key(s)</param>
    /// <returns>The record if it exists, otherwise null</returns>
    ValueTask<TEntity?> GetByIdAsync(params object?[] ids);

    /// <summary>
    /// Add a list of entities to the table referenced by '<typeparamref name="TEntity"/>'
    /// </summary>
    /// <param name="entities">entities to add to the repository</param>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Add a list of entities to the table referenced by '<typeparamref name="TEntity"/>'
    /// </summary>
    /// <param name="entities">entities to add to the repository</param>
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Add one or more entities to the table referenced by '<typeparamref name="TEntity"/>'
    /// </summary>
    /// <param name="entities">entities to add to the repository</param>
    void Add(params TEntity[] entities);

    /// <summary>
    /// Add one or more of entities to the table referenced by '<typeparamref name="TEntity"/>'
    /// </summary>
    /// <param name="entities">entities to add to the repository</param>
    Task AddAsync(params TEntity[] entities);

    /// <summary>
    /// Removes specified entities from the context / table
    /// </summary>
    /// <param name="entities"></param>
    void RemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Removes specified entities from the context / table
    /// </summary>
    /// <param name="entities"></param>
    void Remove(params TEntity[] entities);

    /// <summary>
    /// Saves changes to tracked entities. This method affects all repositories within the same scoped context
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// Saves changes to tracked entities. This method affects all repositories within the same scoped context
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
}
