using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBase;
using DataManager;

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
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public Task<List<Entity>> Get() {
            return UsdToRubCollector.List();
        }
    }
}
