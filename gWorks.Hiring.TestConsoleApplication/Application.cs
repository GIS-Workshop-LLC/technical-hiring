using gWorks.Hiring.Services;

namespace gWorks.Hiring.TestConsoleApplication;

public class Application
{
    private readonly ISchoolDataSeedService _schoolDataSeedService;
    private readonly ISchoolDataService _schoolDataService;

    public Application(ISchoolDataSeedService schoolDataSeedService, ISchoolDataService schoolDataService)
    {
        _schoolDataSeedService = schoolDataSeedService;
        _schoolDataService = schoolDataService;
    }

    public async Task Run()
    {
        await _schoolDataSeedService.SeedData();
        var schedule = _schoolDataService.GetStudentSchedule(1);
        if (schedule == null)
        {
            Console.WriteLine("Student is not found");
            return;
        }

        Console.WriteLine($"Schedule for {schedule.StudentName}:");
        foreach (var course in schedule.Courses)
        {
            Console.WriteLine($"-{course.CourseName} with {course.TeacherName} at {course.Time}");
        }
    }
}