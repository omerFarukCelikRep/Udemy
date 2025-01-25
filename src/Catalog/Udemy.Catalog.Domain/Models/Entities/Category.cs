using Udemy.Common.Models.Entities.Base;

namespace Udemy.Catalog.Domain.Models.Entities;

public class Category : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
}