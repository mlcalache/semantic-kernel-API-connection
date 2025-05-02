
public interface IFundaService
{
    Task<List<FundaObject>> FetchDataAsync(string? search, string? type, int? numberOfListings);
}