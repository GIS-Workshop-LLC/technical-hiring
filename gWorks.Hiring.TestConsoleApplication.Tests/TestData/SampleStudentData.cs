using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.Services.Models;

namespace gWorks.Hiring.TestConsoleApplication.Tests.TestData
{
    public static class SampleStudentData
    {
        public static StudentScheduleDto GetSampleStudentSchedule()
        {
            return new StudentScheduleDto
            {
                StudentId = 1,
                StudentName = "Srinivas Musinuri",
                Schedule = new List<ClassScheduleDto>
                {
                    new ClassScheduleDto
                    {
                        ClassName = "Math",
                        InstructorName = "Michael R",
                        DayOfWeek = DayOfWeek.Monday,
                        StartTime = new TimeOnly(8, 0, 0),
                        EndTime = new TimeOnly(9, 0, 0)
                    },
                    new ClassScheduleDto
                    {
                        ClassName = "Science",
                        InstructorName = "Bob Johnson",
                        DayOfWeek = DayOfWeek.Monday,
                        StartTime = new TimeOnly(9, 15, 0),
                        EndTime = new TimeOnly(10, 15, 0)
                    }
                }
            };
        }

        public static List<InstructedClass> GetMockInstructedClasses(int count)
        {
            var classes = new List<InstructedClass>();

            for (int i = 1; i <= count; i++)
            {
                classes.Add(new InstructedClass
                {
                    Id = i,
                    ClassName = $"Class {i}",
                    Students = new List<InstructedClassStudent>
            {
                new InstructedClassStudent(),
                new InstructedClassStudent()
            },
                    ClassPeriod = new ClassPeriod(),
                    Classroom = new Classroom { Name = $"Room {i}" }
                });
            }

            return classes;
        }


        public static Student GetMockStudentWithSchedule()
        {
            return new Student
            {
                Id = 101,
                FirstName = "Srinivas",
                LastName = "Musinuri",
                InstructedClasses = new List<InstructedClassStudent>
                {
                    new InstructedClassStudent
                    {
                        InstructedClass = new InstructedClass
                        {
                            ClassName = "Math",
                            Teacher = new Teacher { FirstName = "Jane", LastName = "Smith" },
                            ClassPeriod = new ClassPeriod
                            {
                                DayOfWeek = DayOfWeek.Monday,
                                StartTime = new TimeOnly(9, 0, 0),
                                EndTime = new TimeOnly(9, 50, 0)
                            },
                            Classroom=new Classroom(){Name = "Math"}
                        }
                    }
                }
            };
        }
    }
}
