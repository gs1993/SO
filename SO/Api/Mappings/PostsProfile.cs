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
            CreateMap<Posts, PostDetailsDto>();
            CreateMap<Posts, PostListDto>();
        }
    }
}
