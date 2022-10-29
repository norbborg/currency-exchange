using Refit;

namespace currency.exchange.external.client;

public interface IFixerClient
{
    [Get("/latest")]
    Task<FixerResponse> GetExchangeRatesAsync([Query(CollectionFormat.Csv)]string[] symbols, [Query]string @base);
}