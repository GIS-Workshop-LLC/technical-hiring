namespace gWorks.Hiring.Data.Entities;

public interface IPerson
{
    string FirstName { get; set; }

    string LastName { get; set; }

    string? Email { get; set; }
}
