using System.Text.Json.Serialization;

namespace CountryHolidays.Models.Responses
{
    public class HolidayNameResponse
    {

            [JsonPropertyName("lang")]
            public string Lang { get; set; }
            [JsonPropertyName("text")]
            public string Text { get; set; }
    }
}
