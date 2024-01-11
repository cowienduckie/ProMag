using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalData.Domain;

namespace PersonalData.Data.Configurations;

public class WorkspaceConfiguration : BaseEntityConfiguration<Workspace>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.OrganizationId)
            .IsRequired(false);

        builder
            .Property(x => x.OwnerId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasOne(w => w.Organization)
            .WithOne(o => o.Workspace)
            .HasForeignKey<Workspace>(w => w.OrganizationId)
            .IsRequired(false);

        builder.HasMany(w => w.Teams)
            .WithOne(t => t.Workspace)
            .HasForeignKey(t => t.WorkspaceId);

        builder.HasMany(w => w.Members)
            .WithMany(p => p.Workspaces)
            .UsingEntity("WorkspaceMembers");
    }
}