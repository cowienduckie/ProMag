using EfCore.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Common.Enums;
using Shared.Common.Extensions;

namespace Portal.Data.Configurations;

public class AuditTrailConfiguration : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("AuditTrails", DbSchema.MasterData.GetDescription(), t => { t.ExcludeFromMigrations(); });
    }
}