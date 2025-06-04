using System;
using gWorks.Hiring.Services.Models;
using gWorks.Hiring.Data;
using Microsoft.EntityFrameworkCore;

namespace gWorks.Hiring.Services
{
    public class SchoolDataService : ISchoolDataService
    {

        private readonly SchoolDbContext _context;

        public SchoolDataService(SchoolDbContext context)
        {
            _context = context;
        }

        public StudentScheduleDto GetStudentSchedule(int studentId)
        {
            var student = _context.Students
            .Include(s => s.InstructedClasses)
            .ThenInclude(ic => ic.InstructedClass)
            .ThenInclude(c => c.Teacher)
            .Include(s => s.InstructedClasses)
            .ThenInclude(ic => ic.InstructedClass)
            .ThenInclude(c => c.ClassPeriod)
            .FirstOrDefault(s => s.Id == studentId);

            if (student == null)
                return null;

            var courses = student.InstructedClasses.Select(ic => new CourseScheduleDto
            {
                CourseName = $"Classroom {ic.InstructedClass.ClassroomId}", // Customize if needed
                TeacherName = (string.IsNullOrWhiteSpace(ic.InstructedClass.Teacher?.FirstName) &&
               string.IsNullOrWhiteSpace(ic.InstructedClass.Teacher?.LastName))
               ? "TBD"
               : $"{ic.InstructedClass.Teacher?.FirstName} {ic.InstructedClass.Teacher?.LastName}".Trim(),
                Time = ic.InstructedClass.ClassPeriod != null
            ? $"{ic.InstructedClass.ClassPeriod.DayOfWeek} {ic.InstructedClass.ClassPeriod.StartTime} - {ic.InstructedClass.ClassPeriod.EndTime}"
            : "N/A"

            }).ToList();

            return new StudentScheduleDto
            {
                StudentId=student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                Courses = courses
            };
        }

        public int GetStudentCount()
        {
            return _context.Students.Count();
        }

        public double GetAverageClassSize()
        {
            var classSizes = _context.InstructedClasses
            .Include(c => c.Students)
            .Select(c => c.Students.Count);

            return classSizes.Any() ? classSizes.Average() : 0.0;
        }

    }

}

