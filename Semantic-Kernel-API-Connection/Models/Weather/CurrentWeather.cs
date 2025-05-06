using Newtonsoft.Json;
using System.ComponentModel;
using Microsoft.SemanticKernel;

public class CurrentWeather
{
    [JsonProperty("time")]
    [Description("Timestamp of the current weather data (ISO8601 format)")]
    public string Time { get; set; } = string.Empty;

    [JsonProperty("interval")]
    [Description("Time interval in seconds for current weather update")]
    public int Interval { get; set; }

    [JsonProperty("temperature")]
    [Description("Current air temperature in degrees Celsius")]
    public double Temperature { get; set; }

    [JsonProperty("windspeed")]
    [Description("Wind speed in km/h")]
    public double Windspeed { get; set; }

    [JsonProperty("winddirection")]
    [Description("Wind direction in degrees")]
    public int Winddirection { get; set; }

    [JsonProperty("is_day")]
    [Description("Day (1) or night (0) indicator")]
    public int IsDay { get; set; }

    [JsonProperty("weathercode")]
    [Description("WMO weather code representing the current weather condition")]
    public int Weathercode { get; set; }
}
