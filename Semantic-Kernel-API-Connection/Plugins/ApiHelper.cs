using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.Json;

public class ApiHelper : IApiHelper
{
    private readonly IFundaService _searchService;

    public ApiHelper(IFundaService searchService)
    {
        _searchService = searchService;
    }
    
    [KernelFunction("get_all_listings")]
    [Description("Get listings from the Funda API")]
    public List<FundaObject> GetAllListings(
        [Description("Type of listing, e.g., koop or huur")] string type, 
        [Description("Search location or filter path, e.g., /Amsterdam/Tuin")] string search,
        [Description("Number of listings")] int numberOfListings)
    {
        //var result = await fundaService.FetchDataAsync(type: "koop", search: "/Amsterdam/Tuin", currentPage: 1, pageSize: 25);
        return _searchService.FetchDataAsync(search, type, numberOfListings).Result;
    }
}
