using AutoMapper;
using Logic.Dtos;
using Logic.Models.Users;

namespace Api.Mappings
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<Users, UserDetailsDto>()
                .ForMember(dest => dest.UpVotes, opt => opt.MapFrom(u => u.VoteSummary.UpVotes))
                .ForMember(dest => dest.DownVotes, opt => opt.MapFrom(u => u.VoteSummary.DownVotes))
                .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(u => u.VoteSummary.VoteCount));
        }
    }
}
