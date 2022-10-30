namespace Currency.Exchange.External.Client.Models;

public class SymbolsResponse : BaseResponse
{
    public Dictionary<string, string> Symbols { get; set; }
}