using CountryHolidays.Models.Dtos;

namespace CountryHolidays.Services
{
    public interface IHolidayService
    {
        Task<List<HolidayDto>> ImportCountryHolidays(string countryCode, int year);
    }
}
