namespace PostContentEvaluator
{
    internal class PostEvaluatorConfiguration
    {
        public string ConnectionString { get; init; }
        public int ConnectionTimeoutInSeconds { get; init; }
        public long MaxAnalyzedItemCount { get; init; }
        public string SaveModelPath { get; init; }
        public string ModelVersion { get; init; }
    }
}
