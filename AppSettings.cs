public class AppSettings
{
    public EmailConfig EmailConfig { get; set; }

    public ApiConfig ApiConfig { get; set; }
}

public class EmailConfig
{
    public string ToEmail { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
}

public class ApiConfig
{
    public string ApiKey { get; set; }
}