namespace gWorks.Hiring.Data.Entities;

public class Teacher : BaseEntity, IPerson
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }

    public ICollection<InstructedClass> InstructedClasses { get; set; } = [];
}
