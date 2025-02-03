using Udemy.Common.Models.Entities.Enums;

namespace Udemy.Common.Models.Entities.Base;

/// <summary>
/// Base Entity
/// </summary>
/// <typeparam name="TId">Entity Id Type</typeparam>
public abstract class BaseEntity<TId> : IAuditableEntity, IEquatable<BaseEntity<TId>> where TId : struct
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public Statuses Status { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public bool IsTransient() => Id.Equals(default(TId));

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> baseEntity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        if (IsTransient() || baseEntity.IsTransient())
            return false;

        return Id.Equals(baseEntity.Id);
    }

    public bool Equals(BaseEntity<TId>? other) => base.Equals(other);

    public override int GetHashCode()
    {
        if (IsTransient())
#pragma warning disable S3249 // Classes directly extending "object" should not call "base" in "GetHashCode" or "Equals"
            return base.GetHashCode();
#pragma warning restore S3249 // Classes directly extending "object" should not call "base" in "GetHashCode" or "Equals"

        return Id.GetHashCode();
    }

    public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right)
    {
        if (Equals(left, null))
            return Equals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right) => !(left == right);

}

public abstract class BaseEntity : BaseEntity<int>
{
}
