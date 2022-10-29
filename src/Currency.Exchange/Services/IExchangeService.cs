using Currency.Exchange.Public.Contracts.Requests;

namespace Currency.Exchange.Services;

public interface IExchangeService
{
    Task<int> AddTrade(TradeRequest request);
}