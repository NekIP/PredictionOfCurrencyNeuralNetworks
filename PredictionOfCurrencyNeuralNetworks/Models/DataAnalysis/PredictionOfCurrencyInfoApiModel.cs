using System.Collections.Generic;

namespace PredictionOfCurrencyNeuralNetworks.Models.DataAnalysis {
    public class PredictionOfCurrencyInfoApiModel {
        public double MeanErrorForLearnSet { get; set; }
        public double MeanErrorForTestSet { get; set; }
        public double EfficiencyForLearnSet { get; set; }
        public double EfficiencyTailForLearnSet { get; set; }
        public double EfficiencyForTestSet { get; set; }
        public string NameSystem { get; set; }
        public string[] DescriptionSystem { get; set; }
        public List<DataForNeuralNetworkApiModel> InputData { get; set; }
        public List<PredictionOfCurrencyLearnResultApiModel> LearnResult { get; set; }
        public List<PredictionOfCurrencyLearnResultApiModel> TestResult { get; set; }
    }
}
