using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Business {
    public class PredictionOfCurrencyDataTable {
        [JsonProperty]
        public Dictionary<string, int> Names { get; private set; }
        [JsonProperty]
        public List<PredictionOfCurrencyData> Data { get; private set; }

        public PredictionOfCurrencyDataTable() { }

        public PredictionOfCurrencyDataTable(string[] names) {
            Data = new List<PredictionOfCurrencyData>();
            Names = ConvertNamesArrayToDictionary(names);
        }

        public PredictionOfCurrencyDataTable(string[] names, List<PredictionOfCurrencyData> data) {
            Data = data;
            Names = ConvertNamesArrayToDictionary(names);
        }

        public PredictionOfCurrencyData this[int index] {
            get {
                return Data[index];
            }
            set {
                Data[index] = value;
            }
        }

        public PredictionOfCurrencyData this[int index, string name] {
            get {
                return new PredictionOfCurrencyData { Date = Data[index].Date, Vector = new[] { Data[index].Vector[Names[name]] } };
            }
        }

        public double[] this[string name] {
            get {
                return Data.Select(x => x.Vector[Names[name]]).ToArray();
            }
        }

        public void Add(PredictionOfCurrencyData item) => Data.Add(item);
        public void Remove(PredictionOfCurrencyData item) => Data.Remove(item);

        protected Dictionary<string, int> ConvertNamesArrayToDictionary(string[] names) {
            var result = new Dictionary<string, int>();
            for (var i = 0; i < names.Length; i++) {
                result.Add(names[i], i);
            }
            return result;
        }
    }
}
