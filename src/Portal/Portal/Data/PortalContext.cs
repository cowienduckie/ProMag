using System.Diagnostics;
using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using Shared.SecurityContext;
using Shared.Serialization;

namespace Portal.Data;

public sealed class PortalContext : BaseDbContext
{
    private static readonly string _defaultSchema = DbSchema.PersonalData.GetDescription();

    public PortalContext(
        DbContextOptions<PortalContext> options,
        ISerializerService serializer,
        ISecurityContextAccessor securityContext)
        : base(options, serializer, securityContext)
    {
        Debug.WriteLine("PortalContext::ctor ->" + GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(_defaultSchema);
    }
}