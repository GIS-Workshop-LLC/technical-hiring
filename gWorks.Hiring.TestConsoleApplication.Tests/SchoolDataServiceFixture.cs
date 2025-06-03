using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.Services;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gWorks.Hiring.TestConsoleApplication.Tests
{
    [TestFixture]
    public class SchoolDataServiceFixture
    {
        private Mock<IRepository<Student>> _mockStudentRepo = null!;
        private Mock<IRepository<InstructedClass>> _mockClassRepo = null!;
        private SchoolDataService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _mockStudentRepo = new Mock<IRepository<Student>>();
            _mockClassRepo = new Mock<IRepository<InstructedClass>>();
            _service = new SchoolDataService(_mockStudentRepo.Object, _mockClassRepo.Object);
        }

        [Test]
        public async Task StudentCount_ReturnsCorrectCount_WhenStudentsExist()
        {
            var students = new List<Student>
            {
                new() { FirstName = "Testy", LastName = "McTesterton"},
                new() { FirstName = "Shenanigans", LastName = "O'neil"},
                new() { FirstName = "Cache", LastName = "Money"}
            };
            SetupMockStudentRepo(students);

            Assert.That(await _service.GetStudentCount(), Is.EqualTo(3));
        }

        [Test]
        public async Task StudentCount_ReturnsZero_WhenNoStudentsExist()
        {
            SetupMockStudentRepo(new List<Student>());
            Assert.That(await _service.GetStudentCount(), Is.EqualTo(0));
        }

        [Test]
        public async Task AverageClassSize_ReturnsCorrectValue_WhenClassesExist()
        {
            var classes = new List<InstructedClass>
            {
                new() {
                    ClassName = "Math II",
                    Classroom = new Classroom { Name = "1-A"},
                    ClassPeriod = new ClassPeriod {
                        DayOfWeek = DayOfWeek.Monday,
                        StartTime = new TimeOnly(9, 0),
                        EndTime = new TimeOnly(10, 30),
                    },
                    Students = new List<InstructedClassStudent>
                    {
                        new() { Student = new Student {
                            FirstName = "Testy", LastName = "McTesterton"}},
                        new() { Student = new Student {
                            FirstName = "Shenanigans", LastName = "O'neil"}}
                    }
                },
                new() {
                    ClassName = "Chemistry 1",
                    Classroom = new Classroom { Name = "1-B"},
                    ClassPeriod = new ClassPeriod {
                        DayOfWeek = DayOfWeek.Tuesday,
                        StartTime = new TimeOnly(11, 0),
                        EndTime = new TimeOnly(12, 30),
                    },
                    Students = new List<InstructedClassStudent>
                    {
                        new() { Student = new Student {
                            FirstName = "Cache", LastName = "Money"}}
                    }
                }
            };
            SetupMockClassRepo(classes);

            Assert.That(await _service.GetAverageClassSize(), Is.EqualTo(2));
        }

        [Test]
        public async Task AverageClassSize_ReturnsZero_WhenNoClassesExist()
        {
            SetupMockClassRepo(new List<InstructedClass>());
            Assert.That(await _service.GetAverageClassSize(), Is.EqualTo(0));
        }

        [Test]
        public async Task StudentSchedule_ReturnsCorrectData_WhenStudentExistsWithClasses()
        {
            var student = new Student
            {
                Id = 1,
                FirstName = "Testy",
                LastName = "McTesterton",
                InstructedClasses = new List<InstructedClassStudent>
                {
                    new() {
                        InstructedClass = new InstructedClass
                        {
                            ClassName = "Physics I",
                            Classroom = new Classroom { Name = "Room 1-A"},
                            ClassPeriod = new ClassPeriod {
                                DayOfWeek = DayOfWeek.Monday,
                                StartTime = TimeOnly.Parse("07:30"),
                                EndTime = TimeOnly.Parse("08:23"),
                            },
                            Teacher = new Teacher {
                                FirstName = "Test", LastName = "Teacher1", Email = "teacher1@school.edu"
                            }
                        }
                    },
                    new() {
                        InstructedClass = new InstructedClass
                        {
                            ClassName = "History III",
                            Classroom = new Classroom { Name = "Room 1-B"},
                            ClassPeriod = new ClassPeriod {
                                DayOfWeek = DayOfWeek.Monday,
                                StartTime = TimeOnly.Parse("08:30"),
                                EndTime = TimeOnly.Parse("09:23"),
                            },
                            Teacher = new Teacher {
                                FirstName = "Test", LastName = "Teacher2", Email = "teacher2@school.edu"
                            }
                        }
                    }
                }
            };
            SetupMockStudentRepo(new List<Student> { student });

            var schedule = await _service.GetStudentSchedule(student.Id);

            Assert.That(schedule, Is.Not.Null);
            Assert.That(schedule!.StudentId, Is.EqualTo(student.Id));
            Assert.That(schedule.StudentName, Is.EqualTo("Testy McTesterton"));
            Assert.That(schedule.Classes, Has.Count.EqualTo(2));

            var physicsClass = schedule.Classes[0];
            Assert.Multiple(() =>
            {
                Assert.That(physicsClass.ClassName, Is.EqualTo("Physics I"));
                Assert.That(physicsClass.ClassroomName, Is.EqualTo("Room 1-A"));
                Assert.That(physicsClass.Teacher, Is.EqualTo("Test Teacher1"));
                Assert.That(physicsClass.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
                Assert.That(physicsClass.StartTime, Is.EqualTo(TimeOnly.Parse("07:30")));
                Assert.That(physicsClass.EndTime, Is.EqualTo(TimeOnly.Parse("08:23")));
            });

            var historyClass = schedule.Classes[1];
            Assert.Multiple(() =>
            {
                Assert.That(historyClass.ClassName, Is.EqualTo("History III"));
                Assert.That(historyClass.ClassroomName, Is.EqualTo("Room 1-B"));
                Assert.That(historyClass.Teacher, Is.EqualTo("Test Teacher2"));
                Assert.That(historyClass.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
                Assert.That(historyClass.StartTime, Is.EqualTo(TimeOnly.Parse("08:30")));
                Assert.That(historyClass.EndTime, Is.EqualTo(TimeOnly.Parse("09:23")));
            });
        }

        [Test]
        public async Task StudentSchedule_ReturnsNull_WhenStudentNotFound()
        {
            SetupMockStudentRepo(new List<Student>());
            Assert.That(await _service.GetStudentSchedule(1), Is.Null);
        }

        [Test]
        public async Task StudentSchedule_ReturnsEmptyClassList_WhenStudentHasNoClasses()
        {
            var student = new Student
            {
                Id = 1,
                FirstName = "Testy",
                LastName = "McTesterton",
                InstructedClasses = new List<InstructedClassStudent>()
            };
            SetupMockStudentRepo(new List<Student> { student });

            var schedule = await _service.GetStudentSchedule(student.Id);
            Assert.That(schedule!.Classes, Is.Empty);
        }

        [Test]
        public async Task StudentSchedule_HandlesNullTeacher_WhenNoTeacherAssigned()
        {
            var student = new Student
            {
                Id = 1,
                FirstName = "Testy",
                LastName = "McTesterton",
                InstructedClasses = new List<InstructedClassStudent>
                {
                    new() {
                        InstructedClass = new InstructedClass
                        {
                            ClassName = "Lunch",
                            Classroom = new Classroom { Name = "Cafeteria", IsLunchroom = true },
                            ClassPeriod = new ClassPeriod {
                                DayOfWeek = DayOfWeek.Monday,
                                StartTime = TimeOnly.Parse("11:30"),
                                EndTime = TimeOnly.Parse("12:00"),
                                IsLunch = true
                            },
                            Teacher = null
                        }
                    }
                }
            };
            SetupMockStudentRepo(new List<Student> { student });

            var schedule = await _service.GetStudentSchedule(student.Id);

            Assert.That(schedule, Is.Not.Null);

            var lunchClass = schedule!.Classes.Single();
            Assert.Multiple(() =>
            {
                Assert.That(lunchClass.ClassName, Is.EqualTo("Lunch"));
                Assert.That(lunchClass.Teacher, Is.EqualTo("No Teacher Assigned"));
            });
        }

        private void SetupMockStudentRepo(List<Student> students)
        {
            _mockStudentRepo.Setup(r => r.Query).Returns(students.AsQueryable().BuildMock());
        }

        private void SetupMockClassRepo(List<InstructedClass> classes)
        {
            _mockClassRepo.Setup(r => r.Query).Returns(classes.AsQueryable().BuildMock());
        }
    }
}