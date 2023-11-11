using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain;
using Shared.Common.Constants;

namespace Portal.Data.Configurations;

public class AttachmentConfiguration : BaseEntityConfiguration<Attachment>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Host)
            .HasMaxLength(250);

        builder.Property(a => a.ViewUrl)
            .HasColumnType(SqlColumnType.POSTGRES_TEXT);

        builder.Property(a => a.DownloadUrl)
            .HasColumnType(SqlColumnType.POSTGRES_TEXT);

        builder.Property(a => a.ParentId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasOne(a => a.ParentTask)
            .WithMany(t => t.Attachments)
            .HasForeignKey(a => a.ParentId);
    }
}