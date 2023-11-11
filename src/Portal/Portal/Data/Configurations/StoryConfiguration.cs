using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Common.Constants;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class StoryConfiguration : BaseEntityConfiguration<Story>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Story> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Text)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.Source)
            .HasMaxLength(100)
            .HasDefaultValue(ActionSource.System)
            .IsRequired();

        builder.Property(s => s.Liked)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(s => s.LikesCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(s => s.TargetId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Story> builder)
    {
        builder.HasOne(s => s.TargetTask)
            .WithMany(t => t.Stories)
            .HasForeignKey(s => s.TargetId);
    }
}