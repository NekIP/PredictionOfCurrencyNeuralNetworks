using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PredictionOfCurrencyNeuralNetworks.Models.DataManager {
    public class DataForNeuralNetworkInformation {
        public int Count { get; set; }
        public double[] ExpectedValue { get; set; }
        public double[] Dispersion { get; set; }
    }
}
