using CountryHolidays.Models.Dtos;

namespace CountryHolidays.Services
{
    public interface IHolidayService
    {
        Task<List<HolidayListDto>> ImportCountryHolidays(string countryCode, int year);
        Task<List<HolidayListDto>> GetCountryHolidaysPerYear(string countryCode, int year);
        Task<string> GetDayStatus(string date, string countryCode);
        Task<int> MostFreeDays(string countryCode, int year);
    }
}
