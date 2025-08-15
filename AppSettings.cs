using System.Text.Json.Serialization;
public class AppSettings
{
    public EmailConfig? EmailConfig { get; set; }

    public ApiConfig? ApiConfig { get; set; }
}

public class EmailConfig
{
    public string? ToEmail { get; set; }
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
}

public class ApiConfig
{
    public string? ApiKey { get; set; }
}

public class AlphaVantageApiResponse
{

    [JsonPropertyName("Global Quote")]
    public GlobalQuoteData? GlobalQuote { get; set; }
}

public class GlobalQuoteData
{

    [JsonPropertyName("01. symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("05. price")]
    public string? PriceString { get; set; }
    
}