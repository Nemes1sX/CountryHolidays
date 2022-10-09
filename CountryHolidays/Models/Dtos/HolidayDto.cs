namespace CountryHolidays.Models.Dtos
{
    public class HolidayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Type { get; set; }   
    }
}
