namespace Logic.Contracts
{
    public interface IAntySpamPredictionService
    {
        PrecictionResponse Predict(string postBody);
    }

    public sealed record PrecictionResponse(IsSpamPredictionEnum Prediction, float ConfidenceLevel);

    public enum IsSpamPredictionEnum
    {
        ProbablyNotSpam = 4,
        Inconclusive = 3,
        ProbablySpam = 2,
        Spam = 1
    }
}
