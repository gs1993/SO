using Logic.Contracts;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;
using OpenAiBotResponder.GeneratorService;

namespace OpenAiBotResponder
{
    public static class Extensions
    {
        public static void RegisterOpenAiBotResponder(this IServiceCollection services, OpenAiBotResponderSettings settings)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.ApiKey))
                throw new ArgumentNullException(nameof(settings.ApiKey));
            if (string.IsNullOrWhiteSpace(settings.Model))
                throw new ArgumentNullException(nameof(settings.Model));

            services.AddSingleton(new OpenAIAPI(new APIAuthentication(settings.ApiKey)));
            services.AddSingleton<IPostAnswerGenerator, PostAnswerGenerator>();
        }

    }
}
