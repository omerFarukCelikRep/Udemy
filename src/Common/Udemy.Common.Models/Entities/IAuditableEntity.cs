
using Udemy.Common.Models.Entities.Enums;

namespace Udemy.Common.Models.Entities;
public interface IAuditableEntity
{
    DateTime CreatedDate { get; set; }
    string? CreatedBy { get; set; }
    Statuses Status { get; set; }
    string? ModifiedBy { get; set; }
    DateTime? ModifiedDate { get; set; }
}
