using Api.Args.Post;
using ApiGraphQl.GraphQlSchema.Errors;
using FluentValidation;
using Logic.BoundedContexts.Posts.Commands;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ApiGraphQl.GraphQlSchema
{
    public partial class Mutation
    {
        public async Task<MutationResult<int, ValidationError, BusinessLogicError>> UpVote(int postId, int userId,
            IMediator mediator, IValidatorFactory validatorFactory)
        {
            if (postId < 0)
                return new ValidationError("Invalid post id");

            var validationResult = validatorFactory.GetValidator<UpVoteArgs>().Validate(new UpVoteArgs
            {
                UserId = userId
            });
            if (!validationResult.IsValid)
                return new ValidationError(validationResult);

            var result = await mediator.Send(new UpVoteCommand
            (
                postId: postId,
                userId: userId
            ));
            if (result.IsFailure)
                return new BusinessLogicError(result.Error);

            return 1;
        }
    }
}
