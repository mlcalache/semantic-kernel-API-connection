
public interface IApiHelper
{
    ServiceResponse GetAllListings(string type, string search, int numberOfListings);
    ServiceResponse GetListings(string type, string search);
}