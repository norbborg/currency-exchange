namespace Currency.Exchange.External.Client.Models;

public class ExchangeRateResponse : BaseResponse
{
  public string Base { get; set; }
  public DateTime Date { get; set; }
  public Dictionary<string, decimal> Rates { get; set; }
}
