using AutoMapper;
using Currency.Exchange.Data;
using Currency.Exchange.External.Client;
using Currency.Exchange.Public.Contracts.Requests;

namespace Currency.Exchange.Services;

public class ExchangeService : IExchangeService
{
    private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
    private readonly IMapper _mapper;
    private readonly IFixerClient _fixerClient;
    private readonly ICacheStore _cacheStore;
    
    private const string CacheKeyPrefix = "ExchangeRates";

    public ExchangeService(ICurrencyExchangeRepository currencyExchangeRepository, IMapper mapper,
        IFixerClient fixerClient, ICacheStore cacheStore)
    {
        _currencyExchangeRepository = currencyExchangeRepository;
        _mapper = mapper;
        _fixerClient = fixerClient;
        _cacheStore = cacheStore;
    }

    public async Task<int> AddTradeAsync(TradeRequest request)
    {
        var mappedRequest = _mapper.Map<Data.DbContext.Exchange>(request);

        var cacheKey = GetCacheKey(request.FromCurrencyCode,request.ToCurrencyCode);
        
        var exchangeRate = _cacheStore.Get<decimal>(cacheKey);

        if (exchangeRate is 0)
        {
            var response = await _fixerClient.GetExchangeRatesAsync(new[] { request.ToCurrencyCode }, request.FromCurrencyCode);
            
            if (response.Success)
            {
                exchangeRate = response.Rates[request.ToCurrencyCode];
            }
            
            _cacheStore.Add(cacheKey, exchangeRate, TimeSpan.FromMinutes(30));
        }

        mappedRequest.Exchangerate = exchangeRate;
        
        return await _currencyExchangeRepository.AddTradeAsync(mappedRequest);
    }

    private string GetCacheKey(string from, string to)
    {
        return $"{CacheKeyPrefix}.{from}.{to}";
    }
}