using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Responses;

namespace CountryHolidays.Services
{
    public interface ICountryService
    {
        Task<List<CountryListDto>> GetCountries();
        Task<List<CountryListDto>> ImportCountries();
    }
}
