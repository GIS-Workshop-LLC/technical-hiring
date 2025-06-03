using gWorks.Hiring.Services;
using gWorks.Hiring.Services.Models;
using System.Text.Json;

namespace gWorks.Hiring.TestConsoleApplication;

public class Application
{
    private readonly ISchoolDataSeedService _schoolDataSeedService;
    private readonly ISchoolDataService _schoolDataService;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public Application(ISchoolDataSeedService schoolDataSeedService, ISchoolDataService schoolDataService)
    {
        _schoolDataSeedService = schoolDataSeedService;
        _schoolDataService = schoolDataService;
    }

    public async Task Run()
    {
        await SeedSchoolDataAsync();
        await PrintSchoolStatsAsync();
        await DisplayAndExportStudentScheduleAsync(studentId: 1);

    }

    private async Task SeedSchoolDataAsync()
    {
        Console.WriteLine("Seeding school data...");
        await _schoolDataSeedService.SeedData();
        Console.WriteLine("Data seeding complete.\n");
    }

    private async Task PrintSchoolStatsAsync()
    {
        int studentCount = await _schoolDataService.GetStudentCount();
        int avgClassSize = await _schoolDataService.GetAverageClassSize();

        Console.WriteLine($"Total number of students: {studentCount}");
        Console.WriteLine($"Average class size: {avgClassSize}\n");
    }

    private async Task DisplayAndExportStudentScheduleAsync(int studentId)
    {
        var result = await _schoolDataService.GetStudentSchedule(studentId);

        if (result is not StudentScheduleDto schedule)
        {
            Console.WriteLine($"No schedule found for student ID {studentId}.");
            return;
        }

        Console.WriteLine($"Schedule for {schedule.StudentName} (ID: {schedule.StudentId}):\n");

        // group and order schedule by day of week
        var groupedByDay = schedule.Classes
            .GroupBy(c => c.DayOfWeek)
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key.ToString(),
                g => g.OrderBy(c => c.StartTime).ToList()
            );

        // console ouput
        foreach (var day in groupedByDay)
        {
            Console.WriteLine($"{day.Key}:");
            foreach (var cls in day.Value)
            {
                Console.WriteLine($"  {cls.ClassName} with {cls.Teacher} in {cls.ClassroomName} " +
                                  $"from {cls.StartTime:hh\\:mm} to {cls.EndTime:hh\\:mm}");
            }
            Console.WriteLine();
        }

        // JSON file write
        var output = new
        {
            schedule.StudentId,
            schedule.StudentName,
            ClassesByDay = groupedByDay
        };

        string json = JsonSerializer.Serialize(output, _jsonOptions);
        string filePath = $"Student_{schedule.StudentId}_Schedule.json";
        await File.WriteAllTextAsync(filePath, json);

        Console.WriteLine($"Schedule saved to: {filePath}");
    }



}