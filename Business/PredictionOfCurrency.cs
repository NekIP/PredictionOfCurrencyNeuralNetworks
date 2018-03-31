using DataAssistants;
using DataAssistants.Structs;
using DataManager;
using NeuralNetwork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business {
    public interface IPredictionOfCurrencyUsdToRub {
        /*DataParameter DataParameters { get; set; }
        string BackupName { get; }
        string NeuralNetworkName { get; set; }
        Lstm Lstm { get; }
        List<DataForNeuralNetwork> Data { get; set; }
        double Predict(DateTime date);
        List<PredictionOfCurrencyLearnResult> Fit();
        Vector ExpectedValues();
        Vector Dispersions(Vector expectedValues = null);
        Vector Maxs();
        Vector Mins();*/
    }
    public class PredictionOfCurrency : IPredictionOfCurrencyUsdToRub {
        public Lstm Lstm { get; private set; }
        public PredictionOfCurrencyDataManager DataManager { get; private set; }
        public List<IDataCollector> Collectors { get; set; }

        public DataParameter DataParameters { get; private set; } =
            new DataParameter(new DateTime(2007, 10, 14), new DateTime(2018, 3, 23), TimeSpan.FromDays(1));
        public DataValueType DataValueType { get; private set; } =
            DataValueType.Absolute;
        public DataProcessingMethods DataProcessingMethods { get; private set; } =
            DataProcessingMethods.Normalize;
        public string NeuralNetworkName { get; private set; } =
            "DefaultLstmNetworkForDay";
        public LearnParameters LearnParameters { get; set; } =
            new LearnParameters { RecurentParameters = new RecurentParameters(1.2, 0.5, 0.1) };
        public string NameOfCollectorForPredict { get; set; } = 
            "UsdToRubCurrencyCollector";

        public PredictionOfCurrency(List<IDataCollector> collectors, 
            string nameOfCollectorForPredict = null,
            string neuralNetworkName = null,
            DataParameter dataParameters = null, 
            DataProcessingMethods? dataProcessingMethods = null,
            DataValueType? dataType = null,
            LearnParameters learnParameters = null) {
            Collectors = collectors;
            DataParameters = dataParameters ?? DataParameters;
            DataProcessingMethods = dataProcessingMethods ?? DataProcessingMethods;
            DataValueType = dataType ?? DataValueType;
            NeuralNetworkName = neuralNetworkName ?? NeuralNetworkName;
            NameOfCollectorForPredict = nameOfCollectorForPredict ?? NameOfCollectorForPredict;
            if (learnParameters is null) {
                var inputLength = collectors.Count;
                LearnParameters.RecurentCellParameters = new RecurentCellParameters[] {
                    new RecurentCellParameters(inputLength, inputLength),
                    new RecurentCellParameters(inputLength, 1)
                };
            }
            else {
                LearnParameters = learnParameters;
            }
            DataManager = new PredictionOfCurrencyDataManager(DataParameters, DataProcessingMethods, DataValueType, NameOfCollectorForPredict, collectors);
        }

        /*public double Predict(DateTime date) {
            InitData();
            InitNeuralNetwork(Data.First().Count);
            var currentData = DataForNeuralNetworkCollector.Get(date, TimeSpan.FromDays(1));
            currentData.Data = DataForNeuralNetworkCollector.Normalize(currentData.Data, ExpectedValuesLocal, DispersionsLocal);
            var result = Lstm.Run(new Vector[] { currentData.Data });
            return DataForNeuralNetworkCollector
                .Denormalize(result.Last(), 
                    new double[] { ExpectedValuesLocal.Values.Last() }, 
                    new double[] { DispersionsLocal.Values.Last() })
                .Values.First();
        }*/

        public List<PredictionOfCurrencyLearnResult> Learn(int countIteration, bool outputInConsole = false, bool saveLearnResult = false) {
            var result = new List<PredictionOfCurrencyLearnResult>();
            var lastAverage = 0.0;
            for (var i = 0; i < countIteration; i++) {
                var learnResult = Fit();
                if (outputInConsole) {
                    var currAverage = learnResult.Average(x => x.Error[0]);
                    Console.WriteLine(currAverage + "\t" + (currAverage - lastAverage));
                    lastAverage = currAverage;
                }
                result.AddRange(learnResult);
            }
            return result;
        }


        public List<PredictionOfCurrencyLearnResult> Fit(bool saveLearnResult = false) {
            InitData();
            InitNeuralNetwork();
            var result = new List<PredictionOfCurrencyLearnResult>();
            for (var i = 0; i < DataManager.DataTable.Data.Count - 1; i++) {
                var input = new Vector[] { DataManager.DataTable[i].Vector };
                var ideal = new Vector[] { new double[] { DataManager.DataTable[i + 1].Vector.Values.Last() } };
                var (output, error) = Lstm.Learn(input, ideal);
                result.Add(new PredictionOfCurrencyLearnResult {
                    Date = DataManager.DataTable.Data[i].Date,
                    Error = error.Last(),
                    Output = DataManager.ConvertOutput(output.Last()),
                    Ideal = DataManager.ConvertOutput(ideal.Last()),
                    Input = DataManager.ConvertInput(input.Last()),
                });
            }
            Lstm.Save(NeuralNetworkName);
            if (saveLearnResult) {
                SaveLearnProgress(result);
            }
            return result;
        }

        protected void InitData() {
            if (DataManager.DataTable == null) {
                DataManager.InitializeData();
            }
        }

        protected void InitNeuralNetwork() {
            if (Lstm == null) {
                Lstm = new Lstm(Collectors.Count, 1, LearnParameters.RecurentParameters, LearnParameters.RecurentCellParameters);
                Lstm.Load(NeuralNetworkName);
            }
        }

        protected void SaveLearnProgress(List<PredictionOfCurrencyLearnResult> learnResult) {
            var serializer = new Serializer();
            var path = "learnResult" + NeuralNetworkName + ".json";
            var current = new List<PredictionOfCurrencyLearnResult>();
            if (serializer.Exists(path)) {
                current = serializer.Deserialize<List<PredictionOfCurrencyLearnResult>>(path);
            }
            current.AddRange(learnResult);
            serializer.Serialize(current, path);
        }
    }

    public class PredictionOfCurrencyLearnResult {
        [JsonProperty]
        public DateTime Date { get; set; }
        [JsonProperty]
        public Vector Input { get; set; }
        [JsonProperty]
        public Vector Output { get; set; }
        [JsonProperty]
        public Vector Error { get; set; }
        [JsonProperty]
        public Vector Ideal { get; set; }
    }

    public class DataParameter {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public TimeSpan Step { get; set; }
        public DataParameter(DateTime from, DateTime to, TimeSpan step) {
            From = from;
            To = to;
            Step = step;
        }
    }

    public class LearnParameters {
        public RecurentParameters RecurentParameters { get; set; }
        public RecurentCellParameters[] RecurentCellParameters { get; set; }
    }

    public enum DataValueType {
        Absolute,
        RelativePercentage,
        Relative
    }

    public enum DataProcessingMethods {
        None,
        Normalize,
        Scale,
        NormalizeAndScale
    }
}
