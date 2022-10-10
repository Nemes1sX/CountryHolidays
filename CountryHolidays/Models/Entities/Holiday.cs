namespace CountryHolidays.Models.Entities
{
    public class Holiday
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public virtual Country Country { get; set; }
    }
}
