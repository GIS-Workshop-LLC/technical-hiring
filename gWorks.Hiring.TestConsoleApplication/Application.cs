using gWorks.Hiring.Services;

namespace gWorks.Hiring.TestConsoleApplication;

public class Application
{
    private readonly ISchoolDataSeedService _schoolDataSeedService;
    private readonly IStudentService _studentService;

    public Application(ISchoolDataSeedService schoolDataSeedService, IStudentService studentService)
    {
        _schoolDataSeedService = schoolDataSeedService;
        _studentService = studentService;
    }

    public async Task Run()
    {
        await _schoolDataSeedService.SeedData();
        Console.WriteLine("STUDENTS:" + _studentService.StudentCount());
        // TODO: add any other services here
    }
}