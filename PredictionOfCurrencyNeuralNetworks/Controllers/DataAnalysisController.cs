using Business;
using Microsoft.AspNetCore.Mvc;
using PredictionOfCurrencyNeuralNetworks.Models;
using PredictionOfCurrencyNeuralNetworks.Models.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PredictionOfCurrencyNeuralNetworks.Controllers {
    public class DataAnalysisController : Controller {
        public IPredictionOfCurrencyManager PredictionOfCurrencyManager { get; set; }
        public DataAnalysisController(IPredictionOfCurrencyManager predictionOfCurrencyUsdToRub) {
            PredictionOfCurrencyManager = predictionOfCurrencyUsdToRub;
        }

        public IActionResult Index() {
            var model = PredictionOfCurrencyManager.PredictionOfCurrency.Keys.ToList();
            return View(model);
        }

        [HttpGet]
        public PredictionOfCurrencyInfoApiModel Info(string systemName) {
            var predictionOfCurrency = PredictionOfCurrencyManager.PredictionOfCurrency[systemName];
            var result = new PredictionOfCurrencyInfoApiModel();
            var learnResult = predictionOfCurrency.Fit();
            result.LearnResult = learnResult.Select(PredictionOfCurrencyLearnResultApiModel.Map).ToList();
            result.MeanErrorForLearnSet = result.LearnResult.Average(x => x.Error.Last());
            var efficiencyRelation = 0;
            var efficiencyTailUp = 0.0;
            var efficiencyTailDown = 0.0;
            Func<double, int> znak = x => x > 0 ? 1 : -1;
            for (var i = 0; i < result.LearnResult.Count; i++) {
                var ideal = result.LearnResult[i].Ideal.First() - result.LearnResult[i].Input.Last();
                var output = result.LearnResult[i].Output.First() - result.LearnResult[i].Input.Last();
                efficiencyRelation += znak(output) == znak(ideal) ? 1 : 0;
                efficiencyTailUp += Math.Pow(result.LearnResult[i].Ideal.First() - result.LearnResult[i].Output.Last(), 2);
                efficiencyTailDown += Math.Pow(result.LearnResult[i].Ideal.First() - result.LearnResult[i].Input.Last(), 2);
            }
            result.EfficiencyForLearnSet = (((double)efficiencyRelation) / result.LearnResult.Count) * 100;
            result.EfficiencyTailForLearnSet = Math.Sqrt(efficiencyTailUp) / Math.Sqrt(efficiencyTailDown);
            result.NameSystem = systemName;
            result.InputData = predictionOfCurrency.DataManager.DataTable.Data.Select(x => new DataForNeuralNetworkApiModel { Date = x.Date, Data = (double[])x.Vector }).ToList();
            result.DescriptionSystem = new string[] {
                $"Используемые коллекторы` { string.Join(", ", predictionOfCurrency.Collectors.Select(x => x.GetType().Name)) }",
                $"Обучающая выборка за период` ({ predictionOfCurrency.DataParameters.From }, { predictionOfCurrency.DataParameters.To }, { predictionOfCurrency.DataParameters.Step })",
                $"Обработка данных` { predictionOfCurrency.DataProcessingMethods.ToString() }",
                $"Вид данных` { predictionOfCurrency.DataValueType.ToString() }",
                $"Тип нейронной сети` { predictionOfCurrency.UsingNeuralNetwork.ToString() }",
                $"Частей подается за раз` { predictionOfCurrency.Chunk.ToString() }",
                $"Глобальные параметры сети` ActivationCoef = { predictionOfCurrency.LearnParameters.Parameters.ActivationCoefficient }, LearnSpeed = { predictionOfCurrency.LearnParameters.Parameters.LearnSpeed }, Moment = { predictionOfCurrency.LearnParameters.Parameters.Moment }",
                $"Параметры клеток (длина входа, длина выхода)` { string.Join(", ", predictionOfCurrency.LearnParameters.CellParameters.Select(x => $"({ x.LengthOfInput }, { x.LengthOfOutput })")) }",
                $"Имя сети` { predictionOfCurrency.NeuralNetworkName }"
            };
            return result;
        }
    }
}
