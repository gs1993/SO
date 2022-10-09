using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Logic.BoundedContexts.Posts.Queries;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcPostServer
{
    public class PostGrpcService : Post.PostBase
    {
        private readonly IMediator _mediator;
        private readonly IValidatorFactory _validatorFactory;

        public PostGrpcService(IMediator mediator, IValidatorFactory validatorFactory)
        {
            _mediator = mediator;
            _validatorFactory = validatorFactory;
        }

        public override async Task<GetListResponse> GetList(GetListRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var validationResult = _validatorFactory.GetValidator<Api.Args.Post.GetArgs>().Validate(new Api.Args.Post.GetArgs
            {
                Limit = request.Limit,
                Offset = request.Offset
            });
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.ToString(), nameof(request));

            var postsDto = await _mediator.Send(new GetPostsPageQuery
            (
                offset: request.Offset,
                limit: request.Limit
            ));
            var result = new GetListResponse();
            result.Post.AddRange(postsDto.Select(x => new PostDto
            {
                Id = x.Id,
                AnswerCount = x.AnswerCount,
                IsClosed = x.IsClosed
            }));
            return result;
        }

        public override async Task<GetLastestResponse> GetLastest(GetLastestRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var validationResult = _validatorFactory.GetValidator<Api.Args.Post.GetLastestArgs>().Validate(new Api.Args.Post.GetLastestArgs
            {
                Size = request.Size
            });
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.ToString(), nameof(request));

            var postsDto = await _mediator.Send(new GetLastestPostsQuery
            (
                size: request.Size
            ));
            var result = new GetLastestResponse();
            result.Post.AddRange(postsDto.Select(x => new PostDto
            {
                Id = x.Id,
                AnswerCount = x.AnswerCount,
                IsClosed = x.IsClosed
            }));
            return result;
        }

        public override async Task<PostDetailsDto> Get(GetRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.Id < 1)
                throw new ArgumentException("Invalid id", nameof(request.Id));

            var postDetailsDto = await _mediator.Send(new GetPostQuery
            (
                id: request.Id
            ));
            if (postDetailsDto == null)
                return null;

            var result = new PostDetailsDto
            {
                Id = postDetailsDto.Id,
                AnswerCount = postDetailsDto.AnswerCount,
                Body = postDetailsDto.Body,
                ClosedDate = Timestamp.FromDateTime(postDetailsDto.ClosedDate ?? DateTime.MinValue),
                CommunityOwnedDate = Timestamp.FromDateTime(postDetailsDto.CommunityOwnedDate ?? DateTime.MinValue),
                CreationDate = Timestamp.FromDateTime(postDetailsDto.CreationDate),
                CommentCount = postDetailsDto.CommentCount,
                FavoriteCount = postDetailsDto.FavoriteCount,
                IsClosed = postDetailsDto.IsClosed,
                LastActivityDate = Timestamp.FromDateTime(postDetailsDto.LastActivityDate),
                LastEditDate = Timestamp.FromDateTime(postDetailsDto.LastEditDate ?? DateTime.MinValue),
                LastEditorDisplayName = postDetailsDto.LastEditorDisplayName,
                Score = postDetailsDto.Score,
                Tags = postDetailsDto.Tags,
                Title = postDetailsDto.Title,
                ViewCount = postDetailsDto.ViewCount
            };
            result.Comments.AddRange(postDetailsDto.Comments?.Select(x => new CommentDto
            {
                Score = x.Score ?? 0,
                Text = x.Text,
                UserName = x.UserName,
                CreationDate = Timestamp.FromDateTime(x.CreationDate)
            }));
            return result;
        }
    }
}
