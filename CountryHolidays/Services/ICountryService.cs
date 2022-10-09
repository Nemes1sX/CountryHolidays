using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Responses;

namespace CountryHolidays.Services
{
    public interface ICountryService
    {
        Task<List<CountryListDto>> GetCountries();
        Task<CountryListDto> GetCountry(int id);
        Task<List<CountryListDto>> ImportCountries();
        Task<List<HolidayDto>> ImportCountryHolidays(string countryCode, int year);
    }
}
