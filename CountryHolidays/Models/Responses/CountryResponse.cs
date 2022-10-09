using System.Text.Json.Serialization;

namespace CountryHolidays.Models.Responses
{
    public class CountryResponse
    {
        [JsonPropertyName("fullName")]
        public string Name { get; set; }
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }
}
