namespace PredictionEngineApi.Dtos
{
    public class ValidationFailureResponse
    {
        public List<string> Errors { get; init; } = new();
    }
}
