using AutoMapper;
using CountryHolidays.Data;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CountryHolidays.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HolidayContext _db;
        private readonly IMapper _mapper;

        public HolidayService(HolidayContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<HolidayDto>> ImportCountryHolidays(string countryCode, int year)
        {
            var country = await _db.Countries.FirstOrDefaultAsync(x => x.CountryCode == countryCode);
            if (country == null)
            {
                return null;
            }

            var result = new List<HolidayResponse>();
           
            using (var client = new HttpClient())
            {
                var responseMessage = await client.GetAsync($"https://kayaposoft.com/enrico/json/v2.0?action=getHolidaysForYear&year={year}&country={countryCode}");

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<List<HolidayResponse>>(response);
            }

            _mapper.Map<List<HolidayResponse>>(result, opts => country.Id);
        }
    }
}
