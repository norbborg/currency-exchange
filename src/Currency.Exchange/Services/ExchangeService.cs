using AutoMapper;
using Currency.Exchange.Data;
using Currency.Exchange.External.Client;
using Currency.Exchange.Models;
using Currency.Exchange.Public.Contracts.Requests;
using Medallion.Threading.Redis;
using StackExchange.Redis;

namespace Currency.Exchange.Services;

public class ExchangeService : IExchangeService
{
    private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
    private readonly IMapper _mapper;
    private readonly IFixerClient _fixerClient;
    private readonly ICacheStore _cacheStore;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    private const string CacheKeyPrefix = "ExchangeRates";

    public ExchangeService(ICurrencyExchangeRepository currencyExchangeRepository, IMapper mapper,
        IFixerClient fixerClient, ICacheStore cacheStore, IConnectionMultiplexer connectionMultiplexer)
    {
        _currencyExchangeRepository = currencyExchangeRepository;
        _mapper = mapper;
        _fixerClient = fixerClient;
        _cacheStore = cacheStore;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<Result<int>> AddTradeAsync(TradeRequest request)
    {
        var mappedRequest = _mapper.Map<Data.DbContext.Exchange>(request);

        mappedRequest.Exchangerate = await GetExchangeRate(request.FromCurrencyCode, request.ToCurrencyCode);

        var @lock = new RedisDistributedLock($"Add_Trade_{mappedRequest.Clientid}",
            _connectionMultiplexer.GetDatabase());
        
        await using (var handle = await @lock.TryAcquireAsync())
        {
            if (handle != null)
            {
                var tradesInLastHour =
                    await _currencyExchangeRepository.GetTradesInLastHourAsync(mappedRequest.Clientid);

                if (tradesInLastHour >= 10)
                {
                    return Result<int>.ErrorResult(new[] { "Maximum trades limit per hour reached." });
                }

                var tradeId = await _currencyExchangeRepository.AddTradeAsync(mappedRequest);
                
                return Result<int>.SuccessResult(tradeId);
            }
        }

        return Result<int>.ErrorResult(new[] { "Couldn't get number of trades in last hour." });
    }
    
    private async Task<decimal> GetExchangeRate(string fromCurrencyCode, string toCurrencyCode)
    {
        var cacheKey = GetCacheKey(fromCurrencyCode, toCurrencyCode);

        var exchangeRate = _cacheStore.Get<decimal>(cacheKey);

        if (exchangeRate is 0)
        {
            var response =
                await _fixerClient.GetExchangeRatesAsync(new[] { toCurrencyCode }, fromCurrencyCode);

            if (response.Success)
            {
                exchangeRate = response.Rates[toCurrencyCode];
            }

            _cacheStore.Add(cacheKey, exchangeRate, TimeSpan.FromMinutes(30));
        }

        return exchangeRate;
    }

    private static string GetCacheKey(string from, string to)
    {
        return $"{CacheKeyPrefix}.{from}.{to}";
    }
}