using Dawn;
using Logic.Users.Command;
using Logic.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/users")]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet]
        [Route("GetLast")]
        public async Task<IActionResult> GetLastCreated(int size = 10)
        {
            Guard.Argument(size, nameof(size)).Positive();

            var lastUsersDto = await _mediator.Send(new GetLastUsersQuery { Size = size });
            return lastUsersDto.Any()
                ? Ok(lastUsersDto)
                : Error("Not Found");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var userDetailsDto = await _mediator.Send(new GetUserQuery { Id = id });
            return FromResult(userDetailsDto, "User not found");
        }

        [HttpDelete]
        [Route("PermaBan/{id}")]
        public async Task<IActionResult> PermaBan(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var result = await _mediator.Send(new BanUserCommand { Id = id });
            return FromResult(result);
        }
    }
}
