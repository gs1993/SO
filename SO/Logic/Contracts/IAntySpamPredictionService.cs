namespace Logic.Contracts
{
    public interface IAntySpamPredictionService
    {
        IsSpamPredictionEnum Predict(string postBody);
    }

    public enum IsSpamPredictionEnum
    {
        ProbablyNotSpam = 4,
        Inconclusive = 3,
        ProbablySpam = 2,
        Spam = 1
    }
}
