using System;
using System.Threading.Tasks;
using AutoMapper;
using Dawn;
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
        public async Task<IActionResult> GetList([FromQuery]int pageNumber, int pageSize)
        {
            Guard.Argument(pageNumber, nameof(pageNumber)).Positive();
            Guard.Argument(pageSize, nameof(pageSize)).Positive();

            var posts = await _postsRepository.GetPageAsync(pageNumber, pageSize);
            if (posts == null)
                return Error("Not Found");
            
            return Ok(posts);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var post = await _postsRepository.GetByIdAsync(id);
            if (post == null)
                return Error("Not Found");

            var dto = Mapper.Map<PostDetailsDto>(post);
            return Ok(dto);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            throw new NotImplementedException();

            return Ok();
        }

        [HttpPost]
        [Route("Close")]
        public async Task<IActionResult> Close([FromQuery]int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var post = await _postsRepository.GetByIdAsync(id);
            if (post == null)
                return Error("Not Found");

            var result = post.Close();
            if (result.IsFailure)
                return Error(result.Error);

            UnitOfWork.Commit();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var result = await _postsRepository.Delete(id);

            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
