namespace Currency.Exchange.Services;

public interface ISymbolService
{
    Task<IDictionary<string, string>> GetSymbolsAsync();
}