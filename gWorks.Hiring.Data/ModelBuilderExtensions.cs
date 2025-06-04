using Fasterflect;
using gWorks.Hiring.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace gWorks.Hiring.Data;

internal static class ModelBuilderExtensions
{
    internal static void RegisterBaseEntities(this ModelBuilder modelBuilder)
    {
        var entityTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(t => !t.IsAbstract && t.IsClass && t.InheritsOrImplements<BaseEntity>())
            .ToList();

        foreach (var type in entityTypes)
        {
            RegisterEntity(modelBuilder, type)
                .HasKey(nameof(BaseEntity.Id));
        }
    }
    internal static EntityTypeBuilder<T> RegisterEntity<T>(this ModelBuilder modelBuilder) where T : class => modelBuilder.Entity<T>();
    internal static EntityTypeBuilder RegisterEntity(this ModelBuilder modelBuilder, Type t) => modelBuilder.Entity(t);
}
