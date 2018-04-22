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
			var testResult = predictionOfCurrency.Test();
			result.LearnResult = learnResult.Select(PredictionOfCurrencyLearnResultApiModel.Map).ToList();
			result.TestResult = testResult.Select(PredictionOfCurrencyLearnResultApiModel.Map).ToList();
			result.MeanErrorForLearnSet = result.LearnResult.Average(x => x.Error.Last());
			result.MeanErrorForTestSet = result.TestResult.Average(x => x.Error.Last());
			var efficiencyRelationLearn = 0;
            var efficiencyTailUpLearn = 0.0;
            var efficiencyTailDownLearn = 0.0;
            Func<double, int> znak = x => x > 0 ? 1 : -1;
            for (var i = 0; i < result.LearnResult.Count; i++) {
                var ideal = result.LearnResult[i].Ideal.First() - result.LearnResult[i].Input.Last();
                var output = result.LearnResult[i].Output.First() - result.LearnResult[i].Input.Last();
                efficiencyRelationLearn += znak(output) == znak(ideal) ? 1 : 0;
                efficiencyTailUpLearn += Math.Pow(result.LearnResult[i].Ideal.First() - result.LearnResult[i].Output.Last(), 2);
                efficiencyTailDownLearn += Math.Pow(result.LearnResult[i].Ideal.First() - result.LearnResult[i].Input.Last(), 2);
            }
			result.EfficiencyForLearnSet = (((double)efficiencyRelationLearn) / result.LearnResult.Count) * 100;
			result.EfficiencyTailForLearnSet = Math.Sqrt(efficiencyTailUpLearn) / Math.Sqrt(efficiencyTailDownLearn);
			var efficiencyRelationTest = 0;
			var efficiencyTailUpTest = 0.0;
			var efficiencyTailDownTest = 0.0;
			for (var i = 0; i < result.TestResult.Count; i++) {
				var ideal = result.TestResult[i].Ideal.First() - result.TestResult[i].Input.Last();
				var output = result.TestResult[i].Output.First() - result.TestResult[i].Input.Last();
				efficiencyRelationTest += znak(output) == znak(ideal) ? 1 : 0;
				efficiencyTailUpTest += Math.Pow(result.TestResult[i].Ideal.First() - result.TestResult[i].Output.Last(), 2);
				efficiencyTailDownTest += Math.Pow(result.TestResult[i].Ideal.First() - result.TestResult[i].Input.Last(), 2);
			}
			result.EfficiencyForTestSet = (((double)efficiencyRelationTest) / result.TestResult.Count) * 100;
			result.EfficiencyTailForTestSet = Math.Sqrt(efficiencyTailUpTest) / Math.Sqrt(efficiencyTailDownTest);
			result.NameSystem = systemName;
            result.InputData = predictionOfCurrency.DataManager.LearnData.Data.Select(x => new DataForNeuralNetworkApiModel { Date = x.Date, Data = x.Vector }).ToList();
            result.DescriptionSystem = new string[] {
                $"Используемые коллекторы` { string.Join(", ", predictionOfCurrency.Collectors.Select(x => x.GetType().Name)) }",
                $"Обучающая выборка за период` ({ predictionOfCurrency.DataParameters.From }, { predictionOfCurrency.DataParameters.To }, { predictionOfCurrency.DataParameters.Step })",
				$"Тестовая выборка за период` ({ predictionOfCurrency.DataParameters.To }, { DateTime.Now }, { predictionOfCurrency.DataParameters.Step })",
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
