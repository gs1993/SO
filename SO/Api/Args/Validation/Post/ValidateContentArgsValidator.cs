using Api.Args.Post;
using FluentValidation;

namespace Api.Args.Validation.Post
{
    public class ValidateContentArgsValidator : AbstractValidator<ValidateContentArgs>
    {
        public ValidateContentArgsValidator()
        {
            RuleFor(x => x).NotNull();

            RuleFor(x => x.Body)
                .NotNull()
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.Body));
        }
    }
    
}
