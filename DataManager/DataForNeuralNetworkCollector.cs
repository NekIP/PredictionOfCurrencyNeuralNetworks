using DataAssistants.Structs;
using DataBase;
using DataBase.Entities;
using DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
    public interface IDataForNeuralNetworkCollector : IDataCollector<DataForNeuralNetwork> {
        Task<DataForNeuralNetwork> GetData(DateTime date, TimeSpan step, bool caching = true);
        Task LearnExtrapolators(double acceptableError = 0.000001, int maxIteration = 1000);
        Vector ExpectedValues();
        Vector Dispersions(Vector expectedValues = null);
        Vector Maxs(IList<DataForNeuralNetwork> source = null);
        Vector Mins(IList<DataForNeuralNetwork> source = null);
        Vector Normalize(Vector x, Vector expectedValue, Vector dispersion);
        Vector Denormalize(Vector y, Vector expectedValue, Vector dispersion);
        Vector Scaling(Vector x, Vector min, Vector max);
        Vector Descaling(Vector y, Vector min, Vector max);
        Task<IList<DataForNeuralNetwork>> ListRaw();
        Task<IList<DataForNeuralNetwork>> ListNormalized();
        Task<IList<DataForNeuralNetwork>> ListScaled();
        string[] GetNames();
    }
    public class DataForNeuralNetworkCollector : DataCollector<DataForNeuralNetwork>, IDataForNeuralNetworkCollector {
        protected List<DataForNeuralNetworkContext> Contexts { get; set; }

        public DataForNeuralNetworkCollector(IDataForNeuralNetworkRepository repository, 
            ICAC40Collector cac40,
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
            IUsdToRubCurrencyCollector usdToRub) : base(repository) {
            Contexts = new List<DataForNeuralNetworkContext> {
                new DataForNeuralNetworkContext(cac40, IfDataNotExist.Aproximated, "cac40Extrapolator"),
                new DataForNeuralNetworkContext(dowJones, IfDataNotExist.Aproximated, "dowJonesExtrapolator"),
                new DataForNeuralNetworkContext(gdpPerCapitaPpp, IfDataNotExist.TakeNearLeft),
                new DataForNeuralNetworkContext(gold, IfDataNotExist.Aproximated, "goldExtrapolator"),
                new DataForNeuralNetworkContext(inflation, IfDataNotExist.TakeNearLeft),
                new DataForNeuralNetworkContext(mmvb, IfDataNotExist.Aproximated, "mmvbExtrapolator"),
                new DataForNeuralNetworkContext(oliBrent, IfDataNotExist.Aproximated, "oliBrentExtrapolator"),
                new DataForNeuralNetworkContext(oliLight, IfDataNotExist.Aproximated, "oliLightExtrapolator"),
                new DataForNeuralNetworkContext(refinancingRate, IfDataNotExist.TakeNearLeft),
                new DataForNeuralNetworkContext(rts, IfDataNotExist.Aproximated, "rtsExtrapolator"),
                new DataForNeuralNetworkContext(sAndP, IfDataNotExist.Aproximated, "sAndPExtrapolator"),
                new DataForNeuralNetworkContext(tradeBalance, IfDataNotExist.Aproximated, "tradeBalanceExtrapolator"),
                new DataForNeuralNetworkContext(usdToRub, IfDataNotExist.Aproximated, "usdToRubExtrapolator"),
            };
        }

        public async Task<DataForNeuralNetwork> GetData(DateTime date, TimeSpan step, bool caching = true) {
            if (caching && base.TryGet(date, step, out var chachingResult)) {
                return chachingResult as DataForNeuralNetwork;
            }
            var listResult = new List<double>();
            foreach (var context in Contexts) {
                var data = await context.GetData(date, step);
                if (!data.HasValue) {
                    return null;
                }
                listResult.Add(data.Value);
            }
            var result = new DataForNeuralNetwork(listResult.Count) { Data = listResult.ToArray(), Date = date };
            if (caching) {
                await Add(result);
            }
            return result;
        }

        public string[] GetNames() {
            var result = new string[Contexts.Count];
            for (var i = 0; i < result.Length; i++) {
                result[i] = Contexts[i].Collector.GetType().Name.Replace("Collector", "");
            }
            return result;
        }

        public Vector ExpectedValues() {
            var result = new Vector(Contexts.Count);
            for (var i = 0; i < result.Length; i++) {
                result[i] = ExpectedValue(x => DataForNeuralNetworkSelector(x, i));
            }
            return result;
        }

        public Vector Dispersions(Vector expectedValues = null) {
            if (expectedValues == null) {
                expectedValues = ExpectedValues();
            }
            var result = new Vector(Contexts.Count);
            for (var i = 0; i < result.Length; i++) {
                result[i] = Dispersion(expectedValues[i], x => DataForNeuralNetworkSelector(x, i));
            }
            return result;
        }

        public Vector Maxs(IList<DataForNeuralNetwork> source = null) {
            var result = new Vector(Contexts.Count);
            if (source == null) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = Max(x => DataForNeuralNetworkSelector(x, i));
                }
            }
            else {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = source.Max(x => DataForNeuralNetworkSelector(x, i));
                }
            }
            return result;
        }

        public Vector Mins(IList<DataForNeuralNetwork> source = null) {
            var result = new Vector(Contexts.Count);
            if (source == null) {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = Max(x => DataForNeuralNetworkSelector(x, i));
                }
            }
            else {
                for (var i = 0; i < result.Length; i++) {
                    result[i] = source.Max(x => DataForNeuralNetworkSelector(x, i));
                }
            }
            return result;
        }

        public override async Task DownloadMissingData(DateTime before, TimeSpan step) {
            foreach (var context in Contexts) {
                await context.Collector.DownloadMissingData(before, step);
            }
        }

        public async Task LearnExtrapolators(double acceptableError = 0.000001, int maxIteration = 1000) {
            foreach (var context in Contexts) {
                await context.LearnExtrapolator(acceptableError, maxIteration);
            }
        }

        public async Task<IList<DataForNeuralNetwork>> ListRaw() =>
            (await List()).Select(x => x as DataForNeuralNetwork).ToList();

        public async Task<IList<DataForNeuralNetwork>> ListNormalized() {
            var expectedValues = ExpectedValues();
            var dispersions = Dispersions(expectedValues);
            return (await List()).Select(x => new DataForNeuralNetwork {
                    Date = (x as DataForNeuralNetwork).Date,
                    Data = Normalize((x as DataForNeuralNetwork).Data, expectedValues, dispersions)
                }).ToList();
        }

        public async Task<IList<DataForNeuralNetwork>> ListScaled() {
            var normalized = await ListNormalized();
            var mins = Mins(normalized);
            var maxs = Maxs(normalized);
            return normalized.Select(x => new DataForNeuralNetwork {
                    Date = (x as DataForNeuralNetwork).Date,
                    Data = Scaling((x as DataForNeuralNetwork).Data, mins, maxs)
                }).ToList();
        }

        public Vector Normalize(Vector x, Vector expectedValue, Vector dispersion) =>
            (x - expectedValue) / Vector.Convert(dispersion, Math.Sqrt);

        public Vector Denormalize(Vector y, Vector expectedValue, Vector dispersion) =>
            y * Vector.Convert(dispersion, Math.Sqrt) + expectedValue;

        public Vector Scaling(Vector x, Vector min, Vector max) =>
            (x - min) * (1 - (-1)) / (max - min) + (-1);

        public Vector Descaling(Vector y, Vector min, Vector max) =>
            ((max - min) * (y - (-1))) / (1 - (-1)) + min;

        public double DataForNeuralNetworkSelector(DataForNeuralNetwork x, int i) =>
            (double)x.GetType().GetProperty($"D{ i + 1 }").GetValue(x);

        protected class DataForNeuralNetworkContext {
            public IfDataNotExist IfDataNotExist { get; private set; }
            public IDataCollector Collector { get; private set; }
            public NeuralExtrapolation Extrapolator { get; set; }
            public Func<Entity, double, double> Converter { get; set; } = 
                (current, previous) => (current.Selector() - previous) / previous;

            public DataForNeuralNetworkContext(IDataCollector collector,
                IfDataNotExist ifDataNotExist, Func<Entity, double, double> converter) {
                IfDataNotExist = ifDataNotExist;
                Collector = collector;
                Converter = converter;
            }

            public DataForNeuralNetworkContext(IDataCollector collector,
                IfDataNotExist ifDataNotExist, Func<Entity, double, double> converter, string extrapolatorName) {
                IfDataNotExist = ifDataNotExist;
                Collector = collector;
                Converter = converter;
                Extrapolator = new NeuralExtrapolation(extrapolatorName);
            }

            public DataForNeuralNetworkContext(IDataCollector collector,
                IfDataNotExist ifDataNotExist, string extrapolatorName) {
                IfDataNotExist = ifDataNotExist;
                Collector = collector;
                Extrapolator = new NeuralExtrapolation(extrapolatorName);
            }

            public DataForNeuralNetworkContext(IDataCollector collector,
                IfDataNotExist ifDataNotExist) {
                IfDataNotExist = ifDataNotExist;
                Collector = collector;
            }

            public async Task LearnExtrapolator(double acceptableError = 0.000001, int maxIteration = 1000) {
                if (IfDataNotExist == IfDataNotExist.Aproximated) {
                    var list = await Collector.List();
                    var input = new List<Vector>();
                    var ideal = new List<Vector>();
                    for (var i = 2; i < list.Count; i++) {
                        input.Add(new double[] { Converter(list[i - 1], list[i - 2].Selector()) });
                        ideal.Add(new double[] { Converter(list[i], list[i - 1].Selector()) });
                    }
                    Extrapolator.Learn(input.ToArray(), ideal.ToArray(), acceptableError, maxIteration);
                }
            }

            public Task<double?> GetData(DateTime date, TimeSpan step) {
                if (Collector.TryGet(date, step, out var data)) {
                    var near = GetNear(data, step);
                    var result = Converter(data, near);
                    return Task.FromResult(new double?(result));
                }
                return Approximate(date, step);
            }

            protected async Task<double?> Approximate(DateTime date, TimeSpan step) {
                switch (IfDataNotExist) {
                    case IfDataNotExist.TakeNearLeft:
                        var last = (await Collector.List(date - TimeSpan.FromDays(365) * 3, date, step)).LastOrDefault();
                        if (last is null) {
                            return null;
                        }
                        return Converter(last, GetNear(last, step));
                    case IfDataNotExist.Aproximated:
                        var leftValues = await Collector.List(date - TimeSpan.FromDays(365) * 3, date, step);
                        if (leftValues.Count > 1) {
                            var rightValues = await Collector.List(date + step, date + TimeSpan.FromDays(365) * 3, step);
                            if (rightValues.Count > 1) {
                                var values =  new List<Entity>() { leftValues.Last(), rightValues.First() };// leftValues.TakeLast(1).Union(rightValues.Take(1)).ToList();
                                var interpolator = new LinearInterpolation(
                                    new KeyValuePair<double, double>(values[0].Date.Ticks, Converter(values[0], GetNear(values[0], step))),
                                    new KeyValuePair<double, double>(values[1].Date.Ticks, Converter(values[1], GetNear(values[1], step)))
                                );
                                var interpolated = interpolator.GetValue(date.Ticks);
                                return interpolated;
                            }
                            if (!(Extrapolator is null)) {
                                var previous = leftValues.LastOrDefault();
                                var extrapolatedNeural = Extrapolator.GetValue(Converter(previous, GetNear(previous, step)), previous.Date, date);
                                return extrapolatedNeural;
                            }
                            var extrapolator = new LagrangeApproximation(leftValues
                                .TakeLast(2)
                                .Select(x => new KeyValuePair<double, double>(x.Date.Ticks, Converter(x, GetNear(x, step))))
                                .ToList()
                            );
                            var extrapolated = extrapolator.GetValue(date.Ticks);
                            return extrapolated;
                        }
                        return null;
                    case IfDataNotExist.Crush:
                        return null;
                }
                return null;
            }

            protected double GetNear(Entity current, TimeSpan step) {
                if (Collector.TryGet(current.Date - step, step, out var result)) {
                    return result.Selector();
                }
                var previous = Collector.GetNearLeft(current.Date);
                var interpolation = new LinearInterpolation(
                    new KeyValuePair<double, double>(previous.Date.Ticks, previous.Selector()),
                    new KeyValuePair<double, double>(current.Date.Ticks, current.Selector())
                );
                var interpolatedResult = interpolation.GetValue((current.Date - step).Ticks);
                return interpolatedResult;
            }
        }

        protected enum IfDataNotExist {
            TakeNearLeft,
            Aproximated,
            Crush
        }
    }
}
