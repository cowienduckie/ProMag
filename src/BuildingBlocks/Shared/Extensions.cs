using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace Shared;

public static class Extensions
{
    public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);

        return model;
    }

    public static TClass? ConvertTo<TClass>(this string input) where TClass : class
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(typeof(TClass));

            return converter.ConvertFromString(input) as TClass;
        }
        catch (NotSupportedException)
        {
            return default;
        }
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }

    public static string GetGenericTypeName(this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());

            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }
}