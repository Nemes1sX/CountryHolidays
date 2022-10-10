using System.Text.Json.Serialization;

namespace CountryHolidays.Models.Responses
{
    public class HolidayResponse
    {
            [JsonPropertyName("date")]
            public DateResponse DateResponse { get; set; }
            [JsonPropertyName("name")]
            public List<HolidayNameResponse> Name { get; set; }
            [JsonPropertyName("holidayType")]
            public string HolidayType { get; set; }
    }
}
