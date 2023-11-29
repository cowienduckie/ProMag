using EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalData.Common.Enums;
using PersonalData.Domain;
using Shared.Common.Enums;

namespace PersonalData.Data.Configurations;

public class PersonConfiguration : BaseEntityConfiguration<Person>
{
    protected override void ConfigureProperties(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.ActorId)
            .IsRequired();

        builder
            .Property(x => x.FirstName)
            .IsRequired();

        builder
            .Property(x => x.LastName)
            .IsRequired();

        builder.Property(x => x.UserStatus)
            .HasConversion(new EnumToStringConverter<UserStatus>())
            .IsRequired();

        builder.Property(x => x.UserType)
            .HasConversion(new EnumToStringConverter<UserType>())
            .IsRequired();

        builder
            .Property(x => x.Email)
            .IsRequired();
    }

    protected override void ConfigureRelationships(EntityTypeBuilder<Person> builder)
    {
        builder.HasMany(p => p.Workspaces)
            .WithMany(w => w.Members)
            .UsingEntity("WorkspaceMembers");

        builder.HasMany(p => p.Teams)
            .WithMany(t => t.Members)
            .UsingEntity("TeamMembers");
    }
}