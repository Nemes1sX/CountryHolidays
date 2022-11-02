using CountryHolidays.Models.Dtos;

namespace CountryHolidays.Services
{
    public interface IHolidayService
    {
        Task<List<HolidayListDto>> ImportCountryHolidays(string countryCode, int year);
        Task<List<IGrouping<int, HolidayListDto>>> GetCountryHolidaysPerYearGrouped(string countryCode, int year);
        Task<string> GetDayStatus(string date, string countryCode);
        Task<int> MostFreeDays(string countryCode, int year);
    }
}
