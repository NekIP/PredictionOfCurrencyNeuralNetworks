using NeuralNetwork;
using System;

namespace Experiment {
	public class PerceptronNeuralNetworkTest : Experiment {
		public override void Run() {
			var nt = new PerceptronNeuralNetwork(
				new PeceptronNeuralNetworkParameters { LearningSpeed = 0.7, Moment = 0.3 },
				new SigmoidActivation(),
				2, 3, 1);
			Helper.PrintMatrix(nt.Neurons);
			Console.WriteLine("Weights: ");
			Helper.PrintMatrix(nt.Weights);
			for (var i = 0; i < 100; i++) {
				for (var j = 0.0; j < 1; j+=0.01) {
					for (var k = 0.0; k < 1; k += 0.01) {
						var ideal = f(i, k);
						var res = nt.Learn(new double[] { i, k }, new double[] { ideal });
						Console.WriteLine(res.Error[0] + "_" + res.Value[0] + "_" + ideal);
					}
				}
			}
		}

		private double f(double x, double y) {
			return x + y;
		}
	}
}