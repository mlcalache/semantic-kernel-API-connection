using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;

public static class KernelHostBuilder
{
    public static IHost Build(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.None);
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                config.AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true);
                config.AddUserSecrets<Program>();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<FundaOptions>(context.Configuration.GetSection("Funda"));
                var fundaOptions = context.Configuration.GetSection("Funda").Get<FundaOptions>();

                services.Configure<SemanticKernelOptions>(context.Configuration.GetSection("SemanticKernel"));
                var skOptions = context.Configuration.GetSection("SemanticKernel").Get<SemanticKernelOptions>();

                services.AddHttpClient("FundaClient")
                    .AddPolicyHandler(HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            retryCount: fundaOptions.RetryCount,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(fundaOptions.RetryDelaySeconds)
                        ));

                services.AddSingleton<IFundaService, FundaService>();
                services.AddSingleton<IFundaApiHelper, FundaApiHelper>();

                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddAzureOpenAIChatCompletion(
                    deploymentName: skOptions.ModelId,
                    endpoint: skOptions.Endpoint,
                    apiKey: skOptions.ApiKey);
                var kernel = kernelBuilder.Build();

                // Build intermediate service provider to resolve plugin dependency
                var provider = services.BuildServiceProvider();
                var apiHelper = provider.GetRequiredService<IFundaApiHelper>();
                kernel.Plugins.AddFromObject(apiHelper, "ApiHelper");

                services.AddSingleton(kernel);
            })
            .Build();
    }
}
