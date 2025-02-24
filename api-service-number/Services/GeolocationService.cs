
using api_service_number.Models;
using Newtonsoft.Json;

namespace api_service_number.Services;

public class GeolocationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeolocationService> _logger;

    public GeolocationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GeoLocationDTO?> GetGeolocationAsync(string ip)
    {
        _logger.LogInformation("[START] GeolocationService]");
        string url = $"http://ip-api.com/json/{ip}";
        
        var response = await _httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<GeoLocationDTO>(response);
         
    }
}