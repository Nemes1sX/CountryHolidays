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
        private IConfiguration _configuration;
        private readonly HolidayContext _db;
        private readonly IMapper _mapper;

        public CountryService(HolidayContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryListDto>> GetCountries()
        {
            var countries = await _db.Countries.ToListAsync();

            return _mapper.Map<List<CountryListDto>>(countries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryListDto>> ImportCountries()
        {
            var url = _configuration.GetValue<string>("UrlSettings:HolidayApiUrl");
            var importCountriesUrl = url + "=getSupportedCountries";
            var existedCountries = await _db.Countries.ToListAsync();
            if (existedCountries.Any())
            {
                return _mapper.Map<List<CountryListDto>>(existedCountries);
            }
            var result = new List<CountryResponse>();
            using (var client = new HttpClient())
            {
                var responseMessage = await client.GetAsync(importCountriesUrl);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<List<CountryResponse>>(response);
            }

            var entries = _mapper.Map<List<Country>>(result);

            await _db.Countries.AddRangeAsync(entries);
            await _db.SaveChangesAsync();

            var resultDTO = _mapper.Map<List<CountryListDto>>(entries);

            return resultDTO;
        }


    }
}
