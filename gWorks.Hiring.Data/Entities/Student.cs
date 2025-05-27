namespace gWorks.Hiring.Data.Entities;

public class Student : BaseEntity, IPerson
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }

    public ICollection<InstructedClassStudent> InstructedClasses { get; set; } = [];
}
