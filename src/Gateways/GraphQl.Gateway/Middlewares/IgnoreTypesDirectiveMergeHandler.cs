using System.Collections.Immutable;
using HotChocolate.Stitching.Merge;

namespace GraphQl.Gateway.Middlewares;

public class IgnoreTypesDirectiveMergeHandler : IDirectiveMergeHandler
{
    private readonly ImmutableArray<string> _ignoredTypes = ImmutableArray.Create(
        "Tag",
        "Authorize"
    );

    private readonly MergeDirectiveRuleDelegate _next;

    public IgnoreTypesDirectiveMergeHandler(MergeDirectiveRuleDelegate next)
    {
        _next = next;
    }

    public void Merge(ISchemaMergeContext context, IReadOnlyList<IDirectiveTypeInfo> directives)
    {
        var typeName = directives.First().Definition.Name.Value;

        if (_ignoredTypes.Contains(typeName, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        _next(context, directives);
    }
}