using Business;
using System;

namespace PredictionOfCurrencyNeuralNetworks.Models.DataAnalysis {
    public class PredictionOfCurrencyLearnResultApiModel {
        public DateTime Date { get; set; }
        public double[] Input { get; set; }
        public double[] Output { get; set; }
        public double[] Error { get; set; }
        public double[] Ideal { get; set; }

        public static PredictionOfCurrencyLearnResultApiModel Map(PredictionOfCurrencyLearnResult predictionOfCurrencyLearnResult) =>
            new PredictionOfCurrencyLearnResultApiModel {
                Date = predictionOfCurrencyLearnResult.Date,
                Input = predictionOfCurrencyLearnResult.Input,
                Output = predictionOfCurrencyLearnResult.Output,
                Error = predictionOfCurrencyLearnResult.Error,
                Ideal = predictionOfCurrencyLearnResult.Ideal
            };
    }
}
