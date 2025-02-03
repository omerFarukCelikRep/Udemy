namespace Udemy.Common.Models.Entities.Base;
public abstract class SoftDeletableEntity<TId> : BaseEntity<TId>, ISoftDeletableEntity
    where TId : struct
{
    public string? DeletedBy { get; set; }
    public DateTime DeletedDate { get; set; }
}
