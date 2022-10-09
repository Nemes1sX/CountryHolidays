using AutoMapper;
using CountryHolidays.Data;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;
using System.Text.Json;

namespace CountryHolidays.Services
{
    public class CountryHolidayService : ICountryHolidayService
    {
        private readonly HolidayContext _db;
        private readonly IMapper _mapper;

        public CountryHolidayService(HolidayContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task<List<CountryDto>> GetCountries()
        {
            throw new NotImplementedException();
        }

        public Task<CountryDto> GetCountry(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CountryResponse>> ImportCountries()
        {
            var result = new List<CountryResponse>();
            using (var client = new HttpClient())
            {
                var responseMessage = await client.GetAsync("https://kayaposoft.com/enrico/json/v2.0/?action=getSupportedCountries");

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<List<CountryResponse>>(response);
            }

            var entries = _mapper.Map<Country>(result);

            await _db.AddRangeAsync(entries);
            await _db.SaveChangesAsync();

            return result;
        }
    }
}
