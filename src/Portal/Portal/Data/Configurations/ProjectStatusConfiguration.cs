using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class ProjectStatusConfiguration : BaseEntityConfiguration<ProjectStatus>
{
    protected override void ConfigureProperties(EntityTypeBuilder<ProjectStatus> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ps => ps.Text)
            .HasMaxLength(500);

        builder.Property(ps => ps.Color)
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(ps => ps.ProjectId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<ProjectStatus> builder)
    {
        builder.HasOne(ps => ps.Project)
            .WithMany(p => p.Statues)
            .HasForeignKey(ps => ps.ProjectId);
    }
}