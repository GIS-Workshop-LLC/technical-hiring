namespace gWorks.Hiring.Data.Entities;

public class InstructedClassStudent
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public int StudentId { get; set; }

    public Student Student { get; set; }

    public int InstructedClassId { get; set; }

    public InstructedClass InstructedClass { get; set; }
#pragma warning restore CS861
}
