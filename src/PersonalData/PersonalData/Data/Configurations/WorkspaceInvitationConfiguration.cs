using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalData.Domain;

namespace PersonalData.Data.Configurations;

public class WorkspaceInvitationConfiguration : BaseEntityConfiguration<WorkspaceInvitation>
{
    protected override void ConfigureProperties(EntityTypeBuilder<WorkspaceInvitation> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.InvitedPersonId)
            .IsRequired();

        builder
            .Property(x => x.WorkspaceId)
            .IsRequired();

        builder
            .Property(x => x.Accepted)
            .IsRequired()
            .HasDefaultValue(false);
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<WorkspaceInvitation> builder)
    {
        builder.HasOne(w => w.Workspace)
            .WithMany(o => o.Invitations)
            .HasForeignKey(w => w.WorkspaceId);

        builder.HasOne(w => w.InvitedPerson)
            .WithMany(o => o.Invitations)
            .HasForeignKey(w => w.InvitedPersonId);
    }
}