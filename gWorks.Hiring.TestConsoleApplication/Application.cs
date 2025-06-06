using ClosedXML.Excel;
using gWorks.Hiring.Services;
using gWorks.Hiring.Services.Models;
using System.Text;

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
        // TODO: add any other services here
        Console.WriteLine($"Total Students: {_schoolDataService.GetStudentCount()}");
        Console.WriteLine($"Average Class Size: {_schoolDataService.GetAverageClassSize()}");

        int exampleStudentId = 4;
        var scheduleDto = _schoolDataService.GetStudentSchedule(exampleStudentId);

        if (scheduleDto == null)
        {
            Console.WriteLine($"No student found with ID {exampleStudentId}");
            return;
        }

        Console.WriteLine($"Schedule for {scheduleDto.StudentName}:");

        foreach (var cls in scheduleDto.Schedule)
        {
            Console.WriteLine($"{cls.DayOfWeek} {cls.StartTime} - {cls.EndTime}: {cls.ClassName} (Instructor: {cls.InstructorName})");
        }

        // To write code to file, just one other way
        #region Schedule to Text file and Excel file
        StringBuilder output = new StringBuilder();
        output.AppendLine($"Schedule for {scheduleDto.StudentName}:");
        foreach (var cls in scheduleDto.Schedule)
        {
            string line = $"{cls.DayOfWeek} {cls.StartTime} - {cls.EndTime}: {cls.ClassName} (Instructor: {cls.InstructorName})";
            Console.WriteLine(line);
            output.AppendLine(line);
        }
        // Write output to file
        string filePath = $"{scheduleDto.StudentName} StudentSchedule.txt";
        File.WriteAllText(filePath, output.ToString());

        ExportScheduleToExcel(scheduleDto, $"{scheduleDto.StudentName} StudentSchedule.xlsx");
        Console.WriteLine($"\nSchedule also written to: {filePath}");
        #endregion
    }

    #region To extract the schedule to Excel format
    public static void ExportScheduleToExcel(StudentScheduleDto scheduleDto, string filePath)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Schedule");

        worksheet.Cell(1, 1).Value = "Day";
        worksheet.Cell(1, 2).Value = "Start Time";
        worksheet.Cell(1, 3).Value = "End Time";
        worksheet.Cell(1, 4).Value = "Class Name";
        worksheet.Cell(1, 5).Value = "Instructor";

        worksheet.Range("A1:E1").Style.Font.Bold = true;

        int row = 2;

        var groupedSchedule = scheduleDto.Schedule
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime)
            .GroupBy(s => s.DayOfWeek);

        foreach (var group in groupedSchedule)
        {
            bool isFirst = true;
            foreach (var cls in group)
            {
                worksheet.Cell(row, 1).Value = isFirst ? group.Key.ToString() : "";
                worksheet.Cell(row, 2).Value = cls.StartTime.ToString();
                worksheet.Cell(row, 3).Value = cls.EndTime.ToString();
                worksheet.Cell(row, 4).Value = cls.ClassName;
                worksheet.Cell(row, 5).Value = cls.InstructorName;

                isFirst = false;
                row++;
            }
        }

        workbook.SaveAs(filePath);
    }
    #endregion
}