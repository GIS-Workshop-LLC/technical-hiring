using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using gWorks.Hiring.Services;
using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.TestConsoleApplication.Tests.TestData;

namespace gWorks.Hiring.TestConsoleApplication.Tests
{
    [TestFixture]
    public class SchoolDataServiceTests
    {
        private Mock<IRepository<Student>> _studentRepoMock = null!;
        private Mock<IRepository<InstructedClass>> _classRepoMock = null!;
        private Mock<IRepository<InstructedClassStudent>> _enrollmentRepoMock = null!;
        private SchoolDataService _service = null!;

        [SetUp]
        public void Setup()
        {
            _studentRepoMock = new Mock<IRepository<Student>>();
            _enrollmentRepoMock = new Mock<IRepository<InstructedClassStudent>>();
            _classRepoMock = new Mock<IRepository<InstructedClass>>();

            _service = new SchoolDataService(
                _studentRepoMock.Object,
                _enrollmentRepoMock.Object,
                _classRepoMock.Object);
        }

        [Test]
        public void BasicTest()
        {
            Assert.That(2, Is.EqualTo(1+1));
        }

        [Test]
        public void GetStudentCount_ReturnsCorrectCount()
        {
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName="Mike",LastName="R"},
                new Student { Id = 2 , FirstName="Srini", LastName="M"},
                new Student { Id = 3, FirstName="Olyvia", LastName="S" }
            }.AsQueryable();

            _studentRepoMock.Setup(r => r.Query).Returns(students);


            var count = _service.GetStudentCount();
            Assert.That(count, Is.EqualTo(3));

        }

        [Test]
        public void GetAverageClassSize_ReturnsCorrectAverage()
        {
            var classes = SampleStudentData.GetMockInstructedClasses(2).ToList();

            _classRepoMock.Setup(r => r.Query).Returns(classes.AsQueryable);
            var average = _service.GetAverageClassSize();

            Assert.That(average == 2);

        }

        [Test]
        public void GetStudentCount_WhenNoStudents_ReturnsZero()
        {
            _studentRepoMock.Setup(r => r.Query).Returns(new List<Student>().AsQueryable());

            var count = _service.GetStudentCount();

            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void GetAverageClassSize_WhenNoClasses_ReturnsZero()
        {
            _classRepoMock.Setup(r => r.Query).Returns(new List<InstructedClass>().AsQueryable());

            var average = _service.GetAverageClassSize();

            Assert.That(average, Is.EqualTo(0));
        }

        [Test]
        public void GetAverageClassSize_CalculatesAverageCorrectly()
        {
            var classes = SampleStudentData.GetMockInstructedClasses(2).ToList();

            _classRepoMock.Setup(r => r.Query).Returns(classes.AsQueryable);

            var average = _service.GetAverageClassSize();

            Assert.That(average, Is.EqualTo(2)); 
        }

        [Test]
        public void GetStudentSchedule_InvalidStudentId_ReturnsNull()
        {
            _studentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Student?)null);

            var schedule = _service.GetStudentSchedule(999);

            Assert.That(schedule, Is.Null);
        }

        [Test]
        public void GetStudentSchedule_ValidStudentId_ReturnsSchedule()
        {
            var student = SampleStudentData.GetMockStudentWithSchedule;

        _studentRepoMock.Setup(r => r.GetById(1)).Returns(student);

            var schedule = _service.GetStudentSchedule(1);

            Assert.That(schedule, Is.Not.Null);
            Assert.That(schedule.StudentName, Is.EqualTo("Srinivas Musinuri"));
            Assert.That(schedule.Schedule.Count, Is.EqualTo(1));
            Assert.That(schedule.Schedule[0].ClassName, Is.EqualTo("Math"));
            Assert.That(schedule.Schedule[0].InstructorName, Is.EqualTo("Jane Smith"));
        }


        [Test]
        public void GetStudentSchedule_ClassesAreSortedByDayAndStartTime()
        {
            var student = new Student
            {
                Id = 2,
                FirstName = "Alice",
                LastName = "Brown",
                InstructedClasses = new List<InstructedClassStudent>
        {
            new InstructedClassStudent
            {
                InstructedClass = new InstructedClass
                {
                    ClassName = "Science",
                   Classroom=new Classroom(){Name="Maths"},
                    Teacher = new Teacher { FirstName = "Bob", LastName = "Taylor" },
                    ClassPeriod = new ClassPeriod
                    {
                        DayOfWeek = DayOfWeek.Wednesday,
                        StartTime = new TimeOnly(14, 0, 0),
                        EndTime = new TimeOnly(15, 0, 0)
                    }
                }
            },
            new InstructedClassStudent
            {
                InstructedClass = new InstructedClass
                {
                    ClassName = "Art",
                    Classroom=new Classroom(){Name="Maths"},
                    Teacher = new Teacher { FirstName = "Carol", LastName = "Davis" },
                    ClassPeriod = new ClassPeriod
                    {
                        DayOfWeek = DayOfWeek.Monday,
                        StartTime = new TimeOnly(9, 0, 0),
                        EndTime = new TimeOnly(10, 0, 0)
                    }
                }
            }
        }
            };

            _studentRepoMock.Setup(r => r.GetById(2)).Returns(student);

            var schedule = _service.GetStudentSchedule(2);

            Assert.That(schedule, Is.Not.Null);
            Assert.That(schedule!.Schedule[0].ClassName, Is.EqualTo("Art"));
            Assert.That(schedule.Schedule[1].ClassName, Is.EqualTo("Science"));
        }

    }
}

