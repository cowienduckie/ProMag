using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Domain;

namespace PersonalData.Common.Converters;

public static class ConvertToDto
{
    public static PersonDto ToPersonDto(this Person person)
    {
        return new PersonDto
        {
            Id = person.Id.ToString(),
            ActorId = person.ActorId.ToString(),

            UserStatus = person.UserStatus,
            FirstName = person.FirstName,
            MiddleName = person.MiddleName,
            LastName = person.LastName,
            Alias = person.Alias,
            PhotoPath = person.PhotoPath,
            Email = person.Email,

            Address = new AddressDto
            {
                City = person.City,
                Country = person.Country,
                State = person.State,
                Street = person.Street,
                ZipCode = person.ZipCode
            },

            PersonInfo = new PersonInfoDto
            {
                Fax = person.Fax,
                LandLineNumber = person.LandLineNumber,
                MobileNumber = person.MobileNumber,
                Website = person.Website
            },

            Language = person.Language,
            Timezone = person.Timezone,
            CountryLocale = person.CountryLocale,

            CreatedOn = person.CreatedOn,
            CreatedBy = person.CreatedBy.ToString(),
            LastModifiedOn = person.LastModifiedOn,
            LastModifiedBy = person.LastModifiedBy.ToString()
        };
    }

    public static PersonDto ToBriefPersonDto(this Person person)
    {
        return new PersonDto
        {
            Id = person.Id.ToString(),
            ActorId = person.ActorId.ToString(),

            FirstName = person.FirstName,
            MiddleName = person.MiddleName,
            LastName = person.LastName,
            Alias = person.Alias,
            PhotoPath = person.PhotoPath,
            Email = person.Email,
            UserStatus = person.UserStatus
        };
    }
}