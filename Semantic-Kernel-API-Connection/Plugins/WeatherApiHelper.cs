using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class WeatherApiHelper : IWeatherApiHelper
{
    private readonly IWeatherService _weatherService;

    public WeatherApiHelper(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [KernelFunction("get_specific_weather_by_location")]
    [Description("Get the weather from a location using the Geocode API within the Weather API by Meteo-Open.")]
    public WeatherResult GetWeather([Description("The target location to search for weather information.")]string location)
    {
        var result = _weatherService.GetWeatherByLocationNameAsync(location).Result;

        return result;
    }
}