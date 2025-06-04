using gWorks.Hiring.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace gWorks.Hiring.Data;

public class SchoolDbContext : DbContext, IDbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
    : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<InstructedClass> InstructedClasses => Set<InstructedClass>();
    public DbSet<InstructedClassStudent> InstructedClassStudents => Set<InstructedClassStudent>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<ClassPeriod> ClassPeriods => Set<ClassPeriod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<InstructedClassStudent>()
            .HasKey(ics => new { ics.StudentId, ics.InstructedClassId });

        
        modelBuilder.Entity<InstructedClassStudent>()
            .HasOne(ics => ics.Student)
            .WithMany(s => s.InstructedClasses)
            .HasForeignKey(ics => ics.StudentId);

        modelBuilder.Entity<InstructedClassStudent>()
            .HasOne(ics => ics.InstructedClass)
            .WithMany(ic => ic.Students)
            .HasForeignKey(ics => ics.InstructedClassId);
    }

    DbSet<T> IDbContext.GetDbSet<T>() => Set<T>();
}
