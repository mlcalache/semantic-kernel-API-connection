using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;

using System.Net.Http;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.RateLimit;
using Polly.Retry;

using Newtonsoft.Json;


public interface IFundaService
{
    Task<List<FundaObject>> FetchDataAsync(string? search, string? type, int pageSize, int currentPage);
}

public class FundaService : IFundaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly FundaOptions _options;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private readonly SemaphoreSlim _semaphore; // Max 5 concurrent calls

    public FundaService(IHttpClientFactory httpClientFactory, IOptions<FundaOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;

        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(_options.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(_options.RetryDelaySeconds));

        _semaphore = new SemaphoreSlim(_options.SemaphoreLimit);
    }

    public async Task<List<FundaObject>> FetchDataAsync(string? search, string? type, int pageSize, int currentPage)
    {
        var allObjects = new List<FundaObject>();

        var initialResponse = await FetchPageAsync(search, type, currentPage, pageSize);
        var result = JsonConvert.DeserializeObject<FundaApiResponse>(initialResponse);

        allObjects.AddRange(result.Objects);
        int totalPages = result.Paging.AantalPaginas;

        var tasks = new List<Task>();

        for (int page = 2; page <= totalPages; page++)
        {
            var capturedPage = page;
            tasks.Add(Task.Run(async () =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    var response = await FetchPageAsync(search, type, capturedPage, pageSize);
                    var data = JsonConvert.DeserializeObject<FundaApiResponse>(response);
                    lock (allObjects)
                    {
                        allObjects.AddRange(data.Objects);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
                finally
                {
                    _semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
        return allObjects;
    }

    private async Task<string> FetchPageAsync(string search, string type, int currentPage, int pageSize)
    {
        var baseUrl = string.Format(_options.Url, _options.ApiKey);

        var queryParams = new Dictionary<string, string?>
        {
            ["type"] = type,
            ["zo"] = search,
            ["pagesize"] = pageSize.ToString(),
            ["page"] = currentPage.ToString()
        };

        var url = QueryHelpers.AddQueryString(baseUrl, queryParams);

        var client = _httpClientFactory.CreateClient("FundaClient");
        var response = await client.GetStringAsync(url);
        return response;
    }
}