using System;
using Api.Dtos;
using AutoMapper;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/posts")]
    public class PostsController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PostsRepository _postsRepository;

        public PostsController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postsRepository = new PostsRepository(unitOfWork);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var post = _postsRepository.GetById(id);
            if (post == null)
                return Error("Not Found");

            var dto = _mapper.Map<PostDetailsDto>(post);
            return Ok(dto);
        }
    }
}
