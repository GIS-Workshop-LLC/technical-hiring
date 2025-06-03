using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gWorks.Hiring.Services
{
    public class SchoolDataService(IRepository<Student> studentRepo, IRepository<InstructedClass> classRepo) : ISchoolDataService
    {
        private readonly IRepository<Student> _studentRepo = studentRepo;
        private readonly IRepository<InstructedClass> _classRepo = classRepo;

        public Task<int> GetStudentCount()
        {
            return _studentRepo.Query.CountAsync();
        }

        public async Task<int> GetAverageClassSize()
        {
            var classSizes = await _classRepo.Query
                .Select(c => c.Students.Count)
                .ToListAsync();

            return classSizes.Count != 0 ? (int)Math.Ceiling(classSizes.Average()) : 0;
        }

        public async Task<StudentScheduleDto?> GetStudentSchedule(int studentId)
        {
            // query for the student and include their instructed classes, class periods, teachers, and classrooms
            var student = await _studentRepo.Query
                .AsNoTracking()
                .Include(s => s.InstructedClasses)
                    .ThenInclude(sic => sic.InstructedClass)
                        .ThenInclude(ic => ic.ClassPeriod)
                .Include(s => s.InstructedClasses)
                    .ThenInclude(sic => sic.InstructedClass)
                        .ThenInclude(ic => ic.Teacher)
                .Include(s => s.InstructedClasses)
                    .ThenInclude(sic => sic.InstructedClass)
                        .ThenInclude(ic => ic.Classroom)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return null;
            }

            // create the schedule DTO from the student's instructed classes
            var schedule = student.InstructedClasses
                .Select(icStudent => icStudent.InstructedClass)
                .Where(ic => ic?.ClassPeriod != null)
                .OrderBy(ic => ic.ClassPeriod.DayOfWeek)
                .ThenBy(ic => ic.ClassPeriod.StartTime)
                .Select(ic => new ClassScheduleDto
                {
                    ClassName = ic.ClassName,
                    ClassroomName = ic.Classroom?.Name ?? "Unknown Classroom",
                    Teacher = ic.Teacher != null ? $"{ic.Teacher.FirstName} {ic.Teacher.LastName}" : "No Teacher Assigned",
                    StartTime = ic.ClassPeriod.StartTime,
                    EndTime = ic.ClassPeriod.EndTime,
                    DayOfWeek = ic.ClassPeriod.DayOfWeek
                })
                .ToList();

            return new StudentScheduleDto
            {
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                Classes = schedule
            };
        }


    }
}
