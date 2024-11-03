using AutoMapper;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;

namespace Ticketing.API.MapperProfiles
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
