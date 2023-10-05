using System.Diagnostics;
using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using PersonalData.Domain;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using Shared.SecurityContext;
using Shared.Serialization;

namespace PersonalData.Data;

public sealed class PersonalContext : BaseDbContext
{
    private static readonly string _defaultSchema = DbSchema.PersonalData.GetDescription();

    public PersonalContext(
        DbContextOptions<PersonalContext> options,
        ISerializerService serializer,
        ISecurityContextAccessor securityContext)
        : base(options, serializer, securityContext)
    {
        Debug.WriteLine("PersonalContext::ctor ->" + GetHashCode());
    }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(_defaultSchema);
    }
}