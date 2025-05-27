namespace gWorks.Hiring.Data.Entities;

public class InstructedClass : BaseEntity
{
    public int? TeacherId { get; set; }

    public Teacher? Teacher { get; set; }

    public int ClassroomId { get; set; }

    public required Classroom Classroom { get; set; }

    public int ClassPeriodId { get; set; }

    public required ClassPeriod ClassPeriod { get; set; }

    public ICollection<InstructedClassStudent> Students { get; set; } = [];
}
