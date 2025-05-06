using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class WeatherResult
{
    [JsonProperty("latitude")]
    [Description("Latitude of the location")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    [Description("Longitude of the location")]
    public double Longitude { get; set; }

    [JsonProperty("generationtime_ms")]
    [Description("Time taken to generate the weather response in milliseconds")]
    public double GenerationtimeMs { get; set; }

    [JsonProperty("utc_offset_seconds")]
    [Description("UTC offset in seconds for the location")]
    public int UtcOffsetSeconds { get; set; }

    [JsonProperty("timezone")]
    [Description("Timezone of the location")]
    public string Timezone { get; set; } = string.Empty;

    [JsonProperty("timezone_abbreviation")]
    [Description("Abbreviation of the timezone")]
    public string TimezoneAbbreviation { get; set; } = string.Empty;

    [JsonProperty("elevation")]
    [Description("Elevation above sea level in meters")]
    public double Elevation { get; set; }

    [JsonProperty("current_weather_units")]
    [Description("Units of measurement for the current weather data")]
    public CurrentWeatherUnits CurrentWeatherUnits { get; set; } = new();

    [JsonProperty("current_weather")]
    [Description("Current weather conditions")]
    public CurrentWeather CurrentWeather { get; set; } = new();

    [JsonProperty("daily_units")]
    [Description("Units of measurement for daily forecasts")]
    public DailyUnits DailyUnits { get; set; } = new();

    [JsonProperty("daily")]
    [Description("Daily weather forecast data")]
    public Daily Daily { get; set; } = new();
}