using System.Reflection;
using Google.Protobuf;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl;

public static class Extensions
{
    public static void IgnoreProtobufMethods<T>(this IObjectTypeDescriptor<T> descriptor) where T : IMessage<T>
    {
        descriptor.Field(t => t.CalculateSize()).Ignore();
        descriptor.Field(t => t.Clone()).Ignore();
        descriptor.Field(t => t.Equals(default)).Ignore();
    }

    public static IRequestExecutorBuilder RegisterObjectTypes(this IRequestExecutorBuilder builder, Assembly graphTypeAssembly)
    {
        var objectTypes = graphTypeAssembly
            .GetTypes()
            .Where(type => typeof(ObjectType).IsAssignableFrom(type));

        foreach (var objectType in objectTypes)
        {
            builder.AddType(objectType);
        }

        return builder;
    }
}