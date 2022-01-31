using Api.Utils;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }


        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected IActionResult FromResult<T>(T result, string errorMessage = "Not Found")
        {
            return result == null
                ? Error(errorMessage)
                : Ok(result);
        }

        protected IActionResult FromResult<T>(IEnumerable<T> result, string errorMessage = "Not Found")
        {
            return result == null || result.Any()
                ? Error(errorMessage)
                : Ok(result);
        }

        protected IActionResult FromResult(Result result)
        {
            return result.IsFailure
                ? Error(result.Error)
                : Ok(result);
        }
    }
}
