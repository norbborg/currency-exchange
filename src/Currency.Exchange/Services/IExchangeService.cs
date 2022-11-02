using Currency.Exchange.Models;
using Currency.Exchange.Public.Contracts.Requests;

namespace Currency.Exchange.Services;

public interface IExchangeService
{
    Task<Result<int>> AddTradeAsync(TradeRequest request);
}