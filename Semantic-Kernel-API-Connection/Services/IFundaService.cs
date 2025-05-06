
public interface IFundaService
{
    Task<FundaServiceResponse> FetchDataAsync(string? search, string? type, int? numberOfListings);
}