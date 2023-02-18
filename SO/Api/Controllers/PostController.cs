using Api.Args.Post;
using Api.Utils;
using FluentValidation;
using Logic.BoundedContexts.Posts.Commands;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class PostController : BaseController
    {
        public PostController(IMediator mediator, IValidatorFactory validatorFactory) : base(mediator, validatorFactory)
        {
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(PaginatedPostList))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Get([FromQuery] GetArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<GetArgs>().Validate(args);
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var postListResult = await _mediator.Send(new GetPostsPageQuery
            (
                offset: args.Offset,
                limit: args.Limit
            ));
            return FromCustomResult(postListResult);
        }

        [HttpGet]
        [Route("GetLastest")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(IReadOnlyList<PostListDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> GetLastest([FromQuery] GetLastestArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<GetLastestArgs>().Validate(args); ;
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var postsDto = await _mediator.Send(new GetLastestPostsQuery
            (
                size: args.Size
            ));
            return FromCustomResult(postsDto);
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(PostDetailsDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var postsDto = await _mediator.Send(new GetPostQuery(id));
            return FromCustomResult(postsDto);
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Create([FromBody] CreateArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<CreateArgs>().Validate(args);
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var result = await _mediator.Send(new CreatePostCommand
            (
                title: args.Title,
                body: args.Body,
                authorId: args.AuthorId,
                tags: args.Tags,
                parentId: args.ParentId
            ));
            return FromResult(result, successStatusCode: 201);
        }

        [HttpPut]
        [Route("Close/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Close([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var result = await _mediator.Send(new ClosePostCommand(id));
            return FromResult(result);
        }

        [HttpPut]
        [Route("AddComment/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> AddComment([FromRoute] int id, [FromBody] AddCommentArgs args)
        {
            if (id < 1)
                return ValidationIdError();
            var validationResult = _validatorFactory.GetValidator<AddCommentArgs>().Validate(args);
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var result = await _mediator.Send(new AddCommentCommand
            (
                postId: id,
                comment: args.Comment,
                userId: args.UserId
            ));
            return FromResult(result);
        }

        [HttpPut]
        [Route("UpVote/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> UpVote([FromRoute] int id, [FromBody] UpVoteArgs args)
        {
            if (id < 1)
                return ValidationIdError();
            var validationResult = _validatorFactory.GetValidator<UpVoteArgs>().Validate(args);
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var result = await _mediator.Send(new UpVoteCommand
            (
                postId: id,
                userId: args.UserId
            ));
            return FromResult(result);
        }

        [HttpPut]
        [Route("DownVote/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> DownVote([FromRoute] int id, [FromBody] DownVoteArgs args)
        {
            if (id < 1)
                return ValidationIdError();
            var validationResult = _validatorFactory.GetValidator<DownVoteArgs>().Validate(args);
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var result = await _mediator.Send(new DownVoteCommand
            (
                postId: id,
                userId: args.UserId
            ));
            return FromResult(result);
        }
    }
}
