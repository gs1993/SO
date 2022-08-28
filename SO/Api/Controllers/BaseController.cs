using Api.Utils;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowMyOrigin")]
    public class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly IValidatorFactory _validatorFactory;

        public BaseController(IMediator mediator, IValidatorFactory validationFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _validatorFactory = validationFactory ?? throw new ArgumentNullException(nameof(validationFactory));
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

        protected IActionResult ValidationError(ValidationResult validationResult)
        {
            if(validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));
            if (validationResult.IsValid)
                throw new InvalidOperationException();

            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var errorMessage = string.Join(" ", errors);

            return BadRequest(Envelope.Error(errorMessage));
        }

        protected IActionResult ValidationIdError()
        {
            return BadRequest(Envelope.Error("Invalid id"));
        }

        protected IActionResult FromCustomResult<T>(T result, int successStatusCode = 200, string errorMessage = "Not Found")
        {
            return result == null
                ? Error(errorMessage)
                : StatusCode(successStatusCode, result);
        }

        protected IActionResult FromResult(Result result, int successStatusCode = 200)
        {
            return result.IsFailure
                ? Error(result.Error)
                : StatusCode(successStatusCode);
        }
    }
}
