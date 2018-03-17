using DataManager;

namespace Business {
    public interface IPredictionOfCurrencyUsdToRub { }
    public class PredictionOfCurrencyUsdToRub : IPredictionOfCurrencyUsdToRub {
        public IDataForNeuralNetworkCollector DataForNeuralNetworkCollector { get; set; }
        public PredictionOfCurrencyUsdToRub(IDataForNeuralNetworkCollector dataForNeuralNetworkCollector) {
            DataForNeuralNetworkCollector = dataForNeuralNetworkCollector;
        }
    }
}
