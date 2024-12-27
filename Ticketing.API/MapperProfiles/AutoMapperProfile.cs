using AutoMapper;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model.Dto.Category;

namespace Ticketing.API.MapperProfiles
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<Category , CategoryDto>();
            CreateMap<Model.Domain.File , FileResponseDto>();
            CreateMap<SolutionGuide, SolutionGuideResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.SolutionGuideFiles)); 
            CreateMap<Ticket, TicketResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Files, 
                    opt => opt.MapFrom(src => src.TicketFiles));
        }
    }
}
