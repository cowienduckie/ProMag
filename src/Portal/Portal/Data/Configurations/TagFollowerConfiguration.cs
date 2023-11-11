using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class TagFollowerConfiguration : BaseEntityConfiguration<TagFollower>
{
    protected override void ConfigureProperties(EntityTypeBuilder<TagFollower> builder)
    {
        builder.HasKey(tf => new { tf.TagId, tf.UserId });

        builder.Property(tf => tf.TagId)
            .IsRequired();

        builder.Property(tf => tf.UserId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<TagFollower> builder)
    {
        builder.HasOne(tf => tf.Tag)
            .WithMany(t => t.Followers)
            .HasForeignKey(tf => tf.TagId);
    }
}