public class FundaOptions
{
     public string ApiKey { get; set; } = string.Empty;
    public int SemaphoreLimit { get; set; } = 5;
    public int RetryCount { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 2;
    public string Url { get; set; }
}