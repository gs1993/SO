using AutoMapper;
using Logic.Dtos;
using Logic.Models;

namespace Api.Mappings
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<Users, UserDetailsDto>();
        }
    }
}
