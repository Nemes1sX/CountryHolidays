using AutoMapper;
using CountryHolidays.Data;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CountryHolidays.Services
{
    public class CountryService : ICountryService
    {
        private readonly HolidayContext _db;
        private readonly IMapper _mapper;

        public CountryService(HolidayContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<CountryListDto>> GetCountries()
        {
            var countries = await _db.Countries.ToListAsync();

            return _mapper.Map<List<CountryListDto>>(countries);
        }

        public Task<CountryListDto> GetCountry(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CountryListDto>> ImportCountries()
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

            var resultDTO = _mapper.Map<List<CountryListDto>>(result);

            return resultDTO;
        }

        public Task<List<HolidayDto>> ImportCountryHolidays(string countryCode, int year)
        {
            throw new NotImplementedException();
        }
    }
}
