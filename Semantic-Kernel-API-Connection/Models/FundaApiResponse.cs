using System.Text.Json.Serialization;

public class FundaApiResponse
{
    [JsonPropertyName("Objects")]
    public List<FundaObject> Objects { get; set; }
    
    [JsonPropertyName("Paging")]
    public PagingInfo Paging { get; set; }
    
    [JsonPropertyName("TotaalAantalObjecten")]
    public int TotaalAantalObjecten { get; set; }
}