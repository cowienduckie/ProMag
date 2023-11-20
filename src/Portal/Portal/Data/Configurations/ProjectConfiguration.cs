using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class ProjectConfiguration : BaseEntityConfiguration<Project>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Notes)
            .HasMaxLength(500);

        builder.Property(p => p.Color)
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(p => p.DueDate);

        builder.Property(p => p.Archived)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(p => p.OwnerId)
            .IsRequired();

        builder.Property(p => p.TeamId)
            .IsRequired();

        builder.Property(p => p.WorkspaceId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Project> builder)
    {
        builder.HasMany(p => p.Statues)
            .WithOne(ps => ps.Project)
            .HasForeignKey(ps => ps.ProjectId);

        builder.HasMany(p => p.Sections)
            .WithOne(s => s.Project)
            .HasForeignKey(s => s.ProjectId);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId);
    }
}