using Api.Args.Post;
using Api.Utils;
using FluentValidation;
using Logic.Queries.Posts.Dtos;
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
    public class HttpClientReliabilityExampleController : BaseController
    {
        public HttpClientReliabilityExampleController(IMediator mediator, IValidatorFactory validationFactory) : base(mediator, validationFactory)
        {
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> Get([FromQuery] string query)
        {
            


            return FromCustomResult(query);
        }
    }
}
