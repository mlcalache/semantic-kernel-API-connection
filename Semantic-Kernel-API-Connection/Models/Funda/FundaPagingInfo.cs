using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class FundaPagingInfo
{
    [JsonPropertyName("AantalPaginas")]
    [Description("Total number of pages, considering the page size and the total number of objets found in the search.")]
    public int AantalPaginas { get; set; }
    
    [JsonPropertyName("HuidigePagina")]
    [Description("The number of the current page.")]
    public int HuidigePagina { get; set; }
    
    [JsonPropertyName("VolgendeUrl")]
    [Description("The endpoint URL for the next page.")]
    public string VolgendeUrl { get; set; }
    
    [JsonPropertyName("VorigeUrl")]
    [Description("The endpoint URL for the previous page.")]
    public string VorigeUrl { get; set; }
}