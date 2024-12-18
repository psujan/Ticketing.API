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
            CreateMap<Model.Domain.File , FileResponseDto>();
            CreateMap<SolutionGuide, SolutionGuideResponseDto>().ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserResponseDto
            {
                Id = src.User.Id,
                Email = src.User.Email,
            }))
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files)); 
        }
    }
}
