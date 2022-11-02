using Currency.Exchange.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Currency.Exchange.Data;

public class CurrencyExchangeRepository : ICurrencyExchangeRepository
{
    private readonly exchangeContext _exchangeContext;

    public CurrencyExchangeRepository(exchangeContext exchangeContext)
    {
        _exchangeContext = exchangeContext;
    }

    public async Task<int> AddTradeAsync(DbContext.Exchange trade)
    {
        var entity = _exchangeContext.Exchanges.Add(trade);
        await _exchangeContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public Task<int> GetTradesInLastHourAsync(int clientId)
    {
        return _exchangeContext.Exchanges.CountAsync(exchange =>
            exchange.Clientid.Equals(clientId) &&
            exchange.Datecreated.ToUniversalTime() > DateTime.UtcNow.AddHours(-1) 
        );
    }
}