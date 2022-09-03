using CSharpFunctionalExtensions;
using Logic.BoundedContexts.Users.Entities;
using Logic.Utils;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Command
{
    public record CreateUserCommand : IRequest<Result>
    {
        public string DisplayName { get; init; }
        public string? AboutMe { get; init; }
        public int? Age { get; init; }
        public string? Location { get; init; }
        public string? WebsiteUrl { get; init; }

        public CreateUserCommand(string displayName, string? aboutMe, int? age, string? location, string? websiteUrl)
        {
            DisplayName = displayName;
            AboutMe = aboutMe;
            Age = age;
            Location = location;
            WebsiteUrl = websiteUrl;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateUserCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = User.Create(
                displayName: request.DisplayName,
                creationDate: _dateTimeProvider.Now,
                aboutMe: request.AboutMe,
                age: request.Age,
                location: request.Location,
                websiteUrl: request.WebsiteUrl);

            if (result.IsFailure)
                return result.Error;

            _databaseContext.Users.Attach(result.Value);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
