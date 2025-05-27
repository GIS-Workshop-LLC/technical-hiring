using gWorks.Hiring.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gWorks.Hiring.Data.Configurations;

public class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
