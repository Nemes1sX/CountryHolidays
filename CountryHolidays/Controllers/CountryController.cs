using CountryHolidays.Models.Responses;
using CountryHolidays.Services;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryHolidayService _countryHolidayService;

        public CountryController(ICountryHolidayService countryHolidayService)
        {
            _countryHolidayService = countryHolidayService;
        }

        [HttpGet]
        [Route("import")]
        public async Task<IActionResult> ImportCountries()
        {
            var countries = await _countryHolidayService.ImportCountries();

            return Ok(new {result = countries });
        }
    }
}
