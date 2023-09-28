using System.Diagnostics;
using EfCore.Context;
using MasterData.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using Shared.SecurityContext;
using Shared.Serialization;

namespace MasterData.Data;

public sealed class MasterDataDbContext : BaseDbContext
{
    public static readonly string DefaultSchema = DbSchema.MasterData.GetDescription();

    public MasterDataDbContext(
        DbContextOptions<MasterDataDbContext> options,
        ISerializerService serializer,
        ISecurityContextAccessor securityContext)
        : base(options, serializer, securityContext)
    {
        Debug.WriteLine("MasterDataContext::ctor ->" + GetHashCode());
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Timezone> Timezones => Set<Timezone>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(DefaultSchema);
    }
}