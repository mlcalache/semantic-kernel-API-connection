
public interface IWeatherService
{
    Task<WeatherResult?> GetWeatherByLocationNameAsync(string location);
}