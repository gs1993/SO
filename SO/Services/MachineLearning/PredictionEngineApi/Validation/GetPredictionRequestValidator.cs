using FluentValidation;
using PredictionEngineApi.Dtos;

namespace PredictionEngineApi.Validation
{
    public class GetPredictionRequestValidator : AbstractValidator<GetPredictionRequest>
    {
        public GetPredictionRequestValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
