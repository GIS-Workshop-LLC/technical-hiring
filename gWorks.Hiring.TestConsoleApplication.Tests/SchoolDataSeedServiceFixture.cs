using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;
using gWorks.Hiring.Services;
using Moq;
using NUnit.Framework;

namespace gWorks.Hiring.TestConsoleApplication.Tests;

[TestFixture]
public class SchoolDataSeedServiceFixture
{
    private Mock<IRepository<Student>> _mockStudentRepo;
    private ICollection<Student> _students;
    private Mock<IRepository<Teacher>> _mockTeacherRepo;
    private ICollection<Teacher> _teachers;
    private Mock<IRepository<ClassPeriod>> _mockClassPeriodRepo;
    private ICollection<ClassPeriod> _classPeriods;
    private Mock<IRepository<Classroom>> _mockClassroomRepo;
    private ICollection<Classroom> _classrooms;
    private Mock<IRepository<InstructedClass>> _mockInstructedClassRepo;
    private ICollection<InstructedClass> _instructedClasses;
    private SchoolDataSeedService _service;

    [SetUp]
    public void Setup()
    {
        (_mockStudentRepo, _students) = MockHelpers.CreateMockRepo<Student>();
        (_mockTeacherRepo, _teachers) = MockHelpers.CreateMockRepo<Teacher>();
        (_mockClassPeriodRepo, _classPeriods) = MockHelpers.CreateMockRepo<ClassPeriod>();
        (_mockClassroomRepo, _classrooms) = MockHelpers.CreateMockRepo<Classroom>();
        (_mockInstructedClassRepo, _instructedClasses) = MockHelpers.CreateMockRepo<InstructedClass>();
        _service = new SchoolDataSeedService(_mockStudentRepo.Object, _mockTeacherRepo.Object, _mockClassroomRepo.Object, _mockClassPeriodRepo.Object, _mockInstructedClassRepo.Object);
    }

    [Test]
    public async Task SeedDataInsertsData()
    {
        await _service.SeedData();

        Assert.That(_students, Has.Count.GreaterThan(0));
        Assert.That(_teachers, Has.Count.GreaterThan(0));
        Assert.That(_classPeriods, Has.Count.GreaterThan(0));
        Assert.That(_classrooms, Has.Count.GreaterThan(0));
        Assert.That(_instructedClasses, Has.Count.GreaterThan(0));
    }
}
