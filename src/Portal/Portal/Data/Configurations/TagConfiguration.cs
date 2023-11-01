using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Common.Constants;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class TagConfiguration : BaseEntityConfiguration<Tag>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Color)
            .HasDefaultValue(ColorHexCode.Default)
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(t => t.Notes)
            .HasMaxLength(500);
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Tag> builder)
    {
        builder.HasMany(t => t.Tasks)
            .WithMany(t => t.Tags);

        builder.HasMany(t => t.Followers)
            .WithOne(tf => tf.Tag)
            .HasForeignKey(tf => tf.TagId);
    }
}