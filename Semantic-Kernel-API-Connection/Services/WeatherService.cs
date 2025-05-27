using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WeatherOptions _options;

    public WeatherService(IHttpClientFactory httpClientFactory, IOptions<WeatherOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    private async Task<WeatherResult?> GetWeatherAsync(double latitude, double longitude)
    {
        var url = $"{_options.Url}?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
          $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
          "&daily=temperature_2m_max,temperature_2m_min" +
          "&current_weather=true" +
          "&timezone=auto";

        var client = _httpClientFactory.CreateClient("WeatherClient");
        var response = await client.GetStringAsync(url);

        var data = JsonConvert.DeserializeObject<WeatherResult>(response);

        return data;
    }

    public async Task<WeatherResult?> GetWeatherByLocationNameAsync(string location)
    {
        var geocodeUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(location)}&count=1&language=en&format=json";

        var client = _httpClientFactory.CreateClient("GeoCodeClient");

        try
        {
            var geoResponse = await client.GetStringAsync(geocodeUrl);

            var geoResult = JsonConvert.DeserializeObject<GeocodeResult>(geoResponse);

            if (geoResult?.Results == null || geoResult.Results.Length == 0)
            {
                Console.WriteLine($"No coordinates found for location '{location}'.");
                return null;
            }

            var coords = geoResult.Results[0];
            Console.WriteLine($"Resolved '{location}' to: {coords.Latitude}, {coords.Longitude}");

            return await GetWeatherAsync(coords.Latitude, coords.Longitude);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving weather for '{location}': {ex.Message}");
            return null;
        }
    }
}