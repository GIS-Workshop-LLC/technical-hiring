namespace gWorks.Hiring.Data.Entities;

public class ClassPeriod : BaseEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public bool IsLunch { get; set; }

    public ICollection<InstructedClass> InstructedClasses { get; set; } = [];
}
