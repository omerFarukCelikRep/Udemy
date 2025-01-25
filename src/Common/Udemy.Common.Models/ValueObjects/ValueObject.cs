namespace Udemy.Common.Models.ValueObjects;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    protected static bool EqualOperator(ValueObject left, ValueObject right) => Equals(left, right);

    protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !Equals(left, right);

    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject 
               && GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
    }

    public ValueObject? Copy() => MemberwiseClone() as ValueObject;
}