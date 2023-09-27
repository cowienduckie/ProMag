using System.Diagnostics;
using MasterData.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;

namespace MasterData.Data;

public sealed class MasterDataDbContext : DbContext
{
    public static readonly string DefaultSchema = DbSchema.MasterData.GetDescription();

    public MasterDataDbContext(DbContextOptions<MasterDataDbContext> options) : base(options)
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