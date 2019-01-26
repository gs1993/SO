using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Logic.Dtos;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/posts")]
    public class PostsController : BaseController
    {
        private readonly PostsRepository _postsRepository;

        public PostsController(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _postsRepository = new PostsRepository(unitOfWork);
        }

        [HttpGet]
        [Route("GetPage")]
        public async Task<IActionResult> GetList([FromQuery]int pageNumber, [FromQuery]int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                return Error($"Parameter pageNumber cannot have value: {pageSize}");

            var posts = await _postsRepository.GetPageAsync(pageNumber, pageSize);
            if (posts == null)
                return Error("Not Found");
            
            return Ok(posts);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postsRepository.GetByIdAsync(id);
            if (post == null)
                return Error("Not Found");

            var dto = Mapper.Map<PostDetailsDto>(post);
            return Ok(dto);
        }
    }
}
