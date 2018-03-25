using DataAssistants;
using DataAssistants.Structs;
using DataBase;
using DataBase.Entities;
using DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
    public interface IDataForNeuralNetworkCollector {
        List<DataForNeuralNetwork> List(DateTime from, DateTime to, TimeSpan step);
        List<DataForNeuralNetwork> GetSet(DateTime from, DateTime to, TimeSpan step);
        (List<DataForNeuralNetwork> result, Vector expectedValues, Vector dispersions) NormalizeSet(List<DataForNeuralNetwork> set, string backupName = null);
        Vector ExpectedValues(DateTime from, DateTime to, TimeSpan step, string backupName = null);
        Vector Dispersions(DateTime from, DateTime to, TimeSpan step, Vector expectedValues = null, string backupName = null);
        Vector Maxs(DateTime from, DateTime to, TimeSpan step, string backupName = null);
        Vector Mins(DateTime from, DateTime to, TimeSpan step, string backupName = null);
        Vector Normalize(Vector x, Vector expectedValue, Vector dispersion);
        Vector Denormalize(Vector y, Vector expectedValue, Vector dispersion);
        Vector Scaling(Vector x, Vector min, Vector max);
        Vector Descaling(Vector y, Vector min, Vector max);
        Task DownloadMissingData(DateTime before, TimeSpan step);
        DataForNeuralNetwork Get(DateTime date, TimeSpan step);
        string[] GetNames();
    }
    public class DataForNeuralNetworkCollector : IDataForNeuralNetworkCollector {
        protected List<IDataCollector> Collectors { get; set; }

        public DataForNeuralNetworkCollector(ICAC40Collector cac40,
            IDowJonesCollector dowJones,
            IGdpPerCapitaPppCollector gdpPerCapitaPpp,
            IGoldCollector gold,
            IInflationCollector inflation,
            IMMVBCollector mmvb,
            IOliBrentCollector oliBrent,
            IOliLightCollector oliLight,
            IRefinancingRateCollector refinancingRate,
            IRTSCollector rts,
            ISAndP500Collector sAndP,
            ITradeBalanceCollector tradeBalance,
            IUsdToRubCurrencyCollector usdToRub) {
            Collectors = new List<IDataCollector> {
                //cac40,
                dowJones, 
                //gdpPerCapitaPpp,
                gold,
                //inflation,
                mmvb,
                oliBrent,
                oliLight,
                //refinancingRate,
                //rts,
                //sAndP,
                //tradeBalance,
                usdToRub
            };
        }

        public List<DataForNeuralNetwork> List(DateTime from, DateTime to, TimeSpan step) {
            var result = new List<DataForNeuralNetwork>();
            for (var i = from; i < to; i = i.Add(step)) {
                var items = new List<double>();
                foreach (var collector in Collectors) {
                    if (collector.TryGet(i, step, out var data)) {
                        items.Add(data.Selector());
                    }
                    else {
                        break;
                    }
                }
                if (items.Count == Collectors.Count) {
                    result.Add(new DataForNeuralNetwork(items.Count) { Date = i, Data = items.ToArray() });
                }
            }
            return result;
        }

        public DataForNeuralNetwork Get(DateTime date, TimeSpan step) {
            var items = new List<double>();
            foreach (var collector in Collectors) {
                if (collector.TryGet(date, step, out var data)) {
                    items.Add(data.Selector());
                }
                else {
                    break;
                }
            }
            return new DataForNeuralNetwork(items.Count) { Date = date, Data = items.ToArray() };
        }

        public List<DataForNeuralNetwork> GetSet(DateTime from, DateTime to, TimeSpan step) {
            var serializer = new Serializer();
            var result = new List<DataForNeuralNetwork>();
            var backupFileName = $"DataForNeuralNetworkBackup{ from.ToString("ddMMyyyyTHHmmss") }_{ to.ToString("ddMMyyyyTHHmmss") }_{ "D" + step.Days + "H" + step.Hours }.json";
            if (!serializer.Exists(backupFileName)) {
                for (var i = from; i < to; i = i.Add(step)) {
                    var items = new List<double>();
                    foreach (var collector in Collectors) {
                        if (collector.TryGet(i, step, out var data)) {
                            items.Add(data.Selector());
                        }
                        else {
                            break;
                        }
                    }
                    if (items.Count == Collectors.Count) {
                        result.Add(new DataForNeuralNetwork(items.Count) { Date = i, Data = items.ToArray() });
                    }
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<List<DataForNeuralNetwork>>(backupFileName);
            }
            return result;
        }

        public (List<DataForNeuralNetwork> result, Vector expectedValues, Vector dispersions) NormalizeSet(List<DataForNeuralNetwork> set, string backupName = null) {
            var serializer = new Serializer();
            var backupExpectedValuesFileName = $"BackupExpectedValues{ backupName }.json";
            var backupDispersionsFileName = $"BackupDispersions{ backupName }.json";
            var expectedValues = new Vector(set.First().Count);
            var dispersions = new Vector(set.First().Count);
            if (backupName == null || !serializer.Exists(backupExpectedValuesFileName)) {
                for (var i = 0; i < expectedValues.Length; i++) {
                    expectedValues[i] = set.Average(x => x.Data[i]);
                }
                serializer.Serialize(expectedValues, backupExpectedValuesFileName);
            }
            else {
                expectedValues = serializer.Deserialize<Vector>(backupExpectedValuesFileName);
            }
            if (backupName == null || !serializer.Exists(backupDispersionsFileName)) {
                for (var i = 0; i < dispersions.Length; i++) {
                    dispersions[i] = set.Aggregate(0.0, (x, y) => x + Math.Pow(y.Data[i] - expectedValues[i], 2));
                }
                serializer.Serialize(dispersions, backupDispersionsFileName);
            }
            else {
                dispersions = serializer.Deserialize<Vector>(backupDispersionsFileName);
            }
            return (set.Select(x => new DataForNeuralNetwork(x.Count) {
                Date = x.Date,
                Data = Normalize(x.Data, expectedValues, dispersions)
            }).ToList(), expectedValues, dispersions);
        }

        public string[] GetNames() {
            var result = new string[Collectors.Count];
            for (var i = 0; i < result.Length; i++) {
                result[i] = Collectors[i].GetType().Name.Replace("Collector", "");
            }
            return result;
        }

        public Vector ExpectedValues(DateTime from, DateTime to, TimeSpan step, string backupName = null) {
            var serializer = new Serializer();
            var backupFileName = $"BackupExpectedValues{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = Collectors[i].ExpectedValue(from, to, step);
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        public Vector Dispersions(DateTime from, DateTime to, TimeSpan step, Vector expectedValues = null, string backupName = null) {
            if (expectedValues == null) {
                expectedValues = ExpectedValues(from, to, step);
            }
            var serializer = new Serializer();
            var backupFileName = $"BackupDispersions{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = Collectors[i].Dispersion(from, to, step, expectedValues[i]);
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        public Vector Maxs(DateTime from, DateTime to, TimeSpan step, string backupName = null) {
            var serializer = new Serializer();
            var backupFileName = $"BackupMaxs{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = Collectors[i].Max(from, to, step);
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        public Vector Mins(DateTime from, DateTime to, TimeSpan step, string backupName = null) {
            var serializer = new Serializer();
            var backupFileName = $"BackupMins{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = Collectors[i].Min(from, to, step);
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        public async Task DownloadMissingData(DateTime before, TimeSpan step) {
            foreach (var collector in Collectors) {
                await collector.DownloadMissingData(before, step);
            }
        }

        public Vector Normalize(Vector x, Vector expectedValue, Vector dispersion) =>
            (x - expectedValue) / Vector.Convert(dispersion, Math.Sqrt);

        public Vector Denormalize(Vector y, Vector expectedValue, Vector dispersion) =>
            y * Vector.Convert(dispersion, Math.Sqrt) + expectedValue;

        public Vector Scaling(Vector x, Vector min, Vector max) =>
            (x - min) * (1 - (-1)) / (max - min) + (-1);

        public Vector Descaling(Vector y, Vector min, Vector max) =>
            ((max - min) * (y - (-1))) / (1 - (-1)) + min;
    }
}
