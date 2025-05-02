using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddUserSecrets<Program>();
    })
    .ConfigureServices((context, services) =>
    {
        // Bind FundaOptions from configuration
        services.Configure<FundaOptions>(context.Configuration.GetSection("Funda"));
        var fundaOptions = context.Configuration.GetSection("Funda").Get<FundaOptions>();

        // Register HttpClient with Polly retry policy
        services.AddHttpClient("FundaClient")
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: fundaOptions.RetryCount,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(fundaOptions.RetryDelaySeconds)
                ));

        // Register FundaService
        services.AddSingleton<FundaService>();
    });

var app = builder.Build();

using var scope = app.Services.CreateScope();
var fundaService = scope.ServiceProvider.GetRequiredService<FundaService>();

// Example usage
var result = await fundaService.FetchDataAsync(type: "koop", search: "/Amsterdam/Tuin", currentPage: 1, pageSize: 25);
