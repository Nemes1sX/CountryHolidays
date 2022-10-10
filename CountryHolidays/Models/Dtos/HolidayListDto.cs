namespace CountryHolidays.Models.Dtos
{
    public class HolidayListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Type { get; set; } 
        public string CountryName { get; set; }
    }
}
