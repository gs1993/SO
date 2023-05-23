using FluentValidation;
using PredictionEngineApi.Dtos;

namespace PredictionEngineApi.Validation
{
    public class GetChatGptPropositionRequestValidator : AbstractValidator<GetChatGptPropositionRequest>
    {
        public GetChatGptPropositionRequestValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
