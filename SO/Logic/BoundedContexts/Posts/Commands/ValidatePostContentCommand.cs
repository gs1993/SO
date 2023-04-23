using CSharpFunctionalExtensions;
using Dawn;
using Logic.Contracts;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record ValidatePostContentCommand : IRequest<Result<IsSpamPredictionEnum>>
    {
        public string Body { get; init; }

        public ValidatePostContentCommand(string body)
        {
            Body = body;
        }
    }

    public class ValidatePostContentCommandHandler : IRequestHandler<ValidatePostContentCommand, Result<IsSpamPredictionEnum>>
    {
        public readonly IAntySpamPredictionService _postPredictionService;

        public ValidatePostContentCommandHandler(IAntySpamPredictionService postPredictionService)
        {
            _postPredictionService = postPredictionService;
        }

        public async Task<Result<IsSpamPredictionEnum>> Handle(ValidatePostContentCommand request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Body).NotEmpty().NotWhiteSpace();

            var spamValidationResult = _postPredictionService.Predict(request.Body);
            return Result.Success(spamValidationResult);
        }
    }
}
