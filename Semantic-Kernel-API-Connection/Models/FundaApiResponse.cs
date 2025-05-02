using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class FundaApiResponse
{
    [JsonPropertyName("Objects")]
    [Description("The listings (objects) found in the API.")]
    public List<FundaObject> Objects { get; set; }
    
    [JsonPropertyName("Paging")]
    [Description("The information about paging for the results of the objects found in the API.")]
    public PagingInfo Paging { get; set; }
    
    [JsonPropertyName("TotaalAantalObjecten")]
    [Description("The total number of listings found in the API.")]
    public int TotaalAantalObjecten { get; set; }
}