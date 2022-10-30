using Currency.Exchange.External.Client;

namespace Currency.Exchange.Services;

public class SymbolService : ISymbolService
{
    private readonly ICacheStore _cacheStore;
    private readonly IFixerClient _fixerClient;
    
    private const string CacheKey = "Symbols";

    public SymbolService(ICacheStore cacheStore, IFixerClient fixerClient)
    {
        _cacheStore = cacheStore;
        _fixerClient = fixerClient;
    }

    public async Task<IDictionary<string, string>> GetSymbols()
    {
        var symbols = _cacheStore.Get<IDictionary<string, string>>(CacheKey);

        if (symbols is not null)
        {
            return symbols;
        }

        var fixerResult = await _fixerClient.GetSymbolsAsync();

        if (!fixerResult.Success)
        {
            //Log
            return null;
        }

        symbols = fixerResult.Symbols;
        
        _cacheStore.Add(CacheKey, symbols, TimeSpan.FromDays(1));
        
        return symbols;
    }
}