namespace CountryHolidays.Models.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public IList<Holiday> Holidays { get; set; }
    }
}
