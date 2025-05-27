using System.ComponentModel.DataAnnotations;

namespace gWorks.Hiring.Data.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
