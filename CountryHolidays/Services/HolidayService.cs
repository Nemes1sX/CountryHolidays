using AutoMapper;
using CountryHolidays.Data;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public async Task<string> 

        public async Task<List<HolidayListDto>> GetCountryHolidaysPerYear(string countryCode, int year)
        {
            var countryHolidays = await _db.Holidays
                .Where(x => x.Country.CountryCode == countryCode && x.HolidayDate.Year == year)
                .Include(x => x.Country)
               .GroupBy(x => new { 
                    Month = x.HolidayDate.Month,
                    Year = x.HolidayDate.Year,
            }).ToListAsync();

            if (!countryHolidays.Any())
            {
                return null;
            }

            var countryHolidaysDto = _mapper.Map<List<HolidayListDto>>(countryHolidays);

            return countryHolidaysDto;
        }

        public async Task<List<HolidayListDto>> ImportCountryHolidays(string countryCode, int year)
        {
            var country = await _db.Countries.FirstOrDefaultAsync(x => x.CountryCode == countryCode);
            if (country == null)
            {
                return null;
            }

            var result = new List<HolidayResponse>();
            var holidayList = new List<Holiday>();

            using (var client = new HttpClient())
            {
                var responseMessage = await client.GetAsync($"https://kayaposoft.com/enrico/json/v2.0?action=getHolidaysForYear&year={year}&country={countryCode}");

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<List<HolidayResponse>>(response);
                if (result == null)
                {
                    return null;
                }
            }

            foreach (var holidayResult in result)
            {
                var holiday = MapHoliday(holidayResult, country.Id);
                holidayList.Add(holiday);
            }
         
            await _db.Holidays.AddRangeAsync(holidayList);
            await _db.SaveChangesAsync();

            var holidayDtoList = _mapper.Map<List<HolidayListDto>>(holidayList);

            return holidayDtoList;
        }

        private Holiday MapHoliday(HolidayResponse holidayResponse, int countryId)
        {
            var holiday = new Holiday();
            holiday.CountryId = countryId;
            holiday.Name = holidayResponse.Name.FirstOrDefault().Text;
            holiday.Type = holidayResponse.HolidayType;
            holiday.HolidayDate = new DateTime(holidayResponse.DateResponse.Year, holidayResponse.DateResponse.Month, holidayResponse.DateResponse.Day);

            return holiday;
        }
    }
}
