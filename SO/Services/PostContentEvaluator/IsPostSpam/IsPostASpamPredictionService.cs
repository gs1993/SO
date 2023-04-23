using Google.Protobuf.WellKnownTypes;
using Logic.Contracts;
using Microsoft.ML;

namespace PostContentEvaluator.IsPostSpam
{
    public class AntySpamPredictionService : IAntySpamPredictionService
    {
        private readonly PredictionEngine<InputData, IsSpamPredictionOutput> _predictionEngine;

        public AntySpamPredictionService(MLContext mlContext, ITransformer model)
        {
            _predictionEngine = mlContext.Model.CreatePredictionEngine<InputData, IsSpamPredictionOutput>(model);
        }

        public IsSpamPredictionEnum Predict(string postBody)
        {
            var inputData = new InputData { Body = postBody[..Math.Min(500, postBody.Length)] };
            var prediction = _predictionEngine.Predict(inputData);

            int postScorePredictionValue = (int)Math.Round(prediction.PredictedLabel);

            return System.Enum.IsDefined(typeof(IsSpamPredictionEnum), postScorePredictionValue)
                ? (IsSpamPredictionEnum)postScorePredictionValue
                : IsSpamPredictionEnum.Inconclusive;
        }


        private class IsSpamPredictionOutput
        {
            public float PredictedLabel { get; init; }
        }

        private class InputData
        {
            public string Body { get; init; }
            public float Label { get; init; }
        }
    }
}
