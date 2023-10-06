using Api.Utils;
using FluentValidation;
using Logic.Read.FeatureFlagsDemo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
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

        [HttpGet("Percentage")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(int))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> PercentageGet()
        {
            var basicFeatureFlagResult = await _mediator.Send(new PercentageFeatureFlagQuery());
            return FromResult(basicFeatureFlagResult);
        }

        [HttpGet("RolloutPercentageTargeting")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(int))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(EnvelopeError))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(EnvelopeError))]
        public async Task<IActionResult> RolloutPercentageTargetingGet()
        {
            var basicFeatureFlagResult = await _mediator.Send(new RolloutPercentageTargetingFlagQuery());
            return FromResult(basicFeatureFlagResult);
        }
    }
}
