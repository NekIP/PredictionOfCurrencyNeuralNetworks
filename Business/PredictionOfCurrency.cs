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
        public MultilayerPerceptron SimpleNeuralNetwork { get; private set; }
        public PredictionOfCurrencyDataManager DataManager { get; private set; }
        public List<IDataCollector> Collectors { get; set; }

        public int Chunk { get; set; } =
            1;
        public UsingNeuralNetwork UsingNeuralNetwork { get; set; } =
            UsingNeuralNetwork.Lstm;
        public DataParameter DataParameters { get; private set; } =
            new DataParameter(new DateTime(2007, 10, 14), new DateTime(2018, 3, 23), TimeSpan.FromDays(1));
        public DataValueType DataValueType { get; private set; } =
            DataValueType.Absolute;
        public DataProcessingMethods DataProcessingMethods { get; private set; } =
            DataProcessingMethods.Normalize;
        public string NeuralNetworkName { get; private set; } =
            "DefaultLstmNetworkForDay";
        public LearnParameters LearnParameters { get; set; } =
            new LearnParameters { Parameters = new RecurentParameters(1.2, 0.5, 0.1) };
        public string NameOfCollectorForPredict { get; set; } = 
            "UsdToRubCurrencyCollector";
        public bool LoadNetwork { get; set; } = 
            true;

        public PredictionOfCurrency(List<IDataCollector> collectors, 
            string nameOfCollectorForPredict = null,
            string neuralNetworkName = null,
            DataParameter dataParameters = null, 
            DataProcessingMethods? dataProcessingMethods = null,
            DataValueType? dataType = null,
            LearnParameters learnParameters = null,
            bool? loadNetwork = null,
            UsingNeuralNetwork? usingNeuralNetwork = null,
            int? chunk = null) {
            Collectors = collectors;
            DataParameters = dataParameters ?? DataParameters;
            DataProcessingMethods = dataProcessingMethods ?? DataProcessingMethods;
            DataValueType = dataType ?? DataValueType;
            NeuralNetworkName = neuralNetworkName ?? NeuralNetworkName;
            NameOfCollectorForPredict = nameOfCollectorForPredict ?? NameOfCollectorForPredict;
            LearnParameters = learnParameters ?? LearnParameters;
            LoadNetwork = loadNetwork ?? LoadNetwork;
            UsingNeuralNetwork = usingNeuralNetwork ?? UsingNeuralNetwork;
            Chunk = chunk ?? Chunk;
            if (LearnParameters.CellParameters is null) {
                var inputLength = collectors.Count;
                LearnParameters.CellParameters = new RecurentCellParameters[] {
                    new RecurentCellParameters(inputLength, inputLength),
                    new RecurentCellParameters(inputLength, 1)
                };
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

		public List<PredictionOfCurrencyLearnResult> Test() {
			InitData();
			InitLstm();
			var result = new List<PredictionOfCurrencyLearnResult>();
			for (var i = 0; i < DataManager.TestData.Data.Count - 1; i += Chunk) {
				var chunk = Chunk;
				if (i + Chunk > DataManager.TestData.Data.Count - 1) {
					chunk = DataManager.TestData.Data.Count - 1 - i;
				}
				var input = new Vector[chunk];
				var ideal = new Vector[chunk];
				for (var j = i; j < i + chunk && j < DataManager.TestData.Data.Count - 1; j++) {
					input[j - i] = DataManager.TestData[j].Vector;
					ideal[j - i] = new double[] { DataManager.TestData[j + 1].Vector.Values.Last() };
				}
				var output = Lstm.Run(input);
				for (var j = 0; j < chunk; j++) {
					var error = ((output[j] - ideal[j]) ^ 2) * 0.5;
					result.Add(new PredictionOfCurrencyLearnResult {
						Date = DataManager.LearnData.Data[i + j].Date,
						Error = error,
						Output = DataManager.ConvertOutput(output[j]),
						Ideal = DataManager.ConvertOutput(ideal[j]),
						Input = DataManager.ConvertInput(input[j]),
					});
				}
			}
			return result;
		}

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
                //result.AddRange(learnResult);
            }
            return result;
        }

        public List<PredictionOfCurrencyLearnResult> Fit(bool saveLearnResult = false) {
            if (UsingNeuralNetwork == UsingNeuralNetwork.Lstm) {
                return FitLstm(saveLearnResult);
            }
            else {
                return FitSimpleNeuralNetwork(saveLearnResult);
            }
        }

        protected List<PredictionOfCurrencyLearnResult> FitLstm(bool saveLearnResult = false) {
            InitData();
            InitLstm();
            var result = new List<PredictionOfCurrencyLearnResult>();
            for (var i = 0; i < DataManager.LearnData.Data.Count - 1; i += Chunk) {
                var chunk = Chunk;
                if (i + Chunk > DataManager.LearnData.Data.Count - 1) {
                    chunk = DataManager.LearnData.Data.Count - 1 - i;
                }
                var input = new Vector[chunk];
                var ideal = new Vector[chunk];
                for (var j = i; j < i + chunk && j < DataManager.LearnData.Data.Count - 1; j++) {
                    input[j - i] = DataManager.LearnData[j].Vector;
                    ideal[j - i] = new double[] { DataManager.LearnData[j + 1].Vector.Values.Last() };
                }
                var (output, error) = Lstm.Learn(input, ideal);
                for (var j = 0; j < chunk; j++) {
                    result.Add(new PredictionOfCurrencyLearnResult {
                        Date = DataManager.LearnData.Data[i + j].Date,
                        Error = error[j],
                        Output = DataManager.ConvertOutput(output[j]),
                        Ideal = DataManager.ConvertOutput(ideal[j]),
                        Input = DataManager.ConvertInput(input[j]),
                    });
                }
            }
            Lstm.Save(NeuralNetworkName);
            if (saveLearnResult) {
                SaveLearnProgress(result);
            }
            return result;
        }

        protected List<PredictionOfCurrencyLearnResult> FitSimpleNeuralNetwork(bool saveLearnResult = false) {
            InitData();
            InitSimpleNeuralNetwork();
            var result = new List<PredictionOfCurrencyLearnResult>();
            for (var i = 0; i < DataManager.LearnData.Data.Count - 1; i++) {
                var input = DataManager.LearnData[i].Vector;
                var ideal = (Vector)new double[] { DataManager.LearnData[i + 1].Vector.Values.Last() };
                var ideal1 = (Vector)new double[] { DataManager.LearnData[i + 1].Vector.Values.Last() };
                var (output, error) = SimpleNeuralNetwork.Learn(input, ideal);
                result.Add(new PredictionOfCurrencyLearnResult {
                    Date = DataManager.LearnData.Data[i].Date,
                    Error = error,
                    Output = DataManager.ConvertOutput(SimpleNeuralNetwork.ConvertOutput(output)),
                    Ideal = DataManager.ConvertOutput(ideal1),
                    Input = DataManager.ConvertInput(input),
                });
            }
            SimpleNeuralNetwork.Save(NeuralNetworkName);
            if (saveLearnResult) {
                SaveLearnProgress(result);
            }
            return result;
        }

        protected void InitData() {
            if (DataManager.LearnData == null) {
                DataManager.InitializeData();
            }
        }

        protected void InitLstm() {
            if (Lstm == null) {
                Lstm = new Lstm(Collectors.Count, 1, LearnParameters.Parameters, LearnParameters.CellParameters);
                if (LoadNetwork) {
                    Lstm.Load(NeuralNetworkName);
                }
            }
        }

        protected void InitSimpleNeuralNetwork() {
            if (SimpleNeuralNetwork == null) {
                SimpleNeuralNetwork = new MultilayerPerceptron(new PerceptronParameters {
                        LearningSpeed = LearnParameters.Parameters.LearnSpeed,
                        Moment = LearnParameters.Parameters.Moment
                    },
                    new SigmoidActivation(LearnParameters.Parameters.ActivationCoefficient),
                    LearnParameters.CellParameters.Select(x => x.LengthOfInput).Append(1).ToArray()
                );
                if (LoadNetwork) {
                    SimpleNeuralNetwork.Load(NeuralNetworkName);
                }
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
        public RecurentParameters Parameters { get; set; }
        public RecurentCellParameters[] CellParameters { get; set; }
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

    public enum UsingNeuralNetwork {
        Lstm,
        SimpleNeuralNetwork
    }
}
