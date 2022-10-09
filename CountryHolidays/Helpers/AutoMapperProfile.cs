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
            CreateMap<CountryResponse, Country>()
                .ForMember(dest => dest.CountryCode, act => act.MapFrom(
                    src => src.CountryCode
                    ));

            CreateMap<Country, CountryListDto>();
        }
    }
}
