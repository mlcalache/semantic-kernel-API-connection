# Semantic Kernel + External APIs Integration

This project demonstrates how to use [Microsoft Semantic Kernel v1.47.0](https://github.com/microsoft/semantic-kernel) to orchestrate intelligent workflows connected to external APIs:

- [Funda API](http://partnerapi.funda.nl/feeds/Aanbod.svc/json/) – for real estate listings.
- [Open-Meteo API](https://open-meteo.com/en/docs) – for weather forecasts.

Both APIs are abstracted into Semantic Kernel plugins to allow natural language access through an Azure OpenAI model.

---

## Project Structure

- `Services/`
  - `FundaService.cs` – handles communication with the Funda API.
  - `WeatherService.cs` – fetches forecast data from Open-Meteo.

- `Plugins/`
  - `FundaPlugin.cs` – wraps FundaService as a Semantic Kernel plugin.
  - `WeatherPlugin.cs` – wraps WeatherService for the Semantic Kernel runtime.

- `Program.cs` – bootstraps the application, initializes the kernel, loads plugins, and handles user interaction.

---

## Key Technologies

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Microsoft.SemanticKernel v1.47.0](https://www.nuget.org/packages/Microsoft.SemanticKernel/1.47.0)
- [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
- [Polly](https://github.com/App-vNext/Polly) for resilient HTTP requests
- `appsettings.json` + `dotnet user-secrets` for configuration management

---

## Configuration

### appsettings.json

Used to define retry behavior, default Funda search options, and Semantic Kernel defaults:

```json
{
  "Funda": {
    "SemaphoreLimit": 3,
    "RetryAttempts": 3,
    "RetryBackoffBase": 2,
    "Url": "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/{0}/",
    "PageSize": 25
  },
  "Weather": {
    "Url": "https://api.open-meteo.com/v1/forecast"
  }
}
```

### dotnet user-secrets

Used to securely store your Azure OpenAI API key and other secrets locally during development.

```bash
dotnet user-secrets init
dotnet user-secrets set "SemanticKernel:ApiKey" "your-azure-openai-api-key"
```

To inspect or remove secrets, refer to: [Microsoft Docs: Safe storage of app secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)

## Semantic Kernel Integration

This project uses Semantic Kernel's Kernel.CreateBuilder() to load:

- An Azure OpenAI chat completion service
- Custom plugins using AddFromObject(), e.g.:

```csharp
kernel.Plugins.AddFromObject(fundaPlugin, "Funda");
kernel.Plugins.AddFromObject(weatherPlugin, "Weather");
```

User queries are sent to the kernel via a chat loop using IChatCompletionService.

Each plugin method can expose parameters like "location", "type", or "date" for the model to use in natural language interaction.

## Running the App

```bash
dotnet run
```

Example session:

```bash
=================================================
                 Search a Home                   
=================================================

       /\            /\            /\       
      /  \          /  \          /  \      
     /____\        /____\        /____\     
    | .--. |      | .--. |      | .--. |    
    | |  | |      | |[]| |      | |  | |    
    | |__| |      | |__| |      | |__| |    
    |      |      |      |      |      |    
    |  []  |      |  []  |      |  []  |    
    |______|      |______|      |______|    

User > Show me homes with gardens in Amsterdam

Assistant > Here are 3 listings in Amsterdam with gardens...

User > And what's the weather forecast for tomorrow?

Assistant > The forecast for Amsterdam tomorrow is mostly sunny with highs of 18°C.
```

## External Resources
- [Semantic Kernel GitHub](https://github.com/microsoft/semantic-kernel)
- [Funda Partner API](http://partnerapi.funda.nl/feeds/Aanbod.svc/json/)
- [Open-Meteo API Docs](https://open-meteo.com/en/docs)
- [Azure OpenAI Quickstart](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/quickstart)

## Notes
- This project uses retry logic via Polly for robust API communication.
- Weather forecasts are based on latitude/longitude — be sure your plugin parses location input accordingly.
- The Funda API is region-specific (Netherlands) and requires a valid API key.

## TODO
- Add unit tests for plugin logic
- Support multiple OpenAI backends (OpenAI vs Azure)
- Add caching layer for repeated API queries
- Deploy via Docker for cloud use