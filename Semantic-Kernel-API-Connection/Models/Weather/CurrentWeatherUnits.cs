using Newtonsoft.Json;
using System.ComponentModel;
using Microsoft.SemanticKernel;

public class CurrentWeatherUnits
{
    [JsonProperty("time")]
    [Description("Format of the time field")]
    public string Time { get; set; } = string.Empty;

    [JsonProperty("interval")]
    [Description("Time interval in seconds between measurements")]
    public string Interval { get; set; } = string.Empty;

    [JsonProperty("temperature")]
    [Description("Unit of temperature (e.g., °C)")]
    public string Temperature { get; set; } = string.Empty;

    [JsonProperty("windspeed")]
    [Description("Unit of wind speed (e.g., km/h)")]
    public string Windspeed { get; set; } = string.Empty;

    [JsonProperty("winddirection")]
    [Description("Unit for wind direction (degrees)")]
    public string Winddirection { get; set; } = string.Empty;

    [JsonProperty("is_day")]
    [Description("Indicates if it is day (1) or night (0)")]
    public string IsDay { get; set; } = string.Empty;

    [JsonProperty("weathercode")]
    [Description("Weather condition code according to WMO")]
    public string Weathercode { get; set; } = string.Empty;
}
