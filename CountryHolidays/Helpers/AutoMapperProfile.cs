using AutoMapper;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;

namespace CountryHolidays.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CountryResponse, Country>();
 
            CreateMap<Country, CountryListDto>();

            CreateMap<Holiday, HolidayListDto>()
                .ForMember(dest => dest.CountryName, act => act.MapFrom(
                   src => src.Country.Name));
        }
    }
}
