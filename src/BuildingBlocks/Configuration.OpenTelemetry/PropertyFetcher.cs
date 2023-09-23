using System.Reflection;

namespace Configuration.OpenTelemetry.Collectors.Implementations;

internal class PropertyFetcher
{
    private readonly string _propertyName;
    private PropertyFetch? _innerFetcher;

    public PropertyFetcher(string propertyName)
    {
        _propertyName = propertyName;
    }

    public object? Fetch(object obj)
    {
        if (_innerFetcher != null)
        {
            return _innerFetcher?.Fetch(obj);
        }

        var type = obj.GetType().GetTypeInfo();
        var property = type.DeclaredProperties.FirstOrDefault(p =>
            string.Equals(p.Name, _propertyName, StringComparison.InvariantCultureIgnoreCase));

        if (property == null)
        {
            property = type.GetProperty(_propertyName);
        }

        _innerFetcher = PropertyFetch.FetcherForProperty(property);

        return _innerFetcher?.Fetch(obj);
    }

    /// <summary>
    ///     Reference:
    ///     https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/DiagnosticSourceEventSource.cs
    /// </summary>
    private class PropertyFetch
    {
        /// <summary>
        ///     Create a property fetcher from a .NET Reflection PropertyInfo class that
        ///     represents a property of a particular type.
        /// </summary>
        public static PropertyFetch FetcherForProperty(PropertyInfo? propertyInfo)
        {
            if (propertyInfo == null || propertyInfo.DeclaringType == null)
            {
                return new PropertyFetch();
            }

            var typedPropertyFetcher = typeof(TypedFetchProperty<,>);
            var instantiatedTypedPropertyFetcher = typedPropertyFetcher
                .GetTypeInfo()
                .MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);

            return (PropertyFetch)Activator.CreateInstance(instantiatedTypedPropertyFetcher, propertyInfo)!;
        }

        /// <summary>
        ///     Given an object, fetch the property that this propertyFetch represents.
        /// </summary>
        public virtual object? Fetch(object obj)
        {
            return null;
        }

        private class TypedFetchProperty<TObject, TProperty> : PropertyFetch
        {
            private readonly Func<TObject, TProperty>? _propertyFetch;

            public TypedFetchProperty(PropertyInfo property)
            {
                _propertyFetch = property.GetMethod?.CreateDelegate(typeof(Func<TObject, TProperty>)) as Func<TObject, TProperty>;
            }

            public override object? Fetch(object obj)
            {
                if (obj is TObject o && _propertyFetch != null)
                {
                    return _propertyFetch(o);
                }

                return null;
            }
        }
    }
}