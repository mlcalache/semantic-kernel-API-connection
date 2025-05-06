using Newtonsoft.Json;
using System.ComponentModel;
using Microsoft.SemanticKernel;

public class Daily
{
    [JsonProperty("time")]
    [Description("Dates for which the daily forecast is provided")]
    public string[] Time { get; set; } = Array.Empty<string>();

    [JsonProperty("temperature_2m_max")]
    [Description("Daily maximum temperatures at 2 meters height")]
    public double[] Temperature2mMax { get; set; } = Array.Empty<double>();

    [JsonProperty("temperature_2m_min")]
    [Description("Daily minimum temperatures at 2 meters height")]
    public double[] Temperature2mMin { get; set; } = Array.Empty<double>();
}
