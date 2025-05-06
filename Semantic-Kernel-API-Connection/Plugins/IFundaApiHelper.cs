
public interface IFundaApiHelper
{
    FundaServiceResponse GetAllListings(string type, string search, int numberOfListings);
    FundaServiceResponse GetListings(string type, string search);
}