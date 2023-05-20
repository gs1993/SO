using Microsoft.ML;
using Microsoft.ML.Data;
using ModelBuilder.Config;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ModelBuilder.EvaluationEngines
{
    internal static class PostScoreEvaluationEngine
    {
        internal static void Evaluate(PostEvaluatorConfiguration configuration)
        {
            var mlContext = new MLContext()
            {
                GpuDeviceId = 0,
                FallbackToCpu = true
            };
            var databaseLoader = mlContext.Data.CreateDatabaseLoader<PostInput>();

            IDataView inputData = CreateSqlDataLoader(configuration, databaseLoader);

            Console.WriteLine($"Training data (MaxAnalyzedItemCount: {configuration.MaxAnalyzedItemCount}, ModelVersion: {configuration.ModelVersion})");
            var stopwatch = Stopwatch.StartNew();

            var dataSplit = mlContext.Data.TrainTestSplit(inputData, testFraction: 0.2);
            var trainData = dataSplit.TrainSet;
            var testData = dataSplit.TestSet;

            var pipeline = mlContext.Transforms.Conversion.ConvertType(
                outputColumnName: "Label",
                inputColumnName: "Label",
                outputKind: DataKind.Single)
            .Append(mlContext.Transforms.Conversion.MapValueToKey(
                outputColumnName: "Label",
                inputColumnName: "Label")
            .Append(mlContext.Transforms.Text.NormalizeText(
                outputColumnName: "NormalizedBody",
                inputColumnName: "Body")
            .Append(mlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features",
                inputColumnName: "NormalizedBody")
            .Append(mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated(
                labelColumnName: "Label",
                featureColumnName: "Features")
            .Append(mlContext.Transforms.Conversion.MapKeyToValue(
                outputColumnName: "PredictedLabel",
                inputColumnName: "PredictedLabel"))))));

            var model = pipeline.Fit(trainData);

            var transformedTest = model.Transform(testData);
            var metrics = mlContext.MulticlassClassification.Evaluate(transformedTest);

            stopwatch.Stop();
            Console.WriteLine($"Training data completed | {stopwatch.Elapsed}");
            Console.WriteLine($"TopKAccuracy: {metrics.TopKAccuracy:0.##}");
            Console.WriteLine($"TopKAccuracyForAllK: {metrics.TopKAccuracyForAllK:0.##}");
            Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy:0.##}");
            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy:0.##}");

            Console.WriteLine("Saving model");
            stopwatch = Stopwatch.StartNew();

            string modelPath = $"{configuration.SaveModelPath}\\trainedModel_{configuration.ModelVersion}.zip";
            mlContext.Model.Save(model, trainData.Schema, modelPath);

            stopwatch.Stop();
            Console.WriteLine($"Saving model completed | {stopwatch.Elapsed}");
        }

        private static IDataView CreateSqlDataLoader(PostEvaluatorConfiguration configuration, DatabaseLoader databaseLoader)
        {
            string query = string.Format(@"
SELECT top {0}
	CASE
        WHEN Score >= 50 THEN 4
        WHEN Score >= 5 THEN 3
        WHEN Score >= 0 THEN 2
        WHEN Score >= -5 THEN 1
        ELSE 0
    END as [Label],
	LEFT(COALESCE([Body], ''), 500) as [Body]
FROM [Posts] WHERE PostTypeId = 2
ORDER BY Id DESC", configuration.MaxAnalyzedItemCount);

            var dbSource = new DatabaseSource(SqlClientFactory.Instance, configuration.ConnectionString, query, configuration.ConnectionTimeoutInSeconds);
            IDataView inputData = databaseLoader.Load(dbSource);
            return inputData;
        }
    }

    public class PostInput
    {
        public string Body { get; init; }
        public int Label { get; init; }
    }
}
