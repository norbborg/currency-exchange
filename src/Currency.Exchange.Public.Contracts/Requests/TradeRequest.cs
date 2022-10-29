namespace Currency.Exchange.Public.Contracts.Requests;

/// <summary>
/// The trade request.
/// </summary>
public class TradeRequest
{
    /// <summary>
    /// Client Id doing the currency exchange trade.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// The currency code to exchange from. 
    /// </summary>
    public string FromCurrencyCode { get; set; }
    
    /// <summary>
    /// The currency code to exchange to.
    /// </summary>
    public string ToCurrencyCode { get; set; }
    
    /// <summary>
    /// The amount to exchange.
    /// </summary>
    public decimal Amount { get; set; }
}