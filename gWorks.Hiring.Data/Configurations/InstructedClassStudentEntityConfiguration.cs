using gWorks.Hiring.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gWorks.Hiring.Data.Configurations;

public class InstructedClassStudentEntityConfiguration : IEntityTypeConfiguration<InstructedClassStudent>
{
    public void Configure(EntityTypeBuilder<InstructedClassStudent> builder)
    {
        builder.HasKey(x => new { x.StudentId, x.InstructedClassId });

        builder.HasOne(x => x.InstructedClass)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.InstructedClassId);

        builder.HasOne(x => x.Student)
            .WithMany(x => x.InstructedClasses)
            .HasForeignKey(x => x.StudentId);
    }
}