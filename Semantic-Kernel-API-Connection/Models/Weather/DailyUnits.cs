using Newtonsoft.Json;
using System.ComponentModel;
using Microsoft.SemanticKernel;

public class DailyUnits
{
    [JsonProperty("time")]
    [Description("Format of the date values in the daily forecast")]
    public string Time { get; set; } = string.Empty;

    [JsonProperty("temperature_2m_max")]
    [Description("Unit for daily maximum temperature at 2 meters")]
    public string Temperature2mMax { get; set; } = string.Empty;

    [JsonProperty("temperature_2m_min")]
    [Description("Unit for daily minimum temperature at 2 meters")]
    public string Temperature2mMin { get; set; } = string.Empty;
}
