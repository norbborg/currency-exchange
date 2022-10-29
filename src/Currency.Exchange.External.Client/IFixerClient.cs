using Refit;

namespace Currency.Exchange.External.Client;

public interface IFixerClient
{
    [Get("/latest")]
    Task<FixerResponse> GetExchangeRatesAsync([Query(CollectionFormat.Csv)]string[] symbols, [Query]string @base);
}