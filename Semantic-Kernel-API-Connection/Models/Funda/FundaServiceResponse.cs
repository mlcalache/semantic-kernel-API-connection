using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class FundaServiceResponse
{
    [JsonPropertyName("Objects")]
    [Description("The listings (objects) found by the service.")]
    public List<FundaObject> Objects { get; set; }

    [JsonPropertyName("TotalObjects")]
    [Description("The total number of listings found by the service.")]
    public int TotalObjects { get; set; }

    public FundaServiceResponse()
    {
        Objects = new List<FundaObject>();
    }
}