using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.FeatureManagement;
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
            if (!await _featureManager.IsEnabledAsync(FeatureFlagSettings.BasicFlag).ConfigureAwait(false))
                return Result.Failure<int>("Disabled!");

            return Result.Success(1);
        }
    }

    public record PercentageFeatureFlagQuery : IRequest<Result<int>>
    {
    }

    public record PercentageFeatureFlagQueryHandler : IRequestHandler<PercentageFeatureFlagQuery, Result<int>>
    {
        private readonly IFeatureManager _featureManager;

        public PercentageFeatureFlagQueryHandler(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public async Task<Result<int>> Handle(PercentageFeatureFlagQuery request, CancellationToken cancellationToken)
        {
            if (!await _featureManager.IsEnabledAsync(FeatureFlagSettings.PercentageFlag).ConfigureAwait(false))
                return Result.Failure<int>("Disabled!");

            return Result.Success(1);
        }
    }

    public record RolloutPercentageTargetingFlagQuery : IRequest<Result<int>>
    {
    }

    public record RolloutPercentageTargetingFlagHandler : IRequestHandler<RolloutPercentageTargetingFlagQuery, Result<int>>
    {
        private readonly IFeatureManager _featureManager;

        public RolloutPercentageTargetingFlagHandler(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public async Task<Result<int>> Handle(RolloutPercentageTargetingFlagQuery request, CancellationToken cancellationToken)
        {
            if (!await _featureManager.IsEnabledAsync(FeatureFlagSettings.RolloutPercentageTargetingFlag).ConfigureAwait(false))
                return Result.Failure<int>("Disabled!");

            return Result.Success(1);
        }
    }
}
