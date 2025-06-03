using gWorks.Hiring.Services.Models;

namespace gWorks.Hiring.Services;

public interface ISchoolDataService
{
    /// <summary>
    /// Get the number of students at the school
    /// </summary>
    Task<int> GetStudentCount();

    /// <summary>
    /// Get the average number of students in each class
    /// </summary>
    /// reurns>The average class size, rounded up to the nearest integer</returns>
    Task<int> GetAverageClassSize();

    /// <summary>
    /// Gets a student's weekly schedule (sorted by day-of-week, then by start time).
    /// </summary>
    /// <param name="studentId">Student.Id (primary key)</param>
    /// <returns>A StudentScheduleDto of the student's schedule</returns>
    Task<StudentScheduleDto?> GetStudentSchedule(int studentId);
}