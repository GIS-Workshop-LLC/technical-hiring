using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.Data;
using gWorks.Hiring.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gWorks.Hiring.Services
{
    public class SchoolDataService : ISchoolDataService
    {
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<InstructedClassStudent> _classStudentRepo;
        private readonly IRepository<InstructedClass> _classRepo;

        public SchoolDataService(
            IRepository<Student> studentRepo,
            IRepository<InstructedClassStudent> classStudentRepo,
            IRepository<InstructedClass> classRepo)
        {
            _studentRepo = studentRepo;
            _classStudentRepo = classStudentRepo;
            _classRepo = classRepo;
        }

        /// <summary>
        /// Get the number of students at the school
        /// </summary>
        public int GetStudentCount()
        {
            return _studentRepo.Query.Count();
        }

        /// <summary>
        /// Get the average number of students in each class
        /// </summary>
        public int GetAverageClassSize()
        {
            var classes = _classRepo.Query.ToList();
            if (classes.Count == 0)
                return 0;

            int totalStudents = classes.Sum(c => c.Students.Count);
            return totalStudents / classes.Count;
        }

        /// <summary>
        /// Gets a student's weekly schedule (sorted by day-of-week, then by start time).
        /// 
        /// NOTE: You should create the requisite model and implement the method
        /// </summary>
        /// <param name="studentId">Student.Id (primary key)</param>
        /// <returns>A DTO (model) of the student's schedule</returns>
        public StudentScheduleDto GetStudentSchedule(int studentId)
        {
            var student = _studentRepo.GetById(studentId);
            if (student == null) return null;

            var instructedClasses = student.InstructedClasses
                .Select(s => s.InstructedClass)
                .OrderBy(c => c.ClassPeriod.DayOfWeek)
                .ThenBy(c => c.ClassPeriod.StartTime)
                .ToList();

            var schedule = instructedClasses.Select(c => new ClassScheduleDto
            {
                ClassName = c.ClassName,
                InstructorName = c.Teacher != null ? $"{c.Teacher.FirstName} {c.Teacher.LastName}" : "TBD",
                DayOfWeek = c.ClassPeriod.DayOfWeek,
                StartTime = c.ClassPeriod.StartTime,
                EndTime = c.ClassPeriod.EndTime
            }).ToList();

            return new StudentScheduleDto
            {
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                Schedule = schedule
            };
        }
    }

}
