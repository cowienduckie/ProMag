using HotChocolate.Stitching.Merge;

namespace GraphQl.Gateway.Middlewares;

public class IgnoreAuthorizeDirectiveHandler : IDirectiveMergeHandler
{
    private readonly MergeDirectiveRuleDelegate _next;

    public IgnoreAuthorizeDirectiveHandler(MergeDirectiveRuleDelegate next)
    {
        _next = next;
    }

    public void Merge(ISchemaMergeContext context, IReadOnlyList<IDirectiveTypeInfo> directives)
    {
        var directive = directives.First();

        if (!directive.Definition.Name.Value.Equals("authorize", StringComparison.InvariantCultureIgnoreCase))
        {
            _next(context, directives);
        }
    }
}