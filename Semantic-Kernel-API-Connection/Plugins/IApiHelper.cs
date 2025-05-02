
public interface IApiHelper
{
    List<FundaObject> GetAllListings(string type, string search, int numberOfListings);
    List<FundaObject> GetListings(string type, string search);
}