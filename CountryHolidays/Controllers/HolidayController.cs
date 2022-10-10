using CountryHolidays.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet]
        [Route("import")]
        public async Task<IActionResult> ImportHolidaysPerYear(string countryCode, int year)
        {
            var holidayDtoList = await _holidayService.ImportCountryHolidays(countryCode, year);

            return Ok(new { data = holidayDtoList });
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetCountryHolidaysPerYear(string countryCode, int  year)
        {
            var holidayDtoList = await _holidayService.GetCountryHolidaysPerYear(countryCode, year);

            return Ok(new { data = holidayDtoList } );
        }

    }
}
