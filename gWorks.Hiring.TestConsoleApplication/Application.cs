using gWorks.Hiring.Services;

namespace gWorks.Hiring.TestConsoleApplication;

public class Application
{
    private readonly ISchoolDataSeedService _schoolDataSeedService;

    public Application(ISchoolDataSeedService schoolDataSeedService)
    {
        _schoolDataSeedService = schoolDataSeedService;
    }

    public async Task Run()
    {
        await _schoolDataSeedService.SeedData();
        // TODO: add any other services here
    }
}