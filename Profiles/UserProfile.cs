using UserAuth.Data.Dtos;
using UserAuth.Model;
using AutoMapper;

namespace UserAuth.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
        }
    }
}
