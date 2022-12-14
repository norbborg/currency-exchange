namespace Currency.Exchange.External.Client;

public class FixerOptions
{
    public const string SectionName = "FixerClientSettings";

    public const string ApiKeyHeaderName = "apikey";

    public string BaseUrl { get; set; }

    public string ApiKey { get; set; }
}