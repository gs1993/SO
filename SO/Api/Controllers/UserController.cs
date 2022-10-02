using Api.Args.User;
using Api.Utils;
using FluentValidation;
using Logic.BoundedContexts.Users.Command;
using Logic.BoundedContexts.Users.Dto;
using Logic.BoundedContexts.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, IValidatorFactory validatorFactory) : base(mediator, validatorFactory)
        {
        }


        [HttpGet]
        [Route("GetLast")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(IReadOnlyList<LastUserDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> GetLast([FromQuery] GetLastArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<GetLastArgs>().Validate(args); ;
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var lastUsersDto = await _mediator.Send(new GetLastUsersQuery
            ( 
                size: args.Size 
            ));
            return FromCustomResult(lastUsersDto);
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(UserDetailsDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var userDetailsDto = await _mediator.Send(new GetUserQuery(id));
            return FromCustomResult(userDetailsDto);
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Create([FromBody] CreateUserArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<CreateUserArgs>().Validate(args);
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var result = await _mediator.Send(new CreateUserCommand
            (
                displayName: args.DisplayName,
                aboutMe: args.AboutMe,
                age: args.Age,
                location: args.Location,
                websiteUrl: args.WebsiteUrl
            ));
            return FromResult(result, successStatusCode: 201);
        }

        [HttpPut]
        [Route("PermaBan/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> PermaBan([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var result = await _mediator.Send(new BanUserCommand(id));
            return FromResult(result);
        }
    }
}
