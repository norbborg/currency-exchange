using System.Net;
using Currency.Exchange.Exceptions;
using Currency.Exchange.External.Client;
using Microsoft.Extensions.Logging;
using Refit;

namespace Currency.Exchange.Services;

public class SymbolService : ISymbolService
{
    private readonly ICacheStore _cacheStore;
    private readonly IFixerClient _fixerClient;
    private readonly ILogger<SymbolService> _logger;

    private const string CacheKey = "Symbols";

    public SymbolService(ICacheStore cacheStore, IFixerClient fixerClient, ILogger<SymbolService> logger)
    {
        _cacheStore = cacheStore;
        _fixerClient = fixerClient;
        _logger = logger;
    }

    public async Task<IDictionary<string, string>> GetSymbols()
    {
        try
        {
            var symbols = _cacheStore.Get<IDictionary<string, string>>(CacheKey);

            if (symbols is not null)
            {
                return symbols;
            }

            var fixerResult = await _fixerClient.GetSymbolsAsync();

            if (!fixerResult.Success)
            {
                _logger.LogInformation("Fixer endpoint {Endpoint} did not return successful response",
                    nameof(_fixerClient.GetSymbolsAsync));

                throw new FixerUnsuccessfulException("Fixer symbol endpoint did not return successful response");
            }

            symbols = fixerResult.Symbols;

            _cacheStore.Add(CacheKey, symbols, TimeSpan.FromDays(1));

            return symbols;
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Fixer endpoint {Endpoint} was not found", e.Uri);
            }
            
            throw;
        }
    }
}