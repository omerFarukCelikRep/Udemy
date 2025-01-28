using System.Reflection;
using Udemy.Common.Models.Attributes;

namespace Udemy.Common.Models.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    private List<PropertyInfo>? _properties;
    private List<FieldInfo>? _fields;

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (object.Equals(left, null))
        {
            if (object.Equals(right, null))
                return true;

            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

    public bool Equals(ValueObject? obj) => Equals(obj as object);

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return GetProperties().All(p => PropertiesAreEqual(obj, p))
            && GetFields().All(f => FieldsAreEqual(obj, f));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (PropertyInfo property in GetProperties())
            {
                object? value = property.GetValue(this, null);
                hash = HashValue(hash, value);
            }

            foreach (FieldInfo field in GetFields())
            {
                object? value = field.GetValue(this);
                hash = HashValue(hash, value);
            }

            return hash;
        }
    }

    private List<PropertyInfo> GetProperties()
    {
        _properties ??= GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(p => Attribute.IsDefined(GetType().Assembly, typeof(IgnoreMemberAttribute)))
                                 .ToList();
        return _properties;
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo property) => object.Equals(property.GetValue(this, null), property.GetValue(obj, null));

    private List<FieldInfo> GetFields()
    {
        _fields ??= GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                             .Where(f => Attribute.IsDefined(GetType().Assembly, typeof(IgnoreMemberAttribute)))
                             .ToList();

        return _fields;
    }

    private bool FieldsAreEqual(object obj, FieldInfo field) => object.Equals(field.GetValue(this), field.GetValue(obj));

    private static int HashValue(int seed, object? value)
    {
        int currentHash = value != null ? value.GetHashCode() : default;

        return seed * 23 + currentHash;
    }

    public ValueObject? Copy() => MemberwiseClone() as ValueObject;
}
