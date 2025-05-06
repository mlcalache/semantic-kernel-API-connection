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

var app = KernelHostBuilder.Build(args);

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
    MaxTokens = 1000
};

KernelArguments kernelArguments;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=================================================");
Console.WriteLine("                 Search a Home                   ");
Console.WriteLine("=================================================");
Console.ForegroundColor = ConsoleColor.Yellow;

Console.WriteLine();
Console.WriteLine("       /\\            /\\            /\\       ");
Console.WriteLine("      /  \\          /  \\          /  \\      ");
Console.WriteLine("     /____\\        /____\\        /____\\     ");
Console.WriteLine("    | .--. |      | .--. |      | .--. |    ");
Console.WriteLine("    | |  | |      | |[]| |      | |  | |    ");
Console.WriteLine("    | |__| |      | |__| |      | |__| |    ");
Console.WriteLine("    |      |      |      |      |      |    ");
Console.WriteLine("    |  []  |      |  []  |      |  []  |    ");
Console.WriteLine("    |______|      |______|      |______|    ");
Console.WriteLine();

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
