
public interface IFundaService
{
    Task<ServiceResponse> FetchDataAsync(string? search, string? type, int? numberOfListings);
}