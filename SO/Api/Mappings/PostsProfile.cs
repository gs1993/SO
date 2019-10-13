using System.Collections.Generic;
using AutoMapper;
using Logic.Dtos;
using Logic.Models;

namespace Api.Mappings
{
    public class PostsProfile : Profile
    {
        public PostsProfile()
        {
            CreateMap<Posts, PostDetailsDto>()
                .ForMember(m => m.AnswerCount, o => o.MapFrom(s => s.AnswerCount ?? 0))
                .ForMember(m => m.CommentCount, o => o.MapFrom(s => s.CommentCount ?? 0))
                .ForMember(m => m.IsClosed, o => o.MapFrom(s => s.ClosedDate != null));
            CreateMap<Posts, PostListDto>();
        }
    }
}
