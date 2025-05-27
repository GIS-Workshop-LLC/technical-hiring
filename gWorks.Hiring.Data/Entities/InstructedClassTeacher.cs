namespace gWorks.Hiring.Data.Entities;

public class InstructedClassTeacher
{
    public int TeacherId { get; set; }

    public required Teacher Teacher { get; set; }

    public int InstructedClassId { get; set; }

    public required InstructedClass InstructedClass { get; set; }
}
