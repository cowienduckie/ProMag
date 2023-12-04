using PersonalData.Common.Enums;
using PersonalData.Domain;
using PersonalData.UseCases.Commands;
using Shared.Common.Enums;

namespace PersonalData.Common.Converters;

public static class ConvertToDomain
{
    public static Person ToPerson(this InviteUserCommand inviteUserCommand)
    {
        return new Person
        {
            FirstName = inviteUserCommand.FirstName,
            LastName = inviteUserCommand.LastName,
            Email = inviteUserCommand.Email,

            UserStatus = UserStatus.Inactive,
            UserType = UserType.User
        };
    }

    public static Person ToPerson(this RegisterUserCommand registerUserCommand)
    {
        return new Person
        {
            FirstName = registerUserCommand.FirstName,
            LastName = registerUserCommand.LastName,
            Email = registerUserCommand.Email,

            UserStatus = UserStatus.Inactive,
            UserType = UserType.User
        };
    }

    public static Person ToPerson(this EditUserCommand editUser, Person originPerson)
    {
        originPerson.FirstName = editUser.FirstName;
        originPerson.LastName = editUser.LastName;
        originPerson.Alias = editUser.Alias;
        originPerson.MiddleName = editUser.MiddleName;
        originPerson.PhotoPath = editUser.PhotoPath;

        originPerson.City = editUser.Address.City;
        originPerson.Country = editUser.Address.Country;
        originPerson.State = editUser.Address.State;
        originPerson.Street = editUser.Address.Street;
        originPerson.ZipCode = editUser.Address.ZipCode;

        originPerson.MobileNumber = editUser.PersonInfo.MobileNumber;
        originPerson.Website = editUser.PersonInfo.Website;
        originPerson.LandLineNumber = editUser.PersonInfo.LandLineNumber;
        originPerson.Fax = editUser.PersonInfo.Fax;

        originPerson.Language = editUser.Language;
        originPerson.Timezone = editUser.Timezone;
        originPerson.CountryLocale = editUser.CountryLocale;

        return originPerson;
    }
}