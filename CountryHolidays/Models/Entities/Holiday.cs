namespace CountryHolidays.Models.Entities
{
    public class Holiday
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string MonthDay { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
