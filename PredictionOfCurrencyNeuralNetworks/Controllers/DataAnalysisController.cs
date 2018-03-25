using Business;
using Microsoft.AspNetCore.Mvc;
using PredictionOfCurrencyNeuralNetworks.Models.DataAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace PredictionOfCurrencyNeuralNetworks.Controllers {
    public class DataAnalysisController : Controller {
        public IPredictionOfCurrencyUsdToRub PredictionOfCurrencyUsdToRub { get; set; }
        public DataAnalysisController(IPredictionOfCurrencyUsdToRub predictionOfCurrencyUsdToRub) {
            PredictionOfCurrencyUsdToRub = predictionOfCurrencyUsdToRub;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public List<PredictionOfCurrencyLearnResultApiModel> Fit() {
            var result = PredictionOfCurrencyUsdToRub.Fit();
            return result.Select(PredictionOfCurrencyLearnResultApiModel.Map).ToList();
        }
    }
}
