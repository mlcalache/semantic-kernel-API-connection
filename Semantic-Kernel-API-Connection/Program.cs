using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders(); // Remove all default logging providers
        logging.SetMinimumLevel(LogLevel.None); // Disable all logging
    })
    .ConfigureAppConfiguration((context, config) =>
    {
        var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        config.AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true);
        config.AddUserSecrets<Program>();
    })
    .ConfigureServices((context, services) =>
    {
        // Bind FundaOptions from configuration
        services.Configure<FundaOptions>(context.Configuration.GetSection("Funda"));
        var fundaOptions = context.Configuration.GetSection("Funda").Get<FundaOptions>();

        // Bind SemanticKernelOptions from configuration
        services.Configure<SemanticKernelOptions>(context.Configuration.GetSection("SemanticKernel"));
        var skOptions = context.Configuration.GetSection("SemanticKernel").Get<SemanticKernelOptions>();

        // Register HttpClient with Polly retry policy
        services.AddHttpClient("FundaClient")
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: fundaOptions.RetryCount,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(fundaOptions.RetryDelaySeconds)
                ));

        // Register FundaService and ApiHelper
        services.AddSingleton<IFundaService, FundaService>();
        services.AddSingleton<IApiHelper, ApiHelper>();

        // Semantic Kernel
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddAzureOpenAIChatCompletion(
            deploymentName: skOptions.ModelId,
            endpoint: skOptions.Endpoint,
            apiKey: skOptions.ApiKey);
        var kernel = kernelBuilder.Build();

        // Add ApiHelper as a plugin to the kernel
        var provider = services.BuildServiceProvider();
        var apiHelper = provider.GetRequiredService<IApiHelper>();
        kernel.Plugins.AddFromObject(apiHelper, "ApiHelper");

        services.AddSingleton(kernel);
    });

var app = builder.Build();

using var scope = app.Services.CreateScope();

// Get services
var kernel = scope.ServiceProvider.GetRequiredService<Kernel>();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Example of the FundaService use, to call the API
// var fundaService = scope.ServiceProvider.GetRequiredService<IFundaService>();
// var resultFundaService = await fundaService.FetchDataAsync(type: "koop", search: "/Amsterdam/Tuin", currentPage: 1, pageSize: 25);

// Create a chat history
var history = new ChatHistory();

history.AddSystemMessage("You are a strict validator. Never fabricate data. If information is missing, just state which fields are missing. Do not make anything up.");
history.AddSystemMessage("The page size should not change, as it is set in the settings of the application.");

// Execution settings
// Optional: control temperature and other settings
var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
    Temperature = 0,
    TopP = 1,
    MaxTokens = 300
};

KernelArguments kernelArguments;

// Start the chat loop
string? userInput;
do
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("User > ");
    userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput)) break;

    // Create KernelArguments for the userInput
    kernelArguments = new KernelArguments
    {
        { "data", userInput } // "data" should match the parameter name in your plugin
    };

    history.AddUserMessage(userInput);

    var resultChatCompletion = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("Assistant > " + resultChatCompletion);

    history.AddMessage(resultChatCompletion.Role, resultChatCompletion.Content ?? string.Empty);

} while (true);
