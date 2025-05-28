using Bogus;
using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;
using System.Linq;

namespace gWorks.Hiring.Services;

public class SchoolDataSeedService : ISchoolDataSeedService
{
    private const int StudentCount = 500, TeacherCount = 40, ClassroomCount = 35;
    private const string SchoolDomain = "gworks.edu";
    private static Random Random = new Random();

    private readonly IRepository<Student> _studentRepo;
    private readonly IRepository<Teacher> _teacherRepo;
    private readonly IRepository<Classroom> _classroomRepo;
    private readonly IRepository<ClassPeriod> _classPeriodRepo;
    private readonly IRepository<InstructedClass> _classRepo;

    public SchoolDataSeedService(IRepository<Student> studentRepo, IRepository<Teacher> teacherRepo, IRepository<Classroom> classroomRepo, IRepository<ClassPeriod> classPeriodRepo, IRepository<InstructedClass> classRepo)
    {
        _studentRepo = studentRepo;
        _teacherRepo = teacherRepo;
        _classroomRepo = classroomRepo;
        _classPeriodRepo = classPeriodRepo;
        _classRepo = classRepo;
    }

    public async Task SeedData()
    {
        var students = GetStudents().ToList();
        var teachers = GetTeachers().ToList();
        var classrooms = GetClassrooms().ToList();
        var classPeriods = GetClassPeriods().ToList();

        await _studentRepo.AddRangeAsync(students);
        await _teacherRepo.AddRangeAsync(teachers);
        await _classroomRepo.AddRangeAsync(classrooms);
        await _classPeriodRepo.AddRangeAsync(classPeriods);

        var instructedClasses = GetClasses(students, teachers, classrooms, classPeriods).ToList();
        await _classRepo.AddRangeAsync(instructedClasses);
        await _classRepo.SaveChangesAsync();
    }

    private static IEnumerable<InstructedClass> GetClasses(ICollection<Student> students, ICollection<Teacher> teachers, ICollection<Classroom> classrooms, ICollection<ClassPeriod> classPeriods)
    {
        // RULES:
        // - every student must be in a classroom in every period
        // - every classroom that has a student must have a teacher (except lunch)
        var lunchClassroom = classrooms.Single(x => x.IsLunchroom);
        int classroomsUsedPerPeriod = classrooms.Count - 4;
        int studentsPerClassroom = (students.Count / classroomsUsedPerPeriod) + 1;

        foreach (var classPeriod in classPeriods.OrderBy(x => x.StartTime))
        {
            // All students have lunch at the same time
            if (classPeriod.IsLunch)
            {
                yield return new InstructedClass
                {
                    Students = students
                        .Select(s => new InstructedClassStudent
                        {
                            StudentId = s.Id,
                            Student = s,
                        })
                        .ToList(),
                    ClassPeriod = classPeriod,
                    Classroom = lunchClassroom,
                };

                continue;
            }

            var classroomsToUse = Shuffle(classrooms.Except([lunchClassroom]).ToList()).Take(classroomsUsedPerPeriod).ToList();
            var teachersToUse = Shuffle(teachers.ToList()).Take(classroomsUsedPerPeriod).ToList();
            var studentsInClasses = Shuffle(students.ToList()).Chunk(studentsPerClassroom).Select((s, i) => new { Students = s, Classroom = classroomsToUse[i], Teacher = teachersToUse[i] }).ToList();

            foreach (var grp in studentsInClasses)
            {
                yield return new InstructedClass
                {
                    Students = grp.Students
                        .Select(s => new InstructedClassStudent
                        {
                            StudentId = s.Id,
                            Student = s,
                        })
                        .ToList(),
                    ClassPeriod = classPeriod,
                    Classroom = grp.Classroom,
                    Teacher = grp.Teacher,
                };
            }
        }
    }

    private static Faker<TPerson> SetNameFaker<TPerson>(Faker<TPerson> faker)
        where TPerson : class, IPerson
    {
        return faker
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName());
    }

    private static IEnumerable<Student> GetStudents()
    {
        var studentFaker = SetNameFaker(new Faker<Student>())
            .RuleFor(x => x.Id, 0)
            .RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

        return studentFaker.Generate(Random.Next(StudentCount - 50, StudentCount + 50));
    }

    private static IEnumerable<Teacher> GetTeachers()
    {
        var teacherFaker = SetNameFaker(new Faker<Teacher>())
            .RuleFor(x => x.Id, 0)
            .RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName, SchoolDomain));

        return teacherFaker.Generate(TeacherCount);
    }

    private static IEnumerable<ClassPeriod> GetClassPeriods()
    {
        const int classesBeforeLunch = 4, classesAfterLunch = 3;
        TimeSpan lengthOfClass = TimeSpan.FromMinutes(53), breakTime = TimeSpan.FromMinutes(7);
        TimeSpan lunchTime = TimeSpan.FromMinutes(30);

        foreach (var day in Enumerable.Range((int)DayOfWeek.Monday, 5).Cast<DayOfWeek>())
        {
            var classStartTime = TimeSpan.FromHours(7).Add(TimeSpan.FromMinutes(30));
            for (int i = 0; i < classesBeforeLunch; i++, classStartTime += lengthOfClass.Add(breakTime))
            {
                yield return new()
                {
                    DayOfWeek = day,
                    StartTime = TimeOnly.FromTimeSpan(classStartTime),
                    EndTime = TimeOnly.FromTimeSpan(classStartTime + lengthOfClass),
                };
            }

            // lunch time
            yield return new()
            {
                DayOfWeek = day,
                StartTime = TimeOnly.FromTimeSpan(classStartTime),
                EndTime = TimeOnly.FromTimeSpan(classStartTime + lunchTime),
                IsLunch = true,
            };

            classStartTime += lunchTime.Add(breakTime);

            for (int i = 0; i < classesAfterLunch; i++, classStartTime += lengthOfClass.Add(breakTime))
            {
                yield return new()
                {
                    DayOfWeek = day,
                    StartTime = TimeOnly.FromTimeSpan(classStartTime),
                    EndTime = TimeOnly.FromTimeSpan(classStartTime + lengthOfClass),
                };
            }
        }
    }

    private static IEnumerable<Classroom> GetClassrooms()
    {
        const string nameSuffixes = "ABCDEF";
        const string namePrefixes = "123456";

        var allNames = namePrefixes.SelectMany(x => nameSuffixes, (prefix, suffix) => $"{prefix}-{suffix}").ToList();

        var faker = new Faker<Classroom>()
            .RuleFor(x => x.Name, x => allNames[x.IndexFaker % allNames.Count]);

        return faker.Generate(Math.Min(allNames.Count, ClassroomCount)).Concat([
            new Classroom
            {
                Name = "Lunchroom",
                IsLunchroom = true,
            }
        ]);
    }

    private static IList<T> Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Next(0, i - 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return list;
    }
}
