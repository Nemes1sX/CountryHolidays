using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryHolidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        [HttpGet]
        [Route("import")]
        public async Task<IActionResult> ImportHolidaysPerYear(int year, string countryCode)
        {

        }
    }
}
