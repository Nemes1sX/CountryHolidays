using CountryHolidays.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CountryHolidays.Data
{
    public class HolidayContext : DbContext
    {
        public HolidayContext(DbContextOptions<HolidayContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Holiday> Holidays { get; set; }   
    }
}
