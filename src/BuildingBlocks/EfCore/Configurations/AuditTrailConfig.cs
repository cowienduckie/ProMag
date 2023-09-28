using EfCore.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Common.Enums;
using Shared.Common.Extensions;

namespace EfCore.Configurations;

public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("AuditTrails", DbSchema.Auditing.GetDescription());
    }
}