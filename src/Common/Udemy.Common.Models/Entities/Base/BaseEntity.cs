using Udemy.Common.Models.Entities.Enums;

namespace Udemy.Common.Models.Entities.Base;

/// <summary>
/// Base Entity
/// </summary>
/// <typeparam name="TId">Entity Id Type</typeparam>
public abstract class BaseEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public Statuses Status { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class BaseEntityWithId<TId> : BaseEntity, IEquatable<BaseEntityWithId<TId>> where TId : struct
{
    public TId Id { get; set; }

    public bool IsTransient() => Id.Equals(default(TId));

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntityWithId<TId> baseEntity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        if (IsTransient() || baseEntity.IsTransient())
            return false;

        return Id.Equals(baseEntity.Id);
    }

    public bool Equals(BaseEntityWithId<TId>? other) => base.Equals(other);

    public override int GetHashCode()
    {
        if (IsTransient())
#pragma warning disable S3249 // Classes directly extending "object" should not call "base" in "GetHashCode" or "Equals"
            return base.GetHashCode();
#pragma warning restore S3249 // Classes directly extending "object" should not call "base" in "GetHashCode" or "Equals"

        return Id.GetHashCode();
    }

    public static bool operator ==(BaseEntityWithId<TId> left, BaseEntityWithId<TId> right)
    {
        if (Equals(left, null))
            return Equals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntityWithId<TId> left, BaseEntityWithId<TId> right) => !(left == right);

}
