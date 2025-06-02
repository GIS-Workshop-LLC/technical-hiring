using gWorks.Hiring.Data;
using gWorks.Hiring.Data.Entities;

namespace gWorks.Hiring.Services;

public interface ISchoolDataSeedService
{
    /// <summary>
    /// Generates initial seed data for the database.
    /// </summary>
    /// <returns></returns>
    Task SeedData();
}
