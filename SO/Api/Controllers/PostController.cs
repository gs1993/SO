using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Args.Post;
using Api.Utils;
using FluentValidation;
using Logic.Posts.Commands;
using Logic.Posts.Dtos;
using Logic.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    public class PostController : BaseController
    {
        public PostController(IMediator mediator, IValidatorFactory validatorFactory) : base(mediator, validatorFactory)
        {
        }

        [HttpGet()]
        [Route("GetPage")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess<IReadOnlyList<PostListDto>>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> GetPage([FromQuery] GetListArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<GetListArgs>().Validate(args); ;
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var postsDto = await _mediator.Send(new GetPostsPageQuery
            {
                PageNumber = args.PageNumber,
                PageSize = args.PageSize
            }); 
            return FromResult(postsDto);
        }

        [HttpGet]
        [Route("GetLastest")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess<IReadOnlyList<PostListDto>>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> GetLastest([FromQuery] GetLastestArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<GetLastestArgs>().Validate(args); ;
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var postsDto = await _mediator.Send(new GetLastestPostsQuery
            {
                Size = args.Size
            });
            return FromResult(postsDto);
        }

        [HttpGet()]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess<PostDetailsDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();
            
            var postsDto = await _mediator.Send(new GetPostQuery
            {
                Id = id
            });
            return FromResult(postsDto);
        }


        [HttpPost]
        [Route("Close/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Close([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var result = await _mediator.Send(new ClosePostCommand
            {
                Id = id
            });
            return FromResult(result);
        }
    }
}
