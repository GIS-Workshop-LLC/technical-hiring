namespace gWorks.Hiring.Services;

public interface ISchoolDataService
{
    /// <summary>
    /// Get the number of students at the school
    /// </summary>
    int GetStudentCount();

    /// <summary>
    /// Get the average number of students in each class
    /// </summary>
    int GetAverageClassSize();

    /// <summary>
    /// Gets a student's weekly schedule (sorted by day-of-week, then by start time).
    /// 
    /// NOTE: You should create the requisite model and implement the method
    /// </summary>
    /// <param name="studentId">Student.Id (primary key)</param>
    /// <returns>A DTO (model) of the student's schedule</returns>
    object GetStudentSchedule(int studentId);
}