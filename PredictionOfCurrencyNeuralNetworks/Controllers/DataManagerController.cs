using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBase;
using DataManager;
using PredictionOfCurrencyNeuralNetworks.Models.DataManager;
using System.Globalization;
using DataBase.Entities;
using PredictionOfCurrencyNeuralNetworks.Models;
using Business;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PredictionOfCurrencyNeuralNetworks.Controllers {
    public class DataManagerController : Controller {
        public IUsdToRubCurrencyCollector UsdToRubCollector { get; set; }
        public ICAC40Collector Cac40Collector { get; set; }
        public ICSI200Collector Csi200Collector { get; set; }
        public IDataForNeuralNetworkCollector DataForNeuralNetworkCollector { get; set; }
        public IDowJonesCollector DowJonesCollector { get; set; }
        public IGdpPerCapitaPppCollector GdpPerCapitaPppCollector { get; set; }
        public IGoldCollector GoldCollector { get; set; }
        public IInflationCollector InflationCollector { get; set; }
        public IMMVBCollector MmvbCollector { get; set; }
        public IOliBrentCollector OliBrentCollector { get; set; }
        public IOliLightCollector OliLightCollector { get; set; }
        public IRefinancingRateCollector RefinancingRateCollector { get; set; }
        public IRTSCollector RtsCollector { get; set; }
        public ISAndP500Collector SAndPCollector { get; set; }
        public ITradeBalanceCollector TradeBalanceCollector { get; set; }

        public Dictionary<string, IDataCollector> Collectors { get; set; }

        public DataManagerController(IUsdToRubCurrencyCollector usdToRubCollector,
            ICAC40Collector cac40Collector,
            ICSI200Collector csi200Collector,
            IDataForNeuralNetworkCollector dataForNeuralNetworkCollector,
            IDowJonesCollector dowJonesCollector,
            IGdpPerCapitaPppCollector gdpPerCapitaPppCollector,
            IGoldCollector goldCollector,
            IInflationCollector inflationCollector,
            IMMVBCollector mmvbCollector,
            IOliBrentCollector oliBrentCollector,
            IOliLightCollector oliLightCollector,
            IRefinancingRateCollector refinancingRateCollector,
            IRTSCollector rtsCollector,
            ISAndP500Collector sAndPCollector,
            ITradeBalanceCollector tradeBalanceCollector) {
            UsdToRubCollector = usdToRubCollector;
            Cac40Collector = cac40Collector;
            Csi200Collector = csi200Collector;
            DataForNeuralNetworkCollector = dataForNeuralNetworkCollector;
            DowJonesCollector = dowJonesCollector;
            GdpPerCapitaPppCollector = gdpPerCapitaPppCollector;
            GoldCollector = goldCollector;
            InflationCollector = inflationCollector;
            MmvbCollector = mmvbCollector;
            OliBrentCollector = oliBrentCollector;
            OliLightCollector = oliLightCollector;
            RefinancingRateCollector = refinancingRateCollector;
            RtsCollector = rtsCollector;
            SAndPCollector = sAndPCollector;
            TradeBalanceCollector = tradeBalanceCollector;
            Collectors = new Dictionary<string, IDataCollector> {
                { "UsdToRub", UsdToRubCollector },
                { "Cac40", Cac40Collector },
                { "Csi200", Csi200Collector },
                { "DataForNeuralNetwork", DataForNeuralNetworkCollector },
                { "DowJones", DowJonesCollector },
                { "GdpPerCapitaPpp", GdpPerCapitaPppCollector },
                { "Gold", GoldCollector },
                { "Inflation", InflationCollector },
                { "Mmvb", MmvbCollector },
                { "OliBrent", OliBrentCollector },
                { "OliLight", OliLightCollector },
                { "RefinancingRate", RefinancingRateCollector },
                { "Rts", RtsCollector },
                { "SAndP", SAndPCollector },
                { "TradeBalance", TradeBalanceCollector }
            };
        }

        public IActionResult Index() {
            var medel = new DataForNeuralNetworkInformation();
            return View();
        }

        [HttpGet]
        public async Task<DataCollectorResultApiModel> Load(string code) {
            if (!Collectors.ContainsKey(code)) {
                throw new Exception("Code is not exist");
            }
            var collector = Collectors[code];
            var result = await collector.List();
            var expectedValue = collector.ExpectedValue();
            var dispersion = collector.Dispersion(expectedValue);
            return DataCollectorResultApiModel.Map(result, expectedValue, dispersion);
        }
        [HttpGet]
        public async Task<DataForNeuralNetworkInformation> GetDataForNeuralNetworkInformation() {
            var list = await GetDataForNeuralNetwork();
            var result = new DataForNeuralNetworkInformation {
                Count = list.Count,
                From = list.First().Date,
                To = list.Last().Date,
                ExpectedValues = DataForNeuralNetworkCollector.ExpectedValues(),
                FieldsNames = DataForNeuralNetworkCollector.GetNames(),
                Maxs = DataForNeuralNetworkCollector.Maxs(),
                Mins = DataForNeuralNetworkCollector.Mins()
            };
            result.Dispersions = DataForNeuralNetworkCollector.Dispersions(result.ExpectedValues);
            return result;
        }

        [HttpGet]
        public async Task<List<DataForNeuralNetworkApiModel>> GetDataForNeuralNetwork() =>
            (await DataForNeuralNetworkCollector.ListRaw()).Select(DataForNeuralNetworkApiModel.Map).ToList();

        [HttpGet]
        public async Task<List<DataForNeuralNetworkApiModel>> GetDataForNeuralNetworkNormalized() =>
            (await DataForNeuralNetworkCollector.ListNormalized()).Select(DataForNeuralNetworkApiModel.Map).ToList();

        [HttpGet]
        public async Task<List<DataForNeuralNetworkApiModel>> GetDataForNeuralNetworkScaled() =>
            (await DataForNeuralNetworkCollector.ListScaled()).Select(DataForNeuralNetworkApiModel.Map).ToList();

        [HttpPost]
        public Task Add(string code, string dateStr, double value) {
            if (!Collectors.ContainsKey(code)) {
                throw new Exception("Code is not exist");
            }
            var date = DateTime.ParseExact(dateStr, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            return Collectors[code].Add(date, value);
        }

        [HttpPost]
        public Task Remove(string code, int id) {
            if (!Collectors.ContainsKey(code)) {
                throw new Exception("Code is not exist");
            }
            return Collectors[code].Remove(id);
        }

        [HttpPost]
        public Task Update(string code, int id, double value) {
            if (!Collectors.ContainsKey(code)) {
                throw new Exception("Code is not exist");
            }
            return Collectors[code].Update(id, value);
        }

        [HttpPost]
        public Task PrepareData(string code) {
            if (!Collectors.ContainsKey(code)) {
                throw new Exception("Code is not exist");
            }
            return Collectors[code].DownloadMissingData(DateTime.Now, TimeSpan.FromHours(1));
        }
    }
}
