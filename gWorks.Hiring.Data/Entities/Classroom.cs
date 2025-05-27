namespace gWorks.Hiring.Data.Entities;

public class Classroom : BaseEntity
{
    public required string Name { get; set; }

    public bool IsLunchroom { get; set; }

    public ICollection<InstructedClass> InstructedClasses { get; set; } = [];
}
