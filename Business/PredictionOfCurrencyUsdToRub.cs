using DataAssistants.Structs;
using DataBase.Entities;
using DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business {
    public interface IPredictionOfCurrencyUsdToRub { }
    public class PredictionOfCurrencyUsdToRub : IPredictionOfCurrencyUsdToRub {
        public IDataForNeuralNetworkCollector DataForNeuralNetworkCollector { get; set; }
        public PredictionOfCurrencyUsdToRub(IDataForNeuralNetworkCollector dataForNeuralNetworkCollector) {
            DataForNeuralNetworkCollector = dataForNeuralNetworkCollector;
        }
    }
}
