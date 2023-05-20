namespace PredictionEngineApi.Dtos
{
    public record PredictionResponse(bool IsSpam, float ConfidenceLevel);
}
