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

    public ExchangeService(ICurrencyExchangeRepository currencyExchangeRepository, IMapper mapper, IFixerClient fixerClient)
    {
        _currencyExchangeRepository = currencyExchangeRepository;
        _mapper = mapper;
        _fixerClient = fixerClient;
    }

    public async Task<int> AddTrade(TradeRequest request)
    {
        var mappedRequest = _mapper.Map<Data.DbContext.Exchange>(request);
        // TODO: Get exchange rate from cache.
        var response = await _fixerClient.GetExchangeRatesAsync(new[] { request.ToCurrencyCode }, request.FromCurrencyCode);
        
        if (response.Success)
        {
            mappedRequest.Exchangerate = response.Rates[request.ToCurrencyCode];
        }

        return await  _currencyExchangeRepository.AddTradeAsync(mappedRequest);
    }
}