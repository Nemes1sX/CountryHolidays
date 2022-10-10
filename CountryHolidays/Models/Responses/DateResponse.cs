using System.Text.Json.Serialization;

namespace CountryHolidays.Models.Responses
{
    public class DateResponse
    { 
        [JsonPropertyName("day")]
        public int Day { get; set; }
        [JsonPropertyName("month")]

        public int Month { get; set; }
        [JsonPropertyName("year")]

        public int Year { get; set; }
        [JsonPropertyName("dayOfWeek")]

        public int DayOfWeek { get; set; }
    }
}
