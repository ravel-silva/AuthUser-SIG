using UserAuth.Data.Dtos;
using UserAuth.Model;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace UserAuth.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();

            CreateMap<CreateRoleDto, IdentityRole>();

            CreateMap<CreateRoleUserDto, IdentityRole>();
        }
    }
}
