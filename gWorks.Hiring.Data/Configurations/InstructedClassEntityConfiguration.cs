using gWorks.Hiring.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace gWorks.Hiring.Data.Configurations;

public class InstructedClassEntityConfiguration : IEntityTypeConfiguration<InstructedClass>
{
    public void Configure(EntityTypeBuilder<InstructedClass> builder)
    {
        builder.HasOne(x => x.Teacher)
            .WithMany(x => x.InstructedClasses)
            .HasForeignKey(x => x.TeacherId);

        builder.HasOne(x => x.Classroom)
            .WithMany(x => x.InstructedClasses)
            .HasForeignKey(x => x.ClassroomId);

        builder.HasOne(x => x.ClassPeriod)
            .WithMany(x => x.InstructedClasses)
            .HasForeignKey(x => x.ClassPeriodId);
    }
}
