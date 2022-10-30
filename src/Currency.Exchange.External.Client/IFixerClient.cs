using Currency.Exchange.External.Client.Models;
using Refit;

namespace Currency.Exchange.External.Client;

public interface IFixerClient
{
    [Get("/latest")]
    Task<ExchangeRateResponse> GetExchangeRatesAsync([Query(CollectionFormat.Csv)]string[] symbols, [Query]string @base);
    
    [Get("/symbols")]
    Task<SymbolsResponse> GetSymbolsAsync();
}