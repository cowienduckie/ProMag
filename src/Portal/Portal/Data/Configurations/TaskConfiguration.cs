using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Common.Constants;
using Shared.Common.Constants;
using Task = Portal.Domain.Task;

namespace Portal.Data.Configurations;

public class TaskConfiguration : BaseEntityConfiguration<Task>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Notes)
            .HasMaxLength(500);

        builder.Property(t => t.Completed)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.CompletedBy);

        builder.Property(t => t.CompletedOn);

        builder.Property(t => t.StartOn);

        builder.Property(t => t.DueOn);

        builder.Property(t => t.Liked)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.LikesCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(t => t.Likes)
            .HasDefaultValue(JsonString.EmptyArray)
            .HasColumnType(SqlColumnType.POSTGRES_TEXT)
            .IsRequired();

        builder.Property(t => t.CustomFields)
            .HasDefaultValue(JsonString.EmptyArray)
            .HasColumnType(SqlColumnType.POSTGRES_TEXT)
            .IsRequired();

        builder.Property(t => t.WorkspaceId)
            .IsRequired();

        builder.Property(t => t.AssigneeId);
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Task> builder)
    {
        builder.HasMany(t => t.Projects)
            .WithMany(p => p.Tasks);

        builder.HasMany(t => t.Sections)
            .WithMany(s => s.Tasks);

        builder.HasMany(t => t.Stories)
            .WithOne(s => s.TargetTask)
            .HasForeignKey(s => s.TargetId);

        builder.HasMany(t => t.Attachments)
            .WithOne(a => a.ParentTask)
            .HasForeignKey(a => a.ParentId);

        builder.HasMany(t => t.Tags)
            .WithMany(tag => tag.Tasks);

        builder.HasMany(t => t.Followers)
            .WithOne(f => f.Task)
            .HasForeignKey(f => f.TaskId);
    }
}