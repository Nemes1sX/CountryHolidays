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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryHolidayService"></param>
        public CountryController(ICountryService countryHolidayService)
        {
            _countryHolidayService = countryHolidayService;
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var countries = await _countryHolidayService.GetCountries();

            return Ok(new { result = countries });
        }

        /// <summary>
        /// Import all countries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("import")]
        public async Task<IActionResult> ImportCountries()
        {
            var importedCountries = await _countryHolidayService.ImportCountries();

            return Ok(new {result = importedCountries });
        }
    }
}
