using Configuration.MassTransit.IntegrationEvents.Account;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PersonalData.Data;

namespace PersonalData.IntegrationEvents.Consumers;

public class AccountStatusChangedConsumer : IConsumer<AccountStatusChanged>
{
    private readonly PersonalContext _personalContext;

    public AccountStatusChangedConsumer(PersonalContext personalContext)
    {
        _personalContext = personalContext;
    }

    public async Task Consume(ConsumeContext<AccountStatusChanged> context)
    {
        var person = await _personalContext.People.FirstOrDefaultAsync(p => p.Id == Guid.Parse(context.Message.UserId));

        if (person is not null)
        {
            person.UserStatus = context.Message.UserStatus;

            _personalContext.People.Update(person);
            await _personalContext.SaveChangesAsync();
        }
    }
}