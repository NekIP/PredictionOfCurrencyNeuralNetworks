using Business;
using Microsoft.AspNetCore.Mvc;
using PredictionOfCurrencyNeuralNetworks.Models.DataAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace PredictionOfCurrencyNeuralNetworks.Controllers {
    public class DataAnalysisController : Controller {
        public IPredictionOfCurrencyManager PredictionOfCurrencyManager { get; set; }
        public DataAnalysisController(IPredictionOfCurrencyManager predictionOfCurrencyUsdToRub) {
            PredictionOfCurrencyManager = predictionOfCurrencyUsdToRub;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public List<PredictionOfCurrencyLearnResultApiModel> Fit() {
            var result = PredictionOfCurrencyManager.PredictionOfCurrency["defaultRelativeForOneDay"].Fit();
            return result.Select(PredictionOfCurrencyLearnResultApiModel.Map).ToList();
        }
    }
}
