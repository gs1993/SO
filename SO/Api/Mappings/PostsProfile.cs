﻿using Api.Dtos;
using AutoMapper;
using Logic.Models;

namespace Api.Mappings
{
    public class PostsProfile : Profile
    {
        public PostsProfile()
        {
            CreateMap<Posts, PostDetailsDto>();
        }
    }
}