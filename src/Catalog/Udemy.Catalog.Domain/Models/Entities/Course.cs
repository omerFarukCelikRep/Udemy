using Udemy.Common.Models.Entities.Base;

namespace Udemy.Catalog.Domain.Models.Entities;

public class Course : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Thumbnail { get; set; } = string.Empty;
}