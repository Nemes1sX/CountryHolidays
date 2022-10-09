using CountryHolidays.Models.Responses;
using CountryHolidays.Services;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryHolidayService;

        public CountryController(ICountryService countryHolidayService)
        {
            _countryHolidayService = countryHolidayService;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var countries = await _countryHolidayService.GetCountries();

            return Ok(new { result = countries });
        }

        [HttpGet]
        [Route("import")]
        public async Task<IActionResult> ImportCountries()
        {
            var importedCountries = await _countryHolidayService.ImportCountries();

            return Ok(new {result = importedCountries });
        }
    }
}
