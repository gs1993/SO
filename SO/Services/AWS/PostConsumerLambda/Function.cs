using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PostConsumerLambda;

public class Function
{
    private readonly ILambdaEntryPoint _lambdaEntryPoint;

    public Function()
    {
        var startup = new Startup();
        IServiceProvider provider = startup.ConfigureServices();

        _lambdaEntryPoint = provider.GetRequiredService<ILambdaEntryPoint>();
    }

    public Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        return _lambdaEntryPoint.Handler(evnt, context.Logger);
    }
}