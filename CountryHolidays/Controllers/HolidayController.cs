using CountryHolidays.Services;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="holidayService"></param>
        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        /// <summary>
        /// Import Holidays for the country in the selected calendar year
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("import")]
        public async Task<IActionResult> ImportHolidaysPerYear(string countryCode, int year)
        {
            var holidayDtoList = await _holidayService.ImportCountryHolidays(countryCode, year);
            if (holidayDtoList == null)
            {
                return NotFound(new {message = "Country are not found or the year is too early"});
            }

            return Ok(new { data = holidayDtoList });
        }

        /// <summary>
        /// Get country holidays per selected calendar year
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetCountryHolidaysPerYear(string countryCode, int  year)
        {
            var holidayDtoList = await _holidayService.GetCountryHolidaysPerYear(countryCode, year);
            if (holidayDtoList == null)
            {
                return NotFound(new { message = "Country and their related holidays are not found" });
            }

            return Ok(new { data = holidayDtoList } );
        }

        /// <summary>
        /// Get selected calendar year status
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("daystatus")]
        public async Task<IActionResult> GetDayStatus(string countryCode, string date)
        {
            var status = await _holidayService.GetDayStatus(countryCode, date);
            if (status == null)
            {
                return NotFound(new { msg = "Country not found" });
            }

            return Ok(new { day = status });
        }


        /// <summary>
        /// Get max free days per selected year in the chosen country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("freedays")]
        public async Task<IActionResult> MaxFreeDays(string countryCode, int year)
        {
            var freeDays = await _holidayService.MostFreeDays(countryCode, year);

            if (freeDays == 0)
            {
                return NotFound(new {msg = "Country hasn't got imported that calendar year holidays"});
            }

            return Ok(new { maxFreeDays = freeDays });
        }

    }
}
