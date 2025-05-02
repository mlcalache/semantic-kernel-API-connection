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
    
    [KernelFunction("get_specific_number_of_listings")]
    [Description("Get a specific number of listings from the Funda API")]
    public ServiceResponse GetAllListings(
        [Description("Type of listing, e.g., koop or huur")] string type, 
        [Description("Search location or filter path, e.g., /Amsterdam/Tuin")] string search,
        [Description("Number of listings")] int numberOfListings)
    {
        var result = _searchService.FetchDataAsync(search, type, numberOfListings).Result;
        return result;
    }

    [KernelFunction("get_all_listings")]
    [Description("Get all listings from the Funda API")]
    public ServiceResponse GetListings(
        [Description("Type of listing, e.g., koop or huur")] string type, 
        [Description("Search location or filter path, e.g., /Amsterdam/Tuin")] string search)
    {
        var result = _searchService.FetchDataAsync(search, type, null).Result;
        return result;
    }
}
