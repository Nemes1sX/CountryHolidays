using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryHolidays.Migrations
{
    public partial class HolidayUpdateV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthDay",
                table: "Holidays",
                newName: "HolidayDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HolidayDate",
                table: "Holidays",
                newName: "MonthDay");
        }
    }
}
