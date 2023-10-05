using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalData.Common.Enums;
using PersonalData.Domain;

namespace PersonalData.Data.Configurations;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
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
}