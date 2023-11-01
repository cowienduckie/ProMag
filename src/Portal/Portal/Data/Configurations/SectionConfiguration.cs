using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class SectionConfiguration : BaseEntityConfiguration<Section>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.ProjectId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Section> builder)
    {
        builder.HasOne(s => s.Project)
            .WithMany(p => p.Sections)
            .HasForeignKey(s => s.ProjectId);

        builder.HasMany(s => s.Tasks)
            .WithMany(t => t.Sections);
    }
}