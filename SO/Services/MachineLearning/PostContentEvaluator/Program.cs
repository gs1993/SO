using Microsoft.Extensions.Configuration;
using ModelBuilder.Config;
using ModelBuilder.EvaluationEngines;

namespace ModelBuilder
{
    internal class Program
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var settings = new PostEvaluatorConfiguration();
            configuration.GetSection("PostEvaluatorConfiguration").Bind(settings);

            string saveModelPath = settings.SaveModelPath;
            if (!Directory.Exists(saveModelPath))
                Directory.CreateDirectory(saveModelPath);

            try
            {
                PostScoreEvaluationEngine.Evaluate(settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }
    }
}