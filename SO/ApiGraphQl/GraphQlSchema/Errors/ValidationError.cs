namespace ApiGraphQl.GraphQlSchema.Errors
{
    public record ValidationError
    {
        public string Message { get; }

        public ValidationError(FluentValidation.Results.ValidationResult validationResult)
        {
            if (validationResult is null)
                throw new ArgumentNullException(nameof(validationResult));
            if (validationResult.IsValid)
                throw new ArgumentException("Invalid validationResult state", nameof(validationResult));

            var errors = validationResult.Errors?
                .Select(x => x.ErrorMessage)?
                .ToArray() ?? Array.Empty<string>();

            Message = string.Join(" ", errors);
        }

        public ValidationError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            Message = message;
        }
    }
}
