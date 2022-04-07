using AutoMapper;
using TestProject.BLL.Services.JWT.Models;
using TestProject.DAL.Entities;
using TestProject.DTO.Request;
using TestProject.DTO.Response;

namespace TestProject.BLL.MappingProfiles
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
            CreateMap<UserRequest, User>();
        }
    }
}
