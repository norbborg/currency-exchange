namespace Currency.Exchange.External.Client;

public class FixerResponse
{
  public string Base { get; set; }
  public DateTime Date { get; set; }
  public Dictionary<string, decimal> Rates { get; set; }
  public bool Success { get; set; }
}
