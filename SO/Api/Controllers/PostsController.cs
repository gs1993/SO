using System.Linq;
using System.Threading.Tasks;
using Dawn;
using Logic.Posts.Commands;
using Logic.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/posts")]
    [EnableCors("AllowMyOrigin")]
    public class PostsController : BaseController
    {
        public PostsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("GetPage")]
        public async Task<IActionResult> GetList([FromQuery]int pageNumber, int pageSize)
        {
            Guard.Argument(pageNumber, nameof(pageNumber)).Positive();
            Guard.Argument(pageSize, nameof(pageSize)).Positive();
            Guard.Argument(pageSize, nameof(pageSize)).LessThan(1000);

            var postsDto = await _mediator.Send(new GetPostsPageQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return FromResult(postsDto);
        }

        [HttpGet]
        [Route("GetLastest")]
        public async Task<IActionResult> GetLastest([FromQuery]int size)
        {
            Guard.Argument(size, nameof(size)).Positive();
            Guard.Argument(size, nameof(size)).LessThan(1000);

            var postsDto = await _mediator.Send(new GetLastestPostsQuery
            {
                Size = size
            });

            return FromResult(postsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var postsDto = await _mediator.Send(new GetPostQuery
            {
                Id = id
            });

            return FromResult(postsDto);
        }


        [HttpPost]
        [Route("Close")]
        public async Task<IActionResult> Close([FromQuery]int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var result = await _mediator.Send(new ClosePostCommand
            {
                Id = id
            });

            return FromResult(result);
        }
    }
}
