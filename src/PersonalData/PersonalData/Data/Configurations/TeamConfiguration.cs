using EfCore.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalData.Domain;

namespace PersonalData.Data.Configurations;

public class TeamConfiguration : BaseEntityConfiguration<Team>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.WorkspaceId)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Team> builder)
    {
        builder.HasOne(t => t.Workspace)
            .WithMany(w => w.Teams)
            .HasForeignKey(t => t.WorkspaceId);

        builder.HasMany(t => t.Members)
            .WithMany(p => p.Teams)
            .UsingEntity("TeamMembers");
    }
}