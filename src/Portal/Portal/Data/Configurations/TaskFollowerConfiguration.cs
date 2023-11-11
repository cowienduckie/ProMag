using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain;

namespace Portal.Data.Configurations;

public class TaskFollowerConfiguration : BaseEntityConfiguration<TaskFollower>
{
    protected override void ConfigureProperties(EntityTypeBuilder<TaskFollower> builder)
    {
        builder.HasKey(tf => new { tf.TaskId, tf.UserId });

        builder.Property(tf => tf.TaskId)
            .IsRequired();

        builder.Property(tf => tf.UserId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<TaskFollower> builder)
    {
        builder.HasOne(tf => tf.Task)
            .WithMany(t => t.Followers)
            .HasForeignKey(tf => tf.TaskId);
    }
}