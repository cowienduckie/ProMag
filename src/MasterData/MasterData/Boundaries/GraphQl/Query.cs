using System.Diagnostics.CodeAnalysis;
using System.Text;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Boundaries.GraphQl.Filters;
using MasterData.Boundaries.GraphQl.ObjectTypes;
using MasterData.Common.Constants;
using MasterData.UseCases.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Promag.Protobuf.Commons.V1;
using Shared;
using Shared.Caching;
using Shared.Serialization;

namespace MasterData.Boundaries.GraphQl;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Query
{
    [GraphQLName("Ping")]
    public async Task<PongReply> Ping([Service] ISender mediator)
    {
        return await mediator.Send(new PingQuery());
    }

    [GraphQLName("Countries")]
    [Authorize(Policy = AuthorizationPolicy.ADMIN)]
    public async Task<IList<CountryDto>> GetCountries(
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] ISerializerService serializer,
        [Service] IConfiguration configuration)
    {
        return await GetDtos(
            MasterDataCacheKeys.AllCountries,
            () => mediator.Send(new GetCountriesQuery()),
            distributedCache,
            serializer,
            configuration);
    }

    [GraphQLName("Languages")]
    [Authorize(Policy = AuthorizationPolicy.ADMIN)]
    public async Task<IList<LanguageDto>> GetLanguages(
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] ISerializerService serializer,
        [Service] IConfiguration configuration)
    {
        return await GetDtos(
            MasterDataCacheKeys.AllLanguages,
            () => mediator.Send(new GetLanguagesQuery()),
            distributedCache,
            serializer,
            configuration);
    }

    [GraphQLName("Timezones")]
    [Authorize(Policy = AuthorizationPolicy.ADMIN)]
    public async Task<IList<TimezoneDto>> GetTimeZones(
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] ISerializerService serializer,
        [Service] IConfiguration configuration)
    {
        return await GetDtos(
            MasterDataCacheKeys.AllTimezones,
            () => mediator.Send(new GetTimezonesQuery()),
            distributedCache,
            serializer,
            configuration);
    }

    [GraphQLName("Currencies")]
    [Authorize(Policy = AuthorizationPolicy.ADMIN)]
    public async Task<IList<CurrencyDto>> GetCurrencies(
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] ISerializerService serializer,
        [Service] IConfiguration configuration)
    {
        return await GetDtos(
            MasterDataCacheKeys.AllCurrencies,
            () => mediator.Send(new GetCurrenciesQuery()),
            distributedCache,
            serializer,
            configuration);
    }

    [GraphQLName("ActivityLogs")]
    [UseOffsetPaging(typeof(ActivityLogType))]
    [UseFiltering(typeof(ActivityLogFilterInputType))]
    [Authorize(Policy = AuthorizationPolicy.ADMIN)]
    public async Task<IQueryable<ActivityLogDto>> GetActivityLogs([Service] ISender mediator)
    {
        return await mediator.Send(new GetActivityLogsQuery());
    }

    [GraphQLName("ActivityLog")]
    [GraphQLType(typeof(ActivityLogType))]
    [Authorize(Policy = AuthorizationPolicy.ADMIN)]
    public async Task<ActivityLogDto> GetActivityLogById(Guid id, [Service] ISender mediator)
    {
        return await mediator.Send(new GetActivityLogByIdQuery(id));
    }

    private static async Task<IList<TDto>> GetDtos<TDto>(
        string cacheKey,
        Func<Task<IList<TDto>>> getDtos,
        IDistributedCache distributedCache,
        ISerializerService serializer,
        IConfiguration configuration)
    {
        if (!configuration.GetOptions<CacheOptions>("Redis").Enabled)
        {
            return await getDtos();
        }

        IList<TDto>? dtos = null;

        var encodedDtos = await distributedCache.GetAsync(cacheKey);

        if (encodedDtos is not null)
        {
            var serializedDtos = Encoding.UTF8.GetString(encodedDtos);

            dtos = serializer.Deserialize<List<TDto>>(serializedDtos);
        }

        if (dtos is null)
        {
            dtos = await getDtos();

            var serializedDtos = serializer.Serialize(dtos);
            encodedDtos = Encoding.UTF8.GetBytes(serializedDtos);

            await distributedCache.SetAsync(cacheKey, encodedDtos, new DistributedCacheEntryOptions());
        }

        return dtos;
    }
}