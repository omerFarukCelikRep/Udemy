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

public abstract class BaseEntityWithId<TId> : BaseEntity, IEquatable<object> where TId : struct
{
    public TId Id { get; set; }

    public bool IsTransient() => Id.Equals(default(TId));

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not BaseEntityWithId<TId> baseEntity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        if (IsTransient() || baseEntity.IsTransient())
            return false;

        return Id.Equals(baseEntity.Id);
    }

    public static bool operator ==(BaseEntityWithId<TId> left, BaseEntityWithId<TId> right)
    {
        if (Equals(left, null))
            return Equals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntityWithId<TId> left, BaseEntityWithId<TId> right)
    {
        return !(left == right);
    }
}