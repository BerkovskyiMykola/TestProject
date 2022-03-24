using AutoMapper;
using TestProject.Models;
using TestProject.ModelsDTO.Request;
using TestProject.ModelsDTO.Response;
using TestProject.Services.Authorization.Models;

namespace TestProject.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<User, JwtUser>();
            CreateMap<User, AuthorizeResponse>()
                .ForMember(x => x.UserId, opts => opts.MapFrom(s => s.Id))
                .ForMember(x => x.Role, opts => opts.MapFrom(s => s.Role.ToString()));
            CreateMap<User, ProfileResponse>()
                .ForMember(x => x.Role, opts => opts.MapFrom(s => s.Role.ToString()));
            CreateMap<User, UserResponse>();
        }
    }
}
