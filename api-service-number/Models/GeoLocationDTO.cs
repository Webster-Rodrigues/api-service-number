using System.ComponentModel.DataAnnotations;

namespace api_service_number.Models.Models.Enum;

public class GeoLocationDTO
{
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string RegionName { get; set; }
    public string TimeZone { get; set; }
    public string Status { get; set; }
}