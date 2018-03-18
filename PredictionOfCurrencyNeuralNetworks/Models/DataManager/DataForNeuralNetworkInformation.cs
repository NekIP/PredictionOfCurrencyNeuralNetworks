using System;

namespace PredictionOfCurrencyNeuralNetworks.Models.DataManager {
    public class DataForNeuralNetworkInformation {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Count { get; set; }
        public double[] ExpectedValues { get; set; }
        public double[] Dispersions { get; set; }
        public double[] Mins { get; set; }
        public double[] Maxs { get; set; }
        public string[] FieldsNames { get; set; }
    }
}
