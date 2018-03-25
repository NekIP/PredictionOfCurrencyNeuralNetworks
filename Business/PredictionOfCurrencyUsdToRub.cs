using DataAssistants.Structs;
using DataBase.Entities;
using DataManager;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business {
    public interface IPredictionOfCurrencyUsdToRub {
        PredictionOfCurrencyLearnParameters LearnParameters { get; set; }
        string BackupName { get; }
        string NeuralNetworkName { get; set; }
        Lstm Lstm { get; }
        List<DataForNeuralNetwork> Data { get; set; }
        double Predict(DateTime date);
        List<PredictionOfCurrencyLearnResult> Fit();
        Vector ExpectedValues();
        Vector Dispersions(Vector expectedValues = null);
        Vector Maxs();
        Vector Mins();
    }
    public class PredictionOfCurrencyUsdToRub : IPredictionOfCurrencyUsdToRub {
        public IDataForNeuralNetworkCollector DataForNeuralNetworkCollector { get; set; }
        public PredictionOfCurrencyLearnParameters LearnParameters { get; set; } = 
            new PredictionOfCurrencyLearnParameters(new DateTime(2007, 10, 14), new DateTime(2018, 3, 23), TimeSpan.FromDays(1));
        public string BackupName => $"{ LearnParameters.From.ToString("ddMMyyyyTHHmmss") }_{ LearnParameters.To.ToString("ddMMyyyyTHHmmss") }_{ "D" + LearnParameters.Step.Days + "H" + LearnParameters.Step.Hours }";
        public string NeuralNetworkName { get; set; } = "DefaultLstmNetworkForDay";
        public Lstm Lstm { get; private set; }
        public List<DataForNeuralNetwork> Data { get; set; }

        protected Vector ExpectedValuesLocal { get; set; }
        protected Vector DispersionsLocal { get; set; }
        protected string BackupNameInited;
        protected string NeuralNetworkNameInited;
        protected int InputLengthInited;

        public PredictionOfCurrencyUsdToRub(IDataForNeuralNetworkCollector dataForNeuralNetworkCollector) {
            DataForNeuralNetworkCollector = dataForNeuralNetworkCollector;
            InitData();
            InitNeuralNetwork(Data.First().Count);
        }

        public PredictionOfCurrencyUsdToRub(IDataForNeuralNetworkCollector dataForNeuralNetworkCollector,
            PredictionOfCurrencyLearnParameters learnParameters) {
            DataForNeuralNetworkCollector = dataForNeuralNetworkCollector;
            LearnParameters = learnParameters;
            InitData();
            InitNeuralNetwork(Data.First().Count);
        }

        public double Predict(DateTime date) {
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
        }

        public List<PredictionOfCurrencyLearnResult> Fit() {
            InitData();
            InitNeuralNetwork(Data.First().Count);
            var result = new List<PredictionOfCurrencyLearnResult>();
            for (var i = 0; i < Data.Count - 1; i++) {
                var input = new Vector[] { Data[i].Data };
                var ideal = new Vector[] { new double[] { Data[i + 1].Data.Last() } };
                var (output, error) = Lstm.Learn(input, ideal);
                result.Add(new PredictionOfCurrencyLearnResult {
                    Date = Data[i].Date,
                    Error = error.Last(),
                    Output = DataForNeuralNetworkCollector.Denormalize(output.Last(), 
                        new double[] { ExpectedValuesLocal.Values.Last() }, new double[] { DispersionsLocal.Values.Last() }),
                    Ideal = DataForNeuralNetworkCollector.Denormalize(ideal.Last(),
                        new double[] { ExpectedValuesLocal.Values.Last() }, new double[] { DispersionsLocal.Values.Last() }),
                    Input = DataForNeuralNetworkCollector.Denormalize(input.Last(), ExpectedValuesLocal, DispersionsLocal),
                });
            }
            Lstm.Save(NeuralNetworkName);
            return result;
        }

        public Vector ExpectedValues() => 
            DataForNeuralNetworkCollector.ExpectedValues(LearnParameters.From, LearnParameters.To, LearnParameters.Step,
                BackupName);

        public Vector Dispersions(Vector expectedValues = null) {
            if (expectedValues == null) {
                expectedValues = ExpectedValues();
            }
            return DataForNeuralNetworkCollector.Dispersions(LearnParameters.From, LearnParameters.To, LearnParameters.Step, 
                expectedValues,
                BackupName);
        }

        public Vector Maxs() =>
            DataForNeuralNetworkCollector.Maxs(LearnParameters.From, LearnParameters.To, LearnParameters.Step,
                BackupName);

        public Vector Mins() =>
            DataForNeuralNetworkCollector.Mins(LearnParameters.From, LearnParameters.To, LearnParameters.Step,
                BackupName);

        protected void InitData() {
            if (BackupNameInited != BackupName || Data == null) {
                var data = DataForNeuralNetworkCollector.GetSet(LearnParameters.From, LearnParameters.To, LearnParameters.Step);
                (Data, ExpectedValuesLocal, DispersionsLocal) = DataForNeuralNetworkCollector.NormalizeSet(data, BackupName);
                BackupNameInited = BackupName;
            }
        }

        protected void InitNeuralNetwork(int inputLength) {
            if (InputLengthInited != inputLength || NeuralNetworkNameInited != NeuralNetworkName || Lstm == null) {
                Lstm = new Lstm(inputLength, 1, new RecurentParameters(1.2, 0.5, 0.1),
                    new RecurentCellParameters(inputLength, inputLength),
                    new RecurentCellParameters(inputLength, 1));
                Lstm.Load(NeuralNetworkName);
                NeuralNetworkNameInited = NeuralNetworkName;
                InputLengthInited = inputLength;
            }
        }
    }

    public class PredictionOfCurrencyLearnResult {
        public DateTime Date { get; set; }
        public Vector Input { get; set; }
        public Vector Output { get; set; }
        public Vector Error { get; set; }
        public Vector Ideal { get; set; }
    }

    public class PredictionOfCurrencyLearnParameters {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public TimeSpan Step { get; set; }
        public PredictionOfCurrencyLearnParameters(DateTime from, DateTime to, TimeSpan step) {
            From = from;
            To = to;
            Step = step;
        }
    }
}
