using DataAssistants.Structs;
using DataBase.Entities;
using DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business {
    public interface IPredictionOfCurrencyUsdToRub {
        Task<IList<DataForNeuralNetwork>> List();
        Task<IList<DataForNeuralNetwork>> ListNormalized();
        Task<IList<DataForNeuralNetwork>> ListScaled();
    }
    public class PredictionOfCurrencyUsdToRub : IPredictionOfCurrencyUsdToRub {
        public IDataForNeuralNetworkCollector DataForNeuralNetworkCollector { get; set; }
        public PredictionOfCurrencyUsdToRub(IDataForNeuralNetworkCollector dataForNeuralNetworkCollector) {
            DataForNeuralNetworkCollector = dataForNeuralNetworkCollector;
        }

        public async Task<IList<DataForNeuralNetwork>> List() => (await DataForNeuralNetworkCollector.List())
            .Select(x => x as DataForNeuralNetwork).ToList();

        public async Task<IList<DataForNeuralNetwork>> ListNormalized() {
            Vector expectedValues = new double[] {
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D1),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D2),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D3),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D4),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D5),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D6),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D7),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D8),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D9),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D10),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D11),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D12),
                DataForNeuralNetworkCollector.GetExpectedValue(x => x.D13)
            };
            Vector dispersions = new double[]  {
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[0], x => x.D1),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[1], x => x.D2),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[2], x => x.D3),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[3], x => x.D4),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[4], x => x.D5),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[5], x => x.D6),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[6], x => x.D7),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[7], x => x.D8),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[8], x => x.D9),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[9], x => x.D10),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[10], x => x.D11),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[11], x => x.D12),
                DataForNeuralNetworkCollector.GetDispersion(expectedValues[12], x => x.D13)
            };
            return (await DataForNeuralNetworkCollector.List())
                .Select(x => new DataForNeuralNetwork {
                    Date = (x as DataForNeuralNetwork).Date,
                    Data = Normalize((x as DataForNeuralNetwork).Data, expectedValues, dispersions)
                }).ToList();
        }

        public async Task<IList<DataForNeuralNetwork>> ListScaled() {
            var normalized = await ListNormalized();
            Vector mins = new double[] {
                normalized.Min(x => x.D1),
                normalized.Min(x => x.D2),
                normalized.Min(x => x.D3),
                normalized.Min(x => x.D4),
                normalized.Min(x => x.D5),
                normalized.Min(x => x.D6),
                normalized.Min(x => x.D7),
                normalized.Min(x => x.D8),
                normalized.Min(x => x.D9),
                normalized.Min(x => x.D10),
                normalized.Min(x => x.D11),
                normalized.Min(x => x.D12),
                normalized.Min(x => x.D13)
            };
            Vector maxs = new double[] {
                normalized.Max(x => x.D1),
                normalized.Max(x => x.D2),
                normalized.Max(x => x.D3),
                normalized.Max(x => x.D4),
                normalized.Max(x => x.D5),
                normalized.Max(x => x.D6),
                normalized.Max(x => x.D7),
                normalized.Max(x => x.D8),
                normalized.Max(x => x.D9),
                normalized.Max(x => x.D10),
                normalized.Max(x => x.D11),
                normalized.Max(x => x.D12),
                normalized.Max(x => x.D13)
            };
            return normalized
                .Select(x => new DataForNeuralNetwork {
                    Date = (x as DataForNeuralNetwork).Date,
                    Data = Scaling((x as DataForNeuralNetwork).Data, mins, maxs)
                }).ToList();
        }

        protected Vector Normalize(Vector x, Vector expectedValue, Vector dispersion) => (x - expectedValue) / Vector.Convert(dispersion, Math.Sqrt);
        protected Vector Denormalize(Vector y, Vector expectedValue, Vector dispersion) => y * Vector.Convert(dispersion, Math.Sqrt) + expectedValue;
        protected Vector Scaling(Vector x, Vector min, Vector max) => (x - min) * (1 - (-1)) / (max - min) + (-1);
        protected Vector Descaling(Vector y, Vector min, Vector max) => ((max - min) * (y - (-1))) / (1 - (-1)) + min;
    }
}
