
namespace Udemy.Common.Models.Entities;

public interface ISoftDeletableEntity : IAuditableEntity
{
    string? DeletedBy { get; set; }
    DateTime DeletedDate { get; set; }
}
