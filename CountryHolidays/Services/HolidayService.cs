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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<string> GetDayStatus(string countryCode, string date)
        {
            DateTime selectedDate;
            DateTime.TryParse(date, out selectedDate);
            var status = string.Empty;

            var dayStatus = await _db.Holidays.Where(x => x.Country.CountryCode == countryCode && x.HolidayDate == selectedDate).Select(x => x.Type).FirstOrDefaultAsync();
            if (dayStatus != null)
            {
                status += dayStatus + ", ";
            }
            if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                status += "Free day";
            }

            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<int> MostFreeDays(string countryCode, int year)
        {
            var countryHolidays = await _db.Holidays.Where(x => x.Country.CountryCode == countryCode && x.HolidayDate.Year == year).ToListAsync();
            var maxFreeDays = 0;
            if (!countryHolidays.Any())
            {
                return maxFreeDays;
            }

            foreach (var countryHoliday in countryHolidays)
            {
               var currentMaxFreeDays = 0; 
               if (countryHoliday.HolidayDate.DayOfWeek ==  DayOfWeek.Monday || countryHoliday.HolidayDate.DayOfWeek == DayOfWeek.Friday)
                {
                    currentMaxFreeDays = 3;
                }
               if ((countryHoliday.HolidayDate.DayOfWeek == DayOfWeek.Tuesday ||
                    countryHoliday.HolidayDate.DayOfWeek == DayOfWeek.Friday) && 
                    (countryHoliday.HolidayDate.Month == 12 &&
                    countryHoliday.HolidayDate.Day == 26))
                {
                    currentMaxFreeDays = 4;
                }
               else
                {
                    currentMaxFreeDays = 2;
                }
               if (currentMaxFreeDays > maxFreeDays)
                {
                    maxFreeDays = currentMaxFreeDays;
                }
            }

            return maxFreeDays;
        }
    }
}
