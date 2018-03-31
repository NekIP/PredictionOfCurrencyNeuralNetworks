using DataAssistants;
using DataAssistants.Structs;
using DataBase.Entities;
using DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business {
    public class PredictionOfCurrencyDataManager {
        public List<IDataCollector> Collectors { get; private set; }
        public DataParameter DataParameter { get; private set; }
        public DataProcessingMethods DataProcessingMethods { get; private set; }
        public DataValueType DataType { get; private set; }
        public PredictionOfCurrencyDataTable DataTable { get; set; }
        public PredictionOfCurrencyDataTable ExpectedValues { get; protected set; }
        public PredictionOfCurrencyDataTable Dispersions { get; protected set; }
        public PredictionOfCurrencyDataTable Maxs { get; protected set; }
        public PredictionOfCurrencyDataTable Mins { get; protected set; }
        public string NameOfCollectorForPredict { get; set; }

        protected string[] Names { get; set; }

        protected Dictionary<DataValueType, Func<PredictionOfCurrencyDataTable, PredictionOfCurrencyDataTable>> DataValueTypeConverter;
        protected Dictionary<DataProcessingMethods, Func<PredictionOfCurrencyDataTable, PredictionOfCurrencyDataTable>> DataProcessingMethodsConverter;

        public PredictionOfCurrencyDataManager(DataParameter dataParameter, DataProcessingMethods dataProcessingMethods,
            DataValueType dataType, string nameOfCollectorForPredict, List<IDataCollector> collectors) {
            Collectors = collectors;
            DataParameter = dataParameter;
            DataProcessingMethods = dataProcessingMethods;
            DataType = dataType;
            NameOfCollectorForPredict = nameOfCollectorForPredict;
            Names = new string[Collectors.Count];
            for (var i = 0; i < Collectors.Count; i++) {
                Names[i] = Collectors[i].GetType().Name;
            }
            DataValueTypeConverter = new Dictionary<DataValueType, Func<PredictionOfCurrencyDataTable, PredictionOfCurrencyDataTable>> {
                { DataValueType.Absolute, x => x },
                { DataValueType.Relative, ConvertInRelative },
                { DataValueType.RelativePercentage, ConvertInRelativePercentage }
            };
            DataProcessingMethodsConverter = new Dictionary<DataProcessingMethods, Func<PredictionOfCurrencyDataTable, PredictionOfCurrencyDataTable>> {
                { DataProcessingMethods.None, x => x },
                { DataProcessingMethods.Normalize, Normalize },
                { DataProcessingMethods.Scale, Scale },
                { DataProcessingMethods.NormalizeAndScale, NormalizeAndScale},
            };
        }

        public Vector ConvertInput(Vector input) {
            if (ExpectedValues == null) {
                InitExpectedValuesAndDispersions(DataTable);
            }
            if (Maxs == null) {
                InitMaxsAndMins(DataTable);
            }
            switch (DataProcessingMethods) {
                case DataProcessingMethods.None:
                    return input;
                case DataProcessingMethods.Normalize:
                    return Denormalize(input, ExpectedValues[0].Vector, Dispersions[0].Vector);
                case DataProcessingMethods.Scale:
                    return Descaling(input, Mins[0].Vector, Maxs[0].Vector);
                case DataProcessingMethods.NormalizeAndScale:
                    return Denormalize(Descaling(input, Mins[0].Vector, Maxs[0].Vector),
                        ExpectedValues[0].Vector, Dispersions[0].Vector);
            }
            return input;
        }

        public Vector ConvertOutput(Vector output) {
            if (ExpectedValues == null) {
                InitExpectedValuesAndDispersions(DataTable);
            }
            if (Maxs == null) {
                InitMaxsAndMins(DataTable);
            }
            switch (DataProcessingMethods) {
                case DataProcessingMethods.None:
                    return output;
                case DataProcessingMethods.Normalize:
                    return Denormalize(output, ExpectedValues[NameOfCollectorForPredict], Dispersions[NameOfCollectorForPredict]);
                case DataProcessingMethods.Scale:
                    return Descaling(output, Mins[NameOfCollectorForPredict], Maxs[NameOfCollectorForPredict]);
                case DataProcessingMethods.NormalizeAndScale:
                    return Denormalize(Descaling(output, Mins[NameOfCollectorForPredict], Maxs[NameOfCollectorForPredict]), 
                        ExpectedValues[NameOfCollectorForPredict], Dispersions[NameOfCollectorForPredict]);
            }
            return output;
        }

        public void InitializeData() {
            DataTable = List();
        }

        protected PredictionOfCurrencyDataTable List() {
            var result = new PredictionOfCurrencyDataTable(Names);
            var serializer = new Serializer();
            var backupFileName = $"DataForNeuralNetworkBackup{ GetBackupName() }.json";
            if (!serializer.Exists(backupFileName)) {
                for (var i = DataParameter.From; i < DataParameter.To; i = i.Add(DataParameter.Step)) {
                    var vector = GetRawData(i, DataParameter.Step);
                    if (vector.Length == Collectors.Count) {
                        result.Add(new PredictionOfCurrencyData { Date = i, Vector = vector });
                    }
                }
                result = DataValueTypeConverter[DataType](result);
                result = DataProcessingMethodsConverter[DataProcessingMethods](result);
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<PredictionOfCurrencyDataTable>(backupFileName);
            }
            return result;
        }

        protected Vector GetRawData(DateTime date, TimeSpan step) {
            var result = new List<double>();
            foreach (var collector in Collectors) {
                if (collector.TryGet(date, step, out var data)) {
                    result.Add(data.Selector());
                }
                else {
                    break;
                }
            }
            return result.ToArray();
        }

        protected PredictionOfCurrencyDataTable Normalize(PredictionOfCurrencyDataTable table) {
            InitExpectedValuesAndDispersions(table);
            return Convert(table, x => Normalize(x, ExpectedValues[0].Vector, Dispersions[0].Vector));
        }

        protected PredictionOfCurrencyDataTable Scale(PredictionOfCurrencyDataTable table) {
            InitMaxsAndMins(table);
            return Convert(table, x => Scaling(x, Maxs[0].Vector, Mins[0].Vector));
        }

        protected PredictionOfCurrencyDataTable NormalizeAndScale(PredictionOfCurrencyDataTable table) =>
            Scale(Normalize(table));

        protected PredictionOfCurrencyDataTable ConvertInRelative(PredictionOfCurrencyDataTable table) =>
            ConvertRelative(table, (current, previouse) => current - previouse);

        protected PredictionOfCurrencyDataTable ConvertInRelativePercentage(PredictionOfCurrencyDataTable table) =>
            ConvertRelative(table, (current, previouse) => (current - previouse) / previouse);

        protected PredictionOfCurrencyDataTable ConvertRelative(PredictionOfCurrencyDataTable table,
            Func<Vector, Vector, Vector> formula) {
            var result = new PredictionOfCurrencyDataTable(Names);
            for (var i = 1; i < table.Data.Count; i++) {
                result.Add(new PredictionOfCurrencyData {
                    Date = table[i].Date,
                    Vector = formula(table[i].Vector, table[i - 1].Vector)
                });
            }
            return result;
        }

        protected PredictionOfCurrencyDataTable Convert(PredictionOfCurrencyDataTable table,
            Func<Vector, Vector> formula) {
            var result = new PredictionOfCurrencyDataTable(Names);
            for (var i = 0; i < table.Data.Count; i++) {
                result.Add(new PredictionOfCurrencyData {
                    Date = table[i].Date,
                    Vector = formula(table[i].Vector)
                });
            }
            return result;
        }

        protected void InitExpectedValuesAndDispersions(PredictionOfCurrencyDataTable table) {
            ExpectedValues = new PredictionOfCurrencyDataTable(Names);
            ExpectedValues.Add(new PredictionOfCurrencyData { Vector = GetExpectedValues(table, GetBackupName()) });
            Dispersions = new PredictionOfCurrencyDataTable(Names);
            Dispersions.Add(new PredictionOfCurrencyData { Vector = GetDispersions(table, GetBackupName(), ExpectedValues[0].Vector) });
        }

        protected void InitMaxsAndMins(PredictionOfCurrencyDataTable table) {
            Maxs = new PredictionOfCurrencyDataTable(Names);
            Maxs.Add(new PredictionOfCurrencyData { Vector = GetMaxs(table, GetBackupName()) });
            Mins = new PredictionOfCurrencyDataTable(Names);
            Mins.Add(new PredictionOfCurrencyData { Vector = GetMins(table, GetBackupName()) });
        }

        protected Vector GetExpectedValues(PredictionOfCurrencyDataTable table, string backupName) {
            var serializer = new Serializer();
            var backupFileName = $"BackupExpectedValues{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = table[Collectors[i].GetType().Name].Average();
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        protected Vector GetDispersions(PredictionOfCurrencyDataTable table, string backupName, Vector expectedValues = null) {
            if (expectedValues == null) {
                expectedValues = GetExpectedValues(table, backupName);
            }
            var serializer = new Serializer();
            var backupFileName = $"BackupDispersions{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = table[Collectors[i].GetType().Name].Aggregate(0.0, (x, y) => x + Math.Pow(y - expectedValues[i], 2));
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        protected Vector GetMaxs(PredictionOfCurrencyDataTable table, string backupName) {
            var serializer = new Serializer();
            var backupFileName = $"BackupMaxs{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = table[Collectors[i].GetType().Name].Max();
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        protected Vector GetMins(PredictionOfCurrencyDataTable table, string backupName) {
            var serializer = new Serializer();
            var backupFileName = $"BackupMins{ backupName }.json";
            var result = new Vector(Collectors.Count);
            if (backupName == null || !serializer.Exists(backupFileName)) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = table[Collectors[i].GetType().Name].Min();
                }
                serializer.Serialize(result, backupFileName);
            }
            else {
                result = serializer.Deserialize<Vector>(backupFileName);
            }
            return result;
        }

        protected string GetBackupName() {
            var dateStr = $"{ DataParameter.From.ToString("(dd_MM_yyyy)_(HH_mm_ss)") }_{ DataParameter.To.ToString("(dd_MM_yyyy)_(HH_mm_ss)") }";
            var stepStr = $"D{ DataParameter.Step.Days }H{ DataParameter.Step.Hours }";
            var meta = $"{ (int)DataProcessingMethods }{ (int)DataType }";
            return $"{ meta }_{ dateStr }_{ stepStr }";
        }

        protected Vector Normalize(Vector x, Vector expectedValue, Vector dispersion) =>
            (x - expectedValue) / Vector.Convert(dispersion, Math.Sqrt);

        protected Vector Denormalize(Vector y, Vector expectedValue, Vector dispersion) =>
            y * Vector.Convert(dispersion, Math.Sqrt) + expectedValue;

        protected Vector Scaling(Vector x, Vector min, Vector max) =>
            (x - min) * (1 - (-1)) / (max - min) + (-1);

        protected Vector Descaling(Vector y, Vector min, Vector max) =>
            ((max - min) * (y - (-1))) / (1 - (-1)) + min;
    }
}
