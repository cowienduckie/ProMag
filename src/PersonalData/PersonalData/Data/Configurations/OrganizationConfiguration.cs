using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalData.Domain;

namespace PersonalData.Data.Configurations;

public class OrganizationConfiguration : BaseEntityConfiguration<Organization>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.EmailDomain)
            .HasMaxLength(250);
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Organization> builder)
    {
        builder.HasOne(o => o.Workspace)
            .WithOne(w => w.Organization)
            .HasForeignKey<Workspace>(w => w.OrganizationId)
            .IsRequired(false);
    }
}