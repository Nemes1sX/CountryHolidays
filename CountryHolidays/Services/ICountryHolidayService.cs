using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Responses;

namespace CountryHolidays.Services
{
    public interface ICountryHolidayService
    {
        Task<List<CountryDto>> GetCountries();
        Task<CountryDto> GetCountry(int id);
        Task<List<CountryResponse>> ImportCountries();
    }
}
