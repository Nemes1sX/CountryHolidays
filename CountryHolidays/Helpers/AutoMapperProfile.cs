using AutoMapper;
using CountryHolidays.Models.Dtos;
using CountryHolidays.Models.Entities;
using CountryHolidays.Models.Responses;

namespace CountryHolidays.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(int countryId)
        {
            CreateMap<CountryResponse, Country>()
                .ForMember(dest => dest.CountryCode, act => act.MapFrom(
                    src => src.CountryCode
                    ));

            CreateMap<Country, CountryListDto>();

            CreateMap<HolidayResponse, Holiday>()
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.HolidayType))
                .ForMember(dest => dest.CountryId, act => act.MapFrom(src => src.HolidayType == OptionsBuilderConfigurationExtensions.);
        }
    }
}
