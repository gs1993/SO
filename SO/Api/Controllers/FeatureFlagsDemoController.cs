using Api.Args.Post;
using Api.Utils;
using FluentValidation;
using Logic.Queries.Posts.Dtos;
using Logic.Read.FeatureFlagsDemo;
using Logic.Read.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class FeatureFlagsDemoController : BaseController
    {
        public FeatureFlagsDemoController(IMediator mediator, IValidatorFactory validationFactory) : base(mediator, validationFactory)
        {
        }

        [HttpGet("Basic")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(int))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> BasicGet()
        {
            var basicFeatureFlagResult = await _mediator.Send(new BasicFeatureFlagQuery());
            return FromResult(basicFeatureFlagResult);
        }
    }
}
