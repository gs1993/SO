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
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess<IReadOnlyList<LastUserDto>>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> GetLast([FromQuery] GetLastArgs args)
        {
            var validationResult = _validatorFactory.GetValidator<GetLastArgs>().Validate(args); ;
            if (!validationResult.IsValid)
                return ValidationError(validationResult);

            var lastUsersDto = await _mediator.Send(new GetLastUsersQuery { Size = args.Size });
            return lastUsersDto.Any()
                ? Ok(lastUsersDto)
                : Error("Not Found");
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess<UserDetailsDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var userDetailsDto = await _mediator.Send(new GetUserQuery { Id = id });
            return FromResult(userDetailsDto);
        }

        [HttpPost]
        [Route("PermaBan/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EnvelopeSuccess))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> PermaBan([FromRoute] int id)
        {
            if (id < 1)
                return ValidationIdError();

            var result = await _mediator.Send(new BanUserCommand { Id = id });
            return FromResult(result);
        }
    }
}
