using System;
using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace gWorks.Hiring.TestConsoleApplication.Tests
{
    [TestFixture]
    public class SchoolDataServiceTests
    {
        private SchoolDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new SchoolDbContext(options);

            if (!context.Students.Any())
            {
                var teacher = new Teacher { Id = 1, FirstName = "John", LastName = "Doe" };
                var period = new ClassPeriod
                {
                    Id = 1,
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(10, 0),
                    DayOfWeek = DayOfWeek.Monday
                };

                var instructedClass = new InstructedClass
                {
                    Id = 1,
                    Teacher = teacher,
                    TeacherId = teacher.Id,
                    ClassPeriod = period,
                    ClassPeriodId = period.Id,
                    ClassroomId = 101,
                    ClassName = "Math",
                    Classroom = new Classroom { Id = 101, Name = "Room A" }
                };

                var student = new Student
                {
                    Id = 1,
                    FirstName = "Nisar",
                    LastName = "Ahmed",
                    InstructedClasses = new List<InstructedClassStudent>
                 {
                     new InstructedClassStudent
                     {
                         InstructedClass = instructedClass,
                         InstructedClassId = instructedClass.Id
                     }
                 }
                };

                context.Students.Add(student);
                context.Teachers.Add(teacher);
                context.ClassPeriods.Add(period);
                context.InstructedClasses.Add(instructedClass);
                context.SaveChanges();
            }

            return context;
        }

        [Test]
        public void GetStudentCount_ReturnsCorrectCount()
        {
            var context = GetDbContextWithData();
            var service = new SchoolDataService(context);

            var count = service.GetStudentCount();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void GetAverageClassSize_ReturnsCorrectAverage()
        {
            var context = GetDbContextWithData();
            var service = new SchoolDataService(context);

            var avg = service.GetAverageClassSize();
            Assert.That(avg, Is.EqualTo(1.0));
        }

        [Test]
        public void GetStudentSchedule_ReturnsValidSchedule()
        {
            var context = GetDbContextWithData();
            var service = new SchoolDataService(context);

            var result = service.GetStudentSchedule(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StudentName, Is.EqualTo("Nisar Ahmed"));
            Assert.That(result.Courses.Count, Is.EqualTo(1));
            Assert.That(result.Courses[0].TeacherName, Does.Contain("John"));
        }

        [Test]
        public void GetStudentSchedule_InvalidId_ReturnsNull()
        {
            var context = GetDbContextWithData();
            var service = new SchoolDataService(context);

            var result = service.GetStudentSchedule(999);
            Assert.That(result, Is.Null);
        }
    }
}

