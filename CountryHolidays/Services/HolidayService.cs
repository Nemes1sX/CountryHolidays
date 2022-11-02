using AutoMapper;
using CountryHolidays.Data;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace CountryHolidays.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IConfiguration _configuration;
        private readonly HolidayContext _db;
        private readonly IMapper _mapper;

        public HolidayService(HolidayContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<IGrouping<int, HolidayListDto>>> GetCountryHolidaysPerYearGrouped(string countryCode, int year)
        {
            var countryHolidaysList = await _db.Holidays
                .Where(x => x.Country.CountryCode == countryCode && x.HolidayDate.Year == year).ToListAsync();

           var countryHoloidayListDto = _mapper.Map<List<HolidayListDto>>(countryHolidaysList).GroupBy(x => x.HolidayDate.Month).ToList();

            return countryHoloidayListDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<HolidayListDto>> ImportCountryHolidays(string countryCode, int year)
        {
            var url = _configuration.GetValue<string>("UrlSettings:HolidayApiUrl");
            var countryHolidaysUrl = url + $"=getHolidaysForYear&year={year}&country={countryCode}";
            var countryHolidays = await _db.Holidays.Where(x => x.Country.CountryCode == countryCode && x.HolidayDate.Year == year).ToListAsync();
            if (countryHolidays.Any())
            {
                var existedHolidayDtoList = _mapper.Map<List<HolidayListDto>>(countryHolidays);
                return existedHolidayDtoList;
            }

            var result = new List<HolidayResponse>();
            var holidayList = new List<Holiday>();

            using (var client = new HttpClient())
            {
                var responseMessage = await client.GetAsync(countryHolidaysUrl);

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

            var country = await _db.Countries.FirstOrDefaultAsync(x => x.CountryCode == countryCode);
            if (country == null)
            {
                return null;
            }

            var dayStatus = await _db.Holidays.Where(x => x.Country.CountryCode == countryCode).Select(x => x.Type).FirstOrDefaultAsync();
            if (dayStatus != null)
            {
                status += dayStatus;
            }

            else if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                status = "Free day";
            }
            else
            {
                status = "Work day";
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
            var countryHolidays = await _db.Holidays.Where(x => x.Country.CountryCode == countryCode && x.HolidayDate.Year == year).OrderBy(x => x.HolidayDate).ToListAsync();
            var maxFreeDays = 0;
            if (!countryHolidays.Any())
            {
                return 0;
            }

            foreach (var countryHoliday in countryHolidays)
            {
               var currentMaxFreeDays = 1; 
                var daysCount = 1;
                if (countryHoliday.HolidayDate.AddDays(-2).DayOfWeek == DayOfWeek.Saturday && countryHoliday.HolidayDate.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                {
                    currentMaxFreeDays += 2;
                } else if (countryHoliday.HolidayDate.AddDays(-1).DayOfWeek == DayOfWeek.Saturday)
                {
                    currentMaxFreeDays += 1;
                }
                while (true)
                {              
                    var holiday = await _db.Holidays.FirstOrDefaultAsync(x => x.HolidayDate == countryHoliday.HolidayDate.AddDays(daysCount));
                    if (holiday == null && countryHoliday.HolidayDate.AddDays(daysCount).DayOfWeek != DayOfWeek.Saturday && countryHoliday.HolidayDate.AddDays(daysCount).DayOfWeek != DayOfWeek.Sunday)
                    {
                        break;
                    }
                    currentMaxFreeDays++;
                    daysCount++;
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
