namespace Currency.Exchange.Data;

public interface ICurrencyExchangeRepository
{
    Task<int> AddTradeAsync(DbContext.Exchange trade);
    
    Task<int> GetTradesInLastHourAsync(int clientId);
}