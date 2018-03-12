using DataAssistants.Structs;
using NeuralNetwork;
using System;

namespace DataManager {
    public class NeuralExtrapolation {
        public string NeuralNetworkName { get; set; }
        public MultilayerPerceptron NeuralNetwork { get; set; }
        public double Error { get; set; }

        public NeuralExtrapolation(string neuralNetworkName) {
            NeuralNetworkName = neuralNetworkName;
            NeuralNetwork = CreateNew();
            NeuralNetwork.Load(NeuralNetworkName);
        }

        public double GetValue(double previous, DateTime datePrevious, DateTime currentDate) {
            var lastOutput = (Vector)new double[] { previous };
            //for (var i = datePrevious; i < currentDate; i = i.AddHours(1)) {
                var currentResult = NeuralNetwork.Run(new NeuralNetworkData(lastOutput));
                lastOutput = NeuralNetwork.ConvertOutput(currentResult.Value[0, 0]);
            //}
            return lastOutput[0];
        }

        /// <summary>
        /// Step must be one hour
        /// </summary>
        /// <returns>Iterations of learning</returns>
        public int Learn(Vector[] input, Vector[] ideal, double acceptableError = 0.000001, int maxIteration = 1000) {
            var iteration = 0;
            var lastError = acceptableError * input.Length + 1;
            while (lastError / input.Length > acceptableError && iteration < maxIteration) {
                var error = 0.0;
                for (var i = 0; i < input.Length; i++) {
                    var learningResult = NeuralNetwork.Learn(new NeuralNetworkData(input[i]),
                        new NeuralNetworkData(ideal[i]));
                    error += learningResult.Error[0, 0, 0];
                }
                Console.WriteLine($"Extrapolator({ NeuralNetworkName }) learn with mean error:\t{ error / input.Length }");
                lastError = error;
                iteration++;
                NeuralNetwork.Save(NeuralNetworkName);
            }
            Console.WriteLine($"Extrapolator was learned with error \t{ lastError }\t on iteration \t{ iteration }");
            Error = lastError;
            return iteration;
        }

        protected MultilayerPerceptron CreateNew() => new MultilayerPerceptron(
            new PerceptronParameters { LearningSpeed = 0.7, Moment = 0.1 },
            new SigmoidActivation(),
            new[] { 1, 3, 3, 1 });
    }
}
