using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class FundaObject
{
    [JsonPropertyName("Adres")]
    [Description("The address of the listing.")]
    public string Adres { get; set; }

    [JsonPropertyName("Koopprijs")]
    [Description("The asking price of the listing.")]
    public decimal? Koopprijs { get; set; }

    // public string Foto { get; set; }

    [JsonPropertyName("URL")]
    [Description("The URL of the listing published at Funda.")]
    public string URL { get; set; }

    // Other fields...
}