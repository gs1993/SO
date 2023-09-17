using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Read.FeatureFlagsDemo
{
    public record BasicFeatureFlagQuery : IRequest<Result<int>>
    {
    }

    public record BasicFeatureFlagQueryHandler : IRequestHandler<BasicFeatureFlagQuery, Result<int>>
    {
        private readonly IFeatureManager _featureManager;

        public BasicFeatureFlagQueryHandler(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public async Task<Result<int>> Handle(BasicFeatureFlagQuery request, CancellationToken cancellationToken)
        {
            if (!await _featureManager.IsEnabledAsync(FeatureFlagSettings.BasicFeature).ConfigureAwait(false))
                return Result.Failure<int>("Disabled!");

            return Result.Success(1);
        }
    }
}
