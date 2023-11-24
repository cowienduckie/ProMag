using System.Reflection;

namespace Shared.Common.Helpers;

public static class DictionaryHelper
{
    public static IDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this object obj)
    {
        return (IDictionary<TKey, TValue>)obj;
    }

    public static T GetObject<T>(this IDictionary<string, object?> dictionary) where T : new()
    {
        var result = new T();

        foreach (var kvp in dictionary)
        {
            var property = typeof(T).GetProperty(kvp.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null || !property.CanWrite)
            {
                continue;
            }

            var value = kvp.Value;

            if (value is not null)
            {
                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    value = value.ToDateTime();
                }
                else
                {
                    value = Convert.ChangeType(value, property.PropertyType);
                }
            }

            property.SetValue(result, value, null);
        }

        return result;
    }
}