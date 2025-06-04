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

        // Fetch schedule for student ID 1
        var schedule = _schoolDataService.GetStudentSchedule(2);

        if (schedule == null)
        {
            Console.WriteLine("Student with ID 2 wasn't found.");
            return;
        }


        Console.WriteLine($"++++++++++++Student Schedule++++++++++++");
        Console.WriteLine($"Name      : {schedule.StudentName}");
        Console.WriteLine($"Student ID: {schedule.StudentId}");
        Console.WriteLine($"Enrolled Courses: {schedule.Courses.Count}");

        foreach (var course in schedule.Courses.OrderBy(c=>c.CourseName))
        {
            Console.WriteLine($"======================================");
            Console.WriteLine($" {course.CourseName}");
            Console.WriteLine($" Teacher : {course.TeacherName}");
            Console.WriteLine($" Time    : {course.Time}");
            Console.WriteLine($"======================================");
        }

    }
}