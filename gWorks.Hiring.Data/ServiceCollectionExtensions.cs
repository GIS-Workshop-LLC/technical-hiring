using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace gWorks.Hiring.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchoolDbContext(this IServiceCollection services)
    {
        var builder = new DbContextOptionsBuilder<SchoolDbContext>();

        return services
            .AddDbContext<SchoolDbContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(SchoolDbContext))
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            })
            .AddScoped<IDbContext, SchoolDbContext>()
            .AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}