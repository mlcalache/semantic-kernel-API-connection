public class FundaOptions
{
    public string ApiKey { get; set; }
    public int SemaphoreLimit { get; set; }
    public int RetryCount { get; set; }
    public int RetryDelaySeconds { get; set; }
    public string Url { get; set; }
    public int PageSize { get; set; }
}